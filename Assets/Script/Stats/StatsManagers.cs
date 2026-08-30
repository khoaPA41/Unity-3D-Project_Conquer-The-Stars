using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatsManagers
{
    [SerializeField] private float baseValue;

    private readonly List<float> buffList = new();

    private readonly int level;
    public StatsManagers(float baseValue, int level)
    {
        this.baseValue = baseValue;
        this.level = level;
        SetupBaseLevel();
    }


    private void SetupBaseLevel()
    {
        var valueBaseLevel = baseValue * (level / 10f);
        AddBuff(valueBaseLevel);
    }

    public float GetFinalValue()
    {
        var finalValue = baseValue;
        foreach (var value in buffList)
        {
            finalValue += value;
        }
        return finalValue;
    }

    public void AddBuff(float buff)
    {
        if (buff != 0f) buffList.Add(buff);
    }

    public void RemoveBuff(float buff)
    {
        if (buff != 0f) buffList.Remove(buff);
    }

    public float AddValue(float value)
    {
        var finalValue = baseValue;
        return finalValue += value;
    }
}
