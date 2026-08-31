using ConquerTheStars.Factory.Item;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatUseItemState : PlayerCombatBaseState
    {
        private readonly int UseItemIdleAnimationHash = Animator.StringToHash("UseItem");

        public PlayerCombatUseItemState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(UseItemIdleAnimationHash, playerCombatStateMachine.AnimationCrossFade);
            UseItem();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        private void UseItem()
        {
            IItem item = ItemFactory.CreateItem(playerCombatStateMachine.ItemType);
            item.Use(playerCombatStateMachine, PlayerTeam.Instance.GetItemData(playerCombatStateMachine.ItemIndex));
        }
    }
}