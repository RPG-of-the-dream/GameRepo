using System;
using Items.Enums;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class ItemDescriptor
    {
        [field: SerializeField] public ItemId ItemId { get; private set; }
        [field: SerializeField] public Sprite ItemSprite { get; private set; }
        [field: SerializeField] public ItemRarity ItemRarity { get; private set; }
        [field: SerializeField] public float Price { get; private set; }

        public ItemDescriptor(ItemId itemId, Sprite itemSprite, ItemRarity itemRarity, float price)
        {
            ItemId = itemId;
            ItemSprite = itemSprite;
            ItemRarity = itemRarity;
            Price = price;
        }
    }
}