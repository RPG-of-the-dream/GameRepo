using System;
using Battle;
using Core.Animation;
using Core.Movement.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.Behaviour
{
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRadius;
        [SerializeField] private BoxCollider2D _feet;

        [field: SerializeField] public  Vector2 SearchBox { get; private set; }
        [field: SerializeField] public  LayerMask Targets { get; private set; }
        [field: SerializeField] public  Slider HpBar { get; private set; }

        public Vector2 Size => _collider.bounds.size;

        public event Action<IDamageable> Attacked; 
        public event Action AttackSequenceEnded;
        public event Action Fell;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, SearchBox);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
        }
        
        private void Update() => UpdateAnimations();

        public override void Initialize()
        {
            base.Initialize();
            Mover = new DestinationMover(Rigidbody);
            Animator.ChangeDirection(Mover.Direction);
        }

        public void StartAttack() => Animator.SetAnimationState(AnimationType.Attack, true, Attack, AttackEnded);
        
        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            
            if (!IsGrounded()) 
                StartFalling();
        }
        
        private void Attack()
        {
            var targetCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, Targets);
            if (targetCollider is not null && targetCollider.TryGetComponent(out IDamageable target))
            {
                Attacked?.Invoke(target);
            }
        }

        private void AttackEnded() => AttackSequenceEnded?.Invoke();
        
        private bool IsGrounded()
        {
            Bounds feetBounds = _feet.bounds;
            int fallLayer = LayerMask.GetMask("FallingArea");
            return Physics2D.Raycast(
                feetBounds.center, 
                Vector2.down, 
                feetBounds.extents.y, 
                fallLayer
            ).collider == null;
        }

        private void StartFalling() => Fell?.Invoke();
    }
}
