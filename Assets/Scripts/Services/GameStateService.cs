using System;
using UniRx;
using Zenject;

namespace EnemyWaves.Services
{
    public class GameStateService : IGameStateService, ITickable
    {
        private readonly ReactiveProperty<float> _survivalTime = new ReactiveProperty<float>(0f);
        private readonly ReactiveProperty<bool> _isGameOver = new ReactiveProperty<bool>(false);
        private readonly Subject<Unit> _onGameOver = new Subject<Unit>();

        public IReadOnlyReactiveProperty<float> SurvivalTime => _survivalTime;
        public IReadOnlyReactiveProperty<bool> IsGameOver => _isGameOver;
        public IObservable<Unit> OnGameOver => _onGameOver;

        public void Tick()
        {
            if (_isGameOver.Value)
                return;

            _survivalTime.Value += UnityEngine.Time.deltaTime;
        }

        public void ReportPlayerDeath()
        {
            if (_isGameOver.Value)
                return;

            _isGameOver.Value = true;
            _onGameOver.OnNext(Unit.Default);
        }
    }
}
