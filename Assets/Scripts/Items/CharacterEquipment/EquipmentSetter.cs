using Assets.Scripts.Items.Storage;
using Items.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items.CharacterEquipment
{
    public class EquipmentSetter
    {
        private readonly CharacterEquipment _characterEquipment;
        private readonly EquipmentSpritesStorage _equipmentSpritesStorage;
        private readonly Dictionary<ItemType, ItemId> _equippedItems;

        public EquipmentSetter(CharacterEquipment characterEquipment)
        {
            _characterEquipment = characterEquipment;
            _equipmentSpritesStorage = Resources.Load<EquipmentSpritesStorage>("Player/EquipmentSpritesStorage");
            _equippedItems = new Dictionary<ItemType, ItemId>();
            foreach (var slot in _characterEquipment.EquipmentSlots)
                _equippedItems.Add(slot.EquipmentType, ItemId.None);
        }

        public void UpdateEquipment(List<ItemId> items)
        {
            var equipmentItems = _equippedItems.Keys.ToList();
            for (var i = 0; i < equipmentItems.Count; i++)
            {
                var item = items.Find(element => element.GetItemType() == equipmentItems[i]);
                if (item == ItemId.None)
                {
                    UnEquip(equipmentItems[i]);
                    continue;
                }

                Equip(item);
            }
        }

        private void Equip(ItemId itemId)
        {
            var itemType = itemId.GetItemType();

            if (itemId == _equippedItems[itemType])
                return;

            if (_equippedItems[itemType] != ItemId.None)
                UnEquip(itemType);

            _equippedItems[itemType] = itemId;
            var sprites = _equipmentSpritesStorage.GetEquipmentSprites(itemId);
            var slot = _characterEquipment.EquipmentSlots.Find(element => element.EquipmentType == itemId.GetItemType());
            Debug.Log($"Item {slot.EquipmentType} is equipped");
            if (slot.Renderers.Count == 1 && sprites.Count == 1)
            {
                slot.Renderers[0].sprite = sprites[0];
                return;
            }

            //foreach (var sprite in sprites)
            //{
            //    var renderers = slot.Renderers.
            //        Where(element => element.name.Contains(sprite.name));
            //    foreach (var renderer in renderers)
            //        renderer.sprite = sprite;
            //}
        }

        private void UnEquip(ItemType itemType)
        {
            if (_equippedItems[itemType] == ItemId.None)
                return;


            _equippedItems[itemType] = ItemId.None;
            var slot = _characterEquipment.EquipmentSlots.Find(element => element.EquipmentType == itemType);
            Debug.Log($"Item {slot.EquipmentType} is unequipped");
            foreach (var renderer in slot.Renderers)
                renderer.sprite = null;
        }
    }
}
