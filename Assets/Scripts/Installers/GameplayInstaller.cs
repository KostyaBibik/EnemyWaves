using EnemyWaves.Configs;
using EnemyWaves.Core;
using EnemyWaves.Gameplay.Enemies;
using EnemyWaves.Gameplay.Player;
using EnemyWaves.Gameplay.Waves;
using EnemyWaves.Gameplay.Weapon;
using EnemyWaves.Services;
using EnemyWaves.UI;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("Configs")]
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private EnemyDatabase _enemyDatabase;
        [SerializeField] private WaveConfig _waveConfig;
        [SerializeField] private VfxConfig _vfxConfig;

        [Header("Scene References")]
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private JoystickInputService _joystickInput;
        [SerializeField] private HudView _hudView;
        [SerializeField] private GameOverView _gameOverView;

        public override void InstallBindings()
        {
            Container.BindInstance(_playerConfig);
            Container.BindInstance(_weaponConfig);
            Container.BindInstance(_enemyDatabase);
            Container.BindInstance(_waveConfig);
            Container.BindInstance(_vfxConfig);

            Container.Bind<PlayerModel>()
                .FromMethod(() => new PlayerModel(_playerConfig.MaxHealth))
                .AsSingle();

            Container.Bind<ITargetProvider>()
                .WithId(PlayerTargetId.Value)
                .FromInstance(_playerHealth)
                .AsSingle();

            Container.Bind<ITargetRegistry>().To<TargetRegistry>().AsSingle();
            Container.Bind<IInputService>().FromInstance(_joystickInput).AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();

            Container.Bind(typeof(IGameStateService), typeof(ITickable))
                .To<GameStateService>()
                .AsSingle();

            Container.Bind(typeof(IVfxService), typeof(ITickable))
                .To<VfxService>()
                .AsSingle();

            Container.BindInterfacesTo<EnemySpawnService>().AsSingle();
            Container.BindInterfacesTo<PlayerPresenter>().AsSingle();

            Container.Bind<HudView>().FromInstance(_hudView).AsSingle();
            Container.BindInterfacesTo<HudPresenter>().AsSingle();

            Container.Bind<GameOverView>().FromInstance(_gameOverView).AsSingle();
            Container.BindInterfacesTo<GameOverPresenter>().AsSingle();

            Container.BindFactory<Vector3, Quaternion, Projectile, Projectile.Factory>()
                .FromPoolableMemoryPool(poolBinder => poolBinder
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(_weaponConfig.ProjectilePrefab)
                    .UnderTransformGroup("Projectiles"));
        }
    }
}
