using System;
using System.Collections.Generic;
using Assets.Scripts.Items.Core;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Inventory
    {
        private readonly Transform _player;

        public const int InventorySize = 24;
        public List<Item> BackPackItems { get; }
        public List<Equipment> Equipments { get; }

        public event Action BackpackChanged;
        public event Action EquipmentChanged;
        public event Action<Item, Vector2> ItemDropped;

        public Inventory(List<Item> backpackItems, List<Equipment> equipments, Transform player)
        {
            _player = player;
            Equipments = equipments ?? new List<Equipment>();

            if (backpackItems is not null)
                return;

            BackPackItems = new List<Item>();
            for(var i = 0; i < InventorySize; i++)
                BackPackItems.Add(null);
        }

        public void AddItemToBackPack(Item item)
        {
            var index = BackPackItems.FindIndex(element => element == null);
            BackPackItems[index] = item;
            BackpackChanged?.Invoke();
        }

        public void RemoveFromBackPack(Item item, bool toWorld)
        {
            var index = BackPackItems.IndexOf(item);
            BackPackItems[index] = null;
            BackpackChanged?.Invoke();
            if (toWorld)
                ItemDropped?.Invoke(item, _player.position);
        }

        public void UnEquip(Equipment equipment, bool toWorld)
        {
            Equipment.Remove(equipment);
            EquipmentChanged?.Invoke();

            if (toWorld)
                ItemDropped?.Invoke(equipment, _player.position);
        }
    }
}