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

        public bool IsDeath { get; private set; }
        public event Action IsDyingAction = delegate { };
        public event Action<float> HealthUpdateAction = delegate { };
        public event Action<float> ManaUpdateAction = delegate { };

        public float DamageReceived { get; set; }
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
            IsDeath = false;
        }

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

            var finalDamage = Mathf.Max(damage - defense.GetFinalValue(), 0f);
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

        private void SpawnText(string text)
        {

            Vector3 destination = transform.position;
            // destination.x += UnityEngine.Random.Range(-.5f, 1f);
            destination.y += UnityEngine.Random.Range(.5f, 1.5f);
            destination.z += UnityEngine.Random.Range(1f, 2f);

            DynamicTextManager.CreateText(destination, text, textData);
        }

        private IEnumerator SlowTime()
        {
            Time.timeScale = .3f;
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1f;
        }

        public void AddExp(int exp)
        {

        }
    }
}
