using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Factory.Item
{
    public class HealingItem : IItem
    {
        public ItemType ItemType => ItemType.Healing;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();
            var healingAmount = value + (characterStatsManagers.maxHealth.GetFinalValue() - characterStatsManagers.CurrentHealth) * .5f;
            characterStatsManagers.Healing(healingAmount);
            Debug.Log($"{character.CharacterUse().name}: Heal: {value}");
        }
    }

    public class RecoveryMana : IItem
    {
        public ItemType ItemType => ItemType.RecoveryMana;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();
            var manaIncreaseAmount = value + (characterStatsManagers.CurrentMana * .5f);
            characterStatsManagers.AddMana(manaIncreaseAmount);
            Debug.Log($"{character.CharacterUse().name}: Mana: {value}");
        }
    }

    public class IncreaseDefense : IItem
    {
        public ItemType ItemType => ItemType.IncreaseDefense;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();

            characterStatsManagers.IncreaseDefense(value);


            if (value > 0)
            {
                var buffManager = character.CharacterUse().GetComponent<BuffManager>();
                buffManager.AddBuff(itemType, remainingTurn, value);
            }

            Debug.Log($"{character.CharacterUse().name}: Defense: {value}");
        }
    }
    public class IncreaseDamage : IItem
    {
        public ItemType ItemType => ItemType.IncreaseDamage;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();

            characterStatsManagers.IncreaseDamage(value);

            if (value > 0)
            {
                var buffManager = character.CharacterUse().GetComponent<BuffManager>();
                buffManager.AddBuff(itemType, remainingTurn, value);
            }

            Debug.Log($"{character.CharacterUse().name}: IncreaseDamage: {value}");
        }
    }
    public class IncreaseCritical : IItem
    {
        public ItemType ItemType => ItemType.IncreaseCritical;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();


            characterStatsManagers.IncreaseCritical(value);

            if (value > 0)
            {
                var buffManager = character.CharacterUse().GetComponent<BuffManager>();
                buffManager.AddBuff(itemType, remainingTurn, value);
            }
            Debug.Log($"{character.CharacterUse().name}: IncreaseCritical: {value}");
        }
    }
    public class IncreaseSpeed : IItem
    {
        public ItemType ItemType => ItemType.IncreaseSpeed;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {
            var characterStatsManagers = character.CharacterUse().GetComponent<CharacterStatsManagers>();

            // var speedAmount = value + (characterStatsManagers.CurrentMana * .5f);
            characterStatsManagers.IncreaseSpeed(value);

            if (value > 0)
            {
                var buffManager = character.CharacterUse().GetComponent<BuffManager>();
                buffManager.AddBuff(itemType, remainingTurn, value);
            }
            Debug.Log($"{character.CharacterUse().name}: IncreaseSpeed: {value}");
        }
    }
    public class Revive : IItem
    {
        public ItemType ItemType => ItemType.Revive;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {
            var player = character.CharacterUse().GetComponent<PlayerCombatStateMachine>();


            player.CallUseReviveItemEvent();

            if (value > 0)
            {
                var buffManager = character.CharacterUse().GetComponent<BuffManager>();
                buffManager.AddBuff(itemType, remainingTurn, value);
            }
            Debug.Log($"{character.CharacterUse().name}: Revive: {value}");
        }
    }
    public class RemoveDisruption : IItem
    {
        public ItemType ItemType => ItemType.RemoveDisruption;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {

            Debug.Log($"{character.CharacterUse().name}: RemoveDisruption: {value}");
        }
    }
    public class SummonTurret : IItem
    {
        public ItemType ItemType => ItemType.SummonTurret;

        public void Use(ICaster character, ItemType itemType, int remainingTurn, float value)
        {
            if (value > 0)
            {
                var buffManager = character.CharacterUse().GetComponent<BuffManager>();
                buffManager.AddBuff(itemType, remainingTurn, value);
            }
            Debug.Log($"{character.CharacterUse().name}: SummonTurret: {value}");
        }
    }
}