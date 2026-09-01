using System.Collections;
using ConquerTheStars.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace ConquerTheStars.UI.Enemy
{
    public class EnemyUI : MonoBehaviour
    {
        [Header("Health UI")]
        [SerializeField] private Image _healthImage;
        [SerializeField] private float _healthUpdate;

        [Header("Character Stats Manager")]
        [SerializeField]
        private CharacterStatsManagers characterStatsManagers;

        private void OnEnable()
        {
            SetupHealth(1f);
            characterStatsManagers.HealthUpdateAction += UpdateHealth;
        }

        private void OnDisable()
        {
            characterStatsManagers.HealthUpdateAction -= UpdateHealth;

        }

        public void SetupHealth(float health)
        {
            _healthImage.fillAmount = health;
        }

        public void UpdateHealth(float targetHealth)
        {
            StartCoroutine(HealthChanging(targetHealth));
        }

        private IEnumerator HealthChanging(float target)
        {
            float elapsedTime = 0f;
            var currentHealth = _healthImage.fillAmount;
            while (elapsedTime < _healthUpdate)
            {
                elapsedTime += Time.deltaTime;
                var percentageTime = Mathf.Clamp01(elapsedTime / _healthUpdate);
                _healthImage.fillAmount = Mathf.Lerp(currentHealth, target, percentageTime);
                yield return null;
            }
            _healthImage.fillAmount = target;
        }
    }
}
