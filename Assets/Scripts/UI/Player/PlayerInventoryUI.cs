using System.Collections.Generic;
using ConquerTheStars.Fight;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    private readonly PooledObjectId _itemElementName = PooledObjectId.ItemInventory;

    [Header("Parent Object")]
    [SerializeField] private GameObject _parentObject;

    private List<PooledObject> _itemSpawnedList = new();

    private void Start()
    {
        UpdateInventory();
    }

    private void OnEnable()
    {
        UpdateInventory();
    }


    private void UpdateInventory()
    {
        Refresh();
        var team = PlayerTeam.Instance;
        if (team == null) return;

        var itemList = team.ItemDatas;

        if (itemList == null || itemList.Count == 0) return;

        var objectPooling = ObjectPoolingManagers.Instance;

        if (objectPooling == null) return;

        foreach (var item in itemList)
        {
            var itemElement = objectPooling.GetPooledObject(_itemElementName, Vector3.zero);
            itemElement.gameObject.transform.SetParent(_parentObject.transform);
            _itemSpawnedList.Add(itemElement);
            var element = itemElement.GetComponent<ItemElementForInventory>();
            element.Icon.sprite = item.ItemData.ItemIcon;
            element.Quantity.SetText(item.Quantity.ToString());
        }
    }

    private void Refresh()
    {
        if (_itemSpawnedList == null || _itemSpawnedList.Count == 0) return;

        foreach (var item in _itemSpawnedList)
        {
            item.Release();
        }
        _itemSpawnedList.Clear();
    }
}
