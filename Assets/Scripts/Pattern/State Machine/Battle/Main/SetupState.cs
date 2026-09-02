using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConquerTheStars.Fight.Match;
using ConquerTheStars.Fight.Target;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Player;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class SetupState : BattleBaseState
    {
        // List of characters who will be in the match
        private List<CharacterStatsManagers> characterInMatch = new();

        private List<string> enemyTeam = new();
        private List<string> playerTeam = new();
        public SetupState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            battleStateMachine.StartCoroutine(WaitToSetup());
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        public void Initialize()
        {
            // Get enemy & player list
            enemyTeam = BattleInformationManagers.Instance.AreaInformation.enemyTeam;
            playerTeam = PlayerTeam.Instance.TeamNameList;
        }

        private void SetupEnemyPosition()
        {
            for (int i = 0; i < enemyTeam.Count; i++)
            {
                // Spawn enemy at target position
                var enemy = ObjectPoolingManagers.Instance.GetPooledObject(enemyTeam[i], battleStateMachine.Area.enemyTransformList[BattleInformationManagers.Instance.AreaInformation.AreaIndex].enemyTransformList[i].position);
                enemy.transform.Rotate(new Vector3(0f, 90f, 0f));

                // Add to characterInMatch list - prepare for queue
                characterInMatch.Add(enemy.GetComponent<CharacterStatsManagers>());

                // Add to the team list used to manage status throughout the match
                battleStateMachine.TeamController.AddEnemyTeam(enemy.GetComponent<CharacterStatsManagers>());
            }
        }

        private void SetupPlayerPosition() // Spawn player at target position - add to player team list and queue
        {
            for (int i = 0; i < playerTeam.Count; i++)
            {
                // Spawn player at target position
                var player = ObjectPoolingManagers.Instance.GetPooledObject(playerTeam[i], battleStateMachine.Area.playerTransformList[BattleInformationManagers.Instance.AreaInformation.AreaIndex].playerTransformList[i].position);
                player.transform.Rotate(new Vector3(0f, -90f, 0f));

                // Add to characterInMatch list - prepare for queue
                characterInMatch.Add(player.GetComponent<CharacterStatsManagers>());

                // Add to the team list used to manage status throughout the match
                battleStateMachine.TeamController.AddPlayerTeam(player.GetComponent<CharacterStatsManagers>());
            }
        }

        private void AddCharacterToQueue()
        {
            battleStateMachine.CharacterStats = characterInMatch;
        }

        private void SetupPlayerTarget()
        {
            foreach (var enemy in battleStateMachine.TeamController.EnemyTeam)
            {
                // Add all enemy to player target list
                battleStateMachine.PlayerTargeter.SetupTargetList(enemy.GetComponent<Target>());
            }
        }

        private void SetupEnemyTarget()
        {
            foreach (var player in battleStateMachine.TeamController.PlayerTeam)
            {
                // Add all player to enemy target list
                battleStateMachine.EnemyTargeter.SetupTargetList(player.GetComponent<Target>());
            }
        }

        private IEnumerator WaitToSetup()
        {
            Initialize();
            SetupEnemyPosition();
            SetupPlayerPosition();

            // Sort the match list in descending speed order
            characterInMatch.Sort((a, b) => b.CurrentSpeed.CompareTo(a.CurrentSpeed));

            AddCharacterToQueue();
            SetupPlayerTarget();
            SetupEnemyTarget();

            battleStateMachine.SpeedAverage = characterInMatch.Average(character => character.CurrentSpeed);
            Debug.Log(battleStateMachine.SpeedAverage);

            UIManagers.Instance.SetTurnOrder(battleStateMachine.CharacterStats);
            yield return new WaitForSecondsRealtime(3f);
            battleStateMachine.SwitchState(battleStateMachine.StartTurn);
        }
    }
}
