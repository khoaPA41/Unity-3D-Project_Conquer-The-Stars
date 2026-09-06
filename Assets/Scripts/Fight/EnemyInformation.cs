using ConquerTheStars.Fight.Match;
using ConquerTheStars.Managers;
using ConquerTheStars.Pattern.Object_Pooling;
using UnityEngine;


[RequireComponent(typeof(PooledObject))]
public class EnemyInformation : MonoBehaviour
{
    public EnemyTeam enemyTeam;
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
            Debug.Log("Touch");
            BattleInformationManagers.Instance.SetArea(enemyTeam);

            Debug.Log(BattleInformationManagers.Instance.AreaInformation);
            // PlayerTeam.Instance.SavePos(new Vector3(transform.position.x, 0f, transform.position.z));
            if (BattleInformationManagers.Instance.AreaInformation != null)
            {
                pooledObject.Release();

                GameManager.Instance.SetCheckpoint(transform.position); // Set Checkpoint
                Debug.Log(transform.position);
                GameManager.Instance.AutoSaveGame();
                // SceneManager.LoadScene("Battle");
                GameManager.Instance.LoadBattleScene();
            }
        }
    }
}
