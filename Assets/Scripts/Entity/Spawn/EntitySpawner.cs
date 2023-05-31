using System;
using System.Collections.Generic;
using Drawing;
using Entity.Controller;
using Entity.Data;
using Entity.Enums;
using UnityEngine;

namespace Entity.Spawn
{
    public class EntitySpawner : IDisposable
    {
        private readonly LevelDrawer _levelDrawer;
        private readonly List<BaseEntity> _entities;
        private readonly EntitiesFactory _entitiesFactory;

        public EntitySpawner(LevelDrawer levelDrawer)
        {
            _levelDrawer = levelDrawer;
            _entities = new List<BaseEntity>();
            var entitiesSpawnerDataStorage = Resources.Load<EntitiesSpawnerDataStorage>($"{nameof(EntitySpawner)}/{nameof(EntitiesSpawnerDataStorage)}");
            _entitiesFactory = new EntitiesFactory(entitiesSpawnerDataStorage);
        }

        public void SpawnEntity(EntityId entityId, Vector2 position)
        {
            var entity = _entitiesFactory.GetEntityBrain(entityId, position);
            entity.Died += RemoveEntity;
            _levelDrawer.RegisterElement(entity);
            _entities.Add(entity);
        }

        public void Dispose()
        {
            foreach(var entity in _entities)
                DestroyEntity(entity);
            _entities.Clear();
        }

        private void RemoveEntity(BaseEntity baseEntity)
        {
            _entities.Remove(baseEntity);
            DestroyEntity(baseEntity);
        }

        private void DestroyEntity(BaseEntity baseEntity)
        {
            _levelDrawer.UnregisterElement(baseEntity);
            baseEntity.Died -= RemoveEntity;
        }
    }
}