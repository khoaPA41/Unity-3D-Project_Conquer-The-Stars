using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttack", menuName = "Scriptable Objects/PlayerAttack")]
public class PlayerAttack : ScriptableObject
{
    public List<string> AttackName;
    public List<string> AttackInformation;
    public List<Sprite> AttackIcon;
    public List<float> AttackDamage;
}
