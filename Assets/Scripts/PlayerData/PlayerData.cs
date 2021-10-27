using UnityEngine;
using Utils;

namespace PlayerData
{
    public class PlayerData
    {
        public const string DEFAULT_PLAYER_NAME = "Skytec";

        public string Name
        {
            get => PlayerPrefsHelper.GetPrefOrDefault("name", DEFAULT_PLAYER_NAME);
            set
            {
                PlayerPrefs.SetString("name", value);
                PlayerPrefs.Save();
            }
        }

        public int Score
        {
            get => PlayerPrefsHelper.GetPrefOrDefault("score", 0);
            set
            {
                PlayerPrefs.SetInt("score", value);
                PlayerPrefs.Save();
            }
        }

        public MatchData LastMatch
        {
            get
            {
                var lastMatch = PlayerPrefsHelper.GetPrefOrDefault("lastMatch", "");
                return lastMatch == string.Empty ? new MatchData() : JsonUtility.FromJson<MatchData>(lastMatch);
            }
            set
            {
                PlayerPrefs.SetString("lastMatch", JsonUtility.ToJson(value));
                PlayerPrefs.Save();
            }
        }
    }

    /// <summary>
    /// Data of last played match
    /// </summary>
    public struct MatchData
    {
        public string winner;
        public float duration;
        public int scorePrize;
        public string description;
    }
}