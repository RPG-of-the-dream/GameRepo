using System;
using System.Collections.Generic;
using InputReader;
using UI.Core;
using UI.Enums;
using UI.InventoryUI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class UIContext : IDisposable
    {
        private const string LoadPath = "UI/";
        
        private readonly Dictionary<ScreenType, IScreenController> _controllers;
        private readonly Transform _uiContainer;
        private readonly List<IWindowsInputSource> _inputSources;
        
        private IScreenController _currentController;

        public UIContext(List<IWindowsInputSource> inputSources)
        {
            _controllers = new Dictionary<ScreenType, IScreenController>();
            _inputSources = inputSources;
            foreach (IWindowsInputSource inputSource in _inputSources)
            {
                inputSource.InventoryRequested += OpenInventory;
            }

            GameObject container = new GameObject()
            {
                name = nameof(UIContext)
            };
            _uiContainer = container.transform;
        }
        
        public void CloseCurrentScreen()
        {
            if (_currentController == null)
            {
                return;
            }
            _currentController.Complete();
            _currentController = null;
        }

        public void Dispose()
        {
            foreach (IWindowsInputSource inputSource in _inputSources)
            {
                inputSource.InventoryRequested -= OpenInventory;
            }

            foreach (IScreenController screenPresenter in _controllers.Values)
            {
                screenPresenter.CloseRequested -= CloseCurrentScreen;
                screenPresenter.OpenScreenRequested -= OpenScreen;
            }
        }

        private void OpenInventory() => OpenScreen(ScreenType.Inventory);

        private void OpenScreen(ScreenType screenType)
        {
            _currentController?.Complete();

            if (!_controllers.TryGetValue(screenType, out IScreenController screenController))
            {
                screenController = GetPresenter(screenType);
                screenController.CloseRequested += CloseCurrentScreen;
                screenController.OpenScreenRequested += OpenScreen;
                _controllers.Add(screenType, screenController);
            }

            _currentController = screenController;
            _currentController.Initialize();
        }

        private IScreenController GetPresenter(ScreenType screenType)
        {
            return screenType switch
            {
                ScreenType.Inventory => new InventoryScreenPresenter(GetView<InventoryScreenView>(screenType)),
                _ => throw new NullReferenceException()
            };
        }

        private TView GetView<TView>(ScreenType screenType) where TView : ScreenView
        {
            TView prefab = Resources.Load<TView>($"{LoadPath}{screenType.ToString()}");
            return Object.Instantiate(prefab, _uiContainer);
        }
    }
}