using System.Collections;
using System.Linq;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using Unity.Burst.Intrinsics;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class BattleResultState : BattleBaseState
    {
        public BattleResultState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            if (battleStateMachine.TeamController.IsVictory)
            {
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
            yield return new WaitForSecondsRealtime(2f);
            ChangeVictoryState();
            CalculateResultInformation();
            battleStateMachine.VictoryCamera.gameObject.SetActive(true);
            SetupResultBoard();
        }
        private IEnumerator WaitToChangeDefeat()
        {
            yield return new WaitForSecondsRealtime(2f);
            CalculateResultInformation();
            SetupResultBoard();
        }

        private void ChangeVictoryState()
        {
            foreach (var characters in battleStateMachine.TeamController.PlayerTeam)
            {
                characters.GetComponent<PlayerCombatStateMachine>().SwitchVictoryState();
            }
        }
        private void CalculateResultInformation()
        {
            var damageReceived = battleStateMachine.TeamController.PlayerTeam.Sum(player => player.DamageReceived);
            var succesfulDodge = battleStateMachine.TeamController.PlayerTeam.Sum(player => player.SuccessfulDodgeTimes);
            var succesfulParry = battleStateMachine.TeamController.PlayerTeam.Sum(player => player.SuccessfulParryTimes);
            battleStateMachine.DamageReceived = damageReceived;
            battleStateMachine.SuccessfulDodgeTimes = succesfulDodge;
            battleStateMachine.SuccessfulParryTimes = succesfulParry;
        }

        private void SetupResultBoard()
        {
            UIManagers.Instance.SetResultText(
                            Mathf.RoundToInt(battleStateMachine.HighestDamage).ToString(),
                            Mathf.RoundToInt(battleStateMachine.DamageDealt).ToString(),
                            Mathf.RoundToInt(battleStateMachine.DamageReceived).ToString(),
                            Mathf.RoundToInt(battleStateMachine.BattleTime).ToString(),
                            Mathf.RoundToInt(battleStateMachine.SuccessfulParryTimes).ToString(),
                            Mathf.RoundToInt(battleStateMachine.SuccessfulDodgeTimes).ToString()
                        );
            foreach (var enemy in battleStateMachine.TeamController.EnemyTeam)
            {
                UIManagers.Instance.SpawnKillElement(enemy.icon);
            }

            UIManagers.Instance.ActiveResultBoard(battleStateMachine.TeamController.IsVictory);
        }
    }
}

