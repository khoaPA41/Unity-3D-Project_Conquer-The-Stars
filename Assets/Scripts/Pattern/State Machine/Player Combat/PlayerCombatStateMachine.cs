using System;
using ConquerTheStars.Factory.Item;
using ConquerTheStars.Fight.Player;
using ConquerTheStars.Fight.Target;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Pattern.StateMachine.Base;
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Player;
using ConquerTheStars.Vfx;
using Unity.Cinemachine;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatStateMachine : Base.StateMachine, ICaster
    {
        [field: Header("Physics - Movement")]
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        [field: Header("Animator")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; }
        [field: SerializeField] public float AnimationAttackSpeed { get; private set; }
        [field: SerializeField] public bool IsNotMove { get; private set; }

        [field: Header("Attack Data")]
        [field: SerializeField] public PlayerAttack AttackData { get; private set; }
        [field: SerializeField] public Transform AttackTransform { get; private set; }


        [field: Header("Status")]
        [field: SerializeField] public CharacterStatsManagers CharacterStatsManagers { get; private set; }
        [field: SerializeField] public BattleStatistics BattleStatistics { get; private set; }

        [field: Header("PooledObject")]
        [field: SerializeField] public PooledObject PooledObject { get; private set; }

        [field: Header("Setup UI")]
        [field: SerializeField] public PlayerSetupSkillUI PlayerSetupSkillUI { get; private set; }
        [field: SerializeField] public PlayerSetupUI PlayerSetupUI { get; private set; }

        [field: Header("Camera")]
        // [field: SerializeField] public CinemachineCamera CinemachineCamera { get; private set; }
        [field: SerializeField] public GameObject CameraGroup { get; private set; }

        [field: Header("VFX")]
        [field: SerializeField] public HighlightVfx HighlightCurrentTurn { get; private set; }
        [field: SerializeField] public string SlashVfxName { get; private set; }

        public Target Target { get; set; }
        public string AnimationName { get; set; }

        //State
        public State PlayerIdleState { get; private set; }
        public State PlayerCombatIdleState { get; private set; }
        public State PlayerDefenseState { get; private set; }
        public State PlayerUseItemState { get; private set; }
        public State PlayerBlockState { get; private set; }
        public State PlayerAttackState { get; private set; }
        public State PlayerGetHitState { get; private set; }
        public State PlayerDodgeState { get; private set; }
        public State PlayerDyingState { get; private set; }
        public State PlayerVictoryState { get; private set; }

        public Vector3 PlayerStartPosition { get; set; }

        // This event will call when enemy play get hit animation
        public event Action AttackDealDamage = delegate { };

        private readonly int attackSpeedParams = Animator.StringToHash("Attack");
        public bool IsFinished { get; set; }

        public event Action<string, int> PlayerExecuteAction = delegate { }; // Event for active attack
        public event Action<ItemType, int> PlayerUseItem = delegate { }; // Event for use item
        public event Action UseReviveItem = delegate { }; // Event for use revive item
        public int AttackIndexSelected { get; set; }
        public string AttackNameList { get; set; }

        public int ItemIndex { get; set; }
        public ItemType ItemType { get; set; }

        private void Awake()
        {
            PlayerIdleState = new PlayerCombatIdleState(this);
            PlayerCombatIdleState = new PlayerCombatIdleCombatState(this);
            PlayerDefenseState = new PlayerCombatDefenseState(this);
            PlayerUseItemState = new PlayerCombatUseItemState(this);
            PlayerGetHitState = new PlayerCombatGetHitState(this);
            PlayerDodgeState = new PlayerCombatDodgeState(this);
            PlayerDyingState = new PlayerCombatDyingState(this);
            PlayerAttackState = new PlayerCombatAttackState(this);
            PlayerBlockState = new PlayerCombatBlockState(this);
            PlayerVictoryState = new PlayerCombatVictoryState(this);
        }

        private void OnEnable()
        {
            PlayerStartPosition = transform.position;
            CharacterStatsManagers.DyingAction += SwitchDyingState;
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
            CharacterStatsManagers.DyingAction -= SwitchDyingState;
            PlayerExecuteAction -= GetAttackIndex;

        }

        public GameObject CharacterUse()
        {
            return this.gameObject;
        }

        public void ReturnIdle()
        {
            SwitchState(PlayerIdleState);
        }
        public void ReturnCombatIdle()
        {
            SwitchState(PlayerCombatIdleState);
        }

        public void ReturnDefenseIdle()
        {
            ActiveCamera();
            SwitchState(PlayerDefenseState);
        }

        public void SwitchUseItem()
        {
            SwitchState(PlayerUseItemState);
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

        public void SwitchVictoryState()
        {
            SwitchState(PlayerVictoryState);
        }

        public void CallDealDamageEvent()
        {

            AttackDealDamage?.Invoke();
        }

        public void SpawnSlashVfx()
        {
            var vfx = ObjectPoolingManagers.Instance.GetPooledObject(SlashVfxName, AttackTransform.position);
            vfx.transform.eulerAngles = transform.eulerAngles;
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

        public void GetItemIndex(ItemType itemType, int index)
        {
            ItemType = itemType;
            ItemIndex = index;
            PlayerUseItem?.Invoke(itemType, index);
        }

        public void GetAttackIndex(string attackListName, int index)
        {
            AttackNameList = attackListName;
            AttackIndexSelected = index;
        }

        public float GetAttackDameScale()
        {
            var damageScale = AttackNameList == "Attack" ?
                            AttackData.ScaleAttackDamage[AttackIndexSelected] :
                            AttackData.ScaleSkillDamage[AttackIndexSelected];
            return damageScale;
        }

        public void ActiveCamera()
        {
            CameraGroup.SetActive(true);
        }
        public void InactiveCamera()
        {
            CameraGroup.SetActive(false);
        }

        public void CallUseReviveItemEvent()
        {
            UseReviveItem?.Invoke();
        }

        public void RotateToEnemy(Transform target)
        {
            if (target != null)
            {
                var eulers = target.position;
                transform.LookAt(eulers);
            }
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