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
    private List<ItemInUse> activeBuffList = new();
    private PlayerCombatStateMachine playerCombatStateMachine;


    private ItemData itemToUse;

    private void Start()
    {
        playerCombatStateMachine = GetComponent<PlayerCombatStateMachine>();
    }

    public void SetItemToUse(ItemData itemData)
    {
        itemToUse = itemData;
    }

    public void AddBuff(ItemType itemType, int remainingTurn, float value)
    {
        activeBuffList.Add(new ItemInUse(itemType, remainingTurn, value));
    }

    public void ApplyBuff()
    {
        Debug.Log("Add");
        IItem item = ItemFactory.CreateItem(itemToUse.ItemType);
        item.Use(playerCombatStateMachine, itemToUse.ItemType, itemToUse.RemainingTurn, itemToUse.Value);
    }

    private void RemoveBuff(ItemType itemType, int remainingTurn, float value)
    {
        Debug.Log("Remove");
        IItem item = ItemFactory.CreateItem(itemType);
        item.Use(playerCombatStateMachine, itemType, remainingTurn, -value);
    }

    public void CheckRemainingBuff()
    {
        foreach (var item in activeBuffList)
        {
            item.RemainingTurn--;
        }

        activeBuffList.RemoveAll(buff =>
        {
            if (buff.RemainingTurn <= 0)
            {
                RemoveBuff(buff.ItemType, buff.RemainingTurn, buff.Value);
                return true;
            }
            return false;
        });
    }







}
