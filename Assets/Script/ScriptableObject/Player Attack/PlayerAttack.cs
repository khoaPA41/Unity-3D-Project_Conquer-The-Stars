using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttack", menuName = "Scriptable Objects/PlayerAttack")]
public class PlayerAttack : ScriptableObject
{
    public List<string> AttackName;
    public List<string> AttackInformation;
    public List<Sprite> AttackIcon;
    public List<float> AttackDamage;

    public List<string> SkillName;
    public List<float> ManaRequired;
    public List<string> SkillInformation;
    public List<Sprite> SkillIcon;
    public List<float> SkillDamage;

}
