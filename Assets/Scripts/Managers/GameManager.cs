using ConquerTheStars.Fight;
using ConquerTheStars.Fight.Match;
using ConquerTheStars.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using JetBrains.Annotations;

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
        private const int _androidTargetFrameRate = 30;
        private readonly string StartMenuScene = "Start";
        private readonly string MainScene = "Main";
        private readonly string CombatScene = "Battle";
        private readonly string EndScene = "End";
        public static GameManager Instance { get; private set; }


        private ReasonLoadScene _currentLoadReason = ReasonLoadScene.New;
        private Vector3 _checkpointPos;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

#if UNITY_ANDROID && !UNITY_EDITOR
            Application.targetFrameRate = _androidTargetFrameRate;
#endif

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;


        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == StartMenuScene) return;
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            switch (_currentLoadReason)
            {
                case ReasonLoadScene.New:
                    _checkpointPos = player.transform.position;
                    PlayerTeam.Instance.FirstTime();
                    BattleInformationManagers.Instance.RefreshProgress();
                    break;

                case ReasonLoadScene.Reload:
                    BattleInformationManagers.Instance.BattleCompleted = new List<string>(SaveManagers.Instance.CurrentSaveData.BattleCompletedIds);
                    ApplySaveData(player);
                    break;

                case ReasonLoadScene.Exit:
                    break;

                case ReasonLoadScene.ReloadCombat:
                    break;
                case ReasonLoadScene.BackToMain:
                    // ApplySaveData(player);
                    player.transform.position = _checkpointPos;
                    break;
                default:
                    break;
            }
        }

        // ----- New Game / Continue / Exit -----

        public void StartNewGame()
        {
            SaveManagers.Instance.CreateNewSaveData();
            _currentLoadReason = ReasonLoadScene.New;

            SceneManager.LoadScene(MainScene);
        }

        public void ContinueGame()
        {
            var saveData = SaveManagers.Instance.LoadSaveData();
            if (saveData == null)
            {
                Debug.LogWarning("[SaveManagers] Don't have save data");
                StartNewGame();
                return;
            }

            _currentLoadReason = ReasonLoadScene.Reload;
            _checkpointPos = new Vector3(saveData.XPosition, saveData.YPosition, saveData.ZPosition);

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
            _currentLoadReason = ReasonLoadScene.Exit;
            SceneManager.LoadScene(StartMenuScene);
        }

        public void LoadBattleScene()
        {
            _currentLoadReason = ReasonLoadScene.ReloadCombat;
            SceneManager.LoadScene(CombatScene);
        }

        public void BackToMainScene()
        {
            _currentLoadReason = ReasonLoadScene.BackToMain;
            SceneManager.LoadScene(MainScene);
        }

        public void LoadEndScene()
        {
            _currentLoadReason = ReasonLoadScene.Exit;
            SceneManager.LoadScene(EndScene);
        }

        // Save

        public void SetCheckpoint(Vector3 checkpointPosition)
        {
            _checkpointPos = checkpointPosition;
        }

        private void ApplySaveData(GameObject player)
        {
            var saveData = SaveManagers.Instance.CurrentSaveData;
            if (saveData is null) return;

            player.transform.position = new Vector3(saveData.XPosition, saveData.YPosition, saveData.ZPosition);
            PlayerTeam playerTeam = PlayerTeam.Instance;

            // Stats
            playerTeam.TeamLevel = saveData.TeamLevel;
            playerTeam.Exp = saveData.Exp;
            playerTeam.CurrentNeededExp = saveData.CurrentNeededExp;
            playerTeam.SetupItemQuantity(saveData.ItemInfo);
            BattleInformationManagers.Instance.BattleCompleted = new List<string>(saveData.BattleCompletedIds);
        }

        public void AutoSaveGame()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PlayerTeam playerTeam = PlayerTeam.Instance;
            if (player == null) return;

            // Stats

            var saveData = new SaveData
            {
                SceneName = MainScene,
                XPosition = _checkpointPos.x,
                YPosition = _checkpointPos.y,
                ZPosition = _checkpointPos.z,

                TeamLevel = playerTeam.TeamLevel,
                Exp = playerTeam.Exp,
                CurrentNeededExp = PlayerTeam.Instance.CurrentNeededExp,

                ItemInfo = playerTeam.SaveItemInfor(),
                BattleCompletedIds = new List<string>(BattleInformationManagers.Instance.BattleCompleted)
            };
            SaveManagers.Instance.SaveGame(saveData);
        }
    }
}