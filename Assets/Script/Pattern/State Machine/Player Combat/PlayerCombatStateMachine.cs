using System;
using System.Collections.Generic;
using ConquerTheStars.Fight.Player;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Pattern.StateMachine.Base;
using ConquerTheStars.Stats;
using TMPro;
using Unity.Cinemachine;
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
        [field: SerializeField] public float AnimationAttackSpeed { get; private set; }


        [field: Header("Attack Data")]
        [field: SerializeField] public PlayerAttack AttackData { get; private set; }

        [field: Header("Status")]
        [field: SerializeField] public CharacterStatsManagers CharacterStatsManagers { get; private set; }

        [field: Header("PooledObject")]
        [field: SerializeField] public PooledObject PooledObject { get; private set; }

        [field: Header("Setup UI")]
        [field: SerializeField] public PlayerSetupSkillUI PlayerSetupSkillUI { get; private set; }
        [field: SerializeField] public PlayerSetupUI PlayerSetupUI { get; private set; }


        [field: Header("Camera")]
        [field: SerializeField] public CinemachineCamera CinemachineCamera { get; private set; }
        [field: SerializeField] public GameObject CameraGroup { get; private set; }
        [field: SerializeField] public CinemachineTargetGroup CinemachineTargetGroup { get; private set; }

        public Target Target { get; set; }

        public string AnimationName { get; set; }
        public State PlayerIdleState { get; private set; }
        public State PlayerCombatIdleState { get; private set; }
        public State PlayerBlockState { get; private set; }
        public State PlayerAttackState { get; private set; }
        public State PlayerGetHitState { get; private set; }
        public State PlayerDodgeState { get; private set; }
        public State PlayerDyingState { get; private set; }

        public Vector3 PlayerStartPosition { get; set; }

        public event Action AttackDealDamage = delegate { }; // This event will attend when enemy play get hit animation
        private readonly int attackSpeedParams = Animator.StringToHash("Attack"); // This event will attend when enemy play get hit animation

        public bool IsFinished { get; set; }
        public event Action<string, int> PlayerExecuteAction = delegate { };

        public int AttackIndexSelected { get; set; }
        public string AttackNameList { get; set; }

        private void Awake()
        {
            PlayerIdleState = new PlayerCombatIdleState(this);
            PlayerCombatIdleState = new PlayerCombatIdleCombatState(this);
            PlayerGetHitState = new PlayerCombatGetHitState(this);
            PlayerDodgeState = new PlayerCombatDodgeState(this);
            PlayerDyingState = new PlayerCombatDyingState(this);
            PlayerAttackState = new PlayerCombatAttackState(this);
            PlayerBlockState = new PlayerCombatBlockState(this);
        }

        private void OnEnable()
        {
            PlayerStartPosition = transform.position;
            CharacterStatsManagers.IsDyingAction += SwitchDyingState;
            PlayerExecuteAction += GetAttackIndex;
            if (UIManagers.Instance != null)
            {
                PlayerSetupUI.SpawnCharacterHUD();
                PlayerSetupUI.SetupStatusUI(CharacterStatsManagers.CurrentHealth / CharacterStatsManagers.maxHealth.GetFinalValue(),
                CharacterStatsManagers.CurrentMana / CharacterStatsManagers.mana.GetFinalValue());
            }
        }

        private void OnDisable()
        {
            CharacterStatsManagers.IsDyingAction -= SwitchDyingState;
            PlayerExecuteAction -= GetAttackIndex;

        }

        public void ReturnIdle()
        {
            SwitchState(PlayerIdleState);
        }

        public void SwitchAttackState()
        {
            SwitchState(PlayerAttackState);
        }

        public void SwitchDodgeState()
        {
            SwitchState(PlayerDodgeState);
        }

        public void SwitchBlockState()
        {
            SwitchState(PlayerBlockState);
        }

        public void SwitchDyingState()
        {
            SwitchState(PlayerDyingState);
        }

        public void CallDealDamageEvent()
        {
            AttackDealDamage?.Invoke();
        }

        public void ReturnAttackSpeed()
        {
            Animator.SetFloat(attackSpeedParams, 1.5f);
        }

        public void SetAttackSpeed()
        {
            Animator.SetFloat(attackSpeedParams, .3f);
        }

        public void GetIndexAction(string attackListName, int actionIndex)
        {
            PlayerExecuteAction?.Invoke(attackListName, actionIndex);
        }

        public void GetAttackIndex(string attackListName, int index)
        {
            AttackNameList = attackListName;
            AttackIndexSelected = index;
        }

        public void ActiveCamera()
        {
            // CinemachineCamera.gameObject.SetActive(true);
            CameraGroup.SetActive(true);
        }
        public void InactiveCamera()
        {
            // CinemachineCamera.gameObject.SetActive(false);
            CameraGroup.SetActive(false);

        }

        public void ActiveFrame()
        {
            UIManagers.Instance.ActiveActionFrame();
        }

        public float GetManaRequired(string name, int index)
        {
            if (name == "Skill")
            {
                return AttackData.ManaRequired[index];
            }
            return 0;
        }
    }
}