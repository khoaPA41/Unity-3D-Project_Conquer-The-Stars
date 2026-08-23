using System;
using ConquerTheStars.Pattern.StateMachine.Base;
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

        [field: Header("Attack Data")]
        [field: SerializeField] public PlayerAttack AttackData { get; private set; }


        public Target Target { get; set; }
        public bool IsFinished { get; set; } // Detect animation done 

        /*State*/
        public State IdleState { get; private set; }
        public State AttackState { get; private set; }
        public State GethitState { get; private set; }

        public Vector3 EnemyStartPosition { get; set; } // Root pos

        public event Action AttackDealDamage = delegate { }; // This event will attend when enemy play get hit animation
        private void Start()
        {
            IdleState = new EnemyIdleState(this);
            AttackState = new EnemyAttackState(this);
            GethitState = new EnemyGetHitState(this);
            SwitchState(IdleState);
        }

        private void OnEnable()
        {
            EnemyStartPosition = transform.position;
        }

        public void SwitchIdle()
        {
            SwitchState(IdleState);
        }

        public void SwitchAttackState()
        {
            SwitchState(AttackState);
        }

        public void CallDealDamageEvent()
        {
            AttackDealDamage?.Invoke();
        }
    }
}