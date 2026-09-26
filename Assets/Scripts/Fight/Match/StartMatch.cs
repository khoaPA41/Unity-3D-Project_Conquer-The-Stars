using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConquerTheStars.Fight.Match
{
    [Serializable]
    public class EnemyTransformList // This list will constain transform enemy will be spawn
    {
        public List<Transform> EnemyTransforms;
    }

    [Serializable]
    public class PlayerTransformList // This list will constain transform player will be spawn
    {
        public List<Transform> PlayerTransforms;
    }

    public class StartMatch : MonoBehaviour
    {
        [field: Header("Spawn Area")]
        [field: SerializeField]
        public List<EnemyTransformList> EnemyTransformList { get; set; }
        [field: SerializeField] public List<PlayerTransformList> PlayerTransformList { get; set; }
    }
}