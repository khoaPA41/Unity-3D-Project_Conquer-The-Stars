using ConquerTheStars.Fight;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ConquerTheStars.UI.Player
{
    public class ItemAttachElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private static ItemAttachElement _activeDrag;
        private int _dragPointerId;

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _quantity;

        private Canvas _canvas;
        private ItemAttachData _itemAttachData;
        public ItemAttachData ItemAttachData => _itemAttachData;

        // private int quantity;

        private RectTransform _dragIcon;

        private void OnDisable()
        {
            CancelDrag();
        }

        public void Initialize(Sprite icon, string quantity, ItemAttachData itemAttachData, Canvas canvas)
        {
            Reset();
            _icon.sprite = icon;
            _quantity.SetText(quantity);
            _itemAttachData = itemAttachData;
            _canvas = canvas;
        }

        public void Reset()
        {
            CancelDrag();
            _icon.sprite = null;
            _quantity.text = null;
            _itemAttachData = null;
            _canvas = null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_activeDrag != null) return; // if another finger touch item drag 

            var team = PlayerTeam.Instance;
            if (team == null) return;

            if (_itemAttachData == null) return;
            var itemAttach = team.GetItemAttachQuantity(_itemAttachData.ItemAttachType); // Get Item Attach Data

            if (itemAttach == null || itemAttach.Quantity <= 0) return;

            // Only get drag rights if the item is valid
            _activeDrag = this;
            _dragPointerId = eventData.pointerId;

            var newDragIcon = new GameObject(
                "DragIcon",
                typeof(RectTransform),
                typeof(Image),
                typeof(CanvasRenderer)
            );

            _dragIcon = newDragIcon.GetComponent<RectTransform>();
            _dragIcon.SetParent(_canvas.transform, false);
            _dragIcon.SetAsLastSibling();


            _dragIcon.anchorMin = new Vector2(.5f, .5f);
            _dragIcon.anchorMax = new Vector2(.5f, .5f);
            _dragIcon.pivot = new Vector2(.5f, .5f);
            _dragIcon.sizeDelta = _icon.rectTransform.rect.size;

            var image = newDragIcon.GetComponent<Image>();
            image.sprite = _icon.sprite;
            image.preserveAspect = true;

            image.raycastTarget = false;

            UpdateDragPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!OwnsDrag(eventData.pointerId)) return;
            UpdateDragPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!OwnsDrag(eventData.pointerId)) return;
            CancelDrag();
        }

        private void UpdateDragPosition(PointerEventData eventData)
        {
            if (_dragIcon == null) return;

            var rectTransform = (RectTransform)_canvas.transform;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null, out Vector2 localPos))
            {
                _dragIcon.localPosition = (Vector3)localPos;
            }
        }

        public bool OwnsDrag(int pointerId) // Check if the current finger is really a valid finger
        {
            return _activeDrag == this && _dragPointerId == pointerId;
        }

        private void CancelDrag()
        {
            if (_dragIcon != null)
                Destroy(_dragIcon.gameObject);

            _dragIcon = null;

            // Don't unlock someone else's cell.
            if (_activeDrag == this)
                _activeDrag = null;
        }
    }
}