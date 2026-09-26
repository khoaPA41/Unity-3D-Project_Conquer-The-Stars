using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace ConquerTheStars.Fight.Target
{
    public class Targeter : MonoBehaviour
    {
        public List<Target> TargetAvaiable = new();

        public Target CurrentTarget;

        public int CurrentIndex = 0;

        public void SetupTargetList(Target target)
        {
            TargetAvaiable.Add(target);
        }

        public void FirstSelected()
        {
            CurrentTarget = TargetAvaiable[CurrentIndex];
        }

        public void ChooseNextTarget()
        {
            CurrentIndex++;
            CurrentIndex = Mathf.Clamp(CurrentIndex, 0, TargetAvaiable.Count - 1);
            GetTarget();
        }

        public void ChoosePrevTarget()
        {
            CurrentIndex--;
            CurrentIndex = Mathf.Clamp(CurrentIndex, 0, TargetAvaiable.Count - 1);
            GetTarget();
        }

        public void GetTarget()
        {
            CurrentTarget = TargetAvaiable[CurrentIndex];
        }

        public void ResetTarget()
        {
            // targetAvaiable.Remove(currentTarget);
            CurrentIndex = 0;
        }

        public void RemoveTarget()
        {
            TargetAvaiable.Remove(CurrentTarget);
        }
    }
}