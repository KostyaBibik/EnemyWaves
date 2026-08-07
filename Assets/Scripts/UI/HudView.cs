using UnityEngine;
using UnityEngine.UI;

namespace EnemyWaves.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private Image _playerHealthFill;
        [SerializeField] private Text _survivalTimeText;

        [Tooltip("How fast the health bar sweeps to a new value, in fill units per second.")]
        [Min(0.01f)] [SerializeField] private float _healthBarSpeed = 1.2f;

        private FillBarAnimator _healthBar;

        private void Awake()
        {
            _healthBar = new FillBarAnimator(_playerHealthFill, _healthBarSpeed);
        }

        public void SetPlayerHealthFraction(float fraction)
        {
            _healthBar.SetTarget(fraction);
        }

        public void SetSurvivalTime(int totalSeconds)
        {
            if (_survivalTimeText == null)
                return;

            _survivalTimeText.text = $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";
        }

        private void Update()
        {
            _healthBar.Tick(Time.deltaTime);
        }
    }
}
