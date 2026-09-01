using UnityEngine;
namespace ConquerTheStars.Fight.Stats
{
    public class Stats : MonoBehaviour
    {
        [field: Header("Stats")]
        [field: SerializeField] public float Vitality;
        [field: SerializeField] public float Might;
        [field: SerializeField] public float Agility;
        [field: SerializeField] public float Defense;
        [field: SerializeField] public float Luck;
    }
}