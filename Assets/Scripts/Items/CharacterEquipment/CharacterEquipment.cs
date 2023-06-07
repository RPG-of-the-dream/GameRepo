using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items.CharacterEquipment
{
    [Serializable]
    public class CharacterEquipment
    {
        [field: SerializeField] public List<CharacterEquipmentSlot> EquipmentSlots { get; private set; }
    }
}
