using Core.Enums;
using UnityEngine;

namespace Core.Animation
{
    [RequireComponent(typeof(Animator))]
    public class UnityAnimatorController : AnimatorController
    {
        private Animator _animator;

        public override void Initialize() => _animator = GetComponent<Animator>();

        public override void SetAnimationParameter(string parameter, int value) =>
            _animator.SetInteger(parameter, value);

        protected override void PlayAnimation(AnimationType animationType) =>
            _animator.SetInteger(nameof(AnimationType), (int)animationType);

        protected override void SetDirection(Direction direction)
        {
            MapDirection(direction, out Vertical vertical, out Horizontal horizontal);
            _animator.SetFloat(nameof(Vertical), (float)vertical);
            _animator.SetFloat(nameof(Horizontal), (float)horizontal);
        }

        private static void MapDirection(Direction direction, out Vertical vertical, out Horizontal horizontal)
        {
            switch (direction)
            {
                case Direction.Top:
                    vertical = Vertical.Top;
                    horizontal = Horizontal.Center;
                    break;
                case Direction.Left:
                    vertical = Vertical.Center;
                    horizontal = Horizontal.Left;
                    break;
                case Direction.Right:
                    vertical = Vertical.Center;
                    horizontal = Horizontal.Right;
                    break;
                default:
                    vertical = Vertical.Down;
                    horizontal = Horizontal.Center;
                    break;
            }
        }
    }
}