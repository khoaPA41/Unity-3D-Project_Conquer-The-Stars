using UnityEngine;
using NUnit.Framework;
using ConquerTheStars.Stats;
using System.Collections.Generic;
public class TurnOrderServiceTests
{
    private GameObject _gameObject1;
    private GameObject _gameObject2;
    private GameObject _gameObject3;
    private GameObject _gameObject4;

    private CharacterStatsManagers _actor1;
    private CharacterStatsManagers _actor2;
    private CharacterStatsManagers _actor3;
    private CharacterStatsManagers _actor4;

    private TurnOrderService _turnOrderService;

    private void SetupStatsManagers(CharacterStatsManagers target, float speed)
    {
        target.speed = new StatsManagers(speed, 0);
        target.CurrentSpeed = target.speed.GetFinalValue();
    }

    [SetUp]
    public void Setup()
    {
        _turnOrderService = new();
        _gameObject1 = new GameObject("Actor_I");
        _gameObject2 = new GameObject("Actor_II");
        _gameObject3 = new GameObject("Actor_III");
        _gameObject4 = new GameObject("Actor_IV");

        _actor1 = _gameObject1.AddComponent<CharacterStatsManagers>();
        _actor2 = _gameObject2.AddComponent<CharacterStatsManagers>();
        _actor3 = _gameObject3.AddComponent<CharacterStatsManagers>();
        _actor4 = _gameObject4.AddComponent<CharacterStatsManagers>();

        SetupStatsManagers(_actor1, 12);
        SetupStatsManagers(_actor2, 10);
        SetupStatsManagers(_actor3, 10);
        SetupStatsManagers(_actor4, 15);

        var actors = new List<CharacterStatsManagers>{
            _actor1, _actor2, _actor3, _actor4
        };

        _turnOrderService.BuildInitial(actors);
    }

    [Test]
    public void BuilderOrder_SortBySpeedDescending()
    {
        Assert.AreEqual(12, _actor1.CurrentSpeed);
        Assert.AreEqual(10, _actor2.CurrentSpeed);
        Assert.AreEqual(10, _actor3.CurrentSpeed);
        Assert.AreEqual(15, _actor4.CurrentSpeed);

        Assert.AreEqual(15, _turnOrderService.CharacterList[0].CurrentSpeed);
        Assert.AreEqual(12, _turnOrderService.CharacterList[1].CurrentSpeed);
        Assert.AreEqual(10, _turnOrderService.CharacterList[2].CurrentSpeed);
        Assert.AreEqual(10, _turnOrderService.CharacterList[3].CurrentSpeed);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_gameObject1);
        Object.DestroyImmediate(_gameObject2);
        Object.DestroyImmediate(_gameObject3);
        Object.DestroyImmediate(_gameObject4);
    }

    [Test]
    public void TurnOrder_DequeueNextAlive()
    {
        _actor4.SetDead(true);
        var next = _turnOrderService.DequeueNextAlive();

        Assert.AreEqual(2, _turnOrderService.CharacterList.Count);

        Assert.AreEqual(12, next.CurrentSpeed);
    }
}
