using Core.Enums;
using UnityEngine;

namespace Core.Movement.Controller
{
    public abstract class Mover
    {
        protected readonly Rigidbody2D Rigidbody;

        public abstract bool IsMoving { get; }
        public  Direction Direction { get; protected set; }

        public Mover(Rigidbody2D rigidbody) => Rigidbody = rigidbody;

        public abstract void Move(Vector2 direction);
    }
}