using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public abstract class Equipment : Item
    {
        protected readonly StatsController _statsController;
        protected readonly StatChangingItemDescriptor _statChangingItemDescriptor;
        protected bool _equipped;

        public override int Quantity => -1;

        protected Equipment(ItemDescriptor descriptor, StatsController statsController) 
            : base(descriptor)
        {
            _statChangingItemDescriptor = descriptor as StatChangingItemDescriptor;
            _statsController = statsController;
        }
        
        public override void Use()
        {
            if (_equipped)
                UnEquip();
            else
                Equip();
        }

        protected virtual void Equip()
        {
            _equipped = true;
            foreach (var stat in _statChangingItemDescriptor.Stats)
                _statsController.ProcessModificator(stat);
        }

        protected virtual void UnEquip()
        {
            _equipped = false;
            foreach (var stat in _statChangingItemDescriptor.Stats)
                _statsController.ProcessModificator(stat.GetReversedModificator());
        }
    }
}
