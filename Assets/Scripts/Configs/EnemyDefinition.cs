using UnityEngine;

namespace EnemyWaves.Configs
{
    /// <summary>
    /// One enemy "type" fully authored by a game designer: model/prefab + stats.
    /// Adding a new enemy to the game = creating one of these assets and dropping it into an EnemyDatabase, no code required.
    /// </summary>
    [CreateAssetMenu(menuName = "EnemyWaves/Configs/Enemy Definition", fileName = "EnemyDefinition")]
    public class EnemyDefinition : ScriptableObject
    {
        public string DisplayName = "Enemy";
        public GameObject Prefab;

        [Min(1f)] public float MaxHealth = 30f;
        [Min(0f)] public float MoveSpeed = 3f;
        [Min(0f)] public float ContactDamage = 10f;
        [Min(0.05f)] public float AttackInterval = 1f;
        [Min(0f)] public float AttackRange = 1.2f;

        [Tooltip("Relative chance to be picked when the spawner rolls a random enemy type.")]
        [Min(0.01f)] public float SpawnWeight = 1f;
    }
}
