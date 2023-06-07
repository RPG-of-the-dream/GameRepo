using System;
using Core.Enums;
using Core.Animation;
using Core.Movement.Controller;
using UnityEngine;
using Entity.Behaviour;
using Assets.Scripts.Items.CharacterEquipment;
using Battle;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private Direction _initialDirection;
        [SerializeField] private CapsuleCollider2D _feet;

        [field: SerializeField] public PlayerStatsUIView PlayerStatsUIView { get; private set; }
        [field: SerializeField] public CharacterEquipment CharacterEquipment { get; private set; }
        [field: SerializeField] public SpriteRenderer Arrow { get; private set; }
        [field: SerializeField] public Attacker Attacker { get; private set; }

        private Vector2 _startPosition;
        private bool _fellDown;

        public Direction CurrentDirection => Mover.Direction;

        public event Action AttackRequested; 
        public event Action AttackEnded;
        public event Action Respawned; 
        public event Action Fell;

        public override void Initialize()
        {
            base.Initialize();
            _startPosition = Rigidbody.position;
            Animator.ChangeDirection(_initialDirection);
            Mover = new DirectionalMover(Rigidbody, _initialDirection);
        }

        private void Update() => UpdateAnimations();

        public void SetAnimationParameter(string parameter, int value) =>
            Animator.SetAnimationParameter(parameter, value);

        public void StartAttack() => 
            Animator.SetAnimationState(AnimationType.Attack, true, OnAttack, OnAttackEnded);

        public void Respawn()
        {
            transform.position = _startPosition;
            Animator.ChangeDirection(_initialDirection);
            Respawned?.Invoke();
        }

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            Animator.ChangeDirection(Mover.Direction);
            
            if (!IsGrounded()) 
                StartFalling();
        }

        private void OnAttack() => AttackRequested?.Invoke();
        private void OnAttackEnded() => AttackEnded?.Invoke();

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