using System;
using Drawing;
using Entity.Behaviour;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Entity.Controller
{
    public abstract class BaseEntity : ILevelGraphicElement, IDisposable
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly StatsController StatsController;

        private float _currentHp;
        
        public float VerticalPosition => _entityBehaviour.VerticalPosition;
        
        public event Action<BaseEntity> Died;
        public event Action<ILevelGraphicElement> VerticalPositionChanged;

        protected BaseEntity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatsController = statsController;

            _currentHp = StatsController.GetStatValue(StatType.Health);
            _entityBehaviour.DamageTaken += OnDamageTaken;
        }

        public void SetDrawingOrder(int order) => _entityBehaviour.SetDrawingOrder(order);
        
        public virtual void Dispose()
        {
            _entityBehaviour.PlayDeath();
            StatsController.Dispose();
        }

        protected abstract void VisualizeHp(float currentHp);
        protected void OnVerticalPositionChanged() => VerticalPositionChanged?.Invoke(this);

        private void OnDamageTaken(float damage)
        {
            damage -= StatsController.GetStatValue(StatType.Defence);
            if (damage < 0)
            {
                return;
            }

            _currentHp = Mathf.Clamp(_currentHp - damage, 0, _currentHp);
            VisualizeHp(_currentHp);
            if (_currentHp <= 0)
            {
                Died?.Invoke(this);
            }
        }
    }
}
