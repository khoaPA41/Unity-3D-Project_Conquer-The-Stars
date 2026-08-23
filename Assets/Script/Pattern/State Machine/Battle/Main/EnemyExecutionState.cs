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
            EnterTarget();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        private void EnterTarget()
        {
            battleStateMachine.StartCoroutine(WaitToEndAttack());
        }

        private IEnumerator WaitToEndAttack()
        {
            // battleStateMachine.PlayerCombatStateMachine.Target = battleStateMachine.PlayerTargeter.currentTarget; // Get Current Target form select Target state
            battleStateMachine.EnemyStateMachine.AttackDealDamage += EnemyDealDamage; // Subscribe animation event 
            battleStateMachine.EnemyStateMachine.SwitchAttackState();

            yield return new WaitUntil(() => battleStateMachine.EnemyStateMachine.IsFinished = true);
            battleStateMachine.PlayerCombatStateMachine.AttackDealDamage -= EnemyDealDamage; // Subscribe animation event 
            battleStateMachine.SwitchResolve();
        }
        private void EnemyDealDamage()
        {
            var target = battleStateMachine.EnemyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>();
            var enemyStatsManager = battleStateMachine.EnemyTargeter.currentTarget.GetComponent<CharacterStatsManagers>();
            if (target != null)
            {
                target.SwitchState(target.PlayerGetHitState);
                enemyStatsManager.TakeDamage(30);
            }
        }

    }
}
