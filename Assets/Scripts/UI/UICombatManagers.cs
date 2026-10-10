using System.Collections.Generic;
using ConquerTheStars.Fight;
using ConquerTheStars.Managers;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConquerTheStars.UI.Player
{
    [RequireComponent(typeof(Canvas))]
    public class UICombatManagers : MonoBehaviour
    {
        private readonly PooledObjectId _itemReward = PooledObjectId.ItemRewardElement;
        private readonly PooledObjectId _turnOrder = PooledObjectId.TurnOrder;
        private readonly PooledObjectId _killElement = PooledObjectId.KillElement;

        private readonly string _battleSceneName = "Battle";

        public static UICombatManagers Instance;

        [Header("Skill UI")]

        [field: Header("Skill Frame Action")]
        [field: SerializeField] public SkillActionFrame SkillActionFrame { get; set; }

        [field: Header("Status Panel")]
        [field: SerializeField] public RectTransform StatusPanel { get; private set; }

        [SerializeField] private float timeToFill;

        [Header("Result")]

        [SerializeField] private GameObject _resultBoard;
        [SerializeField] private GameObject _victoryText;
        [SerializeField] private GameObject _defeatText;
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private GameObject _revengeButton;
        [SerializeField] private GameObject _endButton;

        [Header("Battle Statistics")]
        [SerializeField] private TextMeshProUGUI _highestDamageText;
        [SerializeField] private TextMeshProUGUI _damageDealtText;
        [SerializeField] private TextMeshProUGUI _damageReceivedText;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _successfulPariesText;
        [SerializeField] private TextMeshProUGUI _successfulDodgesText;
        [SerializeField] private RectTransform _kills;

        [Header("Reward")]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private RectTransform _itemParent;
        [SerializeField] private RectTransform _itemAttachParent;


        [Header("Turn Order")]
        [SerializeField] private GameObject turnOrderBoard;
        public List<TurnOrderElement> turnOrders = new();
        public TurnOrderElement curentTurnOrder;
        private readonly List<PooledObject> _uiPooledObject = new();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void AddUiPooledObjectList(PooledObject ui)
        {
            _uiPooledObject.Add(ui);
        }

        // Action Frame 
        public void ActiveActionFrame()
        {
            SkillActionFrame.gameObject.SetActive(true);
        }

        public void PausePerfectFrame()
        {
            SkillActionFrame.PauseActionFrame();
            SkillActionFrame.CallPausedAction();
        }

        public float GetActionFrameValue() => SkillActionFrame.ActionFrameValue;

        // Result Board 
        public void ActiveResultBoard(bool isVictory, bool isFinalBoss)
        {
            if (isVictory)
            {
                if (isFinalBoss)
                {
                    _continueButton.SetActive(false);
                    _endButton.SetActive(true);
                }
                _victoryText.SetActive(true);
            }
            else
            {
                _defeatText.SetActive(true);
                _revengeButton.SetActive(true);
            }
            _resultBoard.SetActive(true);
        }

        public void SetResultText(string _highestDamageText, string _damageDealtText, string _damageReceivedText,
        string _timeText, string _successfulPariesText, string _successfulDodgesText)
        {
            this._highestDamageText.SetText(_highestDamageText);
            this._damageDealtText.SetText(_damageDealtText);
            this._damageReceivedText.SetText(_damageReceivedText);
            this._timeText.SetText(_timeText);
            this._successfulPariesText.SetText(_successfulPariesText);
            this._successfulDodgesText.SetText(_successfulDodgesText);
        }

        public void SpawnKillElement(Sprite enemyIcon)
        {
            var killElement = ObjectPoolingManagers.Instance.GetPooledObject(_killElement, Vector3.zero);
            AddUiPooledObjectList(killElement);

            var rect = killElement.GetComponent<RectTransform>();
            rect.SetParent(_kills, false);
            rect.localScale = Vector3.one;
            killElement.GetComponent<EnemyKillElement>().SetIcon(enemyIcon);
        }

        public void ReturnMainScene()
        {
            ReleaseAllUiPooledObject();
            GameManager.Instance.BackToMainScene();
        }

        public void EndScene()
        {
            ReleaseAllUiPooledObject();
            GameManager.Instance.LoadEndScene();
        }

        public void ReloadBattle()
        {
            ReleaseAllUiPooledObject();
            // SceneManager.LoadScene(_battleSceneName);
            // SceneManager.LoadScene();
            GameManager.Instance.LoadBattleScene();
        }

        // Release All UI Pooled
        public void ReleaseAllUiPooledObject()
        {
            foreach (var ui in _uiPooledObject)
            {
                ui.Release();
            }
            _uiPooledObject.Clear();
        }

        public void SetTurnOrder(IReadOnlyList<CharacterStatsManagers> characters)
        {
            foreach (var character in characters)
            {
                var turnOrderObject = ObjectPoolingManagers.Instance.GetPooledObject(_turnOrder, Vector3.zero);
                var turnOrderElement = turnOrderObject.GetComponent<TurnOrderElement>();
                turnOrderElement.InactiveHighlight();

                var rect = turnOrderObject.GetComponent<RectTransform>();
                rect.SetParent(turnOrderBoard.transform, false);
                rect.localScale = Vector3.one;
                rect.localRotation = Quaternion.identity;

                turnOrderElement.SetIcon(character.icon);

                turnOrders.Add(turnOrderElement);
            }
        }

        public void ActiveTurnOrderHighlight()
        {
            curentTurnOrder = turnOrders[0];
            curentTurnOrder.ActiveHighlight();
        }

        public void InactiveTurnOrderHighlight()
        {
            if (curentTurnOrder != null)
            {
                curentTurnOrder.InactiveHighlight();
                turnOrders.Remove(curentTurnOrder);
                turnOrders.Add(curentTurnOrder);
            }
        }

        public void ResetTurnOrder()
        {
            foreach (var character in turnOrders)
            {
                character.GetComponent<PooledObject>().Release();
            }
            turnOrders.Clear();
        }

        // Reward
        public void SpawnItemReward(List<ItemInfoForSave> itemRewardList)
        {
            var team = PlayerTeam.Instance;
            if (team == null || itemRewardList == null) return;

            foreach (var item in itemRewardList)
            {
                if (item == null || item.Quantity <= 0) continue;

                var inventoryItem = team.GetItem(item.ItemType);
                if (inventoryItem == null || inventoryItem.ItemData == null) continue;

                var itemReward = ObjectPoolingManagers.Instance.GetPooledObject(_itemReward, Vector3.zero);
                var rect = itemReward.GetComponent<RectTransform>();
                rect.SetParent(_itemParent, false);
                rect.localScale = Vector3.one;
                rect.rotation = Quaternion.identity;

                var itemElement = itemReward.GetComponent<ItemElementForInventory>();
                itemElement.Icon.sprite = inventoryItem.ItemData.ItemIcon;
                itemElement.Quantity.SetText(item.Quantity.ToString());

                _uiPooledObject.Add(itemReward);
            }
        }

        public void SpawnItemAttachReward(ItemAttachData itemAttach)
        {
            if (itemAttach == null) return;

            var team = PlayerTeam.Instance;
            if (team == null) return;

            var itemReward = ObjectPoolingManagers.Instance.GetPooledObject(_itemReward, Vector3.zero);
            var rect = itemReward.GetComponent<RectTransform>();
            rect.SetParent(_itemAttachParent, false);
            rect.localScale = Vector3.one;
            rect.rotation = Quaternion.identity;

            var itemElement = itemReward.GetComponent<ItemElementForInventory>();
            itemElement.Icon.sprite = itemAttach.Icon;
            itemElement.Quantity.SetText("1");

            _uiPooledObject.Add(itemReward);
        }

        public void CurrentLevel()
        {
            var team = PlayerTeam.Instance;
            if (team == null) return;

            _levelText.SetText(team.TeamLevel.ToString());
        }
    }
}