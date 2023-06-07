using Items.Enums;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Assets.Scripts.Items.Storage
{
    [CreateAssetMenu(fileName = nameof(EquipmentSpritesStorage), menuName = ("ItemsSystem/EquipmentSpritesStorage"))]
    public class EquipmentSpritesStorage : ScriptableObject
    {
        [SerializeField] private List<OneTypeEquipmentSpritesStorage> _equipmentStorages;

        public List<Sprite> GetEquipmentSprites(ItemId itemId)
        {
            var storage = _equipmentStorages.Find(element => element.ItemType == itemId.GetItemType());
            if (storage == null)
                throw new InvalidCastException($"Item with id {itemId} is not equipment");

            var equipmentSprites = storage.EquipmentSprites.Find(element => element.ItemId == itemId);
            if (equipmentSprites == null)
                throw new NullReferenceException($"Storage does not contain data for item with id {itemId}");

            return equipmentSprites.Sprites;
        }
    }
}
