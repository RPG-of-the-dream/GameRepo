using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items;
using Core.Services.Updater;
using InputReader;
using Items.Rarity;
using Items.Storage;
using Player;
using UI;
using UnityEngine;

namespace Core.Scene
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private GameUIInputView _gameUIInputView;
        [SerializeField] private ItemRarityDescriptorsStorage _rarityDescriptorsStorage;
        [SerializeField] private LayerMask _whatIsPlayer;
        [SerializeField] private ItemsStorage _itemsStorage;

        private ExternalDevicesInputReader _externalDevicesInput;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;
        private DropGenerator _dropGenerator;
        private ItemsSystem _itemsSystem;
        private UIContext _uiContext;

        private IList<IDisposable> _disposables;
        
        private bool _onPause;

        private void Awake()
        {
            _disposables = new List<IDisposable>();
            if (ProjectUpdater.Instance == null)
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
            else
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;
            
            _externalDevicesInput = new ExternalDevicesInputReader();
            _playerSystem = new PlayerSystem(_playerEntity, new List<IEntityInputSource>()
            {
                _gameUIInputView,
                _externalDevicesInput
            });
            _disposables.Add(_playerSystem);
            
            _uiContext = new UIContext(new List<IWindowsInputSource>()
            {
                _gameUIInputView,
                _externalDevicesInput
            });
            _disposables.Add(_uiContext);

            var itemsFactory = new ItemsFactory(_playerSystem.StatsController);
            var rarityColors = _rarityDescriptorsStorage.RarityDescriptors
                .Cast<IItemRarityColor>()
                .ToList();
            var itemsSystem = new ItemsSystem(rarityColors, itemsFactory, _whatIsPlayer);

            var itemDescriptors = _itemsStorage.ItemScriptables
                .Select(x => x.ItemDescriptor)
                .ToList();
            _dropGenerator = new DropGenerator(_playerEntity, itemDescriptors, itemsSystem);
            bool isDropped;
            do
            {
                isDropped = _dropGenerator.DropRandomItem(_dropGenerator.GetItemRarity());
            } while (!isDropped);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _uiContext.CloseCurrentScreen();
            }
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}