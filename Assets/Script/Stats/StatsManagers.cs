using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatsManagers
{
    [SerializeField] private float baseValue;

    private readonly List<float> buffList = new();

    public StatsManagers(float baseValue)
    {
        this.baseValue = baseValue;
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
