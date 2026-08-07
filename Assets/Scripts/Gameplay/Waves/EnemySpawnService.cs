using EnemyWaves.Configs;
using EnemyWaves.Core;
using EnemyWaves.Gameplay.Enemies;
using EnemyWaves.Services;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Waves
{
    public class EnemySpawnService : ITickable
    {
        private readonly WaveConfig _config;
        private readonly EnemyDatabase _database;
        private readonly IEnemyFactory _enemyFactory;
        private readonly ITargetRegistry _targetRegistry;
        private readonly IGameStateService _gameState;
        private readonly ITargetProvider _playerTarget;

        private float _checkTimer;

        public EnemySpawnService(
            WaveConfig config,
            EnemyDatabase database,
            IEnemyFactory enemyFactory,
            ITargetRegistry targetRegistry,
            IGameStateService gameState,
            [Inject(Id = PlayerTargetId.Value)] ITargetProvider playerTarget)
        {
            _config = config;
            _database = database;
            _enemyFactory = enemyFactory;
            _targetRegistry = targetRegistry;
            _gameState = gameState;
            _playerTarget = playerTarget;
        }

        public void Tick()
        {
            if (_gameState.IsGameOver.Value)
                return;

            _checkTimer -= Time.deltaTime;
            if (_checkTimer > 0f)
                return;

            _checkTimer = _config.SpawnCheckInterval;

            int missing = _config.MinAliveEnemies - _targetRegistry.Enemies.Count;
            for (int i = 0; i < missing; i++)
                SpawnOne();
        }

        private void SpawnOne()
        {
            var definition = _database.GetRandomWeighted();
            if (definition == null || definition.Prefab == null)
                return;

            Vector2 offset = Random.insideUnitCircle.normalized * _config.SpawnRadius;
            Vector3 playerPos = _playerTarget.Transform.position;
            Vector3 spawnPos = playerPos + new Vector3(offset.x, 0f, offset.y);

            _enemyFactory.Spawn(definition, spawnPos);
        }
    }
}
