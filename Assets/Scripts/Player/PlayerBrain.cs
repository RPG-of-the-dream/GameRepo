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
using Battle.Weapon;

namespace Player
{
    public class PlayerBrain : BaseEntity
    {
        private const string WeaponType = "WeaponType";
        
        private readonly PlayerEntityBehaviour _playerEntity;
        private readonly List<IEntityInputSource> _inputSources;

        private readonly EquipmentSetter _equipmentSetter;
        private readonly Inventory _inventory;

        private bool _isAttacking;
        private bool _canAttack;
        private WeaponBase _currentWeapon;
        private WeaponsFactory _weaponsFactory;

        public PlayerBrain(
            PlayerEntityBehaviour entityBehaviour, 
            List<IEntityInputSource> inputSources, 
            StatsController statsController, 
            Inventory inventory,
            WeaponsFactory weaponsFactory) 
            : base(entityBehaviour, statsController)
        {
            _playerEntity = entityBehaviour;
            _playerEntity.AttackEnded += OnAttackEnded;
            _playerEntity.AttackRequested += OnAttackRequested;
            
            _inputSources = inputSources;
            VisualizeHp(StatsController.GetStatValue(StatType.Health));
            _equipmentSetter = new EquipmentSetter(_playerEntity.CharacterEquipment);

            _inventory = inventory;
            _inventory.EquipmentChanged += OnEquipmentChanged;

            _weaponsFactory = weaponsFactory;
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
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

        private void OnAttackRequested() => _currentWeapon?.Attack(StatsController.GetStatValue(StatType.Damage),
                _playerEntity.CurrentDirection);

        private void OnAttackEnded()
        {
            _isAttacking = false;
            _currentWeapon?.EndAttack();
            ProjectUpdater.Instance.Invoke(() => _canAttack = true,
                StatsController.GetStatValue(StatType.AfterAttackDelay));
        }
        
        private void OnEquipmentChanged()
        {
            _equipmentSetter.UpdateEquipment(
                _inventory.Equipments.Select(element => element.Descriptor.ItemId).ToList());

            var weapon = _inventory.Equipments.Find(element => element.IsWeapon());

            if (weapon is null)
            {
                _canAttack = false;
                return; 
            }

            _currentWeapon = _weaponsFactory.GetWeapon(weapon.Descriptor.ItemId);

            _canAttack = true;
            _playerEntity.SetAnimationParameter(WeaponType, (int)weapon.GetItemType());
        }
        
        private void OnFixedUpdate()
        {
            if (_isAttacking)
            {
                return;
            }
            
            Vector2 direction = GetDirection();
            _playerEntity.Move(direction.normalized * StatsController.GetStatValue(StatType.Speed) * Time.fixedDeltaTime);

            if (direction.y != 0)
            {
                OnVerticalPositionChanged();
            }

            if (IsAttack && _canAttack)
            {
                _playerEntity.StartAttack();
                _isAttacking = true;
                _canAttack = false;
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