using System;
using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Food : Countable
    {
        public Food(ItemDescriptor descriptor) 
            : base(descriptor)
        {
        }
    }
}
