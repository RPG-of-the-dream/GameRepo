using System;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public class InventoryScreenView: ScreenView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _gemsText;
        
        [field: SerializeField] public Transform Backpack { get; private set; }
        [field: SerializeField] public Transform Equipment { get; private set; }

        public event Action CloseClicked;

        private void Awake() => _closeButton.onClick.AddListener(() => CloseClicked?.Invoke());

        private void OnDestroy() => _closeButton.onClick.RemoveAllListeners();

        public void SetCoinsAmount(string amount) => _coinsText.text = amount;
        
        public void SetGemsAmount(string amount) => _gemsText.text = amount;
    }
}