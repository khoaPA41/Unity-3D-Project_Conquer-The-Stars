using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillElement : MonoBehaviour
{
    [Header("Skill Element")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI information;
    [field: SerializeField] public Button Button { get; set; }
    public int index;


    private void Start()
    {

    }

    private void OnEnable()
    {
        // Button.onClick.AddListener();
    }

    // private void OnDisable()
    // {
    //     Button.onClick.AddListener();
    // }


    public void SetupSkillElement(Sprite icon, string information)
    {
        this.icon.sprite = icon;
        this.information.SetText(information);
    }
}
