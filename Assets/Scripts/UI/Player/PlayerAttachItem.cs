using System;
using System.Collections.Generic;
using ConquerTheStars.Fight;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class ItemQuantityUI
{
    public GameObject Item;
    public Image Icon;
    public TextMeshProUGUI Quantity;
}

public class PlayerAttachItem : MonoBehaviour
{
    [Header("Item Content")]
    [SerializeField] private List<ItemQuantityUI> _itemQuantityList;

    [Header("Item Equipment")]
    [SerializeField] private Image _item_I;
    [SerializeField] private Image _item_II;

    [Header("Empty Icon")]
    [SerializeField] private Sprite _emptyIcon;

    private int _selectedIndex;

    private void Start()
    {
        // _team = PlayerTeam.Instance;
        RefreshInventory();
        RefreshItemEquipment();
    }

    private void OnEnable()
    {
        RefreshInventory();
        RefreshItemEquipment();
    }

    private void RefreshInventory()
    {
        var team = PlayerTeam.Instance;
        if (team == null) return;

        var itemList = team.ItemAttachInventories;
        if (itemList == null || itemList.Count == 0) return;

        for (int i = 0; i < itemList.Count; i++)
        {
            _itemQuantityList[i].Icon.sprite = itemList[i].ItemAttachData.Icon;
            _itemQuantityList[i].Quantity.SetText(itemList[i].Quantity.ToString());
            _itemQuantityList[i].Item.SetActive(true);
        }
    }

    public void RefreshItemEquipment()
    {
        var team = PlayerTeam.Instance;
        if (team == null) return;

        var playerSlotList = team.PlayerSlotList;
        if (playerSlotList == null || playerSlotList.Count == 0) return;

        // if (index < 0 || index >= playerSlotList.Count) return;

        var playerSlot = playerSlotList[_selectedIndex];
        var icon_I = playerSlot.Slot_I;
        var icon_II = playerSlot.Slot_II;

        _item_I.sprite = icon_I != null ? icon_I.Icon : _emptyIcon;
        _item_II.sprite = icon_II != null ? icon_II.Icon : _emptyIcon;
    }

    public void SwapPlayer()
    {
        var team = PlayerTeam.Instance;
        if (team == null) return;

        var nextIndex = _selectedIndex + 1;
        if (nextIndex >= team.PlayerSlotList.Count)
            nextIndex = 0;

        _selectedIndex = nextIndex;
        RefreshItemEquipment();
    }
}
