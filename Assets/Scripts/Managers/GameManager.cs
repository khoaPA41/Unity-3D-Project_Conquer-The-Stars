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
            checkpointPos = new Vector3(saveData.xPosition, saveData.yPosition, saveData.zPosition);
            currentLoadReason = ReasonLoadScene.Reload;
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

        // Save

        public void SetCheckpoint(Vector3 checkpointPosition)
        {
            checkpointPos = checkpointPosition;
        }

        private void ApplySaveData(GameObject player)
        {
            var saveData = SaveManagers.Instance.CurrentSaveData;
            // var audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSetting>();
            if (saveData is null) return;

            player.transform.position = new Vector3(saveData.xPosition, saveData.yPosition, saveData.zPosition);

            // Stats
            PlayerTeam.Instance.TeamLevel = saveData.teamLevel;

            // // Audio
            // audio._masterVolume = data.masterVolume;
            // audio._bgmVolume = data.BGMVolume;
            // audio._sfxVolume = data.SFXVolume;
            // audio._uiVolume = data.UIVolume;
        }

        public void AutoSaveGame()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            // var audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSetting>();

            if (player == null) return;

            // Stats
            // var teamLevel = PlayerTeam.Instance.TeamLevel;

            var saveData = new SaveData
            {
                sceneName = SceneManager.GetActiveScene().name,
                xPosition = checkpointPos.x,
                yPosition = checkpointPos.y,
                zPosition = checkpointPos.z,

                teamLevel = PlayerTeam.Instance.TeamLevel

                // masterVolume = audio._masterVolume,
                // BGMVolume = audio._bgmVolume,
                // SFXVolume = audio._sfxVolume,
                // UIVolume = audio._uiVolume,

                // resolutionIndex = GraphicManager.Instance.resolutionIndex,
                // displayModeIndex = GraphicManager.Instance.displayModeIndex,
                // fps = GraphicManager.Instance.fps,
                // vsync = GraphicManager.Instance.vsync,
                // qualityPresentIndex = GraphicManager.Instance.qualityPresentIndex,
                // shadow = GraphicManager.Instance.shadow,
                // antiAliasingIndex = GraphicManager.Instance.antiAliasingIndex,
                // textureQualityIndex = GraphicManager.Instance.textureQualityIndex,
                // bloom = GraphicManager.Instance.bloomData,
                // motionBlur = GraphicManager.Instance.motionBlurData,
                // ambientOcclusion = GraphicManager.Instance.ambientOcclusion,
            };
            SaveManagers.Instance.SaveGame(saveData);
        }
    }
}