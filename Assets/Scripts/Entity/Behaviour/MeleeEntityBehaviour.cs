using System;
using Core.Animation;
using Core.Movement.Controller;
using UnityEngine;

namespace Entity.Behaviour
{
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private float _afterAttackDelay;
        [SerializeField] private Collider2D _collider;

        [field: SerializeField] public  Vector2 SearchBox { get; private set; }
        [field: SerializeField] public  LayerMask Targets { get; private set; }

        public Vector2 Size => _collider.bounds.size;

        public event Action AttackSequenceEnded;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, SearchBox);
        }

        public override void Initialize()
        {
            base.Initialize();
            Mover = new DestinationMover(Rigidbody);
            Animator.ChangeDirection(Mover.Direction);
        }

        private void Update() => UpdateAnimations();

        public void StartAttack() => Animator.SetAnimationState(AnimationType.Attack, true, Attack, AttackEnded);

        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void AttackEnded()
        {
            Animator.SetAnimationState(AnimationType.Attack, false);
            Invoke(nameof(EndAttackSequence), _afterAttackDelay);
        }

        private void EndAttackSequence()
        {
            AttackSequenceEnded?.Invoke();
        }
    }
}
