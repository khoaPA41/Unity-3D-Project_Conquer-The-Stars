using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace ConquerTheStars.UI.Player
{
    public class SkillActionFrame : MonoBehaviour
    {
        [Header("Skill Frame Action")]
        [SerializeField] private Image _perfectFrame;
        [SerializeField] private Image _actionFrame;
        [SerializeField] private float _timeToEnd;
        [SerializeField] private Vector3 _actionFrameLocalScaleTarget;
        [SerializeField] private Vector3 _actionFrameLocalScaleRoot;

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
            while (elapsed < _timeToEnd && !isPaused)
            {
                elapsed += Time.deltaTime;

                var percentage = Mathf.Clamp01(elapsed / _timeToEnd);

                _actionFrame.rectTransform.localScale = Vector3.Lerp(_actionFrameLocalScaleRoot, _actionFrameLocalScaleTarget, percentage);

                yield return null;
            }

            var value = _actionFrame.rectTransform.localScale.x;
            ActionFrameValue = value >= 1.01f ? 0f : value;

            _actionFrame.rectTransform.localScale = _actionFrameLocalScaleRoot;
            _perfectFrame.gameObject.SetActive(false);

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