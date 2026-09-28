using NUnit.Framework;
using UnityEngine;

using ConquerTheStars.Fight.Enemy;
using ConquerTheStars.Stats;

using System.Collections.Generic;

public class EnemyEvaluationTests
{
    private GameObject _gameObject;
    private EnemyEvaluation _enemyEvaluation;
    private StrategyEvaluation _strategyEvaluation;

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
        target.maxHealth = new StatsManagers(maxHealth, 1);
        target.attack = new StatsManagers(attack, 1);
        target.speed = new StatsManagers(speed, 1);
        target.defense = new StatsManagers(defense, 1);
        target.critical = new StatsManagers(critical, 1);
        target.mana = new StatsManagers(mana, 1);
        target.luck = new StatsManagers(luck, 1);

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
        _gameObject = new GameObject("EnemyEvaluationTest");
        _enemyEvaluation = _gameObject.AddComponent<EnemyEvaluation>();

        _strategyEvaluation = ScriptableObject.CreateInstance<StrategyEvaluation>();
        _strategyEvaluation.hpWeight = 0.8f;
        _strategyEvaluation.threatWeight = 0.3f;
        _strategyEvaluation.defenseWeight = 0.3f;

        _enemyEvaluation.Initialize(_strategyEvaluation);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_gameObject);
        Object.DestroyImmediate(_strategyEvaluation);
    }

    [Test]
    public void GetBestTarget_EmptyList_ReturnsNull()
    {
        var result = _enemyEvaluation.GetBestTarget(new());

        Assert.IsNull(result);
    }

    [Test]
    public void GetBestTarget_AllTargetsDead_ReturnsNull()
    {
        var targetObject = new GameObject("DeadTarget");
        var target = targetObject.AddComponent<CharacterStatsManagers>();

        target.SetDead(true);

        var targets = new List<CharacterStatsManagers>
        {
            target
        };

        var result = _enemyEvaluation.GetBestTarget(targets);

        Assert.IsNull(result);

        Object.DestroyImmediate(targetObject);
    }

    [Test]
    public void Evaluate_NullList_ReturnsNull()
    {
        var result = _enemyEvaluation.Evaluate(null);

        Assert.IsNull(result);
    }

    [Test]
    public void Evaluate_EmptyList_ReturnsNull()
    {
        var result = _enemyEvaluation.Evaluate(new());

        Assert.IsNull(result);
    }

    [Test]
    public void Evaluate_AllTargetsDead_ReturnsEmptyDictionary()
    {
        var targetObject = new GameObject("DeadTarget");
        var target = targetObject.AddComponent<CharacterStatsManagers>();

        target.SetDead(true);

        var targets = new List<CharacterStatsManagers>
        {
            target
        };

        var result = _enemyEvaluation.Evaluate(targets);

        Assert.IsNotNull(result);
        Assert.IsEmpty(result);

        Object.DestroyImmediate(targetObject);
    }

    [Test]
    public void EvaluateDetailed_NullList_ReturnsEmptyList()
    {
        var result = _enemyEvaluation.EvaluateDetailed(null);

        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [Test]
    public void EvaluateDetailed_EmptyList_ReturnsEmptyList()
    {
        var result = _enemyEvaluation.EvaluateDetailed(new());

        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [Test]
    public void EvaluateDetailed_AllTargetsDead_ReturnsEmptyList()
    {
        var targetObject = new GameObject("DeadTarget");
        var target = targetObject.AddComponent<CharacterStatsManagers>();

        target.SetDead(true);

        var targets = new List<CharacterStatsManagers>
        {
            target
        };

        var result = _enemyEvaluation.EvaluateDetailed(targets);

        Assert.IsNotNull(result);
        Assert.IsEmpty(result);

        Object.DestroyImmediate(targetObject);
    }

    [Test]
    public void Evaluate_AllTargets_ReturnsCorrectScores()
    {
        var targetObject1 = new GameObject("Player_I");
        var targetObject2 = new GameObject("Player_II");
        var targetObject3 = new GameObject("Player_III");

        var target1 = targetObject1.AddComponent<CharacterStatsManagers>();
        var target2 = targetObject2.AddComponent<CharacterStatsManagers>();
        var target3 = targetObject3.AddComponent<CharacterStatsManagers>();

        SetupStatsManagers(
            target1,
            100f, // Max HP
            100f, // Current HP
            100f, // Attack
            10f,  // Speed
            10f,  // Defense
            1.5f,   // Critical
            100f, // Mana
            0.1f    // Luck
        );

        SetupStatsManagers(
            target2,
            100f,
            90f,
            90f,
            12f,
            10f,
            1.5f,
            100f,
            0.1f
        );

        SetupStatsManagers(
            target3,
            100f,
            100f,
            80f,
            12,
            10,
            1.5f,
            100,
            .1f
        );

        var targets = new List<CharacterStatsManagers>
                {
            target1,
            target2,
            target3
                };

        // Act
        var result = _enemyEvaluation.Evaluate(targets);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);

        Assert.Greater(result[target2], result[target1]);
        Assert.Greater(result[target2], result[target3]);
        Assert.AreEqual(result[target1], result[target3], 0.001f);

        Object.DestroyImmediate(targetObject1);
        Object.DestroyImmediate(targetObject2);
        Object.DestroyImmediate(targetObject3);
    }

    [Test]
    public void GetBestTarget_HighestScoreTarget_ReturnsTarget()
    {
        // Arrange
        var targetObject1 = new GameObject("Player_I");
        var targetObject2 = new GameObject("Player_II");
        var targetObject3 = new GameObject("Player_III");

        var target1 = targetObject1.AddComponent<CharacterStatsManagers>();
        var target2 = targetObject2.AddComponent<CharacterStatsManagers>();
        var target3 = targetObject3.AddComponent<CharacterStatsManagers>();

        SetupStatsManagers(
            target1,
            100f, // Max HP
            100f, // Current HP
            100f, // Attack
            10f,  // Speed
            10f,  // Defense
            1.5f, // Critical
            100f, // Mana
            0.1f  // Luck
        );

        SetupStatsManagers(
            target2,
            100f, // Max HP
            90f,  // Current HP
            90f,  // Attack
            12f,  // Speed
            10f,  // Defense
            1.5f, // Critical
            100f, // Mana
            0.1f  // Luck
        );

        SetupStatsManagers(
            target3,
            100f,
            100f,
            80f,
            12f,
            10f,
            1.5f,
            100f,
            0.1f
        );

        var targets = new List<CharacterStatsManagers>
    {
        target1,
        target2,
        target3
    };

        // Act
        var result = _enemyEvaluation.GetBestTarget(targets);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreSame(target2, result);

        // Cleanup
        Object.DestroyImmediate(targetObject1);
        Object.DestroyImmediate(targetObject2);
        Object.DestroyImmediate(targetObject3);
    }
}