using UnityEngine;

namespace ConquerTheStars.Stats
{
    public enum CharacterType
    {
        Player,
        Enemy
    }
    public class CharacterStatsManagers : MonoBehaviour
    {
        [Header("Stats Data")]
        [SerializeField] private StatsData baseStatsData;

        [Header("Stats Infor")]
        public StatsManagers maxHealth;
        public StatsManagers attack;
        public StatsManagers speed;
        public StatsManagers defense;
        public StatsManagers critical;
        public CharacterType characterType;
        public float CurrentHealth;

        private void Awake()
        {
            maxHealth = new StatsManagers(baseStatsData.Health);
            attack = new StatsManagers(baseStatsData.AttackPower);
            speed = new StatsManagers(baseStatsData.Speed);
            defense = new StatsManagers(baseStatsData.Defense);
            critical = new StatsManagers(baseStatsData.Critical);
            characterType = baseStatsData.Type;
            CurrentHealth = maxHealth.GetFinalValue();
        }

        public void TakeDamage(float damage)
        {
            var finalDamage = Mathf.Max(damage - defense.GetFinalValue(), 0f);
            CurrentHealth = Mathf.Max(CurrentHealth - finalDamage, 0f); ;
            if (CurrentHealth <= 0)
            {
                Debug.Log("Die");
            }
        }

    }
}
