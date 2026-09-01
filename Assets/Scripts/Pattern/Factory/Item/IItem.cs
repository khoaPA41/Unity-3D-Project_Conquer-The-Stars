using UnityEngine;

namespace ConquerTheStars.Factory.Item
{
    public interface IItem
    {
        // ItemData ItemData { get; set; }
        ItemType ItemType { get; }
        void Use(ICaster character, ItemData itemData);
    }
}