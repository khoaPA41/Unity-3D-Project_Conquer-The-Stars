using ConquerTheStars.Fight.Match;
using ConquerTheStars.Managers;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class EnemyInformation : MonoBehaviour
{
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
        var battle = BattleInformationManagers.Instance;
        var gameManager = GameManager.Instance;

        if (battle == null || gameManager == null) return;

        if (gameManager.IsLoading) return;

        if (other.CompareTag("Player"))
        {
            battle.SetArea(_battleInfo, _pooledObject);
            gameManager.AutoSaveGame();
            gameManager.LoadBattleScene();
        }
    }
}
