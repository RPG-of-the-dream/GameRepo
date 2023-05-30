using System;
using Drawing;
using Entity.Behaviour;
using StatsSystem;

namespace Entity.Controller
{
    public abstract class BaseEntity : ILevelGraphicElement, IDisposable
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly StatsController StatsController;

        public event Action<BaseEntity> Died;

        public float VerticalPosition => _entityBehaviour.VerticalPosition;
        public event Action<ILevelGraphicElement> VerticalPositionChanged;

        protected BaseEntity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatsController = statsController;
        }
        public void SetDrawingOrder(int order) => _entityBehaviour.SetDrawingOrder(order);
        
        public virtual void Dispose() => StatsController.Dispose();
        
        protected void OnVerticalPositionChanged() => VerticalPositionChanged?.Invoke(this);
    }
}
