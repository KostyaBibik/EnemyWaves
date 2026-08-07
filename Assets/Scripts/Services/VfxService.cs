using System.Collections.Generic;
using EnemyWaves.Configs;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace EnemyWaves.Services
{
    public class VfxService : IVfxService, ITickable
    {
        private readonly VfxConfig _config;
        private readonly Transform _root;

        private readonly Dictionary<GameObject, ObjectPool<ParticleSystem>> _pools =
            new Dictionary<GameObject, ObjectPool<ParticleSystem>>();

        private readonly Dictionary<GameObject, float> _lifetimes = new Dictionary<GameObject, float>();
        private readonly List<ActiveEffect> _active = new List<ActiveEffect>();

        public VfxService(VfxConfig config)
        {
            _config = config;
            _root = new GameObject("Vfx").transform;
        }

        public void Play(GameObject prefab, Vector3 position)
        {
            if (prefab == null)
                return;

            var instance = GetOrCreatePool(prefab).Get();
            instance.transform.position = position;
            instance.Clear(true);
            instance.Play(true);

            _active.Add(new ActiveEffect(prefab, instance, Time.time + GetLifetime(prefab)));
        }

        public void Tick()
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var effect = _active[i];
                if (Time.time < effect.ReleaseTime)
                    continue;

                _active.RemoveAt(i);
                if (effect.Instance != null)
                    _pools[effect.Prefab].Release(effect.Instance);
            }
        }

        private float GetLifetime(GameObject prefab)
        {
            if (_lifetimes.TryGetValue(prefab, out float lifetime))
                return lifetime;

            lifetime = 0f;
            foreach (var system in prefab.GetComponentsInChildren<ParticleSystem>(true))
            {
                var main = system.main;
                lifetime = Mathf.Max(lifetime, main.duration + main.startLifetime.constantMax);
            }

            lifetime += _config.ReleasePadding;
            _lifetimes[prefab] = lifetime;
            return lifetime;
        }

        private ObjectPool<ParticleSystem> GetOrCreatePool(GameObject prefab)
        {
            if (_pools.TryGetValue(prefab, out var pool))
                return pool;

            pool = new ObjectPool<ParticleSystem>(
                createFunc: () => Object.Instantiate(prefab, _root).GetComponent<ParticleSystem>(),
                actionOnGet: instance => instance.gameObject.SetActive(true),
                actionOnRelease: instance => instance.gameObject.SetActive(false),
                actionOnDestroy: instance => Object.Destroy(instance.gameObject),
                defaultCapacity: 8);

            _pools[prefab] = pool;
            return pool;
        }

        private readonly struct ActiveEffect
        {
            public readonly GameObject Prefab;
            public readonly ParticleSystem Instance;
            public readonly float ReleaseTime;

            public ActiveEffect(GameObject prefab, ParticleSystem instance, float releaseTime)
            {
                Prefab = prefab;
                Instance = instance;
                ReleaseTime = releaseTime;
            }
        }
    }
}
