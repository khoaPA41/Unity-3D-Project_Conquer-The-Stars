using System.Collections.Generic;
using System.Linq;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Stats;
using UnityEditor;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class SetupState : BattleBaseState
    {
        private List<CharacterStatsManagers> characterInMatch = new();
        private List<string> enemyTeam = new();
        private List<string> playerTeam = new();
        public SetupState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            Initialize();
            SetupEnemyPosition();
            SetupPlayerPosition();

            characterInMatch.Sort((a, b) => b.speed.GetFinalValue().CompareTo(a.speed.GetFinalValue()));
            AddCharacterToQueue();
            SetupPlayerTarget();
            SetupEnemyTarget();
            battleStateMachine.SwitchState(battleStateMachine.StartTurn);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        public void Initialize() // attach enemy & player list
        {
            enemyTeam = BattleInformationManagers.Instance.AreaInformation.enemyTeam;
            playerTeam = PlayerTeam.Instance.TeamNameList;
        }

        private void SetupEnemyPosition() // Spawn enemy at target position - add to enemy team list and queue
        {
            for (int i = 0; i < enemyTeam.Count; i++)
            {
                var enemy = ObjectPoolingManagers.Instance.GetPooledObject(enemyTeam[i], battleStateMachine.Area.enemyTransformList[BattleInformationManagers.Instance.AreaInformation.AreaIndex].enemyTransformList[i].position);

                enemy.transform.Rotate(new Vector3(0f, 90f, 0f));
                // enemy.transform.Rotate(new Vector3(0f, 90f, 0f));
                characterInMatch.Add(enemy.GetComponent<CharacterStatsManagers>());
                battleStateMachine.TeamController.AddEnemyTeam(enemy.GetComponent<CharacterStatsManagers>());
            }
        }

        private void SetupPlayerPosition() // Spawn player at target position - add to player team list and queue
        {
            for (int i = 0; i < playerTeam.Count; i++)
            {
                var player = ObjectPoolingManagers.Instance.GetPooledObject(playerTeam[i], battleStateMachine.Area.playerTransformList[BattleInformationManagers.Instance.AreaInformation.AreaIndex].playerTransformList[i].position);
                player.transform.Rotate(new Vector3(0f, -90f, 0f));
                Debug.Log(player.transform.rotation);

                characterInMatch.Add(player.GetComponent<CharacterStatsManagers>());
                battleStateMachine.TeamController.AddPlayerTeam(player.GetComponent<CharacterStatsManagers>());
            }
        }

        private void AddCharacterToQueue()
        {
            foreach (var character in characterInMatch)
            {
                battleStateMachine.CharacterStats.Enqueue(character);
            }
        }

        private void SetupPlayerTarget() // Add all enemy to player target list
        {
            foreach (var enemy in characterInMatch.Where(enemy => enemy.characterType == CharacterType.Enemy))
            {
                var target = enemy.GetComponent<Target>();
                battleStateMachine.PlayerTargeter.SetupTargetList(target);
            }
            // battleStateMachine.PlayerTargeter.FirstSelected();
        }

        private void SetupEnemyTarget() // Add all player to enemy target list
        {
            foreach (var player in characterInMatch.Where(player => player.characterType == CharacterType.Player))
            {
                var target = player.GetComponent<Target>();
                battleStateMachine.EnemyTargeter.SetupTargetList(target);
            }
            battleStateMachine.EnemyTargeter.FirstSelected();
        }
    }
}
