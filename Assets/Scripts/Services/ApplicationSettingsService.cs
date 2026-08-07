using EnemyWaves.Configs;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Services
{
    public class ApplicationSettingsService : IInitializable
    {
        private readonly ApplicationConfig _config;

        public ApplicationSettingsService(ApplicationConfig config)
        {
            _config = config;
        }

        public void Initialize()
        {
            QualitySettings.vSyncCount = _config.EnableVSync ? 1 : 0;
            Application.targetFrameRate = _config.TargetFrameRate;

            if (_config.PreventScreenDimming)
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
