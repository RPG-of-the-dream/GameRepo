using Assets.Scripts.Items.CharacterEquipment;
using Items.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items.Storage
{
    [CreateAssetMenu(fileName = nameof(OneTypeEquipmentSpritesStorage), menuName = ("ItemsSystem/OneTypeEquipmentSpritesStorage"))]
    public class OneTypeEquipmentSpritesStorage : ScriptableObject
    {
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public List<EquipmentSprites> EquipmentSprites { get; private set; }
    }
}

