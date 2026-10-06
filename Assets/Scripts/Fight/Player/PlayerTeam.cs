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

    [Serializable]
    public class ItemAttachQuantity
    {
        public ItemAttachData ItemAttachData;
        public int Quantity;
    }

    [Serializable]
    public class PlayerSlot
    {
        public PooledObjectId PlayerId;
        public StatsData Stats;
        public ItemAttachData Slot_I;
        public ItemAttachData Slot_II;

        public float GetEquipmentBonus(ItemAttachType type)
        {
            return ReadBonus(Slot_I, type) + ReadBonus(Slot_II, type);
        }

        public float ReadBonus(ItemAttachData itemAttachData, ItemAttachType itemAttachType)
        {
            return itemAttachData != null && itemAttachData.ItemAttachType == itemAttachType
            ? itemAttachData.Value
            : 0f;
        }
    }

    public class PlayerTeam : MonoBehaviour
    {
        public static PlayerTeam Instance { get; set; }

        [Header("Player Stats Data")]
        [SerializeField] private StatsData _playerStats_I;
        [SerializeField] private StatsData _playerStats_II;
        [SerializeField] private StatsData _playerStats_III;

        // Item using
        [SerializeField] private List<ItemQuantity> _itemDatas;
        public List<ItemQuantity> ItemDatas => _itemDatas;

        // Level
        [SerializeField] private int _expMultipler = 2;
        [SerializeField] private int _originExp = 100;
        [SerializeField] private int _levelStartStep = 10;

        public List<PooledObjectId> TeamNameList { get; set; } = new();
        public Vector3 CurrentPosition { get; private set; } = new Vector3(36f, 0f, 62f);
        private List<ItemInfoForSave> _initialItem = new();

        // Item Attach List
        public List<PlayerSlot> PlayerSlotList = new();
        public List<ItemAttachQuantity> ItemAttachInventories = new();

        public int TeamLevel { get; set; }
        public int Exp { get; set; }
        public int CurrentNeededExp { get; set; }

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

            AddTeamMate(PooledObjectId.Player_I, _playerStats_I);
            AddTeamMate(PooledObjectId.Player_II, _playerStats_II);
            AddTeamMate(PooledObjectId.Player_III, _playerStats_III);

            TryEquip(PooledObjectId.Player_I, 0, ItemAttachInventories[2].ItemAttachData);
            TryEquip(PooledObjectId.Player_II, 0, ItemAttachInventories[1].ItemAttachData);
            TryEquip(PooledObjectId.Player_III, 0, ItemAttachInventories[0].ItemAttachData);

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

        public void AddTeamMate(PooledObjectId name, StatsData statsData)
        {
            TeamNameList.Add(name);
            PlayerSlotList.Add(new PlayerSlot
            {
                PlayerId = name,
                Slot_I = null,
                Slot_II = null,
                Stats = statsData
            });
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

        //Equip Item Attach
        public bool TryEquip(PooledObjectId playerId, int slotIndex, ItemAttachData itemAttachData)
        {
            if (itemAttachData == null || slotIndex < 0 || slotIndex > 1) return false;

            var player = PlayerSlotList.Find(target => target.PlayerId == playerId);
            if (player == null) return false;

            var oldItem = slotIndex == 0 ? player.Slot_I : player.Slot_II;
            if (oldItem == itemAttachData) return true;

            var itemInventory = ItemAttachInventories.Find(has => has.ItemAttachData == itemAttachData);
            if (itemInventory == null || itemInventory.Quantity <= 0) return false;

            itemInventory.Quantity--;

            if (oldItem != null) AddItemAttach(oldItem);

            if (slotIndex == 0) player.Slot_I = itemAttachData;
            if (slotIndex == 1) player.Slot_II = itemAttachData;
            return true;
        }

        public void Unequip()
        {

        }


        // Reward

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

        public void AddItemAttach(ItemAttachData itemAttachData)
        {
            if (itemAttachData == null) return;
            var item = ItemAttachInventories.Find(has => has.ItemAttachData == itemAttachData);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                ItemAttachInventories.Add(new ItemAttachQuantity { ItemAttachData = itemAttachData, Quantity = 1 });
            }
        }
    }
}