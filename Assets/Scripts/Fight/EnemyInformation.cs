using ConquerTheStars.Fight.Match;
using ConquerTheStars.Managers;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PooledObject))]
public class EnemyInformation : MonoBehaviour
{
    private EnemyTeam enemyTeam;
    private PooledObject pooledObject;

    private void Start()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    public void SetEnemyTeam(EnemyTeam enemyTeam)
    {
        this.enemyTeam = enemyTeam;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Auto Save
            GameManager.Instance.SetCheckpoint(transform.position);
            GameManager.Instance.AutoSaveGame();

            BattleInformationManagers.Instance.SetArea(enemyTeam);

            if (BattleInformationManagers.Instance.AreaInformation != null)
            {
                pooledObject.Release();
                GameManager.Instance.AutoSaveGame();
                GameManager.Instance.LoadBattleScene();
            }
        }
    }
}
