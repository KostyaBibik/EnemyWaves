using System;
using EnemyWaves.Services;
using UniRx;
using Zenject;

namespace EnemyWaves.UI
{
    public class GameOverPresenter : IInitializable, IDisposable
    {
        private readonly GameOverView _view;
        private readonly IGameStateService _gameState;
        private readonly ISceneLoaderService _sceneLoader;
        private IDisposable _subscription;

        public GameOverPresenter(GameOverView view, IGameStateService gameState, ISceneLoaderService sceneLoader)
        {
            _view = view;
            _gameState = gameState;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _subscription = _gameState.OnGameOver.Subscribe(_ => _view.Show(_gameState.SurvivalTime.Value));
            _view.RestartRequested += OnRestartRequested;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _view.RestartRequested -= OnRestartRequested;
        }

        private void OnRestartRequested()
        {
            _sceneLoader.RestartToLoading();
        }
    }
}
