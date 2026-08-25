using System;
using System.Collections.Generic;
using ConquerTheStars.Pattern.StateMachine.Base;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class BattleStateMachine : Base.StateMachine
    {
        [field: Header("Input")]
        [field: SerializeField] public BattleInputReader InputReader { get; private set; }

        [field: Header("Area")]
        [field: SerializeField] public StartMatch Area { get; private set; }
        public Queue<CharacterStatsManagers> CharacterStats { get; set; } = new();
        public CharacterStatsManagers CurrentTurn { get; set; }

        [field: Header("Target")]
        [field: SerializeField] public Targeter PlayerTargeter { get; private set; }
        [field: SerializeField] public Targeter EnemyTargeter { get; private set; }

        [field: Header("Team Controller")]
        [field: SerializeField] public TeamController TeamController { get; private set; }

        // State
        public State BattleSetup { get; private set; }
        public State StartTurn { get; private set; }
        public State PlayerTurn { get; private set; }
        public State PlayerSelectSkillTurn { get; private set; }
        public State PlayerSelectTargetTurn { get; private set; }
        public State Playerexecuted { get; private set; }
        public State EnemyTurn { get; private set; }
        public State EnemyExecuted { get; private set; }
        public State Resolve { get; private set; }

        public PlayerCombatStateMachine PlayerCombatStateMachine { get; set; }
        public EnemyStateMachine EnemyStateMachine { get; set; }

        // public event Action<int> PlayerExecuteAction = delegate { };

        // public int AttackIndexSelected { get; set; }

        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            BattleSetup = new SetupState(this);
            StartTurn = new StartTurnState(this);
            PlayerTurn = new PlayerTurnState(this);
            PlayerSelectSkillTurn = new PlayerSelectSkillState(this);
            PlayerSelectTargetTurn = new PlayerSelectTargetState(this);
            Playerexecuted = new PlayerExecutionState(this);
            EnemyTurn = new EnemyTurnState(this);
            EnemyExecuted = new EnemyExecutionState(this);
            Resolve = new ResolveState(this);
            SwitchState(BattleSetup);
        }

        // public void ExecutionAction(int actionIndex)
        // {
        //     PlayerExecuteAction?.Invoke(actionIndex);
        // }

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
    }
}
