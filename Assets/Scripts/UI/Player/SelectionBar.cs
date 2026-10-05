using UnityEngine;

namespace ConquerTheStars.UI.Player
{
    public class SelectionBar : MonoBehaviour
    {
        private readonly int _skillSelectionAppearAnimationHash = Animator.StringToHash("Appear");
        private readonly int _skillSelectionDisappearAnimationHash = Animator.StringToHash("Disappear");

        [Header("Skill Board")]
        [SerializeField] private GameObject _skillBoardRoot;
        [SerializeField] private GameObject _skillBoardOther_I;
        [SerializeField] private GameObject _skillBoardOthe_II;
        [SerializeField] private Animator _animator;


        public void ActiveSkillBoard()
        {
            _skillBoardOther_I.SetActive(false);
            _skillBoardOthe_II.SetActive(false);
            _skillBoardRoot.SetActive(true);
            _animator.SetTrigger(_skillSelectionAppearAnimationHash);
        }
    }
}
