using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionElement : MonoBehaviour
{
    [field: Header("Skill Element")]
    [field: SerializeField] public Image Icon { get; set; }
    [field: SerializeField] public Image Health { get; set; }
    [field: SerializeField] public Image Mana { get; set; }
    [field: SerializeField] public TextMeshProUGUI Information { get; set; }
}
