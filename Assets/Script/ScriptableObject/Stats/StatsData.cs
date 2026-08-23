using ConquerTheStars.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsData", menuName = "Scriptable Objects/StatsData")]

public class StatsData : ScriptableObject
{
    public CharacterType Type;
    public float Health;
    public float AttackPower;
    public float Speed;
    public float Defense;
    public float Critical;
}
