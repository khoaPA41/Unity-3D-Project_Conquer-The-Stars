using ConquerTheStars.Factory.Item;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType ItemType;
    public Sprite ItemIcon;
    public string ItemInformation;
}
