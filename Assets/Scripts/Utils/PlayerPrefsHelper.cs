using UnityEngine;

namespace Utils
{
    public static class PlayerPrefsHelper
    {
        public static string GetPrefOrDefault(string key, string defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }
        
            PlayerPrefs.SetString(key, defaultValue);
            PlayerPrefs.Save();
            return defaultValue;
        }
        
        public static int GetPrefOrDefault(string key, int defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
        
            PlayerPrefs.SetInt(key, defaultValue);
            PlayerPrefs.Save();
            return defaultValue;
        }
        
        public static float GetPrefOrDefault(string key, float defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetFloat(key);
            }
        
            PlayerPrefs.SetFloat(key, defaultValue);
            PlayerPrefs.Save();
            return defaultValue;
        }
    }
}
