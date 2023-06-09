﻿using Assets.Scripts.Items.Enums;
using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Potion : Countable
    {
        public Potion(ItemDescriptor descriptor, StatsController statsController,
            EquipmentType equipmentType) 
            : base(descriptor, statsController, equipmentType)
        {
        }
    }
}
