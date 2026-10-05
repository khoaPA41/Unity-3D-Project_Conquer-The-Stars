using ConquerTheStars.Fight.Match;
using ConquerTheStars.Managers;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class EnemyInformation : MonoBehaviour
{
    // private EnemyTeam _enemyTeam;
    private BattleSpawn _battleInfo;
    private PooledObject _pooledObject;

    private void Start()
    {
        _pooledObject = GetComponent<PooledObject>();
    }

    public void SetEnemyTeam(BattleSpawn infor)
    {
        _battleInfo = infor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BattleInformationManagers.Instance.SetArea(_battleInfo, _pooledObject);
            if (BattleInformationManagers.Instance != null)
            {
                GameManager.Instance.AutoSaveGame();
                GameManager.Instance.LoadBattleScene();
            }
        }
    }
}
