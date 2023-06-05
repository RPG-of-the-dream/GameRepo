using Assets.Scripts.Items;
using Battle;
using Battle.Projectile;
using Battle.Weapon;
using Items.Enums;
using UnityEngine;

namespace Player
{
    public class WeaponsFactory
    {
        private readonly SpriteRenderer _bowArrow;
        private readonly Transform _shooterTransform;
        private readonly Attacker _meleeAttacker;
        private readonly ProjectileFactory _projectileFactory;

        public WeaponsFactory(SpriteRenderer bowArrow, Transform shooterTransform, Attacker meleeAttacker, ProjectileFactory projectileFactory)
        {
            _bowArrow = bowArrow;
            _shooterTransform = shooterTransform;
            _meleeAttacker = meleeAttacker;
            _projectileFactory = projectileFactory;
        }

        public WeaponBase GetWeapon(ItemId itemId)
        {
            var itemType = itemId.GetItemType();
            switch (itemType)
            {
                case ItemType.Bow:
                    return new RangeWeapon(_projectileFactory, _bowArrow, _shooterTransform, itemId);
                case ItemType.OneHandedWeapon:
                case ItemType.TwoHandedWeapon:
                    return new SingleTargetMeleeWeapon(_meleeAttacker);
            }

            return null;
        }
    }
}
