using System.Collections.Generic;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;

namespace ConquerTheStars.Fight.Match
{
    public class BattleInformationManagers : MonoBehaviour
    {
        public static BattleInformationManagers Instance;
        public EnemyTeam AreaInformation { get; set; }

        public PlayerTeam PlayerTeam { get; set; }

        public List<Transform> battleInforTransform;

        public List<EnemyTeam> enemyTeams;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

        }

        private void Start()
        {
            Setup();
        }


        private void Setup()
        {
            for (int i = 0; i < battleInforTransform.Count; i++)
            {
                var enemy = ObjectPoolingManagers.Instance.GetPooledObject("Battle_Infor", battleInforTransform[i].position).GetComponent<EnemyInformation>();
                enemy.SetEnemyTeam(enemyTeams[i]);

            }
        }

        public void SetArea(EnemyTeam enemyTeam)
        {
            AreaInformation = enemyTeam;
        }
    }
}