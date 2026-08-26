using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttack", menuName = "Scriptable Objects/PlayerAttack")]
public class PlayerAttack : ScriptableObject
{
    public List<string> AttackName;
    public List<string> SkillName;

    public List<string> AttackInformation;
    public List<string> SkillInformation;

    public List<Sprite> AttackIcon;
    public List<Sprite> SkillIcon;

    public List<float> AttackDamage;
    public List<float> SkillDamage;

}
