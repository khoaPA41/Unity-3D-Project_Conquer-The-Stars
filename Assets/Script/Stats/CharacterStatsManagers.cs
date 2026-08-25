using System;
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
        public bool IsDeath;
        // { get; private set; }
        public event Action IsDyingAction = delegate { };
        private bool immortal;

        // private void Awake()
        // {
        // maxHealth = new StatsManagers(baseStatsData.Health);
        // attack = new StatsManagers(baseStatsData.AttackPower);
        // speed = new StatsManagers(baseStatsData.Speed);
        // defense = new StatsManagers(baseStatsData.Defense);
        // critical = new StatsManagers(baseStatsData.Critical);
        // characterType = baseStatsData.Type;
        // CurrentHealth = maxHealth.GetFinalValue();
        // }

        private void OnEnable()
        {
            maxHealth = new StatsManagers(baseStatsData.Health);
            attack = new StatsManagers(baseStatsData.AttackPower);
            speed = new StatsManagers(baseStatsData.Speed);
            defense = new StatsManagers(baseStatsData.Defense);
            critical = new StatsManagers(baseStatsData.Critical);
            characterType = baseStatsData.Type;
            CurrentHealth = maxHealth.GetFinalValue();
            IsDeath = false;
        }

        public bool TakeDamage(float damage)
        {
            if (immortal) return false;

            var finalDamage = Mathf.Max(damage - defense.GetFinalValue(), 0f);
            CurrentHealth = Mathf.Max(CurrentHealth - finalDamage, 0f);
            if (CurrentHealth <= 0)
            {
                IsDeath = true;
            }
            return true;
        }

        public void SetImmortal(bool state)
        {
            immortal = state;
        }

        public void CallDyingEvent()
        {
            IsDyingAction?.Invoke();
        }
    }
}
