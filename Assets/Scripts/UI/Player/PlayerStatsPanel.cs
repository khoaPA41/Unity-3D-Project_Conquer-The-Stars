using ConquerTheStars.Factory.Item;
using ConquerTheStars.Fight;
using ConquerTheStars.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConquerTheStars.UI.Player
{
    public class PlayerStatsPanel : MonoBehaviour
    {
        [Header("Team Stats")]
        [SerializeField] private TMP_Text _teamLevelText;
        [SerializeField] private TMP_Text _expText;
        [SerializeField] private Image _expImage;

        [Header("Player Stats")]
        [SerializeField] TextMeshProUGUI _health_Text;
        [SerializeField] TextMeshProUGUI _mana_Text;
        [SerializeField] TextMeshProUGUI _damage_Text;
        [SerializeField] TextMeshProUGUI _speed_Text;
        [SerializeField] TextMeshProUGUI _defense_Text;
        [SerializeField] TextMeshProUGUI _critical_Text;
        [SerializeField] TextMeshProUGUI _luck_Text;

        [Header("Swap Animation")]
        [SerializeField] private Animator _animator;
        private readonly int _triggerSwapAnimation = Animator.StringToHash("Swap");
        private int _selectedIndex;

        private void Start()
        {
            Refresh();
        }

        private void OnEnable()
        {
            Refresh();
        }

        public void SwapPlayer()
        {
            var team = PlayerTeam.Instance;

            if (team == null) return;

            var nextIndex = _selectedIndex + 1;
            if (nextIndex >= team.PlayerSlotList.Count)
                nextIndex = 0;


            _selectedIndex = nextIndex;

            PlayerAnimation();
            Refresh();
        }

        private void Refresh()
        {
            var team = PlayerTeam.Instance;
            if (team == null || _selectedIndex < 0 || _selectedIndex >= team.PlayerSlotList.Count) return;

            _teamLevelText.SetText(team.TeamLevel.ToString());

            // Stats
            var player = team.PlayerSlotList[_selectedIndex];
            var statsData = player.Stats;

            if (statsData == null) return;

            _health_Text.SetText(PredictiveCalculation(statsData.Health, ItemAttachType.Health).ToString());
            _mana_Text.SetText(PredictiveCalculation(statsData.Mana).ToString());
            _damage_Text.SetText(PredictiveCalculation(statsData.AttackPower, ItemAttachType.Damage).ToString());
            _speed_Text.SetText(PredictiveCalculation(statsData.Speed, ItemAttachType.Speed).ToString());
            _defense_Text.SetText(PredictiveCalculation(statsData.Defense, ItemAttachType.Defense).ToString());
            _critical_Text.SetText(PredictiveCalculation(statsData.Critical).ToString());
            _luck_Text.SetText(PredictiveCalculation(statsData.Luck).ToString());

            float PredictiveCalculation(float baseData, ItemAttachType? itemAttachType = null)
            {
                var stat = new StatsManagers(baseData, team.TeamLevel);

                if (itemAttachType.HasValue)
                    stat.SetEquipItemValue(
                        player.GetEquipmentBonus(itemAttachType.Value)
                    );

                return stat.GetFinalValue();
            }

            // Level
            var currentExp = team.Exp;
            var expNeeded = team.CurrentNeededExp;
            _expImage.fillAmount = (float)currentExp / expNeeded;
            _expText.SetText($"{currentExp}/{expNeeded}");
        }

        private void PlayerAnimation()
        {
            _animator.SetTrigger(_triggerSwapAnimation);
        }
    }
}