using System.Collections;
using Battle;
using Core.Services.Updater;
using Entity.Behaviour;
using Pathfinding;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Entity.Controller
{
    public class MeleeEntity : BaseEntity
    {
        private readonly Seeker _seeker;
        private readonly MeleeEntityBehaviour _meleeEntityBehaviour;
        private readonly float _moveDelta;

        private bool _isAttacking;
        private Coroutine _searchCoroutine;
        private Collider2D _target;
        private Vector3 _previousTargetPosition;
        private Vector3 _destination;
        private float _stoppingDistance;
        private Path _currentPath;
        private int _currentWayPoint;

        public MeleeEntity(MeleeEntityBehaviour entityBehaviour, StatsController statsController) 
            : base(entityBehaviour, statsController)
        {
            _seeker = entityBehaviour.GetComponent<Seeker>();
            _meleeEntityBehaviour = entityBehaviour;
            _meleeEntityBehaviour.AttackSequenceEnded += OnAttackEnded;
            _meleeEntityBehaviour.Attacked += OnAttacked;
            _meleeEntityBehaviour.Fell += OnFell;
            VisualizeHp(StatsController.GetStatValue(StatType.Health));
            
            _searchCoroutine = ProjectUpdater.Instance.StartCoroutine(SearchCoroutine());
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdateCalled;
            _moveDelta = StatsController.GetStatValue(StatType.Speed) * Time.fixedDeltaTime;
        }

        public override void Dispose()
        {
            _meleeEntityBehaviour.Fell -= OnFell;
            _meleeEntityBehaviour.Attacked -= OnAttacked;
            _meleeEntityBehaviour.AttackSequenceEnded -= OnAttackEnded;
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdateCalled;
            ProjectUpdater.Instance.StopCoroutine(_searchCoroutine);
            base.Dispose();
        }

        protected sealed override void VisualizeHp(float currentHp)
        {
            if (_meleeEntityBehaviour.HpBar.maxValue < currentHp)
            {
                _meleeEntityBehaviour.HpBar.maxValue = currentHp;
            }

            _meleeEntityBehaviour.HpBar.value = currentHp;
        }
        
        private void OnFixedUpdateCalled()
        {
            if (_isAttacking 
                || _target is null 
                || _currentPath is null 
                || CheckIfCanAttack()
                ||_currentWayPoint >= _currentPath.vectorPath.Count)
                return;
            
            var currentPosition = _meleeEntityBehaviour.transform.position;
            var waypointPosition = _currentPath.vectorPath[_currentWayPoint];
            var waypointDirection = waypointPosition - currentPosition;

            if (Vector2.Distance(waypointPosition, currentPosition) < 0.2f)
            {
                _currentWayPoint++;
                return;
            }
            
            if (waypointDirection.y != 0 || waypointDirection.x != 0)
            {
                waypointDirection.y = waypointDirection.y > 0 ? 1 : -1;
                var newVerticalPosition = currentPosition.y + _moveDelta * waypointDirection.y;
                if (waypointDirection.y > 0 && waypointPosition.y < newVerticalPosition
                    || waypointDirection.y < 0 && waypointPosition.y > newVerticalPosition)
                    newVerticalPosition = waypointPosition.y;
                
                if (waypointDirection.y != 0)
                    OnVerticalPositionChanged();
                
                waypointDirection.x = waypointDirection.x > 0 ? 1 : -1;
                var newHorizontalPosition = currentPosition.x + _moveDelta * waypointDirection.x;
                if (waypointDirection.x > 0 && waypointPosition.x < newHorizontalPosition
                    || waypointDirection.x < 0 && waypointPosition.x > newHorizontalPosition)
                    newHorizontalPosition = waypointPosition.x;

                _meleeEntityBehaviour.Move(new Vector2(newHorizontalPosition, newVerticalPosition));
            }
        }

        private bool CheckIfCanAttack()
        {
            var distance = _destination - _meleeEntityBehaviour.transform.position;
            if (Mathf.Abs(distance.x) > 0.25f || Mathf.Abs(distance.y) > 0.25f)
                return false;

            _meleeEntityBehaviour.Move(_destination);

            ResetMovement();
            _isAttacking = true;
            _meleeEntityBehaviour.StartAttack();
            if (_searchCoroutine is not null)
            {
                ProjectUpdater.Instance.StopCoroutine(_searchCoroutine);
            }            
            
            return true;
        }

        private void ResetMovement()
        {
            _target = null;
            _currentPath = null;
            _previousTargetPosition = Vector2.negativeInfinity;
            _meleeEntityBehaviour.Move(_meleeEntityBehaviour.transform.position);
        }

        private IEnumerator SearchCoroutine()
        {
            while (!_isAttacking)
            {
                if (!TryGetTarget(out _target))
                {
                    ResetMovement();
                }
                else if(_target.transform.position != _previousTargetPosition)
                {
                    Vector2 position = _target.transform.position;
                    _previousTargetPosition = position;
                    _stoppingDistance = (_target.bounds.size.x + _meleeEntityBehaviour.Size.x) / 1.5f;
                    var delta = position.x < _meleeEntityBehaviour.transform.position.x ? 1 : -1;
                    _destination = position + new Vector2(_stoppingDistance * delta, 0);
                    _seeker.StartPath(_meleeEntityBehaviour.transform.position, _destination, OnPathCalculated);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void OnPathCalculated(Path path)
        {
            if(path.error)
                return;

            _currentPath = path;
            _currentWayPoint = 0;
        }

        private bool TryGetTarget(out Collider2D target)
        {
            target = null;
            if (_meleeEntityBehaviour is null && _meleeEntityBehaviour.transform is null)
            {
                return false;
            }
            
            target = Physics2D.OverlapBox(
                _meleeEntityBehaviour.transform.position,
                _meleeEntityBehaviour.SearchBox,
                0,
                _meleeEntityBehaviour.Targets);

            return target is not null;
        }
        
        private void OnAttacked(IDamageable target) => target.TakeDamage(StatsController.GetStatValue(StatType.Damage));

        private void OnAttackEnded()
        {
            _isAttacking = false;
            ProjectUpdater.Instance.Invoke(() =>
            {
                _searchCoroutine = ProjectUpdater.Instance.StartCoroutine(SearchCoroutine());
            }, StatsController.GetStatValue(StatType.AfterAttackDelay));
        }

        private void OnFell() =>
            _meleeEntityBehaviour.TakeDamage(
                StatsController.GetStatValue(StatType.Health) +
                StatsController.GetStatValue(StatType.Defence)
            );
    }
}
