﻿using Assets.Scripts.Items.Enums;
using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Currency : Countable
    {
        public Currency(ItemDescriptor descriptor, StatsController statsController,
            InventoryEquipmentSlotType equipmentType) 
            : base(descriptor, statsController, equipmentType)
        {
        }
    }
}
