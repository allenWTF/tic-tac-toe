using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace UI.Window.Entities
{
    [CreateAssetMenu(menuName = "UI/Window/Entities/Input")]
    public class Input : WindowEntity, IValueEntity<string>
    {
        private TMP_InputField inputComponent;
        private StringUnityEvent onValueChanged = new StringUnityEvent();
        
        public string Value
        {
            get
            {
                if (!(inputComponent is null))
                {
                    return inputComponent.text;
                }

                if (PlayerPrefs.HasKey(playerPrefsKey))
                {
                    return PlayerPrefs.GetString(playerPrefsKey);
                }

                throw new NullReferenceException(
                    "Input entity wasn't yet generated and doesn't have corresponding player pref key");
            }
        }

        public UnityEvent<string> OnValueChanged => onValueChanged;
        
        public override GameObject Generate()
        {
            var inputObject = Instantiate(prefab);
            inputComponent = inputObject.GetComponentInChildren<TMP_InputField>();
            var labelComponent = inputObject.GetComponentInChildren<TMP_Text>();

            labelComponent.text = label;

            if (playerPrefsKey != string.Empty)
            {
                inputComponent.text = PlayerPrefsHelper.GetPrefOrDefault(playerPrefsKey, PlayerData.PlayerData.DEFAULT_PLAYER_NAME);
                inputComponent.onSubmit.AddListener((value) =>
                {
                    PlayerPrefs.SetString(playerPrefsKey, value);
                    PlayerPrefs.Save();
                });
            }
            
            inputComponent.onValueChanged.AddListener(onValueChanged.Invoke);

            return inputObject;
        }
    }
}