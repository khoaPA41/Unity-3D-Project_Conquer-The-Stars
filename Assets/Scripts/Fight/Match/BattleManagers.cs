using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConquerTheStars.Fight.Match
{
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
        private BattleState _battleState;
        private StartMatch _startMatch;

        private void Start()
        {
            _battleState = BattleState.BattleStart;
            _startMatch = GameObject.FindGameObjectWithTag("StartMatch").GetComponent<StartMatch>();
        }

        public IEnumerator BattleCircle()
        {
            while (_battleState != BattleState.BattleEnd)
            {


                yield return null;
            }
        }
    }
}