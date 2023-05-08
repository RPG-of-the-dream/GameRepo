using Items.Data;

namespace Assets.Scripts.Items.Core
{
    public abstract class Item
    {
        public ItemDescriptor Descriptor { get; }
        public abstract int Quantity { get; }
        protected Item(ItemDescriptor descriptor) => Descriptor = descriptor;
        public abstract void Use();
    }
}
