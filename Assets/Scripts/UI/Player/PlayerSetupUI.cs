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

        private PlayerHUD _characterHud;
        private Image _health;
        private Image _mana;
        private TextMeshProUGUI _healthText;
        private TextMeshProUGUI _manaText;


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
            _characterHud = ObjectPoolingManagers.Instance.GetPooledObject(UiName, Vector3.zero).GetComponent<PlayerHUD>();

            UICombatManagers.Instance.AddUiPooledObjectList(_characterHud.GetComponent<PooledObject>());

            _characterHud.GetComponent<RectTransform>().SetParent(UICombatManagers.Instance?.StatusPanel);

            //Setup hud base characterStatsManagers
            _characterHud.Icon.sprite = characterStatsManagers.icon;
            _health = _characterHud.Health;
            _mana = _characterHud.Mana;
            _healthText = _characterHud.HealthText;
            _manaText = _characterHud.ManaText;
        }

        public void InactiveCharacterHUD()
        {
            _characterHud.gameObject.SetActive(false);
        }

        public void ReturnToPool()
        {
            _characterHud.PooledObject.Release();
        }

        public void SetupStatusUI(float healthValue, float manaValue)
        {
            _health.fillAmount = healthValue;
            _mana.fillAmount = manaValue;
            _healthText.SetText($"{characterStatsManagers.CurrentHealth}/{characterStatsManagers.maxHealth.GetFinalValue()}");
            _manaText.SetText($"{characterStatsManagers.CurrentMana}/{characterStatsManagers.mana.GetFinalValue()}");

        }

        public void HealthUpdate(float target)
        {
            StartCoroutine(HealthChanging(_health, target));
            _healthText.SetText($"{characterStatsManagers.CurrentHealth}/{characterStatsManagers.maxHealth.GetFinalValue()}");
        }

        public void ManaUpdate(float target)
        {
            StartCoroutine(HealthChanging(_mana, target));
            _manaText.SetText($"{characterStatsManagers.CurrentMana}/{characterStatsManagers.mana.GetFinalValue()}");
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
