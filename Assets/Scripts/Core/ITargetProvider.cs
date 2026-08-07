using UnityEngine;

namespace EnemyWaves.Core
{
    public interface ITargetProvider
    {
        Transform Transform { get; }
        IDamageable Damageable { get; }
    }
}
