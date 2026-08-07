using EnemyWaves.Services;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Bootstrap
{
    public class LoadingController : MonoBehaviour
    {
        private ISceneLoaderService _sceneLoader;

        [Inject]
        public void Construct(ISceneLoaderService sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            _sceneLoader.LoadGameplay();
        }
    }
}
