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
        [SerializeField] private CapsuleCollider2D _feet;

        private Rigidbody2D _rigidbody;
        private DirectionalMover _directionalMover;
        private Vector2 _startPosition;
        private bool _fellDown;

        public void Initialize(IStatValueGiver statValueGiver)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _startPosition = _rigidbody.position;
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
        
        private void UpdateAnimations()
        {
            _animator.ChangeDirection(_directionalMover.Direction);
            
            if (!IsGrounded()) 
                StartFalling();

            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Walk, _directionalMover.IsMoving);
        }

        public void StartAttack()
        {
            if(!_animator.PlayAnimation(AnimationType.Attack, true))
                return;

            _animator.ActionRequested += Attack;
            _animator.AnimationEnded += EndAttack;
        }

        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void EndAttack()
        {
            _animator.ActionRequested -= Attack;
            _animator.AnimationEnded -= EndAttack;
            _animator.PlayAnimation(AnimationType.Attack, false);
        }

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
        
        private void StartFalling()
        {
            if(!_animator.PlayAnimation(AnimationType.Fall, true))
                return;

            _animator.ActionRequested += Falling;
            _animator.AnimationEnded += EndFalling;
        }

        private void Falling()
        {
            Debug.Log("Falling");
        }

        private void EndFalling()
        {
            _fellDown = true;
            _animator.ActionRequested -= Falling;
            _animator.AnimationEnded -= EndFalling;
            _animator.PlayAnimation(AnimationType.Fall, false);
        }

        private void Respawn()
        {
            transform.position = _startPosition;
            _fellDown = false;
        }
    }

}