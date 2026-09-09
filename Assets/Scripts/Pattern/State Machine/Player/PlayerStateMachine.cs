using UnityEngine;
using ConquerTheStars.Pattern.StateMachine.Base;
using ConquerTheStars.InputController;
using ConquerTheStars.Physic;

namespace ConquerTheStars.Pattern.StateMachine.Player
{
    public class PlayerStateMachine : Base.StateMachine
    {
        [field: Header("Physics - Movement")]
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public float RotationDamping { get; set; }

        [field: Header("Animator")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; }

        [field: Header("Input")]
        [field: SerializeField] public PlayerInputReader InputReader { get; private set; }

        public Camera MainCamera { get; private set; }

        private void Start()
        {
            MainCamera = Camera.main;
            SwitchState(new PlayerLocomotionState(this));
            InputReader.SettingUiAction += ActiveSettingUI;

        }

        private void OnDisable()
        {
            InputReader.SettingUiAction -= ActiveSettingUI;
        }

        private void ActiveSettingUI()
        {
            var isSettingsActiveUi = !UiManagers.Instance.settingsPanel.activeInHierarchy;
            UiManagers.Instance.ActiveSettingsPanel(isSettingsActiveUi);
            Cursor.lockState = isSettingsActiveUi ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
