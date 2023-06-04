using System.Linq;
using System.Collections.Generic;
using Core.Services.Updater;
using Entity.Controller;
using InputReader;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;
using Assets.Scripts.Items.CharacterEquipment;
using Assets.Scripts.Items;

namespace Player
{
    public class PlayerBrain : BaseEntity
    {
        private readonly PlayerEntityBehaviour _playerEntity;
        private readonly List<IEntityInputSource> _inputSources;

        private readonly EquipmentSetter _equipmentSetter;
        private readonly Inventory _inventory;

        public PlayerBrain(PlayerEntityBehaviour entityBehaviour, List<IEntityInputSource> inputSources, 
            StatsController statsController, Inventory inventory) 
            : base(entityBehaviour, statsController)
        {
            _playerEntity = entityBehaviour;
            _inputSources = inputSources;
            VisualizeHp(StatsController.GetStatValue(StatType.Health));
            _equipmentSetter = new EquipmentSetter(_playerEntity.CharacterEquipment);

            _inventory = inventory;
            _inventory.EquipmentChanged += OnEquipmentChanged;
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        private void OnEquipmentChanged()
        {
            _equipmentSetter.UpdateEquipment(_inventory.Equipments.Select(element => element.Descriptor.ItemId).ToList());
        }

        public override void Dispose()
        {
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
            base.Dispose();
        }

        protected sealed override void VisualizeHp(float currentHp)
        {
            if (_playerEntity.PlayerStatsUIView.HpBar.maxValue < currentHp)
            {
                _playerEntity.PlayerStatsUIView.HpBar.maxValue = currentHp;
            }

            _playerEntity.PlayerStatsUIView.HpBar.value = currentHp;
        }

        private void OnFixedUpdate()
        {
            Vector2 direction = GetDirection();
            _playerEntity.Move(direction.normalized * StatsController.GetStatValue(StatType.Speed) * Time.fixedDeltaTime);

            if (direction.y != 0)
            {
                OnVerticalPositionChanged();
            }

            if (IsAttack)
            {
                _playerEntity.StartAttack();
            }

            foreach (var inputSource in _inputSources)
            {
                inputSource.ResetOneTimeActions();
            }
        }

        private Vector2 GetDirection()
        {
            foreach (var inputSource in _inputSources)
            {
                if (inputSource.Direction == Vector2.zero)
                {
                    continue;
                }

                return inputSource.Direction;
            }

            return Vector2.zero;
        }
        private bool IsAttack => _inputSources.Any(source => source.Attack);
    }
}