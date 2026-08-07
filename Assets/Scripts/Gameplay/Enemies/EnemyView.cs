using EnemyWaves.UI;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyWaves.Gameplay.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Image _healthFillImage;
        [SerializeField] private Transform _billboardRoot;

        [Tooltip("How fast the health bar sweeps to a new value, in fill units per second.")]
        [Min(0.01f)] [SerializeField] private float _healthBarSpeed = 1.8f;

        private Transform _cameraTransform;
        private FillBarAnimator _healthBarInstance;

        private FillBarAnimator HealthBar =>
            _healthBarInstance ??= new FillBarAnimator(_healthFillImage, _healthBarSpeed);

        private void Awake()
        {
            ResolveCamera();
        }

        private void OnEnable()
        {
            ResolveCamera();
            AlignToCamera();
        }

        public void SetHealthFraction(float fraction)
        {
            HealthBar.SetTarget(fraction);
        }

        public void ResetHealthFraction(float fraction)
        {
            HealthBar.Reset(fraction);
        }

        private void LateUpdate()
        {
            HealthBar.Tick(Time.deltaTime);
            AlignToCamera();
        }

        private void AlignToCamera()
        {
            if (_billboardRoot == null)
                return;

            if (_cameraTransform == null)
            {
                ResolveCamera();
                if (_cameraTransform == null)
                    return;
            }

            _billboardRoot.rotation = _cameraTransform.rotation;
        }

        private void ResolveCamera()
        {
            var camera = Camera.main;
            _cameraTransform = camera != null ? camera.transform : null;
        }
    }
}
