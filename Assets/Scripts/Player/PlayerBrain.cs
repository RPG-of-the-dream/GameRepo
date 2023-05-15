using System;
using System.Linq;
using System.Collections.Generic;
using Core.Services.Updater;
using InputReader;
using UnityEngine;

namespace Player
{
    public class PlayerBrain : IDisposable
    {
        private readonly PlayerEntityBehaviour _playerEntity;
        private readonly List<IEntityInputSource> _inputSources;

        public PlayerBrain(PlayerEntityBehaviour playerEntity, List<IEntityInputSource> inputSources)
        {
            _playerEntity = playerEntity;
            _inputSources = inputSources;
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        public void Dispose() => ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
        
        private void OnFixedUpdate()
        {
            _playerEntity.Move(GetDirection());

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