using System.Collections;
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
            battleStateMachine.EnemyStateMachine.HighlightCurrentTurn.InactiveHighlight();

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
            //Prepare attack
            battleStateMachine.EnemyStateMachine.SwitchAttackState();

            // Listen event for exact the frame attack deals damage
            battleStateMachine.EnemyStateMachine.AttackDealDamage += EnemyDealDamage;

            //Wait until player attack animation done
            yield return new WaitUntil(() => battleStateMachine.EnemyStateMachine.IsFinished == true);

            // Clear event to avoid double call / memory leak
            battleStateMachine.EnemyStateMachine.AttackDealDamage -= EnemyDealDamage;
            battleStateMachine.SwitchResolve();
        }
        private void EnemyDealDamage()
        {
            var target = battleStateMachine.EnemyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>();
            var playerStatsManager = battleStateMachine.EnemyTargeter.currentTarget.GetComponent<CharacterStatsManagers>();
            if (target != null)
            {
                var damage = battleStateMachine.EnemyStateMachine.CharacterStatsManagers.CurrentAttackDamage;

                // TakeDamage will return false if player block / dodge
                if (playerStatsManager.TakeDamage(damage))
                {
                    target.SwitchState(target.PlayerGetHitState);

                    // Track battle statistics for result screen
                    target.BattleStatistics.DamageReceived += damage;
                }
                else
                {
                    // Track battle statistics for result screen
                    if (playerStatsManager.IsDodge)
                    {
                        target.BattleStatistics.SuccessfulDodgeTimes++;
                    }
                    if (playerStatsManager.IsBlock)
                    {
                        target.BattleStatistics.SuccessfulParryTimes++;
                    }
                }
            }
        }

        private void PlayerDodge()
        {
            battleStateMachine.EnemyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>().SwitchDodgeState();
        }

        private void PlayerBlock()
        {
            battleStateMachine.EnemyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>().SwitchBlockState();
        }
    }
}
