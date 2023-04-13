using System.Collections.Generic;
using System.Linq;
using Items.Data;
using Items.Enums;
using Player;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class DropGenerator
    {
        private readonly PlayerEntity _playerEntity;
        private readonly List<ItemDescriptor> _itemDescriptors;
        private readonly ItemsSystem _itemsSystem;

        public DropGenerator(PlayerEntity playerEntity, List<ItemDescriptor> itemDescriptors, ItemsSystem itemsSystem)
        {
            _playerEntity = playerEntity;
            _itemDescriptors = itemDescriptors;
            _itemsSystem = itemsSystem;
        }

        public void DropRandomItem(ItemRarity rarity)
        {
            var items = _itemDescriptors
                .Where(item => item.ItemRarity == rarity)
                .ToList();
            var itemDescriptor = items[Random.Range(0, items.Count)];
            //_itemsSystem.DropItem(itemDescriptor, _playerEntity.transform.position + Vector3.one);
        }

        public ItemRarity GetItemRarity()
        {
            var chance = Random.Range(0, 100);
            return chance switch
            {
                <= 40 => ItemRarity.Trash,
                > 40 and <= 75 => ItemRarity.Common,
                > 75 and <= 90 => ItemRarity.Rare,
                > 90 and <= 97 => ItemRarity.Legendary,
                > 97 and <= 100 => ItemRarity.Epic,
                _ => ItemRarity.Trash,
            };
        }
    }
}
