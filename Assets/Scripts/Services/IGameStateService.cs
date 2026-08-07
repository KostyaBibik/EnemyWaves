using System;
using UniRx;

namespace EnemyWaves.Services
{
    public interface IGameStateService
    {
        IReadOnlyReactiveProperty<float> SurvivalTime { get; }
        IReadOnlyReactiveProperty<bool> IsGameOver { get; }
        IObservable<Unit> OnGameOver { get; }

        void ReportPlayerDeath();
    }
}
