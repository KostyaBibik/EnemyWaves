using System;
using EnemyWaves.Configs;
using EnemyWaves.Core;
using EnemyWaves.Services;
using UniRx;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Enemies
{
    public class EnemyController : MonoBehaviour, IDamageable, ITargetProvider
    {
        [SerializeField] private EnemyView _view;

        private ITargetRegistry _targetRegistry;
        private IEnemyFactory _enemyFactory;
        private ITargetProvider _playerTarget;

        private readonly EnemyModel _model = new EnemyModel();
        private IDisposable _healthSubscription;
        private float _attackCooldown;

        public EnemyDefinition Definition { get; private set; }

        public Transform Transform => transform;
        public IDamageable Damageable => this;
        public bool IsAlive => _model.IsAlive;

        [Inject]
        public void Construct(ITargetRegistry targetRegistry, IEnemyFactory enemyFactory, [Inject(Id = PlayerTargetId.Value)] ITargetProvider playerTarget)
        {
            _targetRegistry = targetRegistry;
            _enemyFactory = enemyFactory;
            _playerTarget = playerTarget;
        }

        private void Awake()
        {
            _healthSubscription = _model.Health.Subscribe(OnHealthChanged);
        }

        private void OnDestroy()
        {
            _healthSubscription?.Dispose();
        }

        private void OnHealthChanged(float health)
        {
            if (_view != null && _model.MaxHealth > 0f)
                _view.SetHealthFraction(health / _model.MaxHealth);
        }

        public void Activate(EnemyDefinition definition, Vector3 position)
        {
            Definition = definition;
            transform.position = position;
            _attackCooldown = 0f;

            _model.Reset(definition.MaxHealth);
            _targetRegistry.Register(this);
        }

        public void TakeDamage(float amount)
        {
            if (!_model.IsAlive)
                return;

            _model.TakeDamage(amount);
            if (!_model.IsAlive)
                Die();
        }

        private void Update()
        {
            if (!_model.IsAlive || _playerTarget == null)
                return;

            Vector3 toPlayer = _playerTarget.Transform.position - transform.position;
            toPlayer.y = 0f;
            float distance = toPlayer.magnitude;

            if (distance > Definition.AttackRange)
            {
                Vector3 direction = toPlayer / Mathf.Max(distance, 0.0001f);
                float step = Mathf.Min(Definition.MoveSpeed * Time.deltaTime, distance - Definition.AttackRange);
                transform.position += direction * step;
                transform.forward = Vector3.Slerp(transform.forward, direction, 10f * Time.deltaTime);
            }
            else
            {
                _attackCooldown -= Time.deltaTime;
                if (_attackCooldown <= 0f && _playerTarget.Damageable.IsAlive)
                {
                    _playerTarget.Damageable.TakeDamage(Definition.ContactDamage);
                    _attackCooldown = Definition.AttackInterval;
                }
            }
        }

        private void Die()
        {
            _targetRegistry.Unregister(this);
            _enemyFactory.Despawn(this);
        }
    }
}
