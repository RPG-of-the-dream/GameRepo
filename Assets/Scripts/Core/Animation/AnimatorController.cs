using System;
using Core.Enums;
using UnityEngine;

namespace Core.Animation
{
    public abstract class AnimatorController : MonoBehaviour
    {
        private AnimationType _currentAnimationType;
        private Direction _currentDirection;

        private Action _animationAction;
        private Action _animationEndAction;

        public abstract void Initialize();

        public bool SetAnimationState(AnimationType animationType, bool active, 
            Action animationAction = null, Action endAnimationAction = null)
        {
            if (!active)
            {
                if(_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                    return false;

                OnAnimationEnded();
                return false;
            }
            
            if (_currentAnimationType >= animationType)
                return false;

            _animationAction = animationAction;
            _animationEndAction = endAnimationAction;
            SetAnimation(animationType);
            return true;
        }

        public void ChangeDirection(Direction direction)
        {
            if (_currentDirection == direction || direction == Direction.None)
                return;

            _currentDirection = direction;
            SetDirection(_currentDirection);
        }

        private void SetAnimation(AnimationType animationType)
        {
            _currentAnimationType = animationType;
            PlayAnimation(animationType);
        }

        protected abstract void PlayAnimation(AnimationType animationType);
        protected abstract void SetDirection(Direction direction);
        protected void OnActionRequested() => _animationAction?.Invoke();

        private void OnAnimationEnded()
        {
            _animationEndAction?.Invoke();
            _animationAction = null;
            _animationEndAction = null;
            SetAnimation(AnimationType.Idle);
        }
    }
}