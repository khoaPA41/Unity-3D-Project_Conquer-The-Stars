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


        public bool IsFinished { get; set; }
        public State IdleState { get; private set; }
        public State AttackState { get; private set; }
        public State GethitState { get; private set; }

        public event Action AttackDealDamage = delegate { }; // This event will attend when enemy play get hit animation
        private void Start()
        {
            IdleState = new EnemyIdleState(this);
            AttackState = new EnemyAttackState(this);
            GethitState = new EnemyGetHitState(this);
            SwitchState(IdleState);
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
            Debug.Log("Call event");
            AttackDealDamage?.Invoke();
        }
    }


}