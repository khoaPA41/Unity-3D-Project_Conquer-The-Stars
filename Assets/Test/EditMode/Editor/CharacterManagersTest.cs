using ConquerTheStars.Stats;
using NUnit.Framework;
using UnityEngine;

public class CharacterManagersTest
{
    private GameObject _gameObject1;
    private GameObject _gameObject2;
    private CharacterStatsManagers _player;
    private CharacterStatsManagers _enemy;

    private void SetupStatsManagers(
        CharacterStatsManagers target,
        float maxHealth,
        float currentHealth,
        float attack,
        float speed,
        float defense,
        float critical,
        float mana,
        float luck)
    {
        target.maxHealth = new StatsManagers(maxHealth, 0);
        target.attack = new StatsManagers(attack, 0);
        target.speed = new StatsManagers(speed, 0);
        target.defense = new StatsManagers(defense, 0);
        target.critical = new StatsManagers(critical, 0);
        target.mana = new StatsManagers(mana, 0);
        target.luck = new StatsManagers(luck, 0);

        target.CurrentHealth = currentHealth;
        target.CurrentMana = target.mana.GetFinalValue();
        target.CurrentAttackDamage = target.attack.GetFinalValue();
        target.CurrentSpeed = target.speed.GetFinalValue();
        target.CurrentDefense = target.defense.GetFinalValue();
        target.CurrentCritical = target.critical.GetFinalValue();
        target.CurrentLuck = target.luck.GetFinalValue();
    }

    [SetUp]
    public void SetUp()
    {
        _gameObject1 = new GameObject("Player");
        _gameObject2 = new GameObject("Enemy");

        _player = _gameObject1.AddComponent<CharacterStatsManagers>();
        _enemy = _gameObject2.AddComponent<CharacterStatsManagers>();

        SetupStatsManagers(
            _player,
            100f, // Max HP
            100f, // Current HP
            90f, // Attack
            10f,  // Speed
            10f,  // Defense
            1.5f,   // Critical
            100f, // Mana
            0.1f    // Luck
        );

        SetupStatsManagers(
            _enemy,
            100f, // Max HP
            100f, // Current HP
            100f, // Attack
            10f,  // Speed
            10f,  // Defense
            1.5f,   // Critical
            100f, // Mana
            0.1f    // Luck
        );
    }

    [Test]
    public void CalculateFinalDamage_DefenseReducesDamage()
    {
        var playerTakeDamage = _player.CalculateFinalDamage(_enemy.CurrentAttackDamage);

        Assert.AreEqual(90f, playerTakeDamage, 0.001f);
    }

    [Test]
    public void CalculateFinalDamage_DefenseHigherThanDamage_ReturnsZero()
    {
        _player.CurrentDefense = 120f;

        var damage = _player.CalculateFinalDamage(_enemy.CurrentAttackDamage);

        Assert.AreEqual(0f, damage, 0.001f);
    }

    [Test]
    public void Healing_IncreasesCurrentHealth()
    {
        _player.CurrentHealth = 60f;

        _player.Healing(20f);

        Assert.AreEqual(80f, _player.CurrentHealth, 0.001f);
    }

    [Test]
    public void Healing_CannotExceedMaxHealth()
    {
        _player.CurrentHealth = 90f;

        _player.Healing(20f);

        Assert.AreEqual(100f, _player.CurrentHealth, 0.001f);
    }

    [Test]
    public void CalculateCriticalDamage_ReturnsCorrectValue()
    {
        _player.CurrentAttackDamage = 100f;
        _player.CurrentLuck = 0.5f;
        _player.CurrentCritical = 1.5f;

        var criticalDamage = _player.CalculateCriticalDamage();

        Assert.AreEqual(125f, criticalDamage, 0.001f);
    }

    [Test]
    public void Healing_MaxHealthZero_DoesNotIncreaseHealth()
    {
        _player.maxHealth = new StatsManagers(0f, 0);
        _player.CurrentHealth = 0f;

        _player.Healing(20f);

        Assert.AreEqual(0f, _player.CurrentHealth, 0.001f);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_gameObject1);
        Object.DestroyImmediate(_gameObject2);
    }
}
