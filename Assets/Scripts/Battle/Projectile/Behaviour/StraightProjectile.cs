using Core.Enums;
using UnityEngine;

namespace Battle.Projectile.Behaviour
{
    public class StraightProjectile : ProjectileBase
    {
        [SerializeField] private Direction _direction;
        [SerializeField] private float _verticalAttackRadius;

        private float _lastVerticalPosition;
        
        private void FixedUpdate()
        {
            if (!gameObject.activeSelf || _lastVerticalPosition == VerticalPosition)
                return;
            
            OnVerticalPositionChanged();
            _lastVerticalPosition = VerticalPosition;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.TryGetComponent(out IDamageable target) 
               || !(Mathf.Abs(other.transform.position.y - WorldPosition.y) < _verticalAttackRadius))
                return;

            target.TakeDamage(Damage);
            OnAttacked();
        }

        protected override void Move(Vector2 targetInfo, float speed)
        {
            SetDirection(targetInfo);
            RigidBody.velocity = targetInfo * speed;
            _lastVerticalPosition = VerticalPosition;
        }

        private void SetDirection(Vector2 targetDirection)
        {
            var direction = MapDirection(targetDirection); 
            
            if(direction == _direction)
                return;
            
            transform.Rotate(0, 0, -MapAngle(_direction));
            transform.Rotate(0, 0, MapAngle(direction));
            _direction = direction;
        }

        private static Direction MapDirection(Vector2 direction)
        {
            if (Mathf.Abs(direction.y) > 0)
            {
                return direction.y > 0 ? Direction.Top : Direction.Down;
            }
            else
            {
                return direction.x > 0 ? Direction.Right : Direction.Left;
            }
        }
        
        private static float MapAngle(Direction direction)
        {
            return direction switch
            {
                Direction.Right => 0,
                Direction.Left => 180,
                Direction.Down => 270,
                _ => 90
            };
        }
    }
}
