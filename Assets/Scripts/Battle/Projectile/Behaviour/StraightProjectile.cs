using Core.Enums;
using UnityEngine;

namespace Battle.Projectile.Behaviour
{
    public class StraightProjectile : ProjectileBase
    {
        [SerializeField] private Direction _direction;
        [SerializeField] private float _verticalAttackRadius;

        protected override void Move(Vector2 targetInfo, float speed)
        {
            SetDirection(targetInfo);
            RigidBody.velocity = targetInfo * speed;
        }

        private void SetDirection(Vector2 targetDirection)
        {
            var direction = targetDirection.x > 0 ? Direction.Right : Direction.Left;
            if(direction == _direction)
                return;

            transform.Rotate(0, 180, 0);
            _direction = direction;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.TryGetComponent(out IDamageable target) 
               || !(Mathf.Abs(other.transform.position.y - WorldPosition.y) < _verticalAttackRadius))
                return;

            target.TakeDamage(Damage);
            OnAttacked();
        }
    }
}
