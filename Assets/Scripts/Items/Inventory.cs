using System.Collections.Generic;
using Assets.Scripts.Items.Core;

namespace Assets.Scripts.Items
{
    public class Inventory
    {
        public const int InventorySize = 24;
        public List<Item> BackPackItems { get; }
        public List<Equipment> Equipments { get; }

        public Inventory(List<Item> backpackItems, List<Equipment> equipments)
        {
            Equipments = equipments ?? new List<Equipment>();
            BackPackItems = backpackItems ?? new List<Item>();
        }
    }
}