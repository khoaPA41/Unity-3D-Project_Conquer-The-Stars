using System;
using ConquerTheStars.Fight.Enemy;
using ConquerTheStars.Fight.Target;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Pattern.StateMachine.Base;
using ConquerTheStars.Stats;
using ConquerTheStars.Vfx;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyStateMachine : Base.StateMachine
    {
        [field: Header("Physics - Movement")]
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        [field: Header("Animator")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; }

        [field: Header("Status")]
        [field: SerializeField] public CharacterStatsManagers CharacterStatsManagers { get; private set; }
        [field: SerializeField] public bool IsBoss { get; private set; }

        [field: Header("Attack Data")]
        [field: SerializeField] public PlayerAttack AttackData { get; private set; }
        [field: SerializeField] public EnemyAttack EnemyAttack { get; private set; }


        [field: Header("PooledObject")]
        [field: SerializeField] public PooledObject PooledObject { get; private set; }

        [field: Header("AI")]
        [field: SerializeField] public EnemyEvaluation EnemyEvaluation { get; private set; }

        [field: Header("VFX")]
        [field: SerializeField] public HighlightVfx HighlightCurrentTurn { get; private set; }
        [field: SerializeField] public HighlightVfx HighlightTarget { get; private set; }

        public Target Target { get; set; }
        public bool IsFinished { get; set; } // Detect animation done 

        /*State*/
        public State IdleState { get; private set; }
        public State AttackState { get; private set; }
        public State BossAttackState { get; private set; }

        public State GethitState { get; private set; }
        public State DyingState { get; private set; }

        public Vector3 EnemyStartPosition { get; set; } // Root pos

        // This event will call when enemy play get hit animation
        public event Action AttackDealDamage = delegate { };

        private void Start()
        {
            IdleState = new EnemyIdleState(this);
            AttackState = new EnemyAttackState(this);
            BossAttackState = new EnemyBossAttackState(this);
            GethitState = new EnemyGetHitState(this);
            DyingState = new EnemyDyingState(this);
        }

        private void OnEnable()
        {
            SwitchState(IdleState);
            EnemyStartPosition = transform.position;
            CharacterStatsManagers.DyingAction += SwitchDyingState;
        }

        private void OnDisable()
        {
            EnemyStartPosition = transform.position;
            CharacterStatsManagers.DyingAction -= SwitchDyingState;
        }

        public void SwitchIdle()
        {
            SwitchState(IdleState);
        }

        public void SwitchAttackState()
        {
            SwitchState(AttackState);
        }

        public void SwitchBossAttackState()
        {
            SwitchState(BossAttackState);
        }

        public void SwitchDyingState()
        {
            SwitchState(DyingState);
        }

        public void CallDealDamageEvent()
        {
            AttackDealDamage?.Invoke();
        }
    }
}