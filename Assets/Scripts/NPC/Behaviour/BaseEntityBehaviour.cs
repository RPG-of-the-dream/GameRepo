using System;
using System.Collections;
using System.Collections.Generic;
using Core.Animation;
using Core.Movement.Controller;
using Drawing;
using UnityEngine;
using UnityEngine.Rendering;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseEntityBehaviour : MonoBehaviour, ILevelGraphicElement
    {
        [SerializeField] protected AnimatorController Animator;
        [SerializeField] private SortingGroup _sortingGroup;

        protected Rigidbody2D Rigidbody;
        protected DirectionalMover DirectionalMover;

        public event Action<ILevelGraphicElement> VerticalPositionChanged;

        public float VerticalPosition => Rigidbody.position.y;

        public virtual void Initialize()
        {
            Animator.Initialize();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetDrawingOrder(int order) => _sortingGroup.sortingOrder = order;

        public void Move(Vector2 direction)
        {
            DirectionalMover.Move(direction);
            if (direction.y != 0)
            {
                VerticalPositionChanged?.Invoke(this);
            }
        }

        protected virtual void UpdateAnimations()
        {
            Animator.SetAnimationState(AnimationType.Idle, true);
            Animator.SetAnimationState(AnimationType.Walk, DirectionalMover.IsMoving);
        }

        
    }
}