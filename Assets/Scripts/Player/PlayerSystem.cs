using System;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Items;
using Entity.Controller;
using InputReader;
using StatsSystem;
using UnityEngine;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly PlayerEntityBehaviour _playerEntity;
        private readonly List<IDisposable> _disposables;

        public PlayerBrain PlayerBrain { get; }
        public StatsController StatsController { get; }
        public Inventory Inventory { get; }
        
        public PlayerSystem(
            PlayerEntityBehaviour playerEntity, 
            List<IEntityInputSource> inputSources, 
            WeaponsFactory weaponsFactory)
        {
            _disposables = new List<IDisposable>();
            
            var statStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            var stats = statStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            StatsController = new StatsController(stats);
            _disposables.Add(StatsController);
            
            _playerEntity = playerEntity;
            _playerEntity.Initialize();

            Inventory = new Inventory(null, null, _playerEntity.transform, new EquipmentConditionChecker());
            PlayerBrain = new PlayerBrain(playerEntity, inputSources, StatsController, Inventory, weaponsFactory);
            PlayerBrain.Died += OnPlayerDeath;
            _disposables.Add(PlayerBrain);
        }

        public void Dispose()
        {
            PlayerBrain.Died -= OnPlayerDeath;
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }

        private void OnPlayerDeath(BaseEntity baseEntity)
        {
            _playerEntity.Respawn();
        }
    }
}