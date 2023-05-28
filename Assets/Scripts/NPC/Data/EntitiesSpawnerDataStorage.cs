using System.Collections.Generic;
using UnityEngine;

namespace NPC.Data
{
    [CreateAssetMenu(fileName = nameof(EntitiesSpawnerDataStorage), menuName = "EntitiesSpawner/EntitySpawnerDataStorage")]
    public class EntitiesSpawnerDataStorage : ScriptableObject
    {
        [field: SerializeField] public List<EntityDataStorage> EntitiesSpawnData { get; private set; }
    }
}