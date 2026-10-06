using ConquerTheStars.Factory.Item;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemAttachData", menuName = "Scriptable Objects/ItemAttachData")]
public class ItemAttachData : ScriptableObject
{
    public ItemAttachType ItemAttachType;
    public float Value;
    public Sprite Icon;
}
