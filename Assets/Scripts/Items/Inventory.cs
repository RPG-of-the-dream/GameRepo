using Assets.Scripts.Items.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Inventory
    {
        private readonly Transform _player;
        private readonly EquipmentConditionChecker _equipmentFitter;

        public const int InventorySize = 24;
        
        public List<Item> BackPackItems { get; }
        public List<Equipment> Equipments { get; }

        public event Action BackpackChanged;
        public event Action EquipmentChanged;
        public event Action<Item, Vector2> ItemDropped;

        public Inventory(List<Item> backpackItems, List<Equipment> equipments, Transform player, 
            EquipmentConditionChecker equipmentFitter)
        {
            _equipmentFitter = equipmentFitter; 
            _player = player;
            Equipments = new List<Equipment>();
            BackPackItems = new List<Item>();
            for(var i = 0; i < InventorySize; i++)
                BackPackItems.Add(null);
        }

        public bool TryAddToInventory(Item item)
        {
            if (item is Equipment equipment &&
                Equipments.All(equip => equip.Type != equipment.Type) &&
                TryEquip(equipment))
                return true;
            else
            {
                if (item is Countable countable)
                {
                    var count = Equipments.Find(co => co.Type == countable.Type) as Countable;
                    count?.Stuck();
                }
            }

            return TryAddToBackPack(item);
        }

        public bool TryEquip(Item item)
        {
            if (!(item is Equipment equipment))
                return false;

            if (!_equipmentFitter.IsEquipmentConditionFit(equipment, Equipments))
                return false;

            #region InventoryScreen
            if(!_equipmentFitter.TryReplaceEquipment(equipment, Equipments, out var oldEquipment))
                return false;

            if (oldEquipment != null)
                UnEquip(oldEquipment);

            if (BackPackItems.Contains(equipment))
            {
                var indexOfItem = BackPackItems.IndexOf(equipment);
                PlaceToBackPack(oldEquipment, indexOfItem);
            }
            else
                TryAddToBackPack(oldEquipment);
            #endregion

            Equipments.Add(equipment);
            equipment.Use();
            EquipmentChanged?.Invoke();
            return true;
        }

        private bool TryAddToBackPack(Item item)
        {
            if (BackPackItems.All(slot => slot != null))
                return false;

            if (item is Currency curr)
                if (Equipments.Any(c => c.Type == curr.Type))
                    return true;


            var index = BackPackItems.IndexOf(null);
            PlaceToBackPack(item, index);
            return true;
        }

        #region Model

        public void UseItem(Item item)
        {
            if (item is not Equipment equipment)
                return;


            if (Equipments.Contains(equipment))
            {
                if (equipment is Potion potion)
                {
                    potion.Eat();
                    if (potion.Quantity <= 0)
                    {
                        RemoveItem(item, false);
                        return;
                    }

                    else
                    {
                        item = BackPackItems.OfType<Potion>().LastOrDefault();
                        RemoveFromBackPack(item);
                    }
                    EquipmentChanged?.Invoke();
                    return;
                }
                else if (equipment is Food food)
                {
                    food.Eat();
                    if (food.Quantity <= 0)
                    {
                        RemoveItem(item, false);
                        return;
                    }

                    else
                    {
                        item = BackPackItems.OfType<Food>().LastOrDefault();
                        RemoveFromBackPack(item);
                    }

                    EquipmentChanged?.Invoke();
                    return;
                }
                if (TryAddToBackPack(equipment))
                    UnEquip(equipment);
                return;
            }

            if (!TryEquip(equipment))
                return;

            BackPackItems.Remove(item);
            BackpackChanged?.Invoke();
        }

        public void RemoveItem(Item item, bool toWorld)
        {
            if (item is Equipment equipment && Equipments.Contains(equipment))
                UnEquip(equipment);
            else
                RemoveFromBackPack(item);

            if (toWorld)
                ItemDropped?.Invoke(item, _player.position);
        }

        public void MoveItemToPositionInBackPack(Item item, int place)
        {
            if (item is Equipment equipment)
            {
                var backPackItem = BackPackItems[place];
                if (backPackItem != null)
                {
                    TryEquip(backPackItem);
                    return;
                }

                if (TryPlaceToBackPack(item, place))
                    UnEquip(equipment);

                return;
            }

            TryPlaceToBackPack(item, place);
        }

        public void UnEquip(Equipment equipment)
        {
            Equipments.Remove(equipment);
            equipment.Use();
            EquipmentChanged?.Invoke();
        }  
        
        private bool TryPlaceToBackPack(Item item, int index)
        {
            var oldItem = BackPackItems[index];
            if (BackPackItems.Contains(item))
            {
                var indexOfItem = BackPackItems.IndexOf(item);
                BackPackItems[indexOfItem] = oldItem;
            }
            else if (oldItem != null)
                return false;

            BackPackItems[index] = item;
            BackpackChanged?.Invoke();
            return true;
        }

        private void PlaceToBackPack(Item item, int index)
        {
            BackPackItems[index] = item;
            BackpackChanged?.Invoke();
        }

        private void RemoveFromBackPack(Item item)
        {
            var index = BackPackItems.IndexOf(item);
            BackPackItems[index] = null;
            BackpackChanged?.Invoke();
        }
        #endregion
    }
}