using System.Collections.Generic;
using ConquerTheStars.Fight.Match;
using ConquerTheStars.Fight.Target;
using ConquerTheStars.InputController;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Pattern.StateMachine.Base;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using Unity.Cinemachine;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class BattleStateMachine : Base.StateMachine
    {
        [field: Header("Input")]
        [field: SerializeField] public BattleInputReader InputReader { get; private set; }
        [field: SerializeField] public TouchSwipeController TouchSwipeController { get; private set; }

        [field: Header("UI")]
        [field: SerializeField] public GameObject SelectUi { get; private set; }
        [field: SerializeField] public GameObject AcceptAttack { get; private set; }
        [field: SerializeField] public GameObject AcceptUseItem { get; private set; }

        [field: Header("Area")]
        [field: SerializeField] public StartMatch Area { get; private set; }
        public CharacterStatsManagers CurrentTurn { get; set; }
        public TurnOrderService TurnOrderService { get; set; } = new();

        [field: Header("Target")]
        [field: SerializeField] public Targeter PlayerTargeter { get; private set; }
        [field: SerializeField] public Targeter EnemyTargeter { get; private set; }
        [field: SerializeField] public Targeter AllyTargeter { get; private set; }


        [field: Header("Team Controller")]
        [field: SerializeField] public TeamController TeamController { get; private set; }

        [field: Header("Camera")]
        [field: SerializeField] public CinemachineCamera VictoryCamera { get; private set; }

        // State
        public State BattleSetup { get; private set; }
        public State StartTurn { get; private set; }
        public State PlayerTurn { get; private set; }
        public State PlayerSelectSkillTurn { get; private set; }
        public State PlayerSelectTargetTurn { get; private set; }
        public State PlayerExecuted { get; private set; }
        public State PlayerSelectAlly { get; private set; }
        public State EnemyTurn { get; private set; }
        public State EnemyExecuted { get; private set; }
        public State Resolve { get; private set; }
        public State Result { get; private set; }

        // Current Character Turn
        public PlayerCombatStateMachine PlayerCombatStateMachine { get; set; }
        public EnemyStateMachine EnemyStateMachine { get; set; }

        // Battle Statistics
        [field: Header("Battle Time")]
        public float BattleTime;

        // 
        public bool IsTurnOrderChange { get; set; }
        public bool IsWaitingCameraBlend { get; set; }

        public bool IsFinalBoss { get; set; }

        public Reward BattleReward { get; set; }

        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            BattleSetup = new SetupState(this);
            StartTurn = new StartTurnState(this);
            PlayerTurn = new PlayerTurnState(this);
            PlayerSelectAlly = new PlayerSelectAllyState(this);
            PlayerSelectSkillTurn = new PlayerSelectSkillState(this);
            PlayerSelectTargetTurn = new PlayerSelectTargetState(this);
            PlayerExecuted = new PlayerExecutionState(this);
            EnemyTurn = new EnemyTurnState(this);
            EnemyExecuted = new EnemyExecutionState(this);
            Resolve = new ResolveState(this);
            Result = new BattleResultState(this);

            SwitchState(BattleSetup);

            InputReader.SettingUiAction += ActiveSettingUI;
        }

        private void OnEnable()
        {
            CinemachineCore.BlendFinishedEvent.AddListener(OnBlendFinished);
        }

        private void OnDisable()
        {
            CinemachineCore.BlendFinishedEvent.RemoveListener(OnBlendFinished);
            InputReader.SettingUiAction -= ActiveSettingUI;
            ReleaseAllTeam();
        }

        private void ActiveSettingUI()
        {
            var isSettingsActiveUi = !UiManagers.Instance.settingsPanel.activeInHierarchy;
            UiManagers.Instance.ActiveSettingsPanel(isSettingsActiveUi);
            Cursor.lockState = isSettingsActiveUi ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void OnBlendFinished(ICinemachineMixer camera, ICinemachineCamera cinemachineCamera)
        {
            IsWaitingCameraBlend = true;
        }

        public void SwitchPlayerExecuted()
        {
            SwitchState(PlayerExecuted);
        }

        public void SwitchResolve()
        {
            SwitchState(Resolve);
        }

        public void SwitchStartTurn()
        {
            SwitchState(StartTurn);
        }

        public void SwitchSelectAlly()
        {
            SwitchState(PlayerSelectAlly);
        }

        public void ReleaseAllTeam()
        {
            SwitchState(null);
            StopAllCoroutines();
            Time.timeScale = 1;
            foreach (var player in TeamController.PlayerTeam)
            {
                player.GetComponent<PlayerCombatStateMachine>().RevieSuccessAction -= TurnOrderService.EnqueueBack;
                player.GetComponent<PooledObject>().Release();
            }

            foreach (var enemy in TeamController.EnemyTeam)
            {
                enemy.GetComponent<PooledObject>().Release();
            }
        }

        public void ActiveSelectUI(bool isActive, bool isAttack)
        {
            if (!isActive)
            {
                SelectUi.SetActive(isActive);
                return;
            }

            SelectUi.SetActive(isActive);

            AcceptAttack.SetActive(isAttack);
            AcceptUseItem.SetActive(!isAttack);
        }
    }
}
