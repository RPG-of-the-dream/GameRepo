using Core.Enums;

namespace Battle.Weapon
{
    public abstract class WeaponBase
    {
        public abstract void Attack(float damage, Direction projectileDirection);
        public abstract void EndAttack();
    }
}
