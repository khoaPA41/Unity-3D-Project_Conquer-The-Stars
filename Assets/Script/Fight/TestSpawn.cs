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
            if (BattleInformationManagers.Instance.AreaInformation != null)
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene("Battle");
            }
        }
    }
}
