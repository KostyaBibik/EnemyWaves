using System.Collections.Generic;
using UnityEngine;

namespace EnemyWaves.Configs
{
    [CreateAssetMenu(menuName = "EnemyWaves/Configs/Enemy Database", fileName = "EnemyDatabase")]
    public class EnemyDatabase : ScriptableObject
    {
        public List<EnemyDefinition> Enemies = new List<EnemyDefinition>();

        public EnemyDefinition GetRandomWeighted()
        {
            if (Enemies == null || Enemies.Count == 0)
                return null;

            float totalWeight = 0f;
            for (int i = 0; i < Enemies.Count; i++)
                totalWeight += Enemies[i].SpawnWeight;

            float roll = Random.value * totalWeight;
            float cursor = 0f;
            for (int i = 0; i < Enemies.Count; i++)
            {
                cursor += Enemies[i].SpawnWeight;
                if (roll <= cursor)
                    return Enemies[i];
            }

            return Enemies[Enemies.Count - 1];
        }
    }
}
