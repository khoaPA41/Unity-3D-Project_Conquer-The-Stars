using UnityEngine;
using UnityEngine.UI;

namespace ConquerTheStars.UI.Enemy
{
    public class EnemyKillElement : MonoBehaviour
    {
        [Header("Enemy Icon")]
        [SerializeField] private Image _icon;

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
    }
}
