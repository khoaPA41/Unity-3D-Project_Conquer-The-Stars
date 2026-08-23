using System.Collections;
using System.Threading.Tasks;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
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
            battleStateMachine.PlayerCombatStateMachine.IsFinished = false;
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
            battleStateMachine.PlayerCombatStateMachine.Target = battleStateMachine.PlayerTargeter.currentTarget; // Get Current Target form select Target state
            battleStateMachine.PlayerCombatStateMachine.AttackDealDamage += PlayerDealDamage; // Subscribe animation event 
            battleStateMachine.PlayerCombatStateMachine.SwitchAttackState(battleStateMachine.AttackIndexSelected); // Switch Player combat atk state

            yield return new WaitUntil(() => battleStateMachine.PlayerCombatStateMachine.IsFinished == true);
            battleStateMachine.PlayerCombatStateMachine.AttackDealDamage -= PlayerDealDamage; // Wait until atk animation done
            battleStateMachine.SwitchResolve();
        }

        private void PlayerDealDamage()
        {
            var target = battleStateMachine.PlayerTargeter.currentTarget.GetComponent<EnemyStateMachine>();
            var enemyStatsManager = battleStateMachine.PlayerTargeter.currentTarget.GetComponent<CharacterStatsManagers>();
            if (target != null)
            {
                target.SwitchState(target.GethitState);
                enemyStatsManager.TakeDamage(battleStateMachine.PlayerCombatStateMachine.AttackData.AttackDamage[battleStateMachine.AttackIndexSelected]);
            }
        }
    }
}
