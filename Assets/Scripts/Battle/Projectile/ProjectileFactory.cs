using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Battle.Projectile.Data;
using Battle.Projectile.Behaviour;
using Battle.Projectile.Data;
using Battle.Projectile.Enum;
using Drawing;
using Items.Enums;
using UnityEngine;

namespace Battle.Projectile
{
    public class ProjectileFactory
    {
        private readonly ProjectilesStorage _projectilesStorage;
        private readonly Dictionary<ProjectileType, List<ProjectileBase>> _freeProjectiles;
        private readonly Dictionary<ProjectileType, List<ProjectileBase>> _projectilesInUse;
        private readonly LevelDrawer _levelDrawer;

        public ProjectileFactory(LevelDrawer levelDrawer)
        {
            _projectilesStorage = Resources.Load<ProjectilesStorage>("Battle/ProjectilesStorage");
            _freeProjectiles = new Dictionary<ProjectileType, List<ProjectileBase>>();
            _projectilesInUse = new Dictionary<ProjectileType, List<ProjectileBase>>();
            _levelDrawer = levelDrawer;
        }

        public ProjectileBase GetProjectile(ItemId weaponId)
        {
            ProjectileData data = _projectilesStorage.ProjectilesData.Find(element => element.SuitableItems.Contains(weaponId));
            if (data == null)
            {
                Debug.LogError($"{nameof(ProjectilesStorage)} does not contain projectile for bow with id {weaponId}");
                data = _projectilesStorage.ProjectilesData.First();
            }

            if (!_freeProjectiles.TryGetValue(data.ProjectileType, out List<ProjectileBase> projectiles))
            {
                projectiles = new List<ProjectileBase>();
                _freeProjectiles.Add(data.ProjectileType, projectiles);
            }

            ProjectileBase projectile;
            if (projectiles.Count < 1)
                projectile = Object.Instantiate(data.Projectile);
            else
            {
                projectile = projectiles[0];
                projectiles.RemoveAt(0);
            }

            projectile.Attacked += OnAttacked;

            if (!_projectilesInUse.TryGetValue(data.ProjectileType, out List<ProjectileBase> projectilesInUse))
            {
                projectilesInUse = new List<ProjectileBase>();
                _projectilesInUse.Add(data.ProjectileType, projectilesInUse);
            }

            projectilesInUse.Add(projectile);
            _levelDrawer.RegisterElement(projectile);
            return projectile;
        }

        private void OnAttacked(ProjectileBase projectile)
        {
            var projectileType = _projectilesInUse
                .FirstOrDefault(element => element.Value.Contains(projectile)).Key;

            _projectilesInUse[projectileType].Remove(projectile);
            _freeProjectiles[projectileType].Add(projectile);
            projectile.Attacked -= OnAttacked;
            projectile.gameObject.SetActive(false);
            _levelDrawer.UnregisterElement(projectile);
        }
    }
}
