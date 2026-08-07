using EnemyWaves.Configs;
using UnityEngine;

namespace EnemyWaves.Gameplay.Enemies
{
    public interface IEnemyFactory
    {
        EnemyController Spawn(EnemyDefinition definition, Vector3 position);
        void Despawn(EnemyController enemy);
    }
}
