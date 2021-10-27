using System;
using TMPro;
using UnityEngine;

namespace PlayerData
{
    /// <summary>
    /// UI visualizer of PlayerData class
    /// </summary>
    public class PlayerDataVisualizer : MonoBehaviour
    {
        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text lastWinner;
        [SerializeField] private TMP_Text lastDuration;
        [SerializeField] private TMP_Text lastDescription;

        private void Awake()
        {
            var playerData = new PlayerData();

            SetTextField(score, playerData.Score.ToString());
            SetTextField(lastWinner, playerData.LastMatch.winner);
            var timespan = TimeSpan.FromMilliseconds(playerData.LastMatch.duration);
            SetTextField(lastDuration, timespan.ToString(@"mm\:ss"));
            SetTextField(lastDescription, playerData.LastMatch.description);
        }

        private void SetTextField(TMP_Text textField, string value)
        {
            if (textField != null)
            {
                textField.text = value;
            }
        }
    }
}