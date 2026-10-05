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

        public void Reset()
        {
            DamageReceived = 0f;
            DamageHistories.Clear();
            SuccessfulParryTimes = 0;
            SuccessfulDodgeTimes = 0;
        }

        public float GetHighestDamage()
        {
            return DamageHistories.Count == 0 ? 0 : DamageHistories.Max();
        }

        public float DamageDeals()
        {
            return DamageHistories.Sum();
        }
    }
}