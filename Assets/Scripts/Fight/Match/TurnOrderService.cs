using System.Collections.Generic;
using System.Linq;
using ConquerTheStars.Stats;
using UnityEngine;

public class TurnOrderService
{
    private readonly List<CharacterStatsManagers> _queue = new();

    public IReadOnlyList<CharacterStatsManagers> CharacterList => _queue;
    public float SpeedAverage { get; private set; }
    public void BuildInitial(IEnumerable<CharacterStatsManagers> actors)
    {
        _queue.Clear();
        _queue.AddRange(actors.OrderByDescending(actor => actor.CurrentSpeed));
        SpeedAverage = _queue.Average(actor => actor.CurrentSpeed);
    }

    public CharacterStatsManagers DequeueNextAlive()
    {
        while (_queue.Count > 0)
        {
            var character = _queue[0];
            _queue.RemoveAt(0);

            if (character != null && !character.IsDeath)
            {
                return character;
            }
        }
        return null;
    }

    public bool StealTurn()
    {
        foreach (var actor in _queue.ToList())
        {
            if (actor.CurrentSpeed <= SpeedAverage) continue;

            var stealTurnRate = Mathf.Clamp(actor.CurrentLuck * (actor.CurrentSpeed - SpeedAverage) / 100f, 0f, .95f);
            if (!IsStealSuccess(stealTurnRate)) continue;

            MoveToHead(actor);

            return true;
        }
        return false;
    }

    private bool IsStealSuccess(float rate)
    {
        return Random.value < rate;
    }

    public void EnqueueBack(CharacterStatsManagers actor)
    {
        _queue.Add(actor);
    }

    public void RemoveDead()
    {
        _queue.RemoveAll(actor => actor.IsDeath);
    }

    private void MoveToHead(CharacterStatsManagers actor)
    {
        _queue.Remove(actor);
        _queue.Insert(0, actor);
    }
}
