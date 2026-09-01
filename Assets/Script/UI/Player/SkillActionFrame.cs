using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace ConquerTheStars.UI.Player
{
    public class SkillActionFrame : MonoBehaviour
    {
        [Header("Skill Frame Action")]
        [SerializeField] private Image perfectFrame;
        [SerializeField] private Image actionFrame;
        [SerializeField] private float timeToEnd;
        [SerializeField] private Vector3 actionFrameLocalScaleTarget;
        [SerializeField] private Vector3 actionFrameLocalScaleRoot;

        private bool isPaused;
        public float ActionFrameValue { get; set; }
        public event Action PauseSkillActionFrame;

        private void OnEnable()
        {
            isPaused = false;
            StartCoroutine(ActionFrameMovement());
        }

        public IEnumerator ActionFrameMovement()
        {
            var elapsed = 0f;
            while (elapsed < timeToEnd && !isPaused)
            {
                elapsed += Time.deltaTime;

                var percentage = Mathf.Clamp01(elapsed / timeToEnd);

                actionFrame.rectTransform.localScale = Vector3.Lerp(actionFrameLocalScaleRoot, actionFrameLocalScaleTarget, percentage);

                yield return null;
            }

            var value = actionFrame.rectTransform.lossyScale.x;
            ActionFrameValue = value >= 1.01f ? 0f : value;

            actionFrame.rectTransform.localScale = actionFrameLocalScaleRoot;
            perfectFrame.gameObject.SetActive(false);

            CallPausedAction();
        }

        public void PauseActionFrame()
        {
            isPaused = true;
        }

        public void CallPausedAction()
        {
            PauseSkillActionFrame?.Invoke();
        }
    }
}