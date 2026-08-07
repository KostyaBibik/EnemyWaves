using EnemyWaves.Configs;
using EnemyWaves.Services;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        private IInputService _inputService;
        private PlayerConfig _config;
        private CharacterController _controller;

        [Inject]
        public void Construct(IInputService inputService, PlayerConfig config)
        {
            _inputService = inputService;
            _config = config;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector2 input = _inputService.MoveDirection;
            if (input.sqrMagnitude < 0.0001f)
                return;

            Vector3 move = new Vector3(input.x, 0f, input.y);
            move = Vector3.ClampMagnitude(move, 1f) * (_config.MoveSpeed * Time.deltaTime);

            _controller.Move(move + Physics.gravity * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, new Vector3(input.x, 0f, input.y).normalized, 15f * Time.deltaTime);
        }
    }
}
