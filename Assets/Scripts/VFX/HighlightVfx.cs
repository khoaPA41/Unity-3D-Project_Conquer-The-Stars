using UnityEngine;

namespace ConquerTheStars.Vfx
{
    public class HighlightVfx : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hightlightCurrentTurn;


        public void Highlight()
        {
            hightlightCurrentTurn.gameObject.SetActive(true);
            hightlightCurrentTurn.Play();
        }

        public void InactiveHighlight()
        {
            hightlightCurrentTurn.Stop();
            hightlightCurrentTurn.gameObject.SetActive(false);
        }
    }
}