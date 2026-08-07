using System.Collections.Generic;
using EnemyWaves.Core;
using UnityEngine;

namespace EnemyWaves.Services
{
    /// <summary>Tracks currently alive enemies so the player's weapon can find the nearest one without a per-frame scene scan.</summary>
    public interface ITargetRegistry
    {
        IReadOnlyList<ITargetProvider> Enemies { get; }
        void Register(ITargetProvider enemy);
        void Unregister(ITargetProvider enemy);
        ITargetProvider FindNearestEnemy(Vector3 fromPosition, float maxRange);
    }
}
