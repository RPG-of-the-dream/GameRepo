﻿using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Armor : Equipment
    {
        public Armor(ItemDescriptor descriptor, StatsController statsController) 
            : base(descriptor, statsController)
        {
        }

        public override void Equip()
        {
            _equipped = true;
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat);
            }
        }

        public override void UnEquip()
        {
            _equipped = false;
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat.GetReversedModificator());
            }
        }
    }
}
