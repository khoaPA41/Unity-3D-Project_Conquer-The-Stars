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

            if (target == null) return;

            //  Calculate damage if critical
            var isCrit = battleStateMachine.PlayerCombatStateMachine.CharacterStatsManagers.RandomCritical();
            var damage = isCrit ?
                        battleStateMachine.PlayerCombatStateMachine.CharacterStatsManagers.CalculateCriticalDamage() :
                        battleStateMachine.PlayerCombatStateMachine.CharacterStatsManagers.CurrentAttackDamage;

            // Final damage = attack * skill multiplier * perfect timing bonus
            var finalDamage = battleStateMachine.PlayerCombatStateMachine.GetAttackDameScale() *
            damage *
            UIManagers.Instance.GetActionFrameValue();

            // TakeDamage will return false if enemy block / dodge
            if (enemyStatsManager.TakeDamage(finalDamage, isCrit, battleStateMachine.CurrentTurn.HitVFXName))
            {
                // Track battle statistics for result screen
                battleStateMachine.HighestDamage = Mathf.Max(battleStateMachine.HighestDamage, finalDamage);
                battleStateMachine.PlayerCombatStateMachine.BattleStatistics.DamageHistories.Add(finalDamage);

                battleStateMachine.DamageDealt += finalDamage;
                target.HighlightTarget.InactiveHighlight();
                target.SwitchState(target.GethitState);
            }
        }
    }
}
