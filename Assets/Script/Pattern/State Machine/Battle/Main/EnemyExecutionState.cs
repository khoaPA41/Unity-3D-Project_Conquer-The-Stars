using System.Collections;
using System.Linq;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class EnemyExecutionState : BattleBaseState
    {
        public EnemyExecutionState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            battleStateMachine.InputReader.EnterTargetAction += PlayerDodge;
            battleStateMachine.InputReader.BlockAction += PlayerBlock;

            battleStateMachine.EnemyStateMachine.IsFinished = false;

            battleStateMachine.StartCoroutine(WaitToEndAttack());
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            battleStateMachine.InputReader.EnterTargetAction -= PlayerDodge;
            battleStateMachine.InputReader.BlockAction -= PlayerBlock;
        }

        private IEnumerator WaitToEndAttack()
        {
            battleStateMachine.EnemyStateMachine.Target = battleStateMachine.EnemyTargeter.currentTarget; // Get Current Target form select Target state

            battleStateMachine.EnemyStateMachine.AttackDealDamage += EnemyDealDamage; // Subscribe animation event 
            battleStateMachine.EnemyStateMachine.SwitchAttackState();

            yield return new WaitUntil(() => battleStateMachine.EnemyStateMachine.IsFinished == true); // Wait until atk animation done
            battleStateMachine.EnemyStateMachine.AttackDealDamage -= EnemyDealDamage; // UnSubscribe animation event 
            battleStateMachine.SwitchResolve();
        }
        private void EnemyDealDamage()
        {
            var target = battleStateMachine.EnemyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>(); // Get StateMachine from target
            var playerStatsManager = battleStateMachine.EnemyTargeter.currentTarget.GetComponent<CharacterStatsManagers>(); // Get CharacterStatsManagers from target
            if (target != null)
            {
                if (playerStatsManager.TakeDamage(30)) // take damage
                {
                    target.SwitchState(target.PlayerGetHitState); // Switch target state to get hit
                }
            }
        }

        private void PlayerDodge()
        {
            foreach (var character in battleStateMachine.CharacterStats.Where(character => character.characterType == CharacterType.Player))
            {
                character.GetComponent<PlayerCombatStateMachine>().SwitchDodgeState();
            }
        }

        private void PlayerBlock()
        {
            foreach (var character in battleStateMachine.CharacterStats.Where(character => character.characterType == CharacterType.Player))
            {
                character.GetComponent<PlayerCombatStateMachine>().SwitchBlockState();
            }
        }

    }
}
