using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConquerTheStars.UI.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [field: Header("HUD Element")]
        [field: SerializeField] public Image Icon { get; set; }
        [field: SerializeField] public Image Health { get; set; }
        [field: SerializeField] public Image Mana { get; set; }
        [field: SerializeField] public TextMeshProUGUI HealthText { get; set; }
        [field: SerializeField] public TextMeshProUGUI ManaText { get; set; }

    }
}
