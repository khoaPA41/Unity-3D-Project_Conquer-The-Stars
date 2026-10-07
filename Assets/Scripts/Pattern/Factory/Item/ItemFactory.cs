namespace ConquerTheStars.Factory.Item
{
    public enum ItemType
    {
        None,
        Healing,
        RecoveryMana,
        IncreaseDefense,
        Revive,
        IncreaseSpeed,
        IncreaseDamage,
        IncreaseCritical,
        // RemoveDisruption,
        // SummonTurret
    }

    public enum ItemAttachType
    {
        None,
        Damage,
        Speed,
        Defense,
        Health
    }

    public static class ItemFactory
    {
        public static IItem CreateItem(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.Healing => new HealingItem(),
                ItemType.RecoveryMana => new RecoveryMana(),
                ItemType.IncreaseDefense => new IncreaseDefense(),
                ItemType.Revive => new Revive(),
                ItemType.IncreaseSpeed => new IncreaseSpeed(),
                ItemType.IncreaseDamage => new IncreaseDamage(),
                ItemType.IncreaseCritical => new IncreaseCritical(),
                _ => null
            };
        }
    }
}


