﻿using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Accessory : Equipment
    {
        public Accessory(ItemDescriptor descriptor, StatsController statsController) 
            : base(descriptor, statsController)
        {
        }

        protected override void Equip()
        {
            _equipped = true;
        }

        protected override void UnEquip()
        {
            _equipped = true;
        }
    }
}
