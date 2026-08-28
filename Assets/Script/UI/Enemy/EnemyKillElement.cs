using UnityEngine;
using UnityEngine.UI;

public class EnemyKillElement : MonoBehaviour
{
    [Header("Enemy Icon")]
    [SerializeField] private Image icon;

    public void SetIcon(Sprite icon)
    {
        this.icon.sprite = icon;
    }

}
