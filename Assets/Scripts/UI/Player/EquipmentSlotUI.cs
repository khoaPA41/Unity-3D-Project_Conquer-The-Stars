using System;
using ConquerTheStars.Fight;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConquerTheStars.UI.Player
{
    public class EquipmentSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private PlayerAttachItem _playerAttachItem;
        [SerializeField] private Image _icon;
        [SerializeField] private int slotIndex;

        private PlayerTeam _team;

        public void OnDrop(PointerEventData eventData)
        {
            _team = PlayerTeam.Instance;
            if (_team == null) return;

            if (eventData.pointerDrag == null) return;

            var dragItem = eventData.pointerDrag.GetComponent<ItemAttachElement>();
            if (dragItem == null || dragItem.ItemAttachData == null) return;

            if (!dragItem.OwnsDrag(eventData.pointerId)) return;

            var itemAttach = _team.GetItemAttachQuantity(dragItem.ItemAttachData.ItemAttachType); // Get Item Attach Data
            if (itemAttach == null || itemAttach.Quantity <= 0) return;

            if (!_team.TryEquip(_team.PlayerSlotList[_playerAttachItem.SelectIndex].PlayerId, slotIndex, dragItem.ItemAttachData)) return;


            Debug.Log($"Get {dragItem.ItemAttachData.ItemAttachType} in slot {slotIndex} of {_team.PlayerSlotList[_playerAttachItem.SelectIndex].PlayerId}");

            _playerAttachItem.UpdateInventory();
            _playerAttachItem.RefreshItemEquipment();
        }
    }
}