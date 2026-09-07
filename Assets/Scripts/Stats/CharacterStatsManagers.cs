using System;
using UnityEngine;
using System.Collections;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Fight;

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

        [field: Header("VFX Name")]
        [field: SerializeField] public string HitVFXName { get; private set; }


        [Header("Stats Infor")]
        public StatsManagers maxHealth;
        public StatsManagers mana;
        public StatsManagers attack;
        public StatsManagers speed;
        public StatsManagers defense;
        public StatsManagers critical;
        public StatsManagers luck;

        public int level;

        public CharacterType characterType;
        public Sprite icon;
        public float CurrentHealth;
        public float CurrentMana;
        public float CurrentAttackDamage;
        public float CurrentSpeed;
        public float CurrentDefense;
        public float CurrentCritical;
        public float CurrentLuck;


        public bool IsDeath { get; private set; }
        public event Action DyingAction = delegate { };
        public event Action<float> HealthUpdateAction = delegate { };
        public event Action<float> ManaUpdateAction = delegate { };

        public bool IsDodge { get; set; }
        public bool IsBlock { get; set; }


        private float currentChance;
        private void OnEnable()
        {
            SetupLevelByType();

            // Initialize stats from ScriptableObject + level scaling
            maxHealth = new StatsManagers(baseStatsData.Health, level);
            attack = new StatsManagers(baseStatsData.AttackPower, level);
            speed = new StatsManagers(baseStatsData.Speed, level);
            defense = new StatsManagers(baseStatsData.Defense, level);
            critical = new StatsManagers(baseStatsData.Critical, level);
            mana = new StatsManagers(baseStatsData.Mana, level);
            luck = new StatsManagers(baseStatsData.Luck, level);

            icon = baseStatsData.Icon;
            characterType = baseStatsData.Type;

            CurrentHealth = maxHealth.GetFinalValue();
            CurrentMana = 10;
            CurrentAttackDamage = attack.GetFinalValue();
            CurrentSpeed = speed.GetFinalValue();
            CurrentDefense = defense.GetFinalValue();
            CurrentCritical = critical.GetFinalValue();
            CurrentLuck = luck.GetFinalValue();

            IsDeath = false;
        }

        private void SetupLevelByType()
        {
            if (characterType == CharacterType.Player)
            {
                level = PlayerTeam.Instance.TeamLevel;
            }
        }

        /// <summary>
        /// Applies damage to this character
        /// Return turn false if the damage was fully avoided (Block / Dogge)
        /// </summary>

        public bool TakeDamage(float damage, bool isCrit, string hitVfxName)
        {
            if (IsDodge)
            {
                SpawnText("DODGE");
                StartCoroutine(SlowTime());
                return false;
            }

            if (IsBlock)
            {
                // Recover mana if block succesfully
                CurrentMana = Mathf.Min(CurrentMana + 10f, mana.GetFinalValue());
                ManaUpdateAction?.Invoke(CurrentMana / mana.GetFinalValue());

                ObjectPoolingManagers.Instance.GetPooledObject("BlockVFX",
                new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z))
                .transform.Rotate(0f, 0f, -90f);
                StartCoroutine(PauseTime());
                SpawnText("BLOCK");
                return false;
            }

            if (isCrit)
            {
                SpawnText("CRIT");
            }

            var finalDamage = Mathf.Max(damage - CurrentDefense, 0f);
            CurrentHealth = Mathf.Max(CurrentHealth - finalDamage, 0f);
            ObjectPoolingManagers.Instance.GetPooledObject(hitVfxName, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));

            SpawnText(finalDamage.ToString());

            if (CurrentHealth <= 0)
            {
                IsDeath = true;
            }

            HealthUpdateAction?.Invoke(CurrentHealth / maxHealth.GetFinalValue());
            return true;
        }

        public bool RandomCritical()
        {
            currentChance = luck.GetFinalValue();

            if (UnityEngine.Random.value < currentChance)
            {
                currentChance = luck.GetFinalValue();
                return true;
            }
            else
            {
                currentChance += luck.GetFinalValue();
                return false;
            }
        }

        public float CalculateCriticalDamage()
        {
            return CurrentAttackDamage * (1 + CurrentLuck * (CurrentCritical - 1));
        }

        public void CallDyingEvent()
        {
            DyingAction?.Invoke();
        }

        public void Healing(float amount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth.GetFinalValue());
            HealthUpdateAction?.Invoke(CurrentHealth / maxHealth.GetFinalValue());
        }

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

        public void SetIsDodge(bool state)
        {
            IsDodge = state;
        }

        public void SetIsBlock(bool state)
        {
            IsBlock = state;
        }

        private IEnumerator SlowTime()
        {
            Time.timeScale = .3f;
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1f;
        }

        private IEnumerator PauseTime()
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(.1f);
            Time.timeScale = 1f;
        }

        public void IncreaseDefense(float amount)
        {
            CurrentDefense += amount;
        }

        public void IncreaseSpeed(float amount)
        {
            CurrentSpeed += amount;
        }

        public void IncreaseDamage(float amount)
        {
            CurrentAttackDamage += amount;
        }

        public void IncreaseCritical(float amount)
        {
            CurrentCritical += amount;
        }

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
            // TODO: Implement experience and level up logic
        }
    }
}
