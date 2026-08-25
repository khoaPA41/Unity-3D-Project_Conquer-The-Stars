using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

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
        currentTarget = targetAvaiable[0];
    }

    public void RemoveTarget()
    {
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

        cinemachineTargetGroup.RemoveMember(currentTarget.transform);

        currentTarget = targetAvaiable[currentIndex];
        if (cinemachineTargetGroup != null)
            cinemachineTargetGroup.AddMember(currentTarget.transform, 1f, 2f);
    }
}
