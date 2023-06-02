﻿using Assets.Scripts.Items.Enums;
using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Weapon : Equipment
    {
        public Weapon(ItemDescriptor descriptor, StatsController statsController, 
            InventoryEquipmentSlotType equipmentType) 
            : base(descriptor, statsController, equipmentType)
        {
        }

        protected override void Equip()
        {
            _equipped = true;
        }

        protected override void UnEquip()
        {
            _equipped = false;
        }
    }
}
