using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryUI.Elements
{
    public class ItemSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, 
        IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image _itemBack;
        [SerializeField] private Image _emptyImage;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _itemAmount;

        [SerializeField] protected Button RemoveButton;
        [SerializeField] private Button _slotButton;

        public event Action<ItemSlot> SlotClicked;
        public event Action<ItemSlot> SlotClearClicked;

        public event Action<ItemSlot> SlotClickedDown;
        public event Action<ItemSlot> DragStarted;
        public event Action<ItemSlot, Vector2> Dragged;
        public event Action<ItemSlot, Vector2> DragEnded;

        private void Awake()
        {
            RemoveButton.onClick.AddListener(() => SlotClearClicked?.Invoke(this));
        }

        public void OnPointerDown(PointerEventData eventData) => SlotClickedDown?.Invoke(this);
        public void OnBeginDrag(PointerEventData eventData) => DragStarted?.Invoke(this);
        public void OnDrag(PointerEventData eventData) => Dragged?.Invoke(this, eventData.position);
        public void OnEndDrag(PointerEventData eventData) => DragEnded?.Invoke(this, eventData.position);
        public void OnPointerClick(PointerEventData eventData) => SlotClicked?.Invoke(this);

        public void SetItem(Sprite iconSprite, Sprite itemBackSprite, int amount)
        {
            _itemBack.gameObject.SetActive(true);
            _icon.sprite = iconSprite;
            _emptyImage.gameObject.SetActive(false);
            _itemBack.sprite = itemBackSprite;
            RemoveButton.gameObject.SetActive(true);

            if (_itemAmount == null)          
                return;
                        
            _itemAmount.gameObject.SetActive(amount > 0);
            _itemAmount.text = amount.ToString();
        }

        public void RemoveItem(Sprite emptyBackSprite)
        {
            _itemBack.sprite = emptyBackSprite;
            _itemBack.gameObject.SetActive(false);
            _emptyImage.gameObject.SetActive(true);
            RemoveButton.gameObject.SetActive(false);
            if (_itemAmount != null)            
                _itemAmount.gameObject.SetActive(false);            
        }

        private void OnDestroy()
        {
            RemoveButton.onClick.RemoveAllListeners();
        }
    }
}