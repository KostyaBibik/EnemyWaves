using UniRx;

namespace EnemyWaves.Gameplay.Enemies
{
    public class EnemyModel
    {
        public readonly ReactiveProperty<float> Health = new ReactiveProperty<float>(0f);

        public float MaxHealth { get; private set; }
        public bool IsAlive => Health.Value > 0f;

        public void Reset(float maxHealth)
        {
            MaxHealth = maxHealth;
            Health.Value = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (Health.Value <= 0f)
                return;

            Health.Value = UnityEngine.Mathf.Max(0f, Health.Value - amount);
        }
    }
}
