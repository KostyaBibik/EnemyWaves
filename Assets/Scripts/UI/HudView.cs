using UnityEngine;
using UnityEngine.UI;

namespace EnemyWaves.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private Image _playerHealthFill;
        [SerializeField] private Text _survivalTimeText;

        public void SetPlayerHealthFraction(float fraction)
        {
            if (_playerHealthFill != null)
                _playerHealthFill.fillAmount = Mathf.Clamp01(fraction);
        }

        public void SetSurvivalTime(int totalSeconds)
        {
            if (_survivalTimeText == null)
                return;

            _survivalTimeText.text = $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";
        }
    }
}
