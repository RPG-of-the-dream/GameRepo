using System;
using Core.Services.Updater;
using UnityEngine;

namespace InputReader
{
    public class ExternalDevicesInputReader : IEntityInputSource, IWindowsInputSource, IDisposable
    {
        public Vector2 Direction => new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        public bool Attack { get; private set; }
        public event Action InventoryRequested;

        public ExternalDevicesInputReader() => ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        public void ResetOneTimeActions() => Attack = false;
        
        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                InventoryRequested?.Invoke();
            }
        }
    }
}