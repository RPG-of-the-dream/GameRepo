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
            switch (descriptor.ItemId.GetItemType())
            {
                case ItemType.OneHandedWeapon:
                case ItemType.TwoHandedWeapon:
                    return new Weapon(descriptor, _statsController);
                case ItemType.Helmet:
                case ItemType.Breastplate:
                case ItemType.Boots:
                case ItemType.Shield:
                    return new Armor(descriptor, _statsController);
                case ItemType.Potion:
                    return new Potion(descriptor, _statsController);
                case ItemType.Accessory:
                    return new Accessory(descriptor, _statsController);
                case ItemType.Food:
                    return new Food(descriptor, _statsController);
                case ItemType.Currency:
                    return new Currency(descriptor, _statsController);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
