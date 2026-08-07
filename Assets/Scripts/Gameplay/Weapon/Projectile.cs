using EnemyWaves.Core;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Weapon
{
    public class Projectile : MonoBehaviour, IPoolable<Vector3, Quaternion, IMemoryPool>
    {
        private const float HitRadius = 0.35f;

        [SerializeField] private LayerMask _hitLayers = ~0;

        private readonly RaycastHit[] _hitBuffer = new RaycastHit[8];

        private IMemoryPool _pool;
        private Vector3 _startPosition;
        private float _damage;
        private float _speed;
        private float _maxDistance;
        private bool _isSpent;

        public class Factory : PlaceholderFactory<Vector3, Quaternion, Projectile>
        {
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void OnSpawned(Vector3 position, Quaternion rotation, IMemoryPool pool)
        {
            _pool = pool;
            transform.SetPositionAndRotation(position, rotation);
            _startPosition = position;
            _isSpent = false;
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            _pool = null;
            gameObject.SetActive(false);
        }

        public void Launch(float damage, float speed, float maxDistance)
        {
            _damage = damage;
            _speed = speed;
            _maxDistance = maxDistance;
        }

        private void Update()
        {
            if (_isSpent)
                return;

            float step = _speed * Time.deltaTime;

            if (TryHitAlongPath(step, out var target))
            {
                target.TakeDamage(_damage);
                Despawn();
                return;
            }

            transform.position += transform.forward * step;

            if ((transform.position - _startPosition).sqrMagnitude >= _maxDistance * _maxDistance)
                Despawn();
        }

        private bool TryHitAlongPath(float step, out IDamageable target)
        {
            target = null;

            int count = Physics.SphereCastNonAlloc(
                transform.position, HitRadius, transform.forward, _hitBuffer, step, _hitLayers, QueryTriggerInteraction.Collide);

            float nearestDistance = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                if (!_hitBuffer[i].collider.TryGetComponent<IDamageable>(out var candidate) || !candidate.IsAlive)
                    continue;

                if (_hitBuffer[i].distance < nearestDistance)
                {
                    nearestDistance = _hitBuffer[i].distance;
                    target = candidate;
                }
            }

            return target != null;
        }

        private void Despawn()
        {
            if (_isSpent)
                return;

            _isSpent = true;
            _pool?.Despawn(this);
        }
    }
}
