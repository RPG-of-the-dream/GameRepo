using System.Linq;
using System.Collections.Generic;
using Core.Services.Updater;
using Entity.Controller;
using InputReader;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Player
{
    public class PlayerBrain : BaseEntity
    {
        private readonly PlayerEntityBehaviour _playerEntity;
        private readonly List<IEntityInputSource> _inputSources;

        public PlayerBrain(PlayerEntityBehaviour entityBehaviour, List<IEntityInputSource> inputSources, StatsController statsController) 
            : base(entityBehaviour, statsController)
        {
            _playerEntity = entityBehaviour;
            _inputSources = inputSources;
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        public override void Dispose()
        {
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
            base.Dispose();
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