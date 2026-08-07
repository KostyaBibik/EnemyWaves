using UnityEngine;

namespace EnemyWaves.Services
{
    public interface IInputService
    {
        Vector2 MoveDirection { get; }
    }
}
