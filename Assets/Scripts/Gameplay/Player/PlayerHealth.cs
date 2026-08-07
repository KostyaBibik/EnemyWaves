using EnemyWaves.Configs;
using EnemyWaves.Core;
using EnemyWaves.Services;
using UnityEngine;
using Zenject;

namespace EnemyWaves.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable, ITargetProvider
    {
        [Tooltip("Height above the pivot where the damage effect is spawned.")]
        [SerializeField] private float _hitEffectHeight;

        private PlayerModel _model;
        private IVfxService _vfxService;
        private VfxConfig _vfxConfig;

        public Transform Transform => transform;
        public IDamageable Damageable => this;
        public bool IsAlive => _model != null && _model.IsAlive.Value;

        [Inject]
        public void Construct(PlayerModel model, IVfxService vfxService, VfxConfig vfxConfig)
        {
            _model = model;
            _vfxService = vfxService;
            _vfxConfig = vfxConfig;
        }

        public void TakeDamage(float amount)
        {
            if (!_model.IsAlive.Value)
                return;

            _model.TakeDamage(amount);
            _vfxService.Play(_vfxConfig.PlayerHit, transform.position + Vector3.up * _hitEffectHeight);
        }
    }
}
