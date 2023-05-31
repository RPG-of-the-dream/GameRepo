using System;
using Battle;
using Core.Animation;
using Core.Movement.Controller;
using UnityEngine;
using UnityEngine.Rendering;

namespace Entity.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseEntityBehaviour : MonoBehaviour, IDamageable
    {
        [SerializeField] protected AnimatorController Animator;
        [SerializeField] private SortingGroup _sortingGroup;

        protected Rigidbody2D Rigidbody;
        protected Mover Mover;

        public float VerticalPosition => Rigidbody.position.y;
        
        public event Action<float> DamageTaken;

        public virtual void Initialize()
        {
            Animator.Initialize();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage(float damage) => DamageTaken?.Invoke(damage);

        public void SetDrawingOrder(int order) => _sortingGroup.sortingOrder = order;

        public void Move(Vector2 direction) => Mover.Move(direction);

        protected virtual void UpdateAnimations()
        {
            Animator.SetAnimationState(AnimationType.Idle, true);
            Animator.SetAnimationState(AnimationType.Walk, Mover.IsMoving);
        }
    }
}