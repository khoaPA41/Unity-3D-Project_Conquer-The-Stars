using ConquerTheStars.Fight;
using ConquerTheStars.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ConquerTheStars.Managers
{
    public enum ReasonLoadScene
    {
        New,
        Reload,
        Exit,
        ReloadCombat,
        BackToMain
    }

    public class GameManager : MonoBehaviour
    {
        private readonly string StartMenuScene = "Start";
        private readonly string MainScene = "Main";
        private readonly string CommbatScene = "Battle";
        private readonly string EndScene = "End";
        public static GameManager Instance { get; private set; }

        private ReasonLoadScene currentLoadReason = ReasonLoadScene.New;
        private Vector3 checkpointPos;

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

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;


        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == StartMenuScene) return;
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            switch (currentLoadReason)
            {
                case ReasonLoadScene.New:
                    checkpointPos = player.transform.position;
                    break;

                case ReasonLoadScene.Reload:
                    ApplySaveData(player);
                    break;

                case ReasonLoadScene.Exit:
                    break;

                case ReasonLoadScene.ReloadCombat:
                    break;
                case ReasonLoadScene.BackToMain:
                    ApplySaveData(player);
                    break;
                default:
                    break;
            }
        }


        // ----- New Game / Continue / Exit -----

        public void StartNewGame()
        {
            SaveManagers.Instance.CreateNewSaveData();
            currentLoadReason = ReasonLoadScene.New;

            SceneManager.LoadScene(MainScene);
        }

        public void ContinueGame()
        {
            var saveData = SaveManagers.Instance.LoadSaveData();
            if (saveData == null)
            {
                Debug.LogWarning("[SaveManagers] Don't have save data]");
                StartNewGame();
                return;
            }

            currentLoadReason = ReasonLoadScene.Reload;
            checkpointPos = new Vector3(saveData.xPosition, saveData.yPosition, saveData.zPosition);

            SceneManager.LoadScene(MainScene);
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
        }

        public void ExitToTitle()
        {
            currentLoadReason = ReasonLoadScene.Exit;
            SceneManager.LoadScene(StartMenuScene);
        }

        public void LoadBattleScene()
        {
            Debug.Log("Load combat scene");
            currentLoadReason = ReasonLoadScene.ReloadCombat;
            SceneManager.LoadScene(CommbatScene);
        }

        public void BackToMainScene()
        {
            Debug.Log("Load Main scene");
            currentLoadReason = ReasonLoadScene.BackToMain;
            SceneManager.LoadScene(MainScene);
        }

        public void LoadEndScene()
        {
            Debug.Log("Load Main scene");
            currentLoadReason = ReasonLoadScene.Exit;
            SceneManager.LoadScene(EndScene);
        }

        // Save

        public void SetCheckpoint(Vector3 checkpointPosition)
        {
            checkpointPos = checkpointPosition;
        }

        private void ApplySaveData(GameObject player)
        {
            var saveData = SaveManagers.Instance.CurrentSaveData;
            if (saveData is null) return;

            player.transform.position = new Vector3(saveData.xPosition, saveData.yPosition, saveData.zPosition);

            // Stats
            PlayerTeam.Instance.TeamLevel = saveData.teamLevel;

        }

        public void AutoSaveGame()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (player == null) return;

            // Stats

            var saveData = new SaveData
            {
                sceneName = MainScene,
                xPosition = checkpointPos.x,
                yPosition = checkpointPos.y,
                zPosition = checkpointPos.z,

                teamLevel = PlayerTeam.Instance.TeamLevel
            };
            SaveManagers.Instance.SaveGame(saveData);
        }
    }
}