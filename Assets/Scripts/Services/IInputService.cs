using UnityEngine;

namespace EnemyWaves.Services
{
    /// <summary>Abstracts the on-screen joystick asset away from gameplay code.</summary>
    public interface IInputService
    {
        Vector2 MoveDirection { get; }
    }
}
