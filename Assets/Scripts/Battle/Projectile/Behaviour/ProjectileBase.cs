using System;
using Drawing;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battle.Projectile.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class ProjectileBase : MonoBehaviour, ILevelGraphicElement
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform _shadow;
        [SerializeField] private SortingGroup _sortingGroup;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Rigidbody2D _rigidBody;
        
        protected float Damage;

        protected Rigidbody2D RigidBody => _rigidBody ??= GetComponent<Rigidbody2D>();
        protected Vector2 WorldPosition => _shadow.position;

        public float VerticalPosition => _shadow.position.y;
        
        public event Action<ILevelGraphicElement> VerticalPositionChanged;
        public event Action<ProjectileBase> Attacked;

        public void Initialize(Vector2 targetInfo, float damage)
        {
            Move(targetInfo, _speed);
            Damage = damage;
        }

        public void SetDrawingOrder(int order) => _sortingGroup.sortingOrder = order;

        public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;

        protected abstract void Move(Vector2 targetInfo, float speed);

        protected void OnVerticalPositionChanged() => VerticalPositionChanged?.Invoke(this);
        protected void OnAttacked() => Attacked?.Invoke(this);
    }
}
