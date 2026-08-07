using EnemyWaves.Configs;
using EnemyWaves.Services;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        private const float FacingTurnSpeed = 12f;

        private IInputService _inputService;
        private PlayerConfig _config;
        private WeaponConfig _weaponConfig;
        private ITargetRegistry _targetRegistry;
        private CharacterController _controller;

        /// <summary>Move input expressed relative to the current facing (x = strafe, y = forward). Drives the locomotion blend tree.</summary>
        public Vector2 LocalMoveInput { get; private set; }

        [Inject]
        public void Construct(IInputService inputService, PlayerConfig config, WeaponConfig weaponConfig, ITargetRegistry targetRegistry)
        {
            _inputService = inputService;
            _config = config;
            _weaponConfig = weaponConfig;
            _targetRegistry = targetRegistry;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector2 input = _inputService.MoveDirection;
            Vector3 worldMove = new Vector3(input.x, 0f, input.y);
            bool hasInput = worldMove.sqrMagnitude > 0.0001f;

            if (hasInput)
            {
                Vector3 clamped = Vector3.ClampMagnitude(worldMove, 1f);
                _controller.Move(clamped * (_config.MoveSpeed * Time.deltaTime) + Physics.gravity * Time.deltaTime);
            }

            UpdateFacing(worldMove, hasInput);
            LocalMoveInput = hasInput ? ToLocalPlane(worldMove.normalized) : Vector2.zero;
        }

        // Facing tracks the nearest enemy in weapon range (the same target WeaponController auto-fires at),
        // falling back to move direction so the player still turns to face somewhere while nothing is in range.
        private void UpdateFacing(Vector3 worldMove, bool hasInput)
        {
            var target = _targetRegistry.FindNearestEnemy(transform.position, _weaponConfig.Range);
            Vector3 desiredForward;

            if (target != null)
            {
                desiredForward = target.Transform.position - transform.position;
                desiredForward.y = 0f;
            }
            else if (hasInput)
            {
                desiredForward = worldMove;
            }
            else
            {
                return;
            }

            if (desiredForward.sqrMagnitude < 0.0001f)
                return;

            transform.forward = Vector3.Slerp(transform.forward, desiredForward.normalized, FacingTurnSpeed * Time.deltaTime);
        }

        private Vector2 ToLocalPlane(Vector3 worldDirection)
        {
            Vector3 local = transform.InverseTransformDirection(worldDirection);
            return new Vector2(local.x, local.z);
        }
    }
}
