using UnityEngine;
using UnityEngine.UI;

namespace EnemyWaves.Gameplay.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Image _healthFillImage;
        [SerializeField] private Transform _billboardRoot;

        private Transform _cameraTransform;
        private Quaternion _lastCameraRotation;

        private void Awake()
        {
            var camera = Camera.main;
            if (camera != null)
                _cameraTransform = camera.transform;
        }

        public void SetHealthFraction(float fraction)
        {
            if (_healthFillImage != null)
                _healthFillImage.fillAmount = Mathf.Clamp01(fraction);
        }

        private void LateUpdate()
        {
            if (_billboardRoot == null || _cameraTransform == null)
                return;

            var cameraRotation = _cameraTransform.rotation;
            if (cameraRotation == _lastCameraRotation)
                return;

            _lastCameraRotation = cameraRotation;
            _billboardRoot.forward = -_cameraTransform.forward;
        }
    }
}
