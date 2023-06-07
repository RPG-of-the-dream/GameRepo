using Battle.Projectile;
using Battle.Projectile.Behaviour;
using Battle.Projectile.Enum;
using Core.Enums;
using Items.Enums;
using UnityEngine;

namespace Battle.Weapon
{
    public class RangeWeapon : WeaponBase
    {
        private readonly Sprite _projectileSprite;
        private readonly ProjectileType _projectileType;

        private readonly SpriteRenderer _shootArrow;
        private readonly Transform _shooterTransform;
        private readonly ProjectileFactory _projectileFactory;
        private readonly ItemId _itemId;

        public RangeWeapon(
            ProjectileFactory projectileFactory,
            SpriteRenderer shootArrow, 
            Transform shooterTransform,
            ItemId itemId)
        {
            _shootArrow = shootArrow;
            _shooterTransform = shooterTransform;
            _projectileFactory = projectileFactory;
            _itemId = itemId;
        }
        public override void Attack(float damage, Direction projectileDirection)
        {
            ProjectileBase projectile = _projectileFactory.GetProjectile(_itemId);
            projectile.transform.position = _shootArrow.transform.position;
            projectile.SetSprite(_shootArrow.sprite);
            projectile.gameObject.SetActive(true);
            projectile.Initialize(MapDirection(_shooterTransform, projectileDirection), damage);
        }

        public override void EndAttack() { }

        private static Vector2 MapDirection(Transform shooterTransform, Direction projectileDirection)
        {
            return projectileDirection switch
            {
                Direction.Right => shooterTransform.right,
                Direction.Left => -shooterTransform.right,
                Direction.Down => -shooterTransform.up,
                _ => shooterTransform.up
            };
        } 
    }
}
