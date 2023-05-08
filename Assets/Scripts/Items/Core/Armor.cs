using Assets.Scripts.Items.Enums;
using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Armor : Equipment
    {
        public Armor(ItemDescriptor descriptor, StatsController statsController,
            EquipmentType equipmentType) 
            : base(descriptor, statsController, equipmentType)
        {
        }

        protected override void Equip()
        {
            _equipped = true;
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat);
            }
        }

        protected override void UnEquip()
        {
            _equipped = false;
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat.GetReversedModificator());
            }
        }
    }
}
