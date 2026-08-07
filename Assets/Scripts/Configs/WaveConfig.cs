using UnityEngine;

namespace EnemyWaves.Configs
{
    [CreateAssetMenu(menuName = "EnemyWaves/Configs/Wave Config", fileName = "WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        [Min(1)] public int MinAliveEnemies = 5;
        [Min(0f)] public float SpawnRadius = 12f;
        [Min(0f)] public float SpawnCheckInterval = 0.5f;
    }
}
