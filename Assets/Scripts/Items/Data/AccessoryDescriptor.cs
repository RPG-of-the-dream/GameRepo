using System;
using Items.Enums;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class AccessoryDescriptor : ItemDescriptor
    {
        [field: SerializeField] public AccessoryType AccessoryType { get; private set; }
        
        public AccessoryDescriptor(AccessoryType accessoryType, ItemId itemId, ItemType type, Sprite itemSprite, ItemRarity itemRarity, float price) : 
            base(itemId, type, itemSprite, itemRarity, price)
        {
            AccessoryType = accessoryType;
        }
    }
}