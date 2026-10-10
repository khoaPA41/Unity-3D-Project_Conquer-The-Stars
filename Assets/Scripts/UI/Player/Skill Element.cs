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
            _icon.sprite = icon;
            _skillName.SetText(skillName);
            _information.SetText(information);
        }


        // Use for item
        public void UpdateItemQuantity(string quantity)
        {
            _skillName.SetText(quantity);
        }
    }
}
