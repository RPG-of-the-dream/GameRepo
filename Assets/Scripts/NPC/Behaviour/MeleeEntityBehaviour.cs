using System;
using Core.Animation;
using Core.Enums;
using Core.Movement.Controller;
using NPC.Behaviour;
using UnityEngine;

namespace Assets.Scripts.NPC.Behaviour
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
            DirectionalMover = new DirectionalMover(Rigidbody);
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
