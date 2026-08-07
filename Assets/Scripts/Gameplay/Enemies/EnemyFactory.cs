using System.Collections.Generic;
using EnemyWaves.Configs;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace EnemyWaves.Gameplay.Enemies
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly DiContainer _container;
        private readonly Dictionary<GameObject, ObjectPool<EnemyController>> _pools = new Dictionary<GameObject, ObjectPool<EnemyController>>();
        private readonly Transform _root;

        public EnemyFactory(DiContainer container)
        {
            _container = container;
            _root = new GameObject("Enemies").transform;
        }

        public EnemyController Spawn(EnemyDefinition definition, Vector3 position)
        {
            var pool = GetOrCreatePool(definition);
            var enemy = pool.Get();
            enemy.Activate(definition, position);
            return enemy;
        }

        public void Despawn(EnemyController enemy)
        {
            var pool = GetOrCreatePool(enemy.Definition);
            pool.Release(enemy);
        }

        private ObjectPool<EnemyController> GetOrCreatePool(EnemyDefinition definition)
        {
            if (_pools.TryGetValue(definition.Prefab, out var pool))
                return pool;

            pool = new ObjectPool<EnemyController>(
                createFunc: () => _container.InstantiatePrefabForComponent<EnemyController>(definition.Prefab, _root),
                actionOnGet: enemy => enemy.gameObject.SetActive(true),
                actionOnRelease: enemy => enemy.gameObject.SetActive(false),
                actionOnDestroy: enemy => Object.Destroy(enemy.gameObject),
                defaultCapacity: 8);

            _pools[definition.Prefab] = pool;
            return pool;
        }
    }
}
