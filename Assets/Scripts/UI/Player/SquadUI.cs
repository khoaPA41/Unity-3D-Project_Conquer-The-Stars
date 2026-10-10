using UnityEngine;

namespace ConquerTheStars.UI.Player
{
    public class SquadUI : MonoBehaviour
    {
        [Header("Squad UI")]
        [SerializeField] private GameObject _squadUI;

        [Header("Squad Panel Elements")]
        [SerializeField] private GameObject _squadStats;
        [SerializeField] private GameObject _squadEquipment;
        [SerializeField] private GameObject _squadInventory;

        private GameObject currentActive;

        private void Start()
        {
            currentActive = _squadStats;
        }

        // Squad UI
        public void ActiveSquadUI()
        {
            _squadUI.SetActive(true);
        }

        public void InactiveSquadUI()
        {
            _squadUI.SetActive(false);
        }

        public void SwitchPanel(GameObject nextUi)
        {
            if (currentActive != null)
                currentActive.SetActive(false);

            currentActive = nextUi;

            if (currentActive != null)
                currentActive.SetActive(true);
        }
    }
}