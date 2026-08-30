using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Targeter : MonoBehaviour
{
    public List<Target> targetAvaiable = new();

    public Target currentTarget;

    public int currentIndex = 0;

    public CinemachineTargetGroup cinemachineTargetGroup { get; private set; }

    public void SetupTargetCamera(CinemachineTargetGroup cinemachineTargetGroup)
    {
        this.cinemachineTargetGroup = cinemachineTargetGroup;
    }

    public void SetupTargetList(Target target)
    {
        targetAvaiable.Add(target);
    }

    public void FirstSelected()
    {
        currentTarget = targetAvaiable[currentIndex];
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
        if (currentTarget != null) cinemachineTargetGroup.RemoveMember(currentTarget.transform);

        currentTarget = targetAvaiable[currentIndex];
        if (cinemachineTargetGroup != null) cinemachineTargetGroup.AddMember(currentTarget.transform, 1f, 2f);
    }

    public void RemoveTarget(Target target)
    {
        targetAvaiable.Remove(target);
        if (cinemachineTargetGroup != null) cinemachineTargetGroup.RemoveMember(target.transform);
        currentIndex = 0;
    }

    public void RemoveTarget()
    {
        if (currentTarget != null)
        {
            cinemachineTargetGroup.RemoveMember(currentTarget.transform);
            currentIndex = 0;
        }
    }

    public void RemoveTargetCamera()
    {
        if (currentTarget != null)
        {
            cinemachineTargetGroup.RemoveMember(currentTarget.transform);
        }
    }

    public void ClearTarget()
    {
        targetAvaiable.Clear();
    }
}

