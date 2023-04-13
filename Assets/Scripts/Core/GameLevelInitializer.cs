using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items;
using Core.Services.Updater;
using Player;
using InputReader;
using Items.Storage;
using UnityEngine;

namespace Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private GameUIInputView _gameUIInputView;
        [SerializeField] private ItemsStorage _itemsStorage;

        private ExternalDevicesInputReader _externalDevicesInput;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;
        private DropGenerator _dropGenerator;
        private ItemsSystem _itemsSystem;

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

            var itemsFactory = new ItemsFactory(_playerSystem.StatsController);
            var itemsSystem = new ItemsSystem();

            var itemDescriptors = _itemsStorage.ItemScriptables
                .Select(x => x.ItemDescriptor)
                .ToList();
            _dropGenerator = new DropGenerator(_playerEntity, itemDescriptors, itemsSystem);
            _dropGenerator.DropRandomItem(_dropGenerator.GetItemRarity());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _projectUpdater.IsPaused = !_projectUpdater.IsPaused;
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}