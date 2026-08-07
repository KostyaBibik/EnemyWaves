using EnemyWaves.Configs;
using EnemyWaves.Services;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;

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

            Fire(origin, target.Transform.position);
            _cooldown = 1f / _config.FireRate;
        }

        private void Fire(Vector3 origin, Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - origin;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
                direction = transform.forward;

            var rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            var projectile = _projectileFactory.Create(origin, rotation);
            projectile.Launch(_config.Damage, _config.ProjectileSpeed, _config.Range);
        }
    }
}
