using Core.Enums;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class DestinationMover : Mover
    {
        private Vector2 _destination;
        
        public override bool IsMoving => _destination != Rigidbody.position;

        public DestinationMover(Rigidbody2D rigidbody) : base(rigidbody) => Direction = Direction.Right;

        public override void Move(Vector2 destination)
        {
            _destination = destination;
            Rigidbody.MovePosition(destination);
            if (_destination.x != Rigidbody.position.x)
            {
                SetDirection(destination.x > Rigidbody.position.x ? Direction.Right : Direction.Left);
            }
        }

        private void SetDirection(Direction newDirection)
        {
            if (Direction == newDirection)
            {
                return;
            }
            
            Rigidbody.transform.Rotate(0, 180, 0);
            Direction = Direction == Direction.Right ? Direction.Left : Direction.Right;
        }
    }
}