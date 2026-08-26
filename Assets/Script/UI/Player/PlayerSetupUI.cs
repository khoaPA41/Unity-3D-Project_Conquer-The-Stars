using System.Collections;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Stats;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupUI : MonoBehaviour
{
    private readonly string UiName = "Character_HUD";

    [Header("Character Stats Manager")]
    [SerializeField]
    private CharacterStatsManagers characterStatsManagers;


    [Header("Time To Update UI")]
    [SerializeField]
    private float healthUpdate;
    [SerializeField] private float manaUpdate;

    private SkillSelectionElement skillSelectionElement;
    private Image health;
    private Image mana;


    private void OnEnable()
    {
        characterStatsManagers.HealthUpdateAction += HealthUpdate;
    }

    private void OnDisable()
    {
        characterStatsManagers.HealthUpdateAction -= HealthUpdate;
    }

    public void SpawnCharacterHUD()
    {
        skillSelectionElement = ObjectPoolingManagers.Instance.GetPooledObject(UiName, Vector3.zero, Vector3.zero).GetComponent<SkillSelectionElement>();
        skillSelectionElement.GetComponent<RectTransform>().SetParent(UIManagers.Instance?.StatusPanel);
        health = skillSelectionElement.Health;
        mana = skillSelectionElement.Mana;
    }

    public void SetupStatusUI(float healthValue, float manaValue)
    {
        health.fillAmount = healthValue;
        mana.fillAmount = manaValue;
    }

    public void HealthUpdate(float target)
    {
        Debug.Log(target);
        StartCoroutine(HealthChanging(target));
    }

    private IEnumerator HealthChanging(float target)
    {
        float elapsedTime = 0f;
        var currentHealth = health.fillAmount;
        while (elapsedTime < healthUpdate)
        {
            elapsedTime += Time.deltaTime;
            var percentageTime = Mathf.Clamp01(elapsedTime / healthUpdate);
            health.fillAmount = Mathf.Lerp(currentHealth, target, percentageTime);
            yield return null;
        }
        health.fillAmount = target;
    }
}
