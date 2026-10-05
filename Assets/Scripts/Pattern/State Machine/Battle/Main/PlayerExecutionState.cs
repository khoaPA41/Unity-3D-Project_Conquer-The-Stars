using System.Collections;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Player;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerExecutionState : BattleBaseState
    {
        private PlayerCombatStateMachine _attackingPlayer;
        private TouchSwipeController _touchSwipeController;
        private UICombatManagers _combatUi;

        public PlayerExecutionState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            _attackingPlayer = battleStateMachine.PlayerCombatStateMachine;
            _touchSwipeController = battleStateMachine.TouchSwipeController;
            _combatUi = UICombatManagers.Instance;

            _attackingPlayer.HighlightCurrentTurn.InactiveHighlight();
            _attackingPlayer.IsFinished = false;
            _attackingPlayer.Target = battleStateMachine.PlayerTargeter.CurrentTarget;

            _attackingPlayer.AttackDealDamage += PlayerDealDamage;
            _touchSwipeController.AttackAction += _combatUi.PausePerfectFrame;
            _attackingPlayer.SwitchState(_attackingPlayer.PlayerAttackState);
        }

        public override void Tick(float deltaTime)
        {
            if (_attackingPlayer == null || !_attackingPlayer.IsFinished) return;

            battleStateMachine.SwitchResolve();
        }

        public override void Exit()
        {
            if (_attackingPlayer != null)
            {
                _attackingPlayer.AttackDealDamage -= PlayerDealDamage;
                _attackingPlayer.InactiveCamera();
            }

            if (_touchSwipeController != null && _combatUi != null)
                _touchSwipeController.AttackAction -= _combatUi.PausePerfectFrame;

            _attackingPlayer = null;
            _touchSwipeController = null;
            _combatUi = null;
        }

        private void PlayerDealDamage()
        {
            var target = battleStateMachine.PlayerTargeter.CurrentTarget.GetComponent<EnemyStateMachine>();
            var enemyStatsManager = battleStateMachine.PlayerTargeter.CurrentTarget.GetComponent<CharacterStatsManagers>();

            if (target == null) return;

            //  Calculate damage if critical
            var isCrit = _attackingPlayer.CharacterStatsManagers.RandomCritical();
            var damage = isCrit ?
                        _attackingPlayer.CharacterStatsManagers.CalculateCriticalDamage() :
                        _attackingPlayer.CharacterStatsManagers.CurrentAttackDamage;

            // Final damage = attack * skill multiplier * perfect timing bonus
            var finalDamage = _attackingPlayer.GetAttackDameScale() *
            damage *
            UICombatManagers.Instance.GetActionFrameValue();

            // TakeDamage will return false if enemy block / dodge
            if (enemyStatsManager.TakeDamage(finalDamage, isCrit, battleStateMachine.CurrentTurn.HitVFXName))
            {
                // Track battle statistics for result screen
                _attackingPlayer.BattleStatistics.DamageHistories.Add(finalDamage);

                // Play hit sound if player deal dmg succes
                _attackingPlayer.PlayHitSound();

                target.HighlightTarget.InactiveHighlight();
                target.SwitchState(target.GethitState);
            }
        }
    }
}
