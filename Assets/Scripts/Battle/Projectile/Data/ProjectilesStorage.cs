using System.Collections.Generic;
using Battle.Projectile.Data;
using UnityEngine;

namespace Assets.Scripts.Battle.Projectile.Data
{
    [CreateAssetMenu(fileName = "ProjectilesStorage", menuName = "Battle/ProjectilesStorage")]
    public class ProjectilesStorage : ScriptableObject
    {
        [field: SerializeField] public List<ProjectileData> ProjectilesData { get; private set; }
    }
}
