using System;
using Items.Data;

namespace Assets.Scripts.Items.Core
{
    public abstract class Countable : Item
    {
        private int _amount;
        protected Countable(ItemDescriptor descriptor) 
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
