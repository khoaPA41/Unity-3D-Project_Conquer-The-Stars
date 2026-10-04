using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ConquerTheStars.Factory.Item;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Fight
{
    [Serializable]
    public class ItemQuantity
    {
        public ItemData ItemData;
        public int Quantity;
    }

    public class PlayerTeam : MonoBehaviour
    {
        public static PlayerTeam Instance { get; set; }

        [SerializeField] private List<ItemQuantity> _itemDatas;
        public List<PooledObjectId> TeamNameList { get; set; } = new();

        public Vector3 CurrentPosition { get; private set; } = new Vector3(36f, 0f, 62f);

        public int TeamLevel { get; set; } = 1;

        public event Action<ItemType, string> UpdateItemQuantityAction = delegate { };

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            AddTeamMate(PooledObjectId.Player_I);
            AddTeamMate(PooledObjectId.Player_II);
            AddTeamMate(PooledObjectId.Player_III);
        }

        public void AddTeamMate(PooledObjectId name)
        {
            TeamNameList.Add(name);
        }

        public List<ItemQuantity> GetItemList()
        {
            return _itemDatas;
        }

        public bool TryConsumeItem(ItemType itemType)
        {
            var item = _itemDatas.Find(item => item.ItemData.ItemType == itemType);

            if (item == null || item.Quantity <= 0) return false;

            item.Quantity--;
            var quantity = item.Quantity;
            UpdateItemQuantityAction?.Invoke(itemType, quantity.ToString());
            return true;
        }

        public ItemQuantity GetItem(ItemType itemType)
        {
            return _itemDatas.Find(item => item.ItemData.ItemType == itemType);
        }

        public void AddQuantity()
        {
            for (int i = 0; i < _itemDatas.Count; i++)
            {

            }
        }
    }
}