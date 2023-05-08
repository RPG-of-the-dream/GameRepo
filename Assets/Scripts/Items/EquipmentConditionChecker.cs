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
                              
                
                case EquipmentType.TwoHands:
                    {
                        var oneHand = currentEquipment.Find(slot => slot.Type == EquipmentType.OneHand);
                        var shield = currentEquipment.Find(slot => slot.Type == EquipmentType.Shield);
                        if (oneHand != null && shield != null)
                            return false;

                        oldEquipment = oneHand ?? shield;
                        return true;
                    }
                case EquipmentType.OneHand:
                case EquipmentType.Shield:
                    oldEquipment = currentEquipment.Find(slot => slot.Type == EquipmentType.TwoHands);
                    return true;
                case EquipmentType.Helmet:
                case EquipmentType.Breastplate:
                case EquipmentType.Boots:

                case EquipmentType.Currency:
                case EquipmentType.Food:
                case EquipmentType.Potion:
                case EquipmentType.Accessory:
                    return true;
                case EquipmentType.None:
                default:
                    throw new NullReferenceException($"Equipment type of item " +
                        $"{equipment.Descriptor.ItemId} is not available for equipping");
            }
        } 
    }
}
