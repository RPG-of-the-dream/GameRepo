using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items;
using Assets.Scripts.Items.Core;
using Assets.Scripts.Items.Enums;
using Items.Data;
using Items.Enums;
using UI.InventoryUI.Elements;
using UnityEngine;

namespace UI.InventoryUI
{
    public class InventoryScreenPresenter: ScreenController<InventoryScreenView>
    {
        private readonly Inventory _inventory;
        private readonly List<RarityDescriptor> _rarityDescriptors;

        private readonly Dictionary<ItemSlot, Item> _backPackSlots;
        private readonly Dictionary<EquipmentSlot, Equipment> _equipmentSlots;

        private readonly Sprite _emptyBackSprite;

        public InventoryScreenPresenter(InventoryScreenView view, Inventory inventory, List<RarityDescriptor> rarityDescriptors) 
            : base(view)
        {
            _inventory = inventory;
            _rarityDescriptors = rarityDescriptors;
            _emptyBackSprite =
                _rarityDescriptors.Find(descriptor => descriptor.ItemRarity == ItemRarity.None).Sprite;
            _equipmentSlots = new Dictionary<EquipmentSlot, Equipment>();
            _backPackSlots = new Dictionary<ItemSlot, Item>();
            View.CloseClicked += RequestClose;
        }

        public override void Initialize()
        {
            InitializeBackPack();
            InitializeEquipment();
            _inventory.BackpackChanged += UpdateBackPack;
            _inventory.EquipmentChanged += UpdateEquipment;
            base.Initialize();
        }

        public override void Complete()
        {
            ClearBackPack();
            ClearEquipment();
            _inventory.BackpackChanged -= UpdateBackPack;
            _inventory.EquipmentChanged -= UpdateEquipment;
            base.Complete();
        }
        
        private void InitializeBackPack()
        {
            var backPack = View.ItemSlots;
            var numberOfItems = _inventory.BackPackItems.Count;
            for (int i = 0; i < Inventory.InventorySize; i++)
            {
                var slot = backPack[i];
                Item item = null;
                if (numberOfItems > i)
                {
                    item = _inventory.BackPackItems[i];
                }
                _backPackSlots.Add(slot, item);

                if (item == null) 
                    continue;
                
                slot.SetItem(item.Descriptor.ItemSprite, GetBackSprite(item.Descriptor.ItemRarity), item.Amount);
                SubscribeToSlotEvents(slot);
            }
        }

        private void InitializeEquipment()
        {
            var equipment = View.EquipmentSlots;
            foreach (var slot in equipment)
            {
                var item = _inventory.Equipments.Find(equip => equip.Type == slot.EquipmentType);
                if(item == null && slot.EquipmentType == EquipmentType.Shield)
                    item = _inventory.Equipments.Find(equip => equip.Type == EquipmentType.TwoHands);

                _equipmentSlots.Add(slot, item);

                if (slot.EquipmentType == EquipmentType.OneHand)
                {
                    var twoHandWeapon = _inventory.Equipments.Find(equip => equip.Type == EquipmentType.TwoHands);
                    if (twoHandWeapon != null)
                    {
                        slot.SetAfterImage(twoHandWeapon.Descriptor.ItemSprite,
                            GetBackSprite(twoHandWeapon.Descriptor.ItemRarity));
                        continue;
                    }
                }

                if (item == null) 
                    continue;

                slot.SetItem(item.Descriptor.ItemSprite, GetBackSprite(item.Descriptor.ItemRarity), item.Amount);
                SubscribeToSlotEvents(slot);
            }
        }

        private Sprite GetBackSprite(ItemRarity rarity) =>
            _rarityDescriptors.Find(descriptor => descriptor.ItemRarity == rarity).Sprite;

        private void ClearBackPack()
        {
            ClearSlots(_backPackSlots.Select(item => item.Key).ToList());
            _backPackSlots.Clear();
        }

        private void ClearEquipment()
        {
            ClearSlots(_equipmentSlots.Select(item => item.Key).Cast<ItemSlot>().ToList());
            _equipmentSlots.Clear();
        }

        private void ClearSlots(List<ItemSlot> slots)
        {
            foreach (var slot in slots)
            {
                slot.RemoveItem(_emptyBackSprite);
                slot.SlotClicked -= UseSlot;
                slot.SlotClearClicked -= ClearSlot;
            }
        }

        private void SubscribeToSlotEvents(ItemSlot slot)
        {
            slot.SlotClicked += UseSlot;
            slot.SlotClearClicked += ClearSlot;
        }

        private void UpdateBackPack()
        {
            ClearBackPack();
            InitializeBackPack();
        }

        private void UpdateEquipment()
        {
            ClearEquipment();
            InitializeEquipment();
        }

        private void UseSlot(ItemSlot slot)
        {
            throw new NotImplementedException();
        }

        private void ClearSlot(ItemSlot slot)
        {
            if (_backPackSlots.TryGetValue(slot, out var item))
                _inventory.RemoveFromBackPack(item, true);

            if (slot is EquipmentSlot equipmentSlot
                && _equipmentSlots.TryGetValue(equipmentSlot, out var equipment))
                _inventory.UnEquip(equipment, true);
        }
    }
}