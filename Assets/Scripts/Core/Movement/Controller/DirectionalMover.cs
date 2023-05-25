using Core.Enums;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class DirectionalMover : Mover
    {
        private Vector2 _direction;
        
        public override bool IsMoving => _direction.magnitude > 0;

        public DirectionalMover(Rigidbody2D rigidbody, Direction initialDirection) : base(rigidbody)
        {
            Direction = initialDirection;
        }

        public override void Move(Vector2 direction)
        {
            _direction = direction;
            Rigidbody.MovePosition(Rigidbody.position + direction);
            Direction = MapDirection(_direction);
        }
        
        private Direction MapDirection(Vector2 movement)
        {
            Direction direction;
            if (Mathf.Abs(movement.y) > 0.025)
            {
                direction = movement.y > 0 ? Direction.Top : Direction.Down;
            }
            else
            {
                switch (movement.x)
                {
                    case > 0:
                        direction = Direction.Right;
                        break;
                    case < 0:
                        direction = Direction.Left;
                        break;
                    default:
                        direction = Direction.None;
                        break;
                }
            }
            return direction;
        }
    }
}