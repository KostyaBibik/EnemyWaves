using UnityEngine;

namespace EnemyWaves.Services
{
    public class JoystickInputService : MonoBehaviour, IInputService
    {
        [SerializeField] private Joystick _joystick;

        public Vector2 MoveDirection => _joystick != null ? _joystick.Direction : Vector2.zero;
    }
}
