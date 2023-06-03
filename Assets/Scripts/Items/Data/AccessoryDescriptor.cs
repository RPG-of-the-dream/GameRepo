using System;
using Items.Enums;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class AccessoryDescriptor : ItemDescriptor
    {
        [field: SerializeField] public AccessoryType AccessoryType { get; private set; }
        
        public AccessoryDescriptor(AccessoryType accessoryType, ItemId itemId, Sprite itemSprite, ItemRarity itemRarity, float price) : 
            base(itemId, itemSprite, itemRarity, price)
        {
            AccessoryType = accessoryType;
        }
    }
}