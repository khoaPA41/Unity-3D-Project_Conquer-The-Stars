using UnityEngine;

namespace ConquerTheStars.Fight.Stats
{
    public class BattleStats : MonoBehaviour
    {
        [field: Header("Battle Stats")]
        [field: SerializeField] public float Health;
        [field: SerializeField] public float Attack;
        [field: SerializeField] public float Speed;
        [field: SerializeField] public float Defense;
        [field: SerializeField] public float CriticalRate;
    }
}