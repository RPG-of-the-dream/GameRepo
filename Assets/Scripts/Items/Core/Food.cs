using System;
using Assets.Scripts.Items.Enums;
using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Food : Countable
    {
        public Food(ItemDescriptor descriptor, StatsController statsController) : base(descriptor, statsController)
        {
        }
    }
}
