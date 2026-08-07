using System.Collections.Generic;
using EnemyWaves.Core;
using UnityEngine;

namespace EnemyWaves.Services
{
    public interface ITargetRegistry
    {
        IReadOnlyList<ITargetProvider> Enemies { get; }
        void Register(ITargetProvider enemy);
        void Unregister(ITargetProvider enemy);
        ITargetProvider FindNearestEnemy(Vector3 fromPosition, float maxRange);
    }
}
