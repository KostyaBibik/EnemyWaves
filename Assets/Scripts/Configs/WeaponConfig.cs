using UnityEngine;

namespace EnemyWaves.Configs
{
    [CreateAssetMenu(menuName = "EnemyWaves/Configs/Weapon Config", fileName = "WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [Min(0.01f)] public float FireRate = 2f; // shots per second
        [Min(0f)] public float Damage = 10f;
        [Min(0f)] public float Range = 8f;
        [Min(0f)] public float ProjectileSpeed = 20f;
        public GameObject ProjectilePrefab;
    }
}
