using System.Collections;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConquerTheStars.UI.Player
{
    public class PlayerSetupUI : MonoBehaviour
    {
        private readonly string UiName = "Character_HUD";

        [Header("Character Stats Manager")]
        [SerializeField]
        private CharacterStatsManagers characterStatsManagers;
        [SerializeField]
        private BuffManager buffManager;


        [Header("Time To Update UI")]
        [SerializeField]
        private float healthUpdate;
        [SerializeField] private float manaUpdate;

        private PlayerHUD characterHud;
        private Image health;
        private Image mana;
        private TextMeshProUGUI healthText;
        private TextMeshProUGUI manaText;


        private void OnEnable()
        {
            characterStatsManagers.HealthUpdateAction += HealthUpdate;
            characterStatsManagers.ManaUpdateAction += ManaUpdate;

        }

        private void OnDisable()
        {
            characterStatsManagers.HealthUpdateAction -= HealthUpdate;
            characterStatsManagers.ManaUpdateAction -= ManaUpdate;

        }

        public void SpawnCharacterHUD()
        {
            // Spawn character HUD ui
            characterHud = ObjectPoolingManagers.Instance.GetPooledObject(UiName, Vector3.zero).GetComponent<PlayerHUD>();

            UICombatManagers.Instance.AddUiPooledObjectList(characterHud.GetComponent<PooledObject>());

            characterHud.GetComponent<RectTransform>().SetParent(UICombatManagers.Instance?.StatusPanel);

            //Setup hud base characterStatsManagers
            characterHud.Icon.sprite = characterStatsManagers.icon;
            health = characterHud.Health;
            mana = characterHud.Mana;
            healthText = characterHud.HealthText;
            manaText = characterHud.ManaText;
        }

        public void InactiveCharacterHUD()
        {
            characterHud.gameObject.SetActive(false);
        }

        public void SetupStatusUI(float healthValue, float manaValue)
        {
            health.fillAmount = healthValue;
            mana.fillAmount = manaValue;
            healthText.SetText($"{characterStatsManagers.CurrentHealth}/{characterStatsManagers.maxHealth.GetFinalValue()}");
            manaText.SetText($"{characterStatsManagers.CurrentMana}/{characterStatsManagers.mana.GetFinalValue()}");

        }

        public void HealthUpdate(float target)
        {
            StartCoroutine(HealthChanging(health, target));
            healthText.SetText($"{characterStatsManagers.CurrentHealth}/{characterStatsManagers.maxHealth.GetFinalValue()}");
        }

        public void ManaUpdate(float target)
        {
            StartCoroutine(HealthChanging(mana, target));
            manaText.SetText($"{characterStatsManagers.CurrentMana}/{characterStatsManagers.mana.GetFinalValue()}");
        }

        private IEnumerator HealthChanging(Image targetFill, float target)
        {
            float elapsedTime = 0f;
            var currentHealth = targetFill.fillAmount;
            while (elapsedTime < healthUpdate)
            {
                elapsedTime += Time.deltaTime;
                var percentageTime = Mathf.Clamp01(elapsedTime / healthUpdate);
                targetFill.fillAmount = Mathf.Lerp(currentHealth, target, percentageTime);
                yield return null;
            }
            targetFill.fillAmount = target;
        }
    }
}
