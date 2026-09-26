using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConquerTheStars.UI.Player
{
    public class SkillElement : MonoBehaviour
    {
        [Header("Skill Element")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _skillName;
        [SerializeField] private TextMeshProUGUI _information;

        [field: SerializeField] public Button Button { get; set; }
        public int Index;


        public void SetupSkillElement(Sprite icon, string skillName, string information)
        {
            this._icon.sprite = icon;
            this._skillName.SetText(skillName);
            this._information.SetText(information);
        }
    }
}
