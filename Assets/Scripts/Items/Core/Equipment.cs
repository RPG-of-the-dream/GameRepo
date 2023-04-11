using Items.Data;
using Items.Enums;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    internal class Equipment : Item
    {
        private readonly StatsController _statsController;
        private readonly StatChangingItemDescriptor _statChangingItemDescriptor;
        private bool _equipped;
        public Equipment(ItemDescriptor descriptor, StatsController statsController) 
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

        private void Equip()
        {
            _equipped = true;
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat);
            }
        }

        private void UnEquip()
        {
            _equipped = false;
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat.GetReversedModificator());
            }
        }
    }
}
