using EnemyWaves.Core;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable, ITargetProvider
    {
        private PlayerModel _model;

        public Transform Transform => transform;
        public IDamageable Damageable => this;
        public bool IsAlive => _model != null && _model.IsAlive.Value;

        [Inject]
        public void Construct(PlayerModel model)
        {
            _model = model;
        }

        public void TakeDamage(float amount)
        {
            _model.TakeDamage(amount);
        }
    }
}
