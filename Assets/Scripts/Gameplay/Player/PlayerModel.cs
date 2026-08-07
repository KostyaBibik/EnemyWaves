using UniRx;

namespace EnemyWaves.Gameplay.Player
{
    public class PlayerModel
    {
        public readonly ReactiveProperty<float> Health;
        public readonly float MaxHealth;
        public readonly ReadOnlyReactiveProperty<bool> IsAlive;

        public PlayerModel(float maxHealth)
        {
            MaxHealth = maxHealth;
            Health = new ReactiveProperty<float>(maxHealth);
            IsAlive = Health.Select(h => h > 0f).ToReadOnlyReactiveProperty();
        }

        public void TakeDamage(float amount)
        {
            if (Health.Value <= 0f)
                return;

            Health.Value = UnityEngine.Mathf.Max(0f, Health.Value - amount);
        }
    }
}
