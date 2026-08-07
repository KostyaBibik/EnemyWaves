using UnityEngine;

namespace EnemyWaves.Core
{
    /// <summary>Anything that can be targeted by the player's weapon or by enemies (currently only the player and enemies).</summary>
    public interface ITargetProvider
    {
        Transform Transform { get; }
        IDamageable Damageable { get; }
    }
}
