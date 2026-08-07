using UnityEngine;

namespace EnemyWaves.Gameplay.Enemies
{
    /// <summary>
    /// Procedural hop for enemy models that ship without animation clips.
    /// Drives a vertical sine and a squash/stretch deformation off the same phase,
    /// so the body flattens on landing and elongates at the top of the arc.
    /// </summary>
    public class EnemyHopAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _visual;

        [Tooltip("Full hop cycles per second.")]
        [Min(0.01f)] [SerializeField] private float _hopsPerSecond = 2.2f;

        [Tooltip("Peak height of the hop, in world units.")]
        [Min(0f)] [SerializeField] private float _hopHeight = 0.3f;

        [Tooltip("How strongly the body deforms. 0 = rigid.")]
        [Range(0f, 0.6f)] [SerializeField] private float _deform = 0.18f;

        private Vector3 _baseScale;
        private Vector3 _basePosition;
        private float _phase;

        private void Awake()
        {
            if (_visual == null)
                _visual = transform;

            _baseScale = _visual.localScale;
            _basePosition = _visual.localPosition;
        }

        private void OnEnable()
        {
            // Enemies are pooled and often spawn on the same frame; desync so they
            // do not hop in lockstep.
            _phase = Random.value * Mathf.PI * 2f;
        }

        private void Update()
        {
            _phase += _hopsPerSecond * Mathf.PI * 2f * Time.deltaTime;

            float wave = Mathf.Sin(_phase);
            float lift = Mathf.Max(0f, wave);

            _visual.localPosition = _basePosition + Vector3.up * (lift * _hopHeight);

            float vertical = 1f + wave * _deform;
            float horizontal = 1f - wave * _deform * 0.5f;

            _visual.localScale = new Vector3(
                _baseScale.x * horizontal,
                _baseScale.y * vertical,
                _baseScale.z * horizontal);
        }
    }
}
