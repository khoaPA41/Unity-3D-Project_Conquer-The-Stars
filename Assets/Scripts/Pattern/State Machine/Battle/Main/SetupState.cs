using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConquerTheStars.Fight;
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
        private static readonly WaitForSecondsRealtime _waitToSetup = new WaitForSecondsRealtime(3f);

        // List of characters who will be in the match
        private List<CharacterStatsManagers> _characterInMatch = new();

        private List<string> _enemyTeam = new();
        private List<string> _playerTeam = new();
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
            _enemyTeam = BattleInformationManagers.Instance.AreaInformation.enemyTeam;
            _playerTeam = PlayerTeam.Instance.TeamNameList;
        }

        private void SetupEnemyPosition()
        {
            for (int i = 0; i < _enemyTeam.Count; i++)
            {
                // Spawn enemy at target position
                var enemy = ObjectPoolingManagers.Instance.GetPooledObject(_enemyTeam[i], battleStateMachine.Area.EnemyTransformList[BattleInformationManagers.Instance.AreaInformation.AreaIndex].EnemyTransforms[i].position);
                enemy.transform.Rotate(new Vector3(0f, 90f, 0f));
                battleStateMachine.IsFinalBoss = enemy.name == "Final_Boss";
                // Add to characterInMatch list - prepare for queue
                _characterInMatch.Add(enemy.GetComponent<CharacterStatsManagers>());

                // Add to the team list used to manage status throughout the match
                battleStateMachine.TeamController.AddEnemyTeam(enemy.GetComponent<CharacterStatsManagers>());
            }
        }

        private void SetupPlayerPosition() // Spawn player at target position - add to player team list and queue
        {
            for (int i = 0; i < _playerTeam.Count; i++)
            {
                // Spawn player at target position
                var player = ObjectPoolingManagers.Instance.GetPooledObject(_playerTeam[i], battleStateMachine.Area.PlayerTransformList[BattleInformationManagers.Instance.AreaInformation.AreaIndex].PlayerTransforms[i].position);

                player.transform.Rotate(new Vector3(0f, -90f, 0f));

                // Add to characterInMatch list - prepare for queue
                _characterInMatch.Add(player.GetComponent<CharacterStatsManagers>());

                // Add to the team list used to manage status throughout the match
                battleStateMachine.TeamController.AddPlayerTeam(player.GetComponent<CharacterStatsManagers>());
            }
        }

        private void AddCharacterToBattleList()
        {
            battleStateMachine.CharacterStats = _characterInMatch;
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

        private void SetupAllyTarget()
        {
            foreach (var player in battleStateMachine.TeamController.PlayerTeam)
            {
                // Add all player to ally target list
                battleStateMachine.AllyTargeter.SetupTargetList(player.GetComponent<Target>());
            }
        }

        private IEnumerator WaitToSetup()
        {
            Initialize();
            SetupEnemyPosition();
            SetupPlayerPosition();

            // Sort the match list in descending speed order
            _characterInMatch.Sort((a, b) => b.CurrentSpeed.CompareTo(a.CurrentSpeed));

            AddCharacterToBattleList();

            SetupPlayerTarget();
            SetupEnemyTarget();
            SetupAllyTarget();

            battleStateMachine.SpeedAverage = _characterInMatch.Average(character => character.CurrentSpeed);

            UICombatManagers.Instance.SetTurnOrder(battleStateMachine.CharacterStats);
            yield return _waitToSetup;
            battleStateMachine.SwitchState(battleStateMachine.StartTurn);
        }
    }
}
