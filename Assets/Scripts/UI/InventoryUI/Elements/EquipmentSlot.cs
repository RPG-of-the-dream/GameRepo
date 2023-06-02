using Assets.Scripts.Items.Enums;
using UnityEngine;

namespace UI.InventoryUI.Elements
{
    public class EquipmentSlot : ItemSlot
    {
        [field: SerializeField] public InventoryEquipmentSlotType EquipmentType { get; private set; }

        public void SetAfterImage(Sprite sprite, Sprite backSprite)
        {
            SetItem(sprite, backSprite, -1);
            RemoveButton.gameObject.SetActive(false);
        }
    }
}