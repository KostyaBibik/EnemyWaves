using System.Collections.Generic;
using EnemyWaves.Core;
using UnityEngine;

namespace EnemyWaves.Services
{
    public class TargetRegistry : ITargetRegistry
    {
        private readonly List<ITargetProvider> _enemies = new List<ITargetProvider>();

        public IReadOnlyList<ITargetProvider> Enemies => _enemies;

        public void Register(ITargetProvider enemy)
        {
            if (!_enemies.Contains(enemy))
                _enemies.Add(enemy);
        }

        public void Unregister(ITargetProvider enemy)
        {
            _enemies.Remove(enemy);
        }

        public ITargetProvider FindNearestEnemy(Vector3 fromPosition, float maxRange)
        {
            ITargetProvider nearest = null;
            float nearestSqr = maxRange * maxRange;

            for (int i = 0; i < _enemies.Count; i++)
            {
                var candidate = _enemies[i];
                if (candidate?.Damageable == null || !candidate.Damageable.IsAlive)
                    continue;

                float sqrDist = (candidate.Transform.position - fromPosition).sqrMagnitude;
                if (sqrDist <= nearestSqr)
                {
                    nearestSqr = sqrDist;
                    nearest = candidate;
                }
            }

            return nearest;
        }
    }
}
