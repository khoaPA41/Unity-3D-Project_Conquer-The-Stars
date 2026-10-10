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
        private readonly string _startMenuScene = "Start";
        private readonly string _mainScene = "Main";
        private readonly string _combatScene = "Battle";
        private readonly string _endScene = "End";
        private readonly string _loadScene = "Loading";

        public static GameManager Instance { get; private set; }


        private ReasonLoadScene _currentLoadReason = ReasonLoadScene.New;
        private Vector3 _checkpointPos;

        public string PendingSceneName { get; private set; }
        public bool IsLoading { get; private set; }

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
            if (IsLoading && scene.name == PendingSceneName)
            {
                IsLoadingFinish();
            }

            if (scene.name == _loadScene) return;
            if (scene.name == _startMenuScene) return;
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


        // Loading

        private void LoadThroughLoading(string targetScene)
        {
            PendingSceneName = targetScene;
            IsLoading = true;

            SceneManager.LoadScene(_loadScene);
        }

        public void IsLoadingFinish()
        {
            PendingSceneName = null;
            IsLoading = false;
        }

        // ----- New Game / Continue / Exit -----

        public void StartNewGame()
        {
            if (IsLoading) return;

            SaveManagers.Instance.CreateNewSaveData();
            _currentLoadReason = ReasonLoadScene.New;

            // SceneManager.LoadScene(_mainScene);
            LoadThroughLoading(_mainScene);

        }

        public void ContinueGame()
        {
            if (IsLoading) return;

            var saveData = SaveManagers.Instance.LoadSaveData();
            if (saveData == null)
            {
                Debug.LogWarning("[SaveManagers] Don't have save data");
                StartNewGame();
                return;
            }

            _currentLoadReason = ReasonLoadScene.Reload;
            _checkpointPos = new Vector3(saveData.XPosition, saveData.YPosition, saveData.ZPosition);

            // SceneManager.LoadScene(_mainScene);
            LoadThroughLoading(_mainScene);
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
            if (IsLoading) return;
            _currentLoadReason = ReasonLoadScene.Exit;
            // SceneManager.LoadScene(_startMenuScene);
            LoadThroughLoading(_startMenuScene);
        }

        public void LoadBattleScene()
        {
            if (IsLoading) return;
            _currentLoadReason = ReasonLoadScene.ReloadCombat;
            // SceneManager.LoadScene(_combatScene);
            LoadThroughLoading(_combatScene);
        }

        public void BackToMainScene()
        {
            if (IsLoading) return;

            _currentLoadReason = ReasonLoadScene.BackToMain;
            // SceneManager.LoadScene(_mainScene);
            LoadThroughLoading(_mainScene);
        }

        public void LoadEndScene()
        {
            if (IsLoading) return;
            _currentLoadReason = ReasonLoadScene.Exit;
            // SceneManager.LoadScene(_endScene);
            LoadThroughLoading(_endScene);
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
            playerTeam.SetupItemAttachQuantity(saveData.ItemAttach);
            playerTeam.SetupPlayerSlot(saveData.PlayerSlots);
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
                SceneName = _mainScene,
                XPosition = _checkpointPos.x,
                YPosition = _checkpointPos.y,
                ZPosition = _checkpointPos.z,

                TeamLevel = playerTeam.TeamLevel,
                Exp = playerTeam.Exp,
                CurrentNeededExp = PlayerTeam.Instance.CurrentNeededExp,

                ItemInfo = playerTeam.SaveItemInfor(),
                PlayerSlots = playerTeam.SavePlayerSlot(),
                ItemAttach = playerTeam.SaveItemAttach(),
                BattleCompletedIds = new List<string>(BattleInformationManagers.Instance.BattleCompleted)
            };
            SaveManagers.Instance.SaveGame(saveData);
        }
    }
}