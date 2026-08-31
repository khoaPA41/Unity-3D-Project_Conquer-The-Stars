using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Factory.Item
{
    public class HealingItem : IItem
    {
        public ItemType ItemType => ItemType.Healing;

        public void Use(ICaster character, ItemData itemData)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();
            var healingAmount = (characterStatsManagers.maxHealth.GetFinalValue() - characterStatsManagers.CurrentHealth) * .5f;
            characterStatsManagers.Healing(healingAmount);
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");
        }
    }

    public class RecoveryMana : IItem
    {
        public ItemType ItemType => ItemType.RecoveryMana;

        public void Use(ICaster character, ItemData itemData)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();
            var manaIncreaseAmount = 20f + (characterStatsManagers.CurrentMana * .5f);
            characterStatsManagers.AddMana(manaIncreaseAmount);
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");
        }
    }

    public class IncreaseDefense : IItem
    {
        public ItemType ItemType => ItemType.IncreaseDefense;

        public void Use(ICaster character, ItemData itemData)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();
            var defenseAmount = 20f;
            characterStatsManagers.IncreaseDefense(defenseAmount);
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");

        }
    }
    public class IncreaseDamage : IItem
    {
        public ItemType ItemType => ItemType.IncreaseDamage;

        public void Use(ICaster character, ItemData itemData)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();
            var damageAmount = 20f;
            characterStatsManagers.IncreaseDamage(damageAmount);
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");

        }
    }
    public class IncreaseCritical : IItem
    {
        public ItemType ItemType => ItemType.IncreaseCritical;

        public void Use(ICaster character, ItemData itemData)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();
            var criticalcAmount = 20f;
            characterStatsManagers.IncreaseCritical(criticalcAmount);
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");

        }
    }
    public class IncreaseSpeed : IItem
    {
        public ItemType ItemType => ItemType.IncreaseSpeed;

        public void Use(ICaster character, ItemData itemData)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();
            var speedAmount = 20f + (characterStatsManagers.CurrentMana * .5f);
            characterStatsManagers.IncreaseSpeed(speedAmount);
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");

        }
    }
    public class Revive : IItem
    {
        public ItemType ItemType => ItemType.Revive;

        public void Use(ICaster character, ItemData itemData)
        {
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");

        }
    }
    public class RemoveDisruption : IItem
    {
        public ItemType ItemType => ItemType.RemoveDisruption;

        public void Use(ICaster character, ItemData itemData)
        {
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");

        }
    }
    public class SummonTurret : IItem
    {
        public ItemType ItemType => ItemType.SummonTurret;

        public void Use(ICaster character, ItemData itemData)
        {
            Debug.Log($"{character.CharacterUse().name}: {itemData.ItemType}");

        }
    }
}