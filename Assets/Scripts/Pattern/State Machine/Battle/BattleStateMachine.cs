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

        [field: Header("Area")]
        [field: SerializeField] public StartMatch Area { get; private set; }
        // public Queue<CharacterStatsManagers> CharacterStats { get; set; } = new();
        public List<CharacterStatsManagers> CharacterStats = new();

        public CharacterStatsManagers CurrentTurn { get; set; }

        [field: Header("Target")]
        [field: SerializeField] public Targeter PlayerTargeter { get; private set; }
        [field: SerializeField] public Targeter EnemyTargeter { get; private set; }

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
        public State Playerexecuted { get; private set; }
        public State PlayerSelectAlly { get; private set; }
        public State EnemyTurn { get; private set; }
        public State EnemyExecuted { get; private set; }
        public State Resolve { get; private set; }
        public State Result { get; private set; }

        // Current Character Turn
        public PlayerCombatStateMachine PlayerCombatStateMachine { get; set; }
        public EnemyStateMachine EnemyStateMachine { get; set; }

        // Battle Statistics
        public float HighestDamage;
        public float DamageDealt;
        public float DamageReceived;
        public float BattleTime;
        public int SuccessfulParryTimes;
        public int SuccessfulDodgeTimes;

        // 
        public float SpeedAverage;

        public CharacterStatsManagers playerDealsHighestDamageLastTurn;
        void Start()
        {

            Cursor.lockState = CursorLockMode.None;
            BattleSetup = new SetupState(this);
            StartTurn = new StartTurnState(this);
            PlayerTurn = new PlayerTurnState(this);
            PlayerSelectAlly = new PlayerSelectAllyState(this);
            PlayerSelectSkillTurn = new PlayerSelectSkillState(this);
            PlayerSelectTargetTurn = new PlayerSelectTargetState(this);
            Playerexecuted = new PlayerExecutionState(this);
            EnemyTurn = new EnemyTurnState(this);
            EnemyExecuted = new EnemyExecutionState(this);
            Resolve = new ResolveState(this);
            Result = new BattleResultState(this);

            SwitchState(BattleSetup);
        }

        public void SwitchPlayerExecuted()
        {
            SwitchState(Playerexecuted);
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

        public void CalculateDamageReceived(float damage)
        {
            DamageReceived += damage;
        }

        public void CalculateSuccessfulParryTimes()
        {
            SuccessfulParryTimes++;
        }

        public void CalculateSuccessfulDodgeTimes()
        {
            SuccessfulDodgeTimes++;
        }

        public void ReleaseAllTeam()
        {
            foreach (var player in TeamController.PlayerTeam)
            {
                player.GetComponent<PooledObject>().Release();
            }

            foreach (var enemy in TeamController.EnemyTeam)
            {
                enemy.GetComponent<PooledObject>().Release();
            }
        }
    }
}
