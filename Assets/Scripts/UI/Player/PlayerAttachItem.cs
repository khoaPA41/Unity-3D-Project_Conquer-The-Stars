using System;
using System.Collections.Generic;
using ConquerTheStars.Factory.Item;
using ConquerTheStars.Fight;
using ConquerTheStars.Pattern.Object_Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
// public class ItemQuantityUI
// {
//     public GameObject Item;
//     public Image Icon;
//     public TextMeshProUGUI Quantity;
//     public ItemAttachData ItemAttachData;
// }

public class ItemEquipmentInfo
{
    public int SlotIndex;
    public ItemAttachType ItemAttachType;
}

public class PlayerAttachItem : MonoBehaviour
{
    private readonly PooledObjectId _itemElementName = PooledObjectId.ItemAttach;

    [Header("Canvas")]
    [SerializeField] private Canvas _canvas;

    [Header("Item Equipment")]
    [SerializeField] private Image _item_I;
    [SerializeField] private Image _item_II;

    [Header("Parent Object")]
    [SerializeField] private GameObject _parentObject;

    [Header("Empty Icon")]
    [SerializeField] private Sprite _emptyIcon;

    private List<PooledObject> _itemSpawnedList = new();
    private List<ItemEquipmentInfo> _itemInfor = new();

    private int _selectedIndex;
    public int SelectIndex => _selectedIndex;

    private void Start()
    {
        UpdateInventory();
        RefreshItemEquipment();
    }

    private void OnEnable()
    {
        UpdateInventory();
        RefreshItemEquipment();
    }

    public void UpdateInventory()
    {
        RefreshItem();
        var team = PlayerTeam.Instance;
        if (team == null) return;

        var itemList = team.ItemAttachInventories;
        if (itemList == null || itemList.Count == 0) return;

        foreach (var itemAttach in itemList)
        {
            var item = ObjectPoolingManagers.Instance.GetPooledObject(_itemElementName, Vector3.zero);
            var rect = item.GetComponent<RectTransform>();
            rect.SetParent(_parentObject.transform, false);
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;

            var itemElement = item.GetComponent<ItemAttachElement>();
            itemElement.Initialize(itemAttach.ItemAttachData.Icon, itemAttach.Quantity.ToString(), itemAttach.ItemAttachData, _canvas);

            _itemSpawnedList.Add(item);
        }
    }

    private void RefreshItem()
    {
        if (_itemSpawnedList == null || _itemSpawnedList.Count == 0) return;

        foreach (var item in _itemSpawnedList)
        {
            item.Release();
        }
        _itemSpawnedList.Clear();
    }

    public void RefreshItemEquipment()
    {
        var team = PlayerTeam.Instance;
        if (team == null) return;

        var playerSlotList = team.PlayerSlotList;
        if (playerSlotList == null || playerSlotList.Count == 0) return;

        var playerSlot = playerSlotList[_selectedIndex];
        var icon_I = playerSlot.Slot_I;
        var icon_II = playerSlot.Slot_II;



        _item_I.sprite = icon_I != null ? icon_I.Icon : _emptyIcon;
        _item_II.sprite = icon_II != null ? icon_II.Icon : _emptyIcon;

        _itemInfor.Clear();

        if (icon_I != null)
        {
            _itemInfor.Add(new ItemEquipmentInfo
            {
                SlotIndex = 0,
                ItemAttachType = playerSlot.Slot_I.ItemAttachType
            });
        }

        if (icon_II != null)
        {
            _itemInfor.Add(new ItemEquipmentInfo
            {
                SlotIndex = 1,
                ItemAttachType = playerSlot.Slot_II.ItemAttachType
            });
        }
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

    public void Remove(int itemEquipmentInfoIndex)
    {
        var team = PlayerTeam.Instance;
        if (team == null) return;

        var playerSlotList = team.PlayerSlotList;
        if (playerSlotList == null || playerSlotList.Count == 0) return;
        var playerSlot = playerSlotList[_selectedIndex];

        var item = _itemInfor[itemEquipmentInfoIndex];
        if (!team.TryUnequip(playerSlot.PlayerId, item.SlotIndex, team.GetItemAttachData(item.ItemAttachType))) return;

        UpdateInventory();
        RefreshItemEquipment();
    }
}
