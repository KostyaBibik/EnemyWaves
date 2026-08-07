using System;
using EnemyWaves.Gameplay.Player;
using EnemyWaves.Services;
using UniRx;
using UnityEngine;
using Zenject;

namespace EnemyWaves.UI
{
    public class HudPresenter : IInitializable, IDisposable
    {
        private readonly HudView _view;
        private readonly PlayerModel _playerModel;
        private readonly IGameStateService _gameState;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public HudPresenter(HudView view, PlayerModel playerModel, IGameStateService gameState)
        {
            _view = view;
            _playerModel = playerModel;
            _gameState = gameState;
        }

        public void Initialize()
        {
            _playerModel.Health
                .Subscribe(h => _view.SetPlayerHealthFraction(h / _playerModel.MaxHealth))
                .AddTo(_disposables);

            _gameState.SurvivalTime
                .Select(Mathf.FloorToInt)
                .DistinctUntilChanged()
                .Subscribe(_view.SetSurvivalTime)
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
