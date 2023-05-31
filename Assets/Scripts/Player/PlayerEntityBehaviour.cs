using Core.Enums;
using Core.Animation;
using Core.Movement.Controller;
using UnityEngine;
using Entity.Behaviour;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntityBehaviour : BaseEntityBehaviour
    {        
        [SerializeField] private Direction _initialDirection;
        [SerializeField] private CapsuleCollider2D _feet;
        
        [field: SerializeField] public PlayerStatsUIView PlayerStatsUIView { get; private set; }

        private Vector2 _startPosition;
        private bool _fellDown;
        private bool _inAction;             

        public override void Initialize()
        {
            base.Initialize();
            _startPosition = Rigidbody.position;
            Animator.ChangeDirection(_initialDirection);
            Mover = new DirectionalMover(Rigidbody, _initialDirection);
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

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            Animator.ChangeDirection(Mover.Direction);
            
            if (!IsGrounded()) 
                StartFalling();
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