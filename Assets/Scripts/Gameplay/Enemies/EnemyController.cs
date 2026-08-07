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

        [Tooltip("How quickly enemies push each other out of an overlap. 0 disables separation.")]
        [Min(0f)] [SerializeField] private float _separationStrength = 10f;

        private static readonly Collider[] SeparationBuffer = new Collider[24];
        private static float s_LargestBodyRadius;

        private ITargetRegistry _targetRegistry;
        private IEnemyFactory _enemyFactory;
        private ITargetProvider _playerTarget;

        private readonly EnemyModel _model = new EnemyModel();
        private IDisposable _healthSubscription;
        private float _attackCooldown;
        private float _bodyRadius;

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

            _bodyRadius = MeasureBodyRadius(GetComponent<Collider>(), transform.lossyScale);
            s_LargestBodyRadius = Mathf.Max(s_LargestBodyRadius, _bodyRadius);
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

            if (_view != null)
                _view.ResetHealthFraction(1f);

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

            ApplySeparation();
        }

        private void ApplySeparation()
        {
            if (_separationStrength <= 0f || _bodyRadius <= 0f)
                return;

            Vector3 center = transform.position + Vector3.up * _bodyRadius;
            float queryRadius = _bodyRadius + s_LargestBodyRadius;
            int mask = 1 << gameObject.layer;
            int count = Physics.OverlapSphereNonAlloc(center, queryRadius, SeparationBuffer, mask, QueryTriggerInteraction.Ignore);

            Vector3 push = Vector3.zero;

            for (int i = 0; i < count; i++)
            {
                var other = SeparationBuffer[i];
                if (other == null || other.transform == transform)
                    continue;

                Vector3 delta = transform.position - other.transform.position;
                delta.y = 0f;

                float minDistance = _bodyRadius + MeasureBodyRadius(other, other.transform.lossyScale);
                float distance = delta.magnitude;
                if (distance >= minDistance)
                    continue;

                Vector3 direction = distance > 0.0001f
                    ? delta / distance
                    : SideStepDirection(other.GetInstanceID());

                push += direction * (minDistance - distance);
            }

            if (push.sqrMagnitude <= 0f)
                return;

            Vector3 correction = push * 0.5f;
            transform.position += Vector3.ClampMagnitude(correction * (_separationStrength * Time.deltaTime), correction.magnitude);
        }

        private Vector3 SideStepDirection(int otherId)
        {
            int selfId = GetInstanceID();

            float angle = (selfId ^ otherId) * 0.001f;
            var axis = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            return selfId < otherId ? axis : -axis;
        }

        private static float MeasureBodyRadius(Collider collider, Vector3 lossyScale)
        {
            float scale = Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z));

            switch (collider)
            {
                case null:
                    return 0f;
                case CapsuleCollider capsule:
                    return capsule.radius * scale;
                case SphereCollider sphere:
                    return sphere.radius * scale;
                default:
                    var extents = collider.bounds.extents;
                    return Mathf.Max(extents.x, extents.z);
            }
        }

        private void Die()
        {
            _targetRegistry.Unregister(this);
            _enemyFactory.Despawn(this);
        }
    }
}
