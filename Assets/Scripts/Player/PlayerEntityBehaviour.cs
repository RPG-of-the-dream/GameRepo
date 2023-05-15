using System;
using Core.Enums;
using Core.Animation;
using Core.Movement.Controller;
using Drawing;
using StatsSystem;
using UnityEngine;
using UnityEngine.Rendering;
using NPC.Behaviour;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntityBehaviour : BaseEntityBehaviour
    {        
        [SerializeField] private Direction _initialDirection;
        [SerializeField] private CapsuleCollider2D _feet;

        private Vector2 _startPosition;
        private bool _fellDown;
        private bool _inAction;             

        public void Initialize(IStatValueGiver statValueGiver)
        {
            base.Initialize();
            _startPosition = Rigidbody.position;
            Animator.ChangeDirection(_initialDirection);
            DirectionalMover = new DirectionalMover(Rigidbody, statValueGiver);
        }

        private void Update()
        {
            if (_fellDown)            
                Respawn();           
            else           
                UpdateAnimations();           
        }

        public void StartAttack()
        {
            if (_inAction)
                return;

            _inAction = Animator.SetAnimationState(AnimationType.Attack, true, Attack, EndAction);
        }
        
        private void UpdateAnimations()
        {
            Animator.ChangeDirection(DirectionalMover.Direction);
            
            if (!IsGrounded()) 
                StartFalling();

            Animator.SetAnimationState(AnimationType.Idle, true);
            Animator.SetAnimationState(AnimationType.Walk, DirectionalMover.IsMoving);
        }

        private void Attack() => Debug.Log("Attack");

        private void EndAction() =>  _inAction = false;

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
        
        private void StartFalling() => 
            _inAction = Animator.SetAnimationState(AnimationType.Fall, true, Falling, EndFalling);

        private void Falling() => Debug.Log("Falling");

        private void EndFalling()
        {
            EndAction();
            _fellDown = true;
        }

        private void Respawn()
        {
            transform.position = _startPosition;
            _fellDown = false;
        }
    }
}