using System;
using Items.Data;
using StatsSystem;

namespace Assets.Scripts.Items.Core
{
    public class Food : Item
    {
        private int _amount;
        public Food(ItemDescriptor descriptor) 
            : base(descriptor)
        {
            _amount = 1;
        }

        public override int Amount => _amount;
        public override void Use()
        {
            _amount--;
            if (_amount <= 0)
                Destroy();
        }

        private void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
