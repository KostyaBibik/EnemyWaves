using EnemyWaves.Gameplay.Weapon;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Player
{
    /// <summary>
    /// Bridges gameplay state (movement, firing, damage, death) into the player's Animator parameters.
    /// Reads health by polling rather than subscribing in OnEnable, since Zenject injection on scene
    /// objects is not guaranteed to complete before OnEnable runs.
    /// </summary>
    public class PlayerAnimatorDriver : MonoBehaviour
    {
        private static readonly int MoveXHash = Animator.StringToHash("MoveX");
        private static readonly int MoveYHash = Animator.StringToHash("MoveY");
        private static readonly int ShootHash = Animator.StringToHash("Shoot");
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int DieHash = Animator.StringToHash("Die");

        private const float ParameterDamping = 0.12f;

        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerMotor _motor;
        [SerializeField] private WeaponController _weapon;

        private PlayerModel _model;
        private float _lastHealth;
        private bool _healthInitialized;
        private bool _dead;

        [Inject]
        public void Construct(PlayerModel model)
        {
            _model = model;
        }

        private void OnEnable()
        {
            if (_weapon != null)
                _weapon.Fired += HandleFired;
        }

        private void OnDisable()
        {
            if (_weapon != null)
                _weapon.Fired -= HandleFired;
        }

        private void Update()
        {
            if (_motor != null && _animator != null)
            {
                Vector2 move = _motor.LocalMoveInput;
                _animator.SetFloat(MoveXHash, move.x, ParameterDamping, Time.deltaTime);
                _animator.SetFloat(MoveYHash, move.y, ParameterDamping, Time.deltaTime);
            }

            PollHealth();
        }

        private void PollHealth()
        {
            if (_model == null || _dead)
                return;

            float health = _model.Health.Value;
            if (!_healthInitialized)
            {
                _lastHealth = health;
                _healthInitialized = true;
                return;
            }

            if (health <= 0f)
            {
                _animator.SetTrigger(DieHash);
                _dead = true;
            }
            else if (health < _lastHealth)
            {
                _animator.SetTrigger(HitHash);
            }

            _lastHealth = health;
        }

        private void HandleFired()
        {
            if (_animator != null)
                _animator.SetTrigger(ShootHash);
        }
    }
}
