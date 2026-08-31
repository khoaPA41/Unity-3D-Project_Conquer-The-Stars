using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConquerTheStars.Pattern.Object_Pooling;

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

        [Header("Dynamic Text")]
        [SerializeField] private DynamicTextData textData;

        [Header("Stats Infor")]
        public StatsManagers maxHealth;
        public StatsManagers mana;
        public StatsManagers attack;
        public StatsManagers speed;
        public StatsManagers defense;
        public StatsManagers critical;
        public int level;

        public CharacterType characterType;
        public Sprite icon;
        public float CurrentHealth;
        public float CurrentMana;
        public float CurrentAttackDamage;
        public float CurrentSpeed;
        public float CurrentDefense;
        public float CurrentCritical;


        public bool IsDeath { get; private set; }
        public event Action IsDyingAction = delegate { };
        public event Action<float> HealthUpdateAction = delegate { };
        public event Action<float> ManaUpdateAction = delegate { };

        public float DamageReceived { get; set; }
        public List<float> DamageHistories { get; set; } = new();
        public int SuccessfulParryTimes { get; set; }
        public int SuccessfulDodgeTimes { get; set; }
        private bool isDodge;
        private bool isBlock;

        private void OnEnable()
        {
            /*Setup Value*/

            maxHealth = new StatsManagers(baseStatsData.Health, level);
            attack = new StatsManagers(baseStatsData.AttackPower, level);
            speed = new StatsManagers(baseStatsData.Speed, level);
            defense = new StatsManagers(baseStatsData.Defense, level);
            critical = new StatsManagers(baseStatsData.Critical, level);
            mana = new StatsManagers(baseStatsData.Mana, level);

            icon = baseStatsData.Icon;
            /************************/

            characterType = baseStatsData.Type;
            CurrentHealth = maxHealth.GetFinalValue();
            CurrentMana = 10;
            CurrentAttackDamage = attack.GetFinalValue();
            CurrentSpeed = speed.GetFinalValue();
            CurrentDefense = defense.GetFinalValue();
            CurrentCritical = critical.GetFinalValue();
            IsDeath = false;
        }

        /*************************************Health*************************************/
        public bool TakeDamage(float damage)
        {
            if (isDodge)
            {
                SpawnText("DODGE");
                StartCoroutine(SlowTime());
                SuccessfulDodgeTimes++; // Calculate result infor
                return false;
            }

            if (isBlock)
            {
                CurrentMana = Mathf.Min(CurrentMana + 10f, mana.GetFinalValue());
                ManaUpdateAction?.Invoke(CurrentMana / mana.GetFinalValue());
                ObjectPoolingManagers.Instance.GetPooledObject("BlockVFX",
                new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z))
                .transform.Rotate(0f, 0f, -90f);
                StartCoroutine(SlowTime());
                SpawnText("BLOCK");
                SuccessfulParryTimes++; // Calculate result infor
                return false;
            }

            var finalDamage = Mathf.Max(damage - CurrentDefense, 0f);
            CurrentHealth = Mathf.Max(CurrentHealth - finalDamage, 0f);

            DamageReceived += damage; // Calculate result infor
            SpawnText(finalDamage.ToString());

            if (CurrentHealth <= 0)
            {
                IsDeath = true;
            }
            HealthUpdateAction?.Invoke(CurrentHealth / maxHealth.GetFinalValue());
            return true;
        }

        public void CallDyingEvent()
        {
            IsDyingAction?.Invoke();
        }

        public void Healing(float amount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth.GetFinalValue());
            HealthUpdateAction?.Invoke(CurrentHealth / maxHealth.GetFinalValue());
        }

        /*************************************Mana*************************************/
        public void AddMana(float amount)
        {
            CurrentMana = Mathf.Min(CurrentMana + amount, mana.GetFinalValue());
            ManaUpdateAction?.Invoke(CurrentMana / mana.GetFinalValue());
        }
        public void SubtractMana(float value)
        {
            CurrentMana = Mathf.Max(CurrentMana - value, 0f);
            ManaUpdateAction?.Invoke(CurrentMana / mana.GetFinalValue());
        }

        /*************************************Situation Award*************************************/
        public void SetIsDodge(bool state)
        {
            isDodge = state;
        }

        public void SetIsBlock(bool state)
        {
            isBlock = state;
        }

        private IEnumerator SlowTime()
        {
            Time.timeScale = .3f;
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1f;
        }

        /*************************************Defense*************************************/
        public void IncreaseDefense(float amount)
        {
            // CurrentDefense = Mathf.Min(CurrentDefense + amount, defense.GetFinalValue());
            CurrentDefense += amount;
        }

        /*************************************Speed*************************************/
        public void IncreaseSpeed(float amount)
        {
            // CurrentSpeed = Mathf.Min(CurrentSpeed + amount, speed.GetFinalValue());
            CurrentSpeed += amount;

        }

        /*************************************Damage*************************************/
        public void IncreaseDamage(float amount)
        {
            // CurrentAttackDamage = Mathf.Min(CurrentAttackDamage + amount, attack.GetFinalValue());
            CurrentAttackDamage += amount;

        }

        /*************************************Critical*************************************/
        public void IncreaseCritical(float amount)
        {
            // CurrentCritical = Mathf.Min(CurrentCritical + amount, defense.GetFinalValue());
            CurrentCritical += amount;

        }

        /*************************************Dynamic Text*************************************/
        private void SpawnText(string text)
        {

            Vector3 destination = transform.position;
            // destination.x += UnityEngine.Random.Range(-.5f, 1f);
            destination.y += UnityEngine.Random.Range(.5f, 1.5f);
            destination.z += UnityEngine.Random.Range(1f, 2f);

            DynamicTextManager.CreateText(destination, text, textData);
        }


        public void AddExp(int exp)
        {

        }
    }
}
