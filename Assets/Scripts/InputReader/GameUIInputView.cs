using System;
using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource, IWindowsInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _inventoryButton;

        public Vector2 Direction => new(_joystick.Horizontal, _joystick.Vertical);
        public bool Attack { get; private set; }
        public event Action InventoryRequested;
        
        public void ResetOneTimeActions() => Attack = false;

        private void Awake()
        {
            _attackButton.onClick.AddListener(() => Attack = true);
            _inventoryButton.onClick.AddListener(() => InventoryRequested?.Invoke());
        }

        private void OnDestroy()
        {
            _attackButton.onClick.RemoveAllListeners();
            _inventoryButton.onClick.RemoveAllListeners();
        }
    }
}