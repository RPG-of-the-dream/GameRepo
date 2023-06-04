using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using Items.Data;
using Items.Enums;
using Player;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class DropGenerator
    {
        private readonly PlayerEntityBehaviour _playerEntity;
        private readonly List<ItemDescriptor> _itemDescriptors;
        private readonly ItemsSystem _itemsSystem;

        

        public DropGenerator(PlayerEntityBehaviour playerEntity, List<ItemDescriptor> itemDescriptors, ItemsSystem itemsSystem)
        {
            _playerEntity = playerEntity;
            _itemDescriptors = itemDescriptors;
            _itemsSystem = itemsSystem;
            ProjectUpdater.Instance.UpdateCalled += Update;
        }

        public bool DropRandomItem(ItemRarity rarity)
        {
            var items = _itemDescriptors
                .Where(item => item.ItemRarity == rarity)
                .ToList();
            var numberOfPossibleItems = items.Count;
            if (numberOfPossibleItems == 0)
            {
                return false;
            }
            var itemDescriptor = items[Random.Range(0, numberOfPossibleItems)];
            _itemsSystem.DropItem(itemDescriptor, (Vector2)_playerEntity.transform.position + Vector2.one);
            return true;
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

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                DropRandomItem(ItemRarity.Epic);
                DropRandomItem(ItemRarity.Common);
            }
                
                //DropRandomItem(GetItemRarity());
        }
    }
}
