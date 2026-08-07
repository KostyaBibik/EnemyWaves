using UnityEngine;

namespace EnemyWaves.Configs
{
    [CreateAssetMenu(menuName = "EnemyWaves/Configs/Vfx Config", fileName = "VfxConfig")]
    public class VfxConfig : ScriptableObject
    {
        public GameObject EnemyHit;
        public GameObject EnemyDeath;
        public GameObject PlayerHit;

        [Tooltip("Extra seconds an effect is kept alive past its computed lifetime before returning to the pool.")]
        [Min(0f)] public float ReleasePadding = 0.25f;
    }
}
