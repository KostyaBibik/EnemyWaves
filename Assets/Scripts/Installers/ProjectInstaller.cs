using EnemyWaves.Configs;
using EnemyWaves.Services;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ApplicationConfig _applicationConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_applicationConfig);
            Container.BindInterfacesTo<ApplicationSettingsService>().AsSingle();
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
        }
    }
}
