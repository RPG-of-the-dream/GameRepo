using Core.Enums;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class DirectionalMover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly IStatValueGiver _statValueGiver;
        
        private Vector2 _movement;
        
        public bool IsMoving => _movement.magnitude > 0;
        public Direction Direction => MapDirection(_movement);
        
        public DirectionalMover(Rigidbody2D rigidbody, IStatValueGiver statValueGiver)
        {
            _rigidbody = rigidbody;
            _statValueGiver = statValueGiver;
        }
        
        public void Move(Vector2 direction)
        {
            _movement = direction;
            Vector2 position = _rigidbody.position;
            position += direction.normalized * (_statValueGiver.GetStatValue(StatType.Speed) * Time.fixedDeltaTime);
            _rigidbody.MovePosition(position);
        }
        private static Direction MapDirection(Vector2 movement)
        {
            Direction direction;
            if (Mathf.Abs(movement.y) > 0.5)
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