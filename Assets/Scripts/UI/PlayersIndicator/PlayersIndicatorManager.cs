using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayersIndicator
{
    /// <summary>
    /// Controls UI of players indicators
    /// </summary>
    public class PlayersIndicatorManager : MonoBehaviour
    {
        [SerializeField] private GameObject crossIndicatorReference;
        [SerializeField] private GameObject circleIndicatorReference;

        private PlayerIndicator crossIndicator;
        private PlayerIndicator circleIndicator;

        public void Init(string crossPlayerName, string circlePlayerName)
        {
            crossIndicator = new PlayerIndicator(crossIndicatorReference.GetComponentInChildren<TMP_Text>(),
                crossIndicatorReference.GetComponentInChildren<Image>());
            circleIndicator = new PlayerIndicator(circleIndicatorReference.GetComponentInChildren<TMP_Text>(),
                circleIndicatorReference.GetComponentInChildren<Image>());

            crossIndicator.SetName(crossPlayerName);
            circleIndicator.SetName(circlePlayerName);
        }

        public void SetActiveIndicator(CellState indicatorTeam)
        {
            switch (indicatorTeam)
            {
                case CellState.Empty:
                    return;
                case CellState.Circle:
                    circleIndicator.ToggleActive(true);
                    crossIndicator.ToggleActive(false);
                    return;
                default:
                    circleIndicator.ToggleActive(false);
                    crossIndicator.ToggleActive(true);
                    break;
            }
        }
    }
}