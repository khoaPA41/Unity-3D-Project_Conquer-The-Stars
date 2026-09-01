using UnityEngine;

[CreateAssetMenu(fileName = "StrategyEvaluation", menuName = "Scriptable Objects/StrategyEvaluation")]
public class StrategyEvaluation : ScriptableObject
{
    public float hpWeight;
    public float threatWeight;
    public float defenseWeight;
    public float statusWeight;
}
