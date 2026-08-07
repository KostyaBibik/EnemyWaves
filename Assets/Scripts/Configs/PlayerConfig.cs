using UnityEngine;

namespace EnemyWaves.Configs
{
    [CreateAssetMenu(menuName = "EnemyWaves/Configs/Player Config", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Min(1f)] public float MaxHealth = 100f;
        [Min(0f)] public float MoveSpeed = 6f;
    }
}
