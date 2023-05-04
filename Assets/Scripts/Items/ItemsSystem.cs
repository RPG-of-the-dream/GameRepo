using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items.Core;
using Items.Behaviour;
using Items.Rarity;
using Items.Data;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemsSystem
    {
        private readonly SceneItem _sceneItem;
        private readonly Transform _transform;
        private readonly List<IItemRarityColor> _colors;
        private readonly Dictionary<SceneItem, Item> _itemsOnScene;
        private readonly LayerMask _whatIsPlayer;
        private readonly ItemsFactory _itemsFactory;
        private readonly Inventory _inventory;

        public ItemsSystem(List<IItemRarityColor> colors, ItemsFactory itemsFactory, LayerMask whatIsPLayer, Inventory inventory)
        {
            _sceneItem = Resources.Load<SceneItem>($"{nameof(ItemsSystem)}/{nameof(SceneItem)}");
            _itemsOnScene = new Dictionary<SceneItem, Item>();
            GameObject gameObject = new GameObject();
            _transform = gameObject.transform;
            _colors = colors;
            _whatIsPlayer = whatIsPLayer;
            _itemsFactory = itemsFactory;
            _inventory = inventory;
            _inventory.ItemDropped += DropItem;
        }

        public void DropItem(ItemDescriptor descriptor, Vector2 position) =>
            DropItem(_itemsFactory.CreateItem(descriptor), position);
        
        private void TryPickSceneItem(SceneItem sceneItem)
        {
            Collider2D player =
                Physics2D.OverlapCircle(sceneItem.Position, sceneItem.InteractionDistance, _whatIsPlayer);
            if (player == null)
                return;

            Item item = _itemsOnScene[sceneItem];
            if(_inventory.BackPackItems.All(item => item is not null))
                return;

            _inventory.AddItemToBackPack(item);
            _itemsOnScene.Remove(sceneItem);
            sceneItem.ItemClicked -= TryPickSceneItem;
            Object.Destroy(sceneItem.gameObject);
        }
        
        private void DropItem(Item item, Vector2 position)
        {
            SceneItem sceneItem = Object.Instantiate(_sceneItem, _transform);
            sceneItem.SetItem(item.Descriptor.ItemSprite, item.Descriptor.ItemId.ToString(),
                _colors.Find(color => color.ItemRarity == item.Descriptor.ItemRarity).Color);
            sceneItem.PlayDrop(position);
            sceneItem.ItemClicked += TryPickSceneItem;
            _itemsOnScene.Add(sceneItem, item);
        }
    }
}
