using UnityEngine;
using UnityEngine.UI;

namespace EnemyWaves.UI
{
    public class FillBarAnimator
    {
        private readonly Image _image;
        private readonly float _unitsPerSecond;

        private float _target;
        private bool _hasValue;

        public FillBarAnimator(Image image, float unitsPerSecond)
        {
            _image = image;
            _unitsPerSecond = Mathf.Max(0.01f, unitsPerSecond);
        }

        public void SetTarget(float fraction)
        {
            _target = Mathf.Clamp01(fraction);

            if (_hasValue)
                return;

            _hasValue = true;
            Apply(_target);
        }

        public void Reset(float fraction)
        {
            _target = Mathf.Clamp01(fraction);
            _hasValue = true;
            Apply(_target);
        }

        public void Tick(float deltaTime)
        {
            if (_image == null || !_hasValue)
                return;

            float current = _image.fillAmount;
            if (Mathf.Approximately(current, _target))
                return;

            Apply(Mathf.MoveTowards(current, _target, _unitsPerSecond * deltaTime));
        }

        private void Apply(float fraction)
        {
            if (_image != null)
                _image.fillAmount = fraction;
        }
    }
}
