using Items.Enums;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Assets.Scripts.Items.CharacterEquipment
{
    [Serializable]
    public class CharacterEquipmentSlot
    {
        [field: SerializeField] public ItemType EquipmentType { get; private set; }
        [field: SerializeField] public List<SpriteRenderer> Renderers { get; private set; }
    }
}
