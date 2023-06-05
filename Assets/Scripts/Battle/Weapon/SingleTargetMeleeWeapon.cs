using Unity.VisualScripting;
using UnityEngine;

namespace Battle.Weapon
{
    public class SingleTargetMeleeWeapon : WeaponBase
    {
        private readonly Attacker _attacker;
        private float _damage;

        public SingleTargetMeleeWeapon(Attacker attacker)
        {
            if(attacker.TryGetComponent(out PolygonCollider2D collider))
                Object.Destroy(collider);

            _attacker = attacker;
            _attacker.AddComponent<PolygonCollider2D>().isTrigger = true;
        }

        public override void Attack(float damage)
        {
            _attacker.Reset();
            _attacker.gameObject.SetActive(true);
            _damage = damage;
        }

        public override void EndAttack()
        {
            if(_attacker.Targets.Count < 1)
                return;

            _attacker.Targets[0].TakeDamage(_damage);
        }
    }
}
