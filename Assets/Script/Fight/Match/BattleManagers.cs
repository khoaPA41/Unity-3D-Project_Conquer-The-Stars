using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    BattleStart,
    DetermineTurnOrder,
    PlayerTurn,
    ActionSelection,
    ActionExecution,
    EnemyTurn,
    BattleEnd
}

public class BattleManagers : MonoBehaviour
{
    private BattleState battleState;
    private StartMatch startMatch;

    private void Start()
    {
        battleState = BattleState.BattleStart;
        startMatch = GameObject.FindGameObjectWithTag("StartMatch").GetComponent<StartMatch>();
    }

    public IEnumerator BattleCircle()
    {
        while (battleState != BattleState.BattleEnd)
        {


            yield return null;
        }
    }

    // private void CheckFirstAttack()
    // {
    //     var speed = 0f;
    //     foreach (var player in startMatch.ObjectInBattle)
    //     {
    //         var maxSpeed = Mathf.Max(speed, player.Value.Speed);

    //     }
    //     if (startMatch.ObjectInBattle.ContainsKey(maxSpeed).)
    //     {

    //     }
    // }



}
