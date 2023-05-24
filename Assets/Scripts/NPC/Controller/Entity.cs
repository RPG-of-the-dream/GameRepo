using System;
using Drawing;
using NPC.Behaviour;
using StatsSystem;

namespace Assets.Scripts.NPC.Controller
{
    public abstract class Entity : ILevelGraphicElement, IDisposable
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly StatsController StatsController;

        public event Action<Entity> Died;

        public float VerticalPosition => _entityBehaviour.VerticalPosition;
        public event Action<ILevelGraphicElement> VerticalPositionChanged;

        protected Entity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
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
