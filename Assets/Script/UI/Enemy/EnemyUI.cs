using System.Collections;
using ConquerTheStars.Stats;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Image healthImage;
    [SerializeField] private float healthUpdate;

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
        healthImage.fillAmount = health;
    }

    public void UpdateHealth(float targetHealth)
    {
        StartCoroutine(HealthChanging(targetHealth));
    }

    private IEnumerator HealthChanging(float target)
    {
        float elapsedTime = 0f;
        var currentHealth = healthImage.fillAmount;
        while (elapsedTime < healthUpdate)
        {
            elapsedTime += Time.deltaTime;
            var percentageTime = Mathf.Clamp01(elapsedTime / healthUpdate);
            healthImage.fillAmount = Mathf.Lerp(currentHealth, target, percentageTime);
            yield return null;
        }
        healthImage.fillAmount = target;
    }


}
