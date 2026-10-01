using System.Collections;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class EnemyExecutionState : BattleBaseState
    {

        private EnemyStateMachine _attackingEnemy;
        private TouchSwipeController _touchSwipeController;

        public EnemyExecutionState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            _attackingEnemy = battleStateMachine.EnemyStateMachine;
            _touchSwipeController = battleStateMachine.TouchSwipeController;

            _attackingEnemy.HighlightCurrentTurn.InactiveHighlight();
            _attackingEnemy.AttackDealDamage += EnemyDealDamage;
            _touchSwipeController.DodgeAction += PlayerDodge;
            _touchSwipeController.ParryAction += PlayerBlock;

            _attackingEnemy.IsFinished = false;


            SwitchAttackByType();
        }

        public override void Tick(float deltaTime)
        {
            if (_attackingEnemy == null || !_attackingEnemy.IsFinished) return;
            battleStateMachine.SwitchResolve();
        }

        public override void Exit()
        {
            if (_touchSwipeController != null)
            {
                _touchSwipeController.DodgeAction -= PlayerDodge;
                _touchSwipeController.ParryAction -= PlayerBlock;
            }

            if (_attackingEnemy != null)
                _attackingEnemy.AttackDealDamage -= EnemyDealDamage;

            _attackingEnemy = null;
            _touchSwipeController = null;
        }

        private void EnemyDealDamage()
        {
            var target = battleStateMachine.EnemyTargeter.CurrentTarget.GetComponent<PlayerCombatStateMachine>();
            var playerStatsManager = battleStateMachine.EnemyTargeter.CurrentTarget.GetComponent<CharacterStatsManagers>();
            if (target == null) return;

            //  Calculate damage if critical
            var isCrit = _attackingEnemy.CharacterStatsManagers.RandomCritical();
            var damage = isCrit ?
                        _attackingEnemy.CharacterStatsManagers.CalculateCriticalDamage() :
                        _attackingEnemy.CharacterStatsManagers.CurrentAttackDamage;


            // TakeDamage will return false if player block / dodge
            if (playerStatsManager.TakeDamage(damage, isCrit, battleStateMachine.CurrentTurn.HitVFXName))
            {
                // Play hit sound if player take dmg
                _attackingEnemy.PlayHitSound();

                target.SwitchState(target.PlayerGetHitState);

                // Track battle statistics for result screen
                target.BattleStatistics.DamageReceived += damage;
            }
            else
            {
                // Track battle statistics for result screen
                if (playerStatsManager.IsDodge)
                {
                    // Play dodge sound if player dodge succes
                    target.PlayDodgeSound();

                    target.BattleStatistics.SuccessfulDodgeTimes++;
                }

                if (playerStatsManager.IsBlock)
                {
                    // Play parry sound if player dodge succes
                    target.PlayParrySound();

                    target.BattleStatistics.SuccessfulParryTimes++;
                }
            }
        }

        private void PlayerDodge()
        {
            battleStateMachine.EnemyTargeter.CurrentTarget.GetComponent<PlayerCombatStateMachine>().SwitchDodgeState();
        }

        private void PlayerBlock()
        {
            battleStateMachine.EnemyTargeter.CurrentTarget.GetComponent<PlayerCombatStateMachine>().SwitchBlockState();
        }

        private void SwitchAttackByType()
        {
            if (_attackingEnemy.IsBoss)
            {
                _attackingEnemy.SwitchBossAttackState();
                return;
            }
            _attackingEnemy.SwitchAttackState();
        }
    }
}
