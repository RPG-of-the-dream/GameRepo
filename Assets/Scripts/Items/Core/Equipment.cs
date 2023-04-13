using Items.Data;
using Items.Enums;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public abstract class Equipment : Item
    {
        protected readonly StatsController _statsController;
        protected readonly StatChangingItemDescriptor _statChangingItemDescriptor;
        protected bool _equipped;

        protected Equipment(ItemDescriptor descriptor, StatsController statsController) 
            : base(descriptor)
        {
            _statChangingItemDescriptor = descriptor as StatChangingItemDescriptor;
            _statsController = statsController;
        }

        public override int Amount => -1;
        public ItemType Type { get; }
        public override void Use()
        {
            if (_equipped)
                UnEquip();
            else
                Equip();
        }

        public abstract void Equip();

        public abstract void UnEquip();
    }
}
