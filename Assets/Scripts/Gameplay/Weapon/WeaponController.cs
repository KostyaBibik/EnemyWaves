using System;
using EnemyWaves.Configs;
using EnemyWaves.Core;
using EnemyWaves.Services;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;

        /// <summary>Raised each time a shot is fired. Used by PlayerAnimatorDriver to trigger the fire animation.</summary>
        public event Action Fired;

        private WeaponConfig _config;
        private ITargetRegistry _targetRegistry;
        private Projectile.Factory _projectileFactory;
        private float _cooldown;

        [Inject]
        public void Construct(WeaponConfig config, ITargetRegistry targetRegistry, Projectile.Factory projectileFactory)
        {
            _config = config;
            _targetRegistry = targetRegistry;
            _projectileFactory = projectileFactory;
        }

        private void Update()
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown > 0f)
                return;

            Vector3 origin = _muzzle != null ? _muzzle.position : transform.position;
            var target = _targetRegistry.FindNearestEnemy(origin, _config.Range);
            if (target == null)
                return;

            Fire(origin, ResolveAimPoint(target));
            _cooldown = 1f / _config.FireRate;
        }

        private static Vector3 ResolveAimPoint(ITargetProvider target)
        {
            var targetTransform = target.Transform;

            return targetTransform.TryGetComponent<Collider>(out var targetCollider)
                ? targetCollider.bounds.center
                : targetTransform.position;
        }

        private void Fire(Vector3 origin, Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - origin;
            if (direction.sqrMagnitude < 0.0001f)
                direction = transform.forward;

            var rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            var projectile = _projectileFactory.Create(origin, rotation);
            projectile.Launch(_config.Damage, _config.ProjectileSpeed, _config.Range);
            Fired?.Invoke();
        }
    }
}
