﻿using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Weapon : Equipment
    {
        public Weapon(ItemDescriptor descriptor, StatsController statsController) 
            : base(descriptor, statsController)
        {
        }

        public override void Equip()
        {
            _equipped = true;
        }

        public override void UnEquip()
        {
            _equipped = false;
        }
    }
}
