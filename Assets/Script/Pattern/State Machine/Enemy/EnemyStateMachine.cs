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

        public bool IsFinished { get; set; }
        public State IdleState;
        public State AttackState;

        private void Start()
        {
            IdleState = new EnemyIdleState(this);
            AttackState = new EnemyAttackState(this);
            SwitchState(IdleState);
        }

        private void OnEnable()
        {

        }

        public void SwitchIdle()
        {
            SwitchState(IdleState);
        }

        public void SwitchAttackState()
        {
            SwitchState(AttackState);
        }
    }


}