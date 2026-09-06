using System;
using System.Collections;
using System.Collections.Generic;
using ConquerTheStars.Managers;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ConquerTheStars.UI.Player
{
    [RequireComponent(typeof(Canvas))]
    public class UIManagers : MonoBehaviour
    {
        private readonly string mainSceneName = "Main";
        private readonly string battleSceneName = "Battle";

        private readonly string killElementName = "KillElement";
        public static UIManagers Instance;

        [Header("Skill UI")]

        [field: Header("Skill Frame Action")]
        [field: SerializeField] public SkillActionFrame SkillActionFrame { get; set; }

        [field: Header("Status Panel")]
        [field: SerializeField] public RectTransform StatusPanel { get; private set; }

        [SerializeField] private float timeToFill;

        [Header("Result")]
        [SerializeField] private GameObject resultBoard;
        [SerializeField] private GameObject victoryText;
        [SerializeField] private GameObject defeatText;
        [SerializeField] private GameObject revengeButton;

        [SerializeField] private TextMeshProUGUI highestDamageText;
        [SerializeField] private TextMeshProUGUI damageDealtText;
        [SerializeField] private TextMeshProUGUI damageReceivedText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI successfulPariesText;
        [SerializeField] private TextMeshProUGUI successfulDodgesText;
        [SerializeField] private RectTransform Kills;

        [Header("Turn Order")]
        [SerializeField] private GameObject turnOrderBoard;
        public GameObject turnOrderRootParent;
        public List<TurnOrderElement> turnOrders = new();
        public TurnOrderElement curentTurnOrder;
        private List<PooledObject> uiPooledObject { get; set; } = new();

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
            uiPooledObject.Add(ui);
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
        public void ActiveResultBoard(bool isVictory)
        {
            if (isVictory)
            {
                victoryText.SetActive(true);
            }
            else
            {
                defeatText.SetActive(true);
                revengeButton.SetActive(true);
            }
            resultBoard.SetActive(true);
        }

        public void SetResultText(string highestDamageText, string damageDealtText, string damageReceivedText,
        string timeText, string successfulPariesText, string successfulDodgesText)
        {
            this.highestDamageText.SetText(highestDamageText);
            this.damageDealtText.SetText(damageDealtText);
            this.damageReceivedText.SetText(damageReceivedText);
            this.timeText.SetText(timeText);
            this.successfulPariesText.SetText(successfulPariesText);
            this.successfulDodgesText.SetText(successfulDodgesText);
        }

        public void SpawnKillElement(Sprite enemyIcon)
        {
            var killElement = ObjectPoolingManagers.Instance.GetPooledObject(killElementName, Vector3.zero);
            AddUiPooledObjectList(killElement);
            killElement.GetComponent<RectTransform>().SetParent(Kills);
            killElement.GetComponent<EnemyKillElement>().SetIcon(enemyIcon);
        }

        public void ReturnMainScene()
        {
            GameManager.Instance.BackToMainScene();
            // SceneManager.LoadScene(mainSceneName);
        }

        public void ReloadBattle()
        {
            SceneManager.LoadScene(battleSceneName);
        }

        // Release All UI Pooled
        public void ReleaseAllUiPooledObject()
        {
            foreach (var ui in uiPooledObject)
            {
                ui.Release();
            }
        }

        public void SetTurnOrder(List<CharacterStatsManagers> characters)
        {
            foreach (var character in characters)
            {
                var turnOrderObject = ObjectPoolingManagers.Instance.GetPooledObject("TurnOrder", Vector3.zero);
                var turnOrderElement = turnOrderObject.GetComponent<TurnOrderElement>();
                turnOrderElement.InactiveHighlight();

                turnOrderRootParent = turnOrderObject.gameObject.transform.parent.gameObject;
                turnOrderObject.transform.SetParent(turnOrderBoard.transform);

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
                character.transform.SetParent(turnOrderRootParent.transform);
                character.GetComponent<PooledObject>().Release();
            }
            turnOrders.Clear();
        }
    }
}