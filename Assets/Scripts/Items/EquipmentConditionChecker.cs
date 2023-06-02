using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items.Core;
using Assets.Scripts.Items.Enums;

namespace Assets.Scripts.Items
{
    public class EquipmentConditionChecker
    {
        public bool IsEquipmentConditionFit(Equipment equipment, List<Equipment> currentEquipment)
        {
            return true;   
        }

        public bool TryReplaceEquipment(Equipment equipment, List<Equipment> currentEquipment,
            out Equipment oldEquipment)
        {
            oldEquipment = currentEquipment.Find(slot => slot.Type == equipment.Type);
            if (oldEquipment != null)
                return true;

            switch (equipment.Type)
            {
                              
                
                case InventoryEquipmentSlotType.TwoHands:
                    {
                        var oneHand = currentEquipment.Find(slot => slot.Type == InventoryEquipmentSlotType.OneHand);
                        var shield = currentEquipment.Find(slot => slot.Type == InventoryEquipmentSlotType.Shield);
                        if (oneHand != null && shield != null)
                            return false;

                        oldEquipment = oneHand ?? shield;
                        return true;
                    }
                case InventoryEquipmentSlotType.OneHand:
                case InventoryEquipmentSlotType.Shield:
                    oldEquipment = currentEquipment.Find(slot => slot.Type == InventoryEquipmentSlotType.TwoHands);
                    return true;
                case InventoryEquipmentSlotType.Helmet:
                case InventoryEquipmentSlotType.Breastplate:
                case InventoryEquipmentSlotType.Boots:

                case InventoryEquipmentSlotType.Currency:
                case InventoryEquipmentSlotType.Food:
                case InventoryEquipmentSlotType.Potion:
                case InventoryEquipmentSlotType.Necklace:
                case InventoryEquipmentSlotType.Ring:
                    return true;
                case InventoryEquipmentSlotType.None:
                default:
                    throw new NullReferenceException($"Equipment type of item " +
                        $"{equipment.Descriptor.ItemId} is not available for equipping");
            }
        } 
    }
}
