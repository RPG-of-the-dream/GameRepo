using System;
using Assets.Scripts.Items.Core;
using Assets.Scripts.Items.Enums;
using Items.Data;
using Items.Enums;
using StatsSystem;

namespace Assets.Scripts.Items
{
    public class ItemsFactory
    {
        private readonly StatsController _statsController;

        public ItemsFactory(StatsController statsController)
        {
            _statsController = statsController;
        }

        public Item CreateItem(ItemDescriptor descriptor)
        {
            switch (descriptor.Type)
            {
                case ItemType.Weapon:
                    return new Weapon(descriptor, _statsController, GetEquipmentType(descriptor));
                case ItemType.Helmet:
                case ItemType.Breastplate:
                case ItemType.Boots:
                case ItemType.Shield:
                    return new Armor(descriptor, _statsController, GetEquipmentType(descriptor));
                case ItemType.Potion:
                    return new Potion(descriptor, _statsController, GetEquipmentType(descriptor));
                case ItemType.Accessory:
                    return new Accessory(descriptor, _statsController, GetEquipmentType(descriptor));
                case ItemType.Food:
                    return new Food(descriptor, _statsController, GetEquipmentType(descriptor));
                case ItemType.Currency:
                    return new Currency(descriptor, _statsController, GetEquipmentType(descriptor));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public EquipmentType GetEquipmentType(ItemDescriptor descriptor)
        {
            switch (descriptor.Type)
            {
                case ItemType.Weapon:
                    var weaponDescriptor = descriptor as WeaponDescriptor;
                    switch (weaponDescriptor!.WeaponType)
                    {
                        case WeaponType.OneHand:
                            return EquipmentType.OneHand;
                        case WeaponType.TwoHands:
                            return EquipmentType.TwoHands;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case ItemType.Helmet:
                    return EquipmentType.Helmet;
                case ItemType.Breastplate:
                    return EquipmentType.Breastplate;
                case ItemType.Boots:
                    return EquipmentType.Boots;
                case ItemType.Shield:
                    return EquipmentType.Shield;
                case ItemType.Accessory:
                    var accessoryDescriptor = descriptor as AccessoryDescriptor;
                    switch (accessoryDescriptor!.AccessoryType)
                    {
                        case AccessoryType.Necklace:
                            return EquipmentType.Necklace;
                        case AccessoryType.Ring:
                            return EquipmentType.Ring;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case ItemType.Potion:
                    return EquipmentType.Potion;
                case ItemType.Food:
                    return EquipmentType.Food;
                case ItemType.Currency:
                    return EquipmentType.Currency;
                case ItemType.None:
                    return EquipmentType.None;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
