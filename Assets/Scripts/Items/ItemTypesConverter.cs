using Assets.Scripts.Items.Core;
using Assets.Scripts.Items.Enums;
using Items.Data;
using Items.Enums;
using System;

namespace Assets.Scripts.Items
{
    public static class ItemTypesConverter
    {
        private const int MaxOneHandWeaponIndex = 20;
        private const int MaxTwoHandsWeaponIndex = 40;
        private const int MaxArmorIndex = 60;
        private const int MaxHelmetIndex = 80;
        private const int MaxBootsIndex = 100;
        private const int MaxShieldIndex = 120;
        private const int MaxCurrencyIndex = 140;
        private const int MaxPotionIndex = 160;
        private const int MaxFoodIndex = 180;
        private const int MaxAccessoryIndex = 200;
        private const int MaxBowIndex = 220;

        public static ItemType GetItemType(this Item item) => item.Descriptor.ItemId.GetItemType();

        public static ItemType GetItemType(this ItemId itemId)
        {
            return (int)itemId switch
            {
                <= MaxOneHandWeaponIndex => ItemType.OneHandedWeapon,
                <= MaxTwoHandsWeaponIndex => ItemType.TwoHandedWeapon,
                <= MaxArmorIndex => ItemType.Breastplate,
                <= MaxHelmetIndex => ItemType.Helmet,
                <= MaxBootsIndex => ItemType.Boots,
                <= MaxShieldIndex => ItemType.Shield,
                <= MaxCurrencyIndex => ItemType.Currency,
                <= MaxPotionIndex => ItemType.Potion,
                <= MaxFoodIndex => ItemType.Food,
                <= MaxAccessoryIndex => ItemType.Accessory,
                <= MaxBowIndex => ItemType.Bow,
                _ => ItemType.None
            };
        }

        public static bool IsItemTypeEqual(this ItemId itemId, ItemId otherItemId)
            => itemId.GetItemType() == otherItemId.GetItemType();

        public static bool IsItemTypeEqual(this ItemId itemId, ItemType itemType)
            => itemId.GetItemType() == itemType;

        public static bool IsWeapon(this Item item)
            => item.GetItemType().IsWeapon();

        public static bool IsWeapon(this ItemId itemId)
            => itemId.GetItemType().IsWeapon();

        public static bool IsWeapon(this ItemType itemType)
            => itemType == ItemType.OneHandedWeapon || itemType == ItemType.TwoHandedWeapon || itemType == ItemType.Bow;

        public static InventoryEquipmentSlotType GetItemInventorySlotType(this Item item)
        {
            switch (item.GetItemType())
            {
                case ItemType.OneHandedWeapon:
                    return InventoryEquipmentSlotType.OneHand;
                case ItemType.TwoHandedWeapon:
                case ItemType.Bow:
                    return InventoryEquipmentSlotType.TwoHands;
                case ItemType.Helmet:
                    return InventoryEquipmentSlotType.Helmet;
                case ItemType.Breastplate:
                    return InventoryEquipmentSlotType.Breastplate;
                case ItemType.Boots:
                    return InventoryEquipmentSlotType.Boots;
                case ItemType.Shield:
                    return InventoryEquipmentSlotType.Shield;
                case ItemType.Potion:
                    return InventoryEquipmentSlotType.Potion;
                case ItemType.Accessory:
                    {
                        var accessoryType = item.Descriptor as AccessoryDescriptor;
                        switch (accessoryType!.AccessoryType)
                        {
                            case AccessoryType.Necklace:
                                return InventoryEquipmentSlotType.Necklace;
                            case AccessoryType.Ring:
                                return InventoryEquipmentSlotType.Ring;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                case ItemType.Food:
                    return InventoryEquipmentSlotType.Food;
                case ItemType.Currency:
                    return InventoryEquipmentSlotType.Currency;
                default:
                    return InventoryEquipmentSlotType.None;
            }
        }

        public static bool IsEquipmentSlotEqual(this Item item, InventoryEquipmentSlotType slotType) =>
            item.GetItemInventorySlotType() == slotType;

        public static bool IsEquipmentSlotEqual(this Item item, Item otherItem) =>
            item.GetItemInventorySlotType() == otherItem.GetItemInventorySlotType();
    }
}
