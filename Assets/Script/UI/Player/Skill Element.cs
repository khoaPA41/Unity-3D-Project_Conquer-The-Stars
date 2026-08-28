using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillElement : MonoBehaviour
{
    [Header("Skill Element")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI information;

    [field: SerializeField] public Button Button { get; set; }
    public int index;


    public void SetupSkillElement(Sprite icon, string skillName, string information)
    {
        this.icon.sprite = icon;
        this.skillName.SetText(skillName);
        this.information.SetText(information);
    }
}
