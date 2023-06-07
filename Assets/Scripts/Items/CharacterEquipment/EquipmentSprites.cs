using Items.Enums;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Assets.Scripts.Items.CharacterEquipment
{
    [Serializable]
    public class EquipmentSprites
    {
        [field: SerializeField] public ItemId ItemId { get; private set; }
        [field: SerializeField] public List<Sprite> Sprites { get; private set; }
    }
}
