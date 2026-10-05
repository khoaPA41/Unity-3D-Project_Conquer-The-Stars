using System.Collections.Generic;
using System.Linq;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Fight.Match
{
    public class TeamController : MonoBehaviour
    {
        public List<CharacterStatsManagers> PlayerTeam = new();
        public List<CharacterStatsManagers> EnemyTeam = new();

        public bool IsVictory { get; set; }

        private bool IsDeadTeam(List<CharacterStatsManagers> team)
        {
            return team.All(character => character.IsDeath);
        }

        public bool CheckBattleResult()
        {
            if (IsDeadTeam(PlayerTeam)) // Check player team
            {
                IsVictory = false;
                return true; // if all dead
            }

            if (IsDeadTeam(EnemyTeam)) // Check enemy team
            {
                IsVictory = true;
                return true; // if all dead
            }
            return false;
        }

        public void AddPlayerTeam(CharacterStatsManagers player)
        {
            player.UpdateLevel(Fight.PlayerTeam.Instance.TeamLevel);
            player.Init(); // Setup Stats before add to list
            PlayerTeam.Add(player);
        }

        public void AddEnemyTeam(CharacterStatsManagers enemy)
        {
            enemy.Init(); // Setup Stats before add to list
            EnemyTeam.Add(enemy);
        }

        public List<CharacterStatsManagers> ReturnAllyDeath()
        {
            return PlayerTeam.FindAll(ally => ally.IsDeath);
        }

        public bool IsSomeOneInPlayerDead()
        {
            return PlayerTeam.Find(ally => ally.IsDeath);
        }

        public bool IsSomeOneInEnemyDead()
        {
            return EnemyTeam.Find(enemy => enemy.IsDeath);
        }
    }
}