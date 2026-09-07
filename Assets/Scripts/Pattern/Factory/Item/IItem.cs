using UnityEngine;

namespace ConquerTheStars.Factory.Item
{
    public interface IItem
    {
        ItemType ItemType { get; }
        void Use(ICaster character, ItemType itemType, int remainingTurn, float value);
    }
}