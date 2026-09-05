using ConquerTheStars.Fight.Match;
using ConquerTheStars.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EnemyTeam))]
public class TestSpawn : MonoBehaviour
{
    private EnemyTeam enemyTeam;

    private void Start()
    {
        enemyTeam = GetComponent<EnemyTeam>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BattleInformationManagers.Instance.SetArea(enemyTeam);
            // PlayerTeam.Instance.SavePos(new Vector3(transform.position.x, 0f, transform.position.z));
            if (BattleInformationManagers.Instance.AreaInformation != null)
            {
                BattleInformationManagers.Instance.SetAreaActive(false);
                BattleInformationManagers.Instance.area.Remove(this.gameObject);

                GameManager.Instance.SetCheckpoint(transform.position); // Set Checkpoint
                GameManager.Instance.AutoSaveGame();
                // SceneManager.LoadScene("Battle");
                GameManager.Instance.LoadBattleScene();
            }
        }
    }
}
