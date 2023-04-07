using System;
using Core.Enums;
using UnityEngine;

namespace Core.Animation
{
    public abstract class AnimatorController : MonoBehaviour
    {
        private AnimationType _currentAnimationType;
        private Direction _currentDirection;

        public event Action ActionRequested;
        public event Action AnimationEnded;

        public bool PlayAnimation(AnimationType animationType, bool active)
        {
            if (!active)
            {
                if(_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                    return false;

                _currentAnimationType = AnimationType.Idle;
                PlayAnimation(_currentAnimationType);
                return false;
            }
            
            if (_currentAnimationType >= animationType)
                return false;

            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
            return true;
        }

        public bool ChangeDirection(Direction direction)
        {
            if (_currentDirection == direction || direction == Direction.None)
                return false;

            _currentDirection = direction;
            SetDirection(_currentDirection);
            return true;
        }

        protected abstract void PlayAnimation(AnimationType animationType);
        protected abstract void SetDirection(Direction direction);
        protected void OnActionRequested() => ActionRequested?.Invoke();
        protected void OnAnimationEnded() => AnimationEnded?.Invoke();
    }
}