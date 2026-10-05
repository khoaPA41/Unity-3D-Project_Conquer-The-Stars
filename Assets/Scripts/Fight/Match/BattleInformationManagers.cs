using System;
using System.Collections.Generic;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;

// Setup: PooledObject ID = Battle_Infor
namespace ConquerTheStars.Fight.Match
{
    [Serializable]
    public class BattleInformation
    {
        public string BattleId;
        public Transform BattleInforTransformList;
        public EnemyTeam EnemyTeam;
    }

    [Serializable]
    public class BattleSpawn
    {
        public string BattleId;
        public Vector3 SpawnPosition;
        public EnemyTeam EnemyTeam;
    }

    public class BattleInformationManagers : MonoBehaviour
    {
        [SerializeField] List<BattleInformation> BattleInforList;
        public static BattleInformationManagers Instance;
        public BattleSpawn CurrentBattleInformation { get; set; }
        public PooledObject CurrentEnemyInfoObject { get; private set; }
        public PlayerTeam PlayerTeam { get; set; }
        public List<string> BattleCompleted = new();
        private List<BattleSpawn> _battleSpawn = new();
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
            SetupBattleSpawn();
            Setup();
        }

        private void Setup()
        {
            foreach (var battle in _battleSpawn)
            {
                // if (SaveManagers.Instance.CurrentSaveData.BattleCompletedIds.Contains(battle.BattleId)) continue;
                if (BattleCompleted.Contains(battle.BattleId)) continue;

                Debug.Log("a");
                var enemy = ObjectPoolingManagers.Instance.GetPooledObject(PooledObjectId.Battle_Infor, battle.SpawnPosition);
                var enemyInfor = enemy.GetComponent<EnemyInformation>();

                enemyInfor.SetEnemyTeam(battle);
            }
        }

        private void SetupBattleSpawn()
        {
            _battleSpawn = new();
            foreach (var battle in BattleInforList)
            {
                _battleSpawn.Add(new BattleSpawn
                {
                    BattleId = battle.BattleId,
                    SpawnPosition = battle.BattleInforTransformList.position,
                    EnemyTeam = battle.EnemyTeam
                });
            }
        }

        private void Rebuild()
        {
            foreach (var battle in _battleSpawn)
            {
                if (!BattleCompleted.Contains(battle.BattleId)) continue;
                Debug.Log("a");
                var enemy = ObjectPoolingManagers.Instance.GetPooledObject(PooledObjectId.Battle_Infor, battle.SpawnPosition);
                var enemyInfor = enemy.GetComponent<EnemyInformation>();

                enemyInfor.SetEnemyTeam(battle);
            }
        }

        public void SetArea(BattleSpawn enemyTeam, PooledObject pooledObject)
        {
            CurrentBattleInformation = enemyTeam;
            CurrentEnemyInfoObject = pooledObject;
        }

        public void Win()
        {
            CurrentEnemyInfoObject.Release();
            BattleCompleted.Add(CurrentBattleInformation.BattleId);
        }

        public void RefreshProgress()
        {
            Rebuild();
            BattleCompleted.Clear();
            CurrentBattleInformation = null;
            CurrentEnemyInfoObject = null;
        }
    }
}