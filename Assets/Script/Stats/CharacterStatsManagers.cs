using System;
using NUnit.Framework;
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
        public StatsManagers mana;
        public StatsManagers attack;
        public StatsManagers speed;
        public StatsManagers defense;
        public StatsManagers critical;
        public CharacterType characterType;
        public float CurrentHealth;
        public float CurrentMana;

        public bool IsDeath { get; private set; }
        public event Action IsDyingAction = delegate { };
        public event Action<float> HealthUpdateAction = delegate { };
        public event Action<float> ManaUpdateAction = delegate { };
        // public event Action<float> IsBlockAction = delegate { };

        private bool isDodge;

        private bool isBlock;

        private void OnEnable()
        {
            /*Setup Value*/
            maxHealth = new StatsManagers(baseStatsData.Health);
            attack = new StatsManagers(baseStatsData.AttackPower);
            speed = new StatsManagers(baseStatsData.Speed);
            defense = new StatsManagers(baseStatsData.Defense);
            critical = new StatsManagers(baseStatsData.Critical);
            mana = new StatsManagers(baseStatsData.Mana);
            /************************/

            characterType = baseStatsData.Type;
            CurrentHealth = maxHealth.GetFinalValue();
            CurrentMana = 10;
            IsDeath = false;
        }

        public bool TakeDamage(float damage)
        {
            if (isDodge) return false;
            if (isBlock)
            {
                CurrentMana = Mathf.Min(CurrentMana + 10f, mana.GetFinalValue());
                ManaUpdateAction?.Invoke(CurrentMana / mana.GetFinalValue());
                return false;
            }

            var finalDamage = Mathf.Max(damage - defense.GetFinalValue(), 0f);
            CurrentHealth = Mathf.Max(CurrentHealth - finalDamage, 0f);
            if (CurrentHealth <= 0)
            {
                IsDeath = true;
            }
            HealthUpdateAction?.Invoke(CurrentHealth / maxHealth.GetFinalValue());
            return true;
        }

        public void SubtractMana(float value)
        {
            CurrentMana = Mathf.Max(CurrentMana - value, 0f);
            ManaUpdateAction?.Invoke(CurrentMana / mana.GetFinalValue());
        }

        public void SetIsDodge(bool state)
        {
            isDodge = state;
        }

        public void SetIsBlock(bool state)
        {
            isBlock = state;
        }

        public void CallDyingEvent()
        {
            IsDyingAction?.Invoke();
        }
    }
}
