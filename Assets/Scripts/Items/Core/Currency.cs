using Items.Data;

namespace Assets.Scripts.Items.Core
{
    public class Currency : Countable
    {
        public Currency(ItemDescriptor descriptor) 
            : base(descriptor)
        {
        }
    }
}
