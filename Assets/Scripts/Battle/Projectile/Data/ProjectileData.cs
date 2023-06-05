using System;
using System.Collections.Generic;
using Battle.Projectile.Behaviour;
using Battle.Projectile.Enum;
using Items.Enums;
using UnityEngine;

namespace Battle.Projectile.Data
{
    [Serializable]
    public class ProjectileData
    {
        [field: SerializeField] public ProjectileType ProjectileType { get; private set; }
        [field: SerializeField] public ProjectileBase Projectile { get; private set; }
        [field: SerializeField] public List<ItemId> SuitableItems { get; private set; }
    }
}
