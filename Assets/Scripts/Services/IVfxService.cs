using UnityEngine;

namespace EnemyWaves.Services
{
    public interface IVfxService
    {
        void Play(GameObject prefab, Vector3 position);
    }
}
