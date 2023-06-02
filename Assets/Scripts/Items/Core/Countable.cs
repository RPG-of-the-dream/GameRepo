using System;
using Assets.Scripts.Items.Enums;
using Items.Data;
using StatsSystem;
using UnityEngine;

namespace Assets.Scripts.Items.Core
{
    public abstract class Countable : Equipment
    {
        private int _quantity;
        protected Countable(ItemDescriptor descriptor, StatsController statsController) 
            : base(descriptor, statsController)
        {
            _quantity = 1;
        }

        public override int Quantity => _quantity;

        public void Stuck()
        {
            _quantity++;
        }
        public override void Use()
        {
            
        }

        public void Eat()
        {
            _quantity--;
        }
    }
}
