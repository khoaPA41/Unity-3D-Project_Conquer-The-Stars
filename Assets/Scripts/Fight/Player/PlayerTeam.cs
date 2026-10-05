using System;
using System.Collections.Generic;
using ConquerTheStars.Factory.Item;
using UnityEngine;

namespace ConquerTheStars.Fight
{
    [Serializable]
    public class ItemQuantity
    {
        public ItemData ItemData;
        public int Quantity;
    }

    [Serializable]
    public class ItemInfoForSave
    {
        public ItemType ItemType;
        public int Quantity;
    }

    public class PlayerTeam : MonoBehaviour
    {
        public static PlayerTeam Instance { get; set; }

        [SerializeField] private List<ItemQuantity> _itemDatas;
        [SerializeField] private int _expMultipler = 2;
        [SerializeField] private int _originExp = 100;
        [SerializeField] private int _levelStartStep = 10;

        public List<PooledObjectId> TeamNameList { get; set; } = new();
        public Vector3 CurrentPosition { get; private set; } = new Vector3(36f, 0f, 62f);

        private List<ItemInfoForSave> _initialItem = new();

        public int TeamLevel;
        public int Exp;
        public int CurrentNeededExp;

        public event Action<ItemType, string> UpdateItemQuantityAction = delegate { };
        public event Action<int> LevelUp = delegate { };

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

            _initialItem = SaveItemInfor();
        }

        public void FirstTime()
        {
            TeamLevel = 1;
            Exp = 0;
            CurrentNeededExp = _originExp;

            SetupItemQuantity(_initialItem);
        }

        public void SetupItemQuantity(List<ItemInfoForSave> itemInfos)
        {
            if (itemInfos == null || itemInfos.Count == 0) return;
            foreach (var itemInfo in itemInfos)
            {
                var item = GetItem(itemInfo.ItemType);
                if (item == null) continue;
                item.Quantity = Mathf.Max(0, itemInfo.Quantity);

                UpdateItemQuantityAction?.Invoke(itemInfo.ItemType, item.Quantity.ToString());
            }
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

        public List<ItemInfoForSave> SaveItemInfor()
        {
            var result = new List<ItemInfoForSave>();

            foreach (var item in _itemDatas)
            {
                result.Add(new ItemInfoForSave
                {
                    ItemType = item.ItemData.ItemType,
                    Quantity = item.Quantity
                });
            }
            return result;
        }

        public void AddQuantity()
        {
            for (int i = 0; i < _itemDatas.Count; i++)
            {

            }
        }

        public void AddExp(int value)
        {
            if (value <= 0) return;

            Exp += value;
            bool isLevelUp = false;

            while (Exp >= CurrentNeededExp)
            {
                TeamLevel++;
                Exp -= CurrentNeededExp;
                isLevelUp = true;
                if (TeamLevel % _levelStartStep == 0)
                {
                    CurrentNeededExp *= _expMultipler;
                }
            }

            if (isLevelUp)
            {
                LevelUp?.Invoke(TeamLevel);
            }
        }
    }
}