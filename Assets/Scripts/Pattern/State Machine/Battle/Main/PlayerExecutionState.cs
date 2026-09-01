using System.Collections;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Player;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerExecutionState : BattleBaseState
    {

        public PlayerExecutionState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            battleStateMachine.PlayerCombatStateMachine.HighlightCurrentTurn.InactiveHighlight();
            battleStateMachine.PlayerCombatStateMachine.IsFinished = false;
            // battleStateMachine.PlayerTargeter.RemoveTargetCamera();
            battleStateMachine.StartCoroutine(WaitToEndAttack());
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {

        }

        private IEnumerator WaitToEndAttack()
        {
            //Prepare attack
            battleStateMachine.PlayerCombatStateMachine.Target = battleStateMachine.PlayerTargeter.currentTarget;
            battleStateMachine.PlayerCombatStateMachine.SwitchState(battleStateMachine.PlayerCombatStateMachine.PlayerAttackState);

            // Listen event for exact the frame attack deals damage
            battleStateMachine.PlayerCombatStateMachine.AttackDealDamage += PlayerDealDamage;
            battleStateMachine.InputReader.EnterTargetAction += UIManagers.Instance.PausePerfectFrame;

            //Wait until player attack animation done
            yield return new WaitUntil(() => battleStateMachine.PlayerCombatStateMachine.IsFinished == true);

            // Clear event to avoid double call / memory leak
            battleStateMachine.PlayerCombatStateMachine.AttackDealDamage -= PlayerDealDamage;
            battleStateMachine.InputReader.EnterTargetAction -= UIManagers.Instance.PausePerfectFrame;

            // battleStateMachine.PlayerCombatStateMachine.InactiveCamera();
            battleStateMachine.SwitchResolve();
        }

        private void PlayerDealDamage()
        {
            var target = battleStateMachine.PlayerTargeter.currentTarget.GetComponent<EnemyStateMachine>();
            var enemyStatsManager = battleStateMachine.PlayerTargeter.currentTarget.GetComponent<CharacterStatsManagers>();
            if (target != null)
            {
                // Final damage = base attack * skill multiplier * perfect timing bonus
                var damage = battleStateMachine.PlayerCombatStateMachine.GetAttackDameScale() *
                battleStateMachine.PlayerCombatStateMachine.CharacterStatsManagers.CurrentAttackDamage *
                UIManagers.Instance.GetActionFrameValue();

                // TakeDamage will return false if enemy block / dodge
                if (enemyStatsManager.TakeDamage(damage))
                {
                    // Track battle statistics for result screen
                    battleStateMachine.HighestDamage = Mathf.Max(battleStateMachine.HighestDamage, damage);
                    battleStateMachine.PlayerCombatStateMachine.BattleStatistics.DamageHistories.Add(damage);

                    battleStateMachine.DamageDealt += damage;
                    target.HighlightTarget.InactiveHighlight();
                    target.SwitchState(target.GethitState);
                }
            }
        }
    }
}
