using Core.Enums;
using Core.Animation;
using Core.Movement.Controller;
using StatsSystem;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimatorController _animator;
        [SerializeField] private Direction _initialDirection;
        [SerializeField] private CapsuleCollider2D _feet;

        private Rigidbody2D _rigidbody;
        private DirectionalMover _directionalMover;
        private Vector2 _startPosition;
        private bool _fellDown;
        private bool _inAction;

        public void Initialize(IStatValueGiver statValueGiver)
        {
            _animator.Initialize();
            _rigidbody = GetComponent<Rigidbody2D>();
            _startPosition = _rigidbody.position;
            _animator.ChangeDirection(_initialDirection);
            _directionalMover = new DirectionalMover(_rigidbody, statValueGiver);
        }

        private void Update()
        {
            if (_fellDown)
            {
                Respawn();
            }
            else
            {
                UpdateAnimations();
            }
        }

        public void Move(Vector2 direction) => _directionalMover.Move(direction);
        
        public void StartAttack()
        {
            if (_inAction)
                return;

            _inAction = _animator.SetAnimationState(AnimationType.Attack, true, Attack, EndAction);
        }
        
        private void UpdateAnimations()
        {
            _animator.ChangeDirection(_directionalMover.Direction);
            
            if (!IsGrounded()) 
                StartFalling();

            _animator.SetAnimationState(AnimationType.Idle, true);
            _animator.SetAnimationState(AnimationType.Walk, _directionalMover.IsMoving);
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
            _inAction = _animator.SetAnimationState(AnimationType.Fall, true, Falling, EndFalling);

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