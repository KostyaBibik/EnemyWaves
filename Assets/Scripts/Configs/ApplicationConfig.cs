using UnityEngine;

namespace EnemyWaves.Configs
{
    [CreateAssetMenu(menuName = "EnemyWaves/Configs/Application Config", fileName = "ApplicationConfig")]
    public class ApplicationConfig : ScriptableObject
    {
        [Tooltip("Frame rate the game aims for. Unity defaults to 30 on mobile, so this must be set explicitly.")]
        [Min(15)] public int TargetFrameRate = 60;

        [Tooltip("Leave off so targetFrameRate governs pacing; VSync would override it.")]
        public bool EnableVSync = false;

        [Tooltip("Keep the screen awake during play.")]
        public bool PreventScreenDimming = true;
    }
}
