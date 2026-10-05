using System.Collections;
using System.Linq;
using ConquerTheStars.Fight;
using ConquerTheStars.Fight.Match;
using ConquerTheStars.Managers;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.UI.Player;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class BattleResultState : BattleBaseState
    {
        private static readonly WaitForSecondsRealtime _waitToChangeVictory = new(2f);

        private static readonly WaitForSecondsRealtime _waitToChangeDefeat = new(2f);

        public BattleResultState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            UICombatManagers.Instance.ResetTurnOrder();
            if (battleStateMachine.TeamController.IsVictory)
            {
                PlayerTeam.Instance.AddExp(battleStateMachine.BattleReward.ExpReward); // Get Reward
                BattleInformationManagers.Instance.Win(); // Save battle id
                GameManager.Instance.AutoSaveGame();
                battleStateMachine.StartCoroutine(WaitToChangeVictory());
            }
            else
            {
                battleStateMachine.StartCoroutine(WaitToChangeDefeat());
            }
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        private IEnumerator WaitToChangeVictory()
        {
            yield return _waitToChangeVictory;
            ChangeVictoryState();
            CalculateAndActiveResultInformation();
            battleStateMachine.VictoryCamera.gameObject.SetActive(true);
        }

        private IEnumerator WaitToChangeDefeat()
        {
            yield return _waitToChangeDefeat;
            CalculateAndActiveResultInformation();
        }

        private void ChangeVictoryState()
        {
            foreach (var characters in battleStateMachine.TeamController.PlayerTeam)
            {
                if (characters.IsDeath) continue;
                characters.GetComponent<PlayerCombatStateMachine>().SwitchVictoryState();
            }
        }

        private void CalculateAndActiveResultInformation()
        {
            var highestDamage = battleStateMachine.TeamController.PlayerTeam.Max(player => player.GetComponent<PlayerCombatStateMachine>().BattleStatistics.GetHighestDamage());
            var damageDeals = battleStateMachine.TeamController.PlayerTeam.Sum(player => player.GetComponent<PlayerCombatStateMachine>().BattleStatistics.DamageDeals());
            var damageReceived = battleStateMachine.TeamController.PlayerTeam.Sum(player => player.GetComponent<PlayerCombatStateMachine>().BattleStatistics.DamageReceived);
            var succesfulParry = battleStateMachine.TeamController.PlayerTeam.Sum(player => player.GetComponent<PlayerCombatStateMachine>().BattleStatistics.SuccessfulParryTimes);
            var succesfulDodge = battleStateMachine.TeamController.PlayerTeam.Sum(player => player.GetComponent<PlayerCombatStateMachine>().BattleStatistics.SuccessfulDodgeTimes);

            SetupResultBoard(highestDamage, damageDeals, damageReceived, succesfulParry, succesfulDodge);
        }

        private void SetupResultBoard(float highestDmg, float dmgDeals, float dmgReceiver, float parryTime, float dodgeTime)
        {
            UICombatManagers.Instance.SetResultText(
                Mathf.RoundToInt(highestDmg).ToString(),
                Mathf.RoundToInt(dmgDeals).ToString(),
                Mathf.RoundToInt(dmgReceiver).ToString(),
                FormatBattleTime(Time.time - battleStateMachine.BattleTime),
                Mathf.RoundToInt(parryTime).ToString(),
                Mathf.RoundToInt(dodgeTime).ToString()
            );

            foreach (var enemy in battleStateMachine.TeamController.EnemyTeam)
            {
                UICombatManagers.Instance.SpawnKillElement(enemy.icon);
            }

            UICombatManagers.Instance.ActiveResultBoard(battleStateMachine.TeamController.IsVictory, battleStateMachine.IsFinalBoss);
        }

        public static string FormatBattleTime(float seconds)
        {
            var totalSeconds = Mathf.FloorToInt(seconds);
            var minutes = totalSeconds / 60;
            var secs = totalSeconds % 60;
            return $"{minutes:00}:{secs:00}";
        }
    }
}

