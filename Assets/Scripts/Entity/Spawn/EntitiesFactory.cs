using System;
using System.Linq;
using Entity.Behaviour;
using Entity.Controller;
using Entity.Data;
using Entity.Enums;
using StatsSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Entity.Spawn
{
    public class EntitiesFactory
    {
        private readonly Transform _entitiesContainer;
        private readonly EntitiesSpawnerDataStorage _entitiesSpawnerDataStorage;

        public EntitiesFactory(EntitiesSpawnerDataStorage entitiesSpawnerDataStorage)
        {
            _entitiesSpawnerDataStorage = entitiesSpawnerDataStorage;
            var gameObject = new GameObject
            {
                name = nameof(EntitySpawner)
            };
            _entitiesContainer = gameObject.transform;
        }

        public BaseEntity GetEntityBrain(EntityId entityId, Vector2 position)
        {
            var data = _entitiesSpawnerDataStorage.EntitiesSpawnData.Find(element => element.Id == entityId);
            var baseEntityBehaviour = Object.Instantiate(data.EntityBehaviourPrefab, position, Quaternion.identity);
            baseEntityBehaviour.transform.SetParent(_entitiesContainer);
            var stats = data.Stats.Select(stat => stat.GetCopy()).ToList();
            var statsController = new StatsController(stats);
            switch (entityId)
            {
                case EntityId.Warhog:
                    return new MeleeEntity(baseEntityBehaviour as MeleeEntityBehaviour, statsController);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}