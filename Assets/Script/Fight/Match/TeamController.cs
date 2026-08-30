using System.Collections.Generic;
using System.Linq;
using ConquerTheStars.Stats;
using UnityEngine;

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
        PlayerTeam.Add(player);
    }

    public void AddEnemyTeam(CharacterStatsManagers enemy)
    {
        EnemyTeam.Add(enemy);
    }
}
