using System.Collections.Generic;
using ConquerTheStars.Fight;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;

namespace ConquerTheStars.UI.Player
{
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

        private void OnDisable()
        {
            Refresh();
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
                var rect = itemElement.GetComponent<RectTransform>();
                rect.SetParent(_parentObject.transform, false);
                rect.localScale = Vector3.one;
                rect.localRotation = Quaternion.identity;


                _itemSpawnedList.Add(itemElement);
                var element = itemElement.GetComponent<ItemElementForInventory>();
                element.Icon.sprite = item.ItemData.ItemIcon;
                element.Quantity.SetText(item.Quantity.ToString());
            }
        }

        private void Refresh()
        {
            foreach (var item in _itemSpawnedList)
            {
                if (item != null)
                    item.Release();
            }
            _itemSpawnedList.Clear();
        }
    }
}