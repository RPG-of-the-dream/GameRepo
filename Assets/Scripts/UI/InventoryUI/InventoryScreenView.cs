using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UI.Core;
using UI.InventoryUI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public class InventoryScreenView: ScreenView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _gemsText;
        [SerializeField] private Transform _backPackContainer;
        [SerializeField] private Transform _equipmentContainer;
        
        public  List<ItemSlot> ItemSlots { get; private set; }
        public  List<EquipmentSlot> EquipmentSlots { get; private set; }

        [field: SerializeField] public Image MovingImage { get; private set; }

        public event Action CloseClicked;

        private void Awake()
        {
            _closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
            ItemSlots = GetComponentsInChildren<ItemSlot>().ToList();
            EquipmentSlots = GetComponentsInChildren<EquipmentSlot>().ToList();
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        } 

        public void SetCoinsAmount(string amount) => _coinsText.text = amount;
        
        public void SetGemsAmount(string amount) => _gemsText.text = amount;
    }
}