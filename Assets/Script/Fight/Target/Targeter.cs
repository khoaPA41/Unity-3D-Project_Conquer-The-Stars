using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public List<Target> targetAvaiable = new();

    public Target currentTarget;

    public int currentIndex = 0;

    public void SetupTargetList(Target target)
    {
        targetAvaiable.Add(target);
    }

    public void FirstSelected()
    {
        currentTarget = targetAvaiable[0];
    }

    public void RemoveTarget()
    {
        Debug.Log("Reset target");
        targetAvaiable.RemoveAll(target => !target.gameObject.activeInHierarchy);
    }

    public void ChooseNextTarget()
    {
        currentIndex++;
        currentIndex = Mathf.Clamp(currentIndex, 0, targetAvaiable.Count - 1);
        GetTarget();
    }

    public void ChoosePrevTarget()
    {
        currentIndex--;
        currentIndex = Mathf.Clamp(currentIndex, 0, targetAvaiable.Count - 1);
        GetTarget();
    }

    public void GetTarget()
    {
        Debug.Log("GET TARGET");
        currentTarget = targetAvaiable[currentIndex];
    }
}
