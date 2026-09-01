using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ConquerTheStars.Stats
{
    public class BattleStatistics : MonoBehaviour
    {
        public float DamageReceived { get; set; }
        public List<float> DamageHistories { get; set; } = new();
        public int SuccessfulParryTimes { get; set; }
        public int SuccessfulDodgeTimes { get; set; }


        public float GetHighestDamage()
        {
            return DamageHistories.Max();
        }
    }
}