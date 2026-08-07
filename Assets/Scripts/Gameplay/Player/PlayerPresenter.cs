using System;
using EnemyWaves.Services;
using UniRx;
using Zenject;

namespace EnemyWaves.Gameplay.Player
{
    public class PlayerPresenter : IInitializable, IDisposable
    {
        private readonly PlayerModel _model;
        private readonly IGameStateService _gameState;
        private IDisposable _subscription;

        public PlayerPresenter(PlayerModel model, IGameStateService gameState)
        {
            _model = model;
            _gameState = gameState;
        }

        public void Initialize()
        {
            _subscription = _model.IsAlive
                .Where(alive => !alive)
                .Subscribe(_ => _gameState.ReportPlayerDeath());
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
