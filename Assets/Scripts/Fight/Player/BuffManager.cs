using System;
using System.Collections.Generic;
using ConquerTheStars.Factory.Item;
using ConquerTheStars.Fight;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;
[RequireComponent(typeof(PlayerCombatStateMachine))]

[RequireComponent(typeof(PlayerTeam))]

[Serializable]
public class ItemInUse
{
    public ItemType ItemType;
    public int RemainingTurn;
    public float Value;
    public ItemInUse(ItemType itemType, int remainingTurn, float value)
    {
        ItemType = itemType;
        RemainingTurn = remainingTurn;
        Value = value;
    }
}
public class BuffManager : MonoBehaviour
{
    private List<ItemInUse> _activeBuffList = new();
    private PlayerCombatStateMachine _playerCombatStateMachine;


    private ItemData _itemToUse;

    public event Action<Sprite> OnBuffSuccess;

    private void Start()
    {
        _playerCombatStateMachine = GetComponent<PlayerCombatStateMachine>();
    }

    public void SetItemToUse(ItemData itemData)
    {
        _itemToUse = itemData;
    }

    public void AddBuff(ItemType itemType, int remainingTurn, float value)
    {
        _activeBuffList.Add(new ItemInUse(itemType, remainingTurn, value));
    }

    public void ApplyBuff()
    {
        IItem item = ItemFactory.CreateItem(_itemToUse.ItemType);
        item.Use(_playerCombatStateMachine, _itemToUse.ItemType, _itemToUse.RemainingTurn, _itemToUse.Value);
    }

    private void RemoveBuff(ItemType itemType, int remainingTurn, float value)
    {
        IItem item = ItemFactory.CreateItem(itemType);
        item.Use(_playerCombatStateMachine, itemType, remainingTurn, -value);
    }

    public void CheckRemainingBuff()
    {
        foreach (var item in _activeBuffList)
        {
            item.RemainingTurn--;
        }

        _activeBuffList.RemoveAll(buff =>
        {
            if (buff.RemainingTurn <= 0)
            {
                RemoveBuff(buff.ItemType, buff.RemainingTurn, buff.Value);
                return true;
            }
            return false;
        });
    }



    public void CallSetupBuffUi(Sprite sprite)
    {
        OnBuffSuccess?.Invoke(sprite);
    }
}
