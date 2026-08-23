using System;
using System.Collections.Generic;
using ConquerTheStars.Pattern.StateMachine.Base;
using TMPro;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatStateMachine : Base.StateMachine
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

        public string AnimationName { get; set; }
        public State PlayerIdleState { get; private set; }
        public State PlayerAttackState { get; private set; }
        public State PlayerGetHitState { get; private set; }

        public Vector3 PlayerStartPosition { get; set; }

        public event Action AttackDealDamage = delegate { }; // This event will attend when enemy play get hit animation

        public bool IsFinished { get; set; }
        private void Awake()
        {
            PlayerIdleState = new PlayerCombatIdleState(this);
            PlayerGetHitState = new PlayerCombatGetHitState(this);

        }

        private void OnEnable()
        {
            PlayerStartPosition = transform.position;
        }

        public void ReturnIdle()
        {
            SwitchState(PlayerIdleState);
        }

        public void SwitchAttackState(int index)
        {
            SwitchState(new PlayerCombatAttackState(this, index));
        }

        public void CallDealDamageEvent()
        {
            Debug.Log("Call event");
            AttackDealDamage?.Invoke();
        }
    }
}