using UnityEngine;
using UnityEngine.UI;

public class TurnOrderElement : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject highlight;


    public void SetIcon(Sprite icon)
    {
        this.icon.sprite = icon;
    }

    public void ActiveHighlight()
    {
        highlight.SetActive(true);
    }
    public void InactiveHighlight()
    {
        highlight.SetActive(false);
    }
}
