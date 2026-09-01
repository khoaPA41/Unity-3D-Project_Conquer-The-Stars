using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace ConquerTheStars.Fight.Target
{
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
            currentTarget = targetAvaiable[currentIndex];
        }

        public void RemoveTarget()
        {
            if (currentTarget != null)
            {
                currentIndex = 0;
            }
        }
    }
}