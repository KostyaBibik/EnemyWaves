using System;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyWaves.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private Text _survivalTimeText;
        [SerializeField] private Button _restartButton;

        public event Action RestartRequested;

        private void Awake()
        {
            if (_root != null)
                _root.SetActive(false);

            if (_restartButton != null)
                _restartButton.onClick.AddListener(() => RestartRequested?.Invoke());
        }

        public void Show(float survivalSeconds)
        {
            if (_root != null)
                _root.SetActive(true);

            if (_survivalTimeText == null)
                return;

            int totalSeconds = Mathf.FloorToInt(survivalSeconds);
            _survivalTimeText.text = $"You survived {totalSeconds / 60:00}:{totalSeconds % 60:00}";
        }
    }
}
