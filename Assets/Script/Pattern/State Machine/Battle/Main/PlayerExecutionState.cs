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
            battleStateMachine.InputReader.EnterTargetAction += UIManagers.Instance.PausePerfectFrame;

            yield return new WaitUntil(() => battleStateMachine.PlayerCombatStateMachine.IsFinished == true); // Wait until atk animation done
            battleStateMachine.PlayerCombatStateMachine.AttackDealDamage -= PlayerDealDamage; // UnSubscribe animation event 
            battleStateMachine.SwitchResolve();
        }

        private void PlayerDealDamage()
        {
            var target = battleStateMachine.PlayerTargeter.currentTarget.GetComponent<EnemyStateMachine>(); // Get StateMachine from target
            var enemyStatsManager = battleStateMachine.PlayerTargeter.currentTarget.GetComponent<CharacterStatsManagers>(); // Get CharacterStatsManagers from target
            if (target != null)
            {
                if (enemyStatsManager.TakeDamage(battleStateMachine.PlayerCombatStateMachine.AttackData.AttackDamage[battleStateMachine.AttackIndexSelected]
                * UIManagers.Instance.GetActionFrameValue())) // take damage
                {
                    Debug.Log(UIManagers.Instance.GetActionFrameValue());
                    target.SwitchState(target.GethitState); // Switch target state to get hit
                }
            }
        }
    }
}
