using Items.Data;
using UnityEngine;

namespace Items.Scriptable
{
    
    [CreateAssetMenu(fileName = "Accessory", menuName = "ItemsSystem/Accessory")]
    public class AccessoryScriptable : BaseItemScriptable
    {
        [SerializeField] private AccessoryDescriptor _accessoryDescriptor;
        public override ItemDescriptor ItemDescriptor => _accessoryDescriptor;
    }
}