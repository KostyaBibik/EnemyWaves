using UnityEngine.SceneManagement;

namespace EnemyWaves.Services
{
    public class SceneLoaderService : ISceneLoaderService
    {
        public const string LoadingSceneName = "Loading";
        public const string GameplaySceneName = "Gameplay";

        public void LoadGameplay()
        {
            SceneManager.LoadScene(GameplaySceneName);
        }

        public void RestartToLoading()
        {
            SceneManager.LoadScene(LoadingSceneName);
        }
    }
}
