using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace UI.Window.Entities
{
    [CreateAssetMenu(menuName = "UI/Window/Entities/Slider")]
    public class Slider : WindowEntity, IValueEntity<float>
    {
        [SerializeField] protected float minValue;
        [SerializeField] protected float maxValue = 1f;

        private UnityEngine.UI.Slider sliderComponent;
        private FloatUnityEvent onValueChanged = new FloatUnityEvent();

        public float Value
        {
            get
            {
                if (!(sliderComponent is null))
                {
                    return sliderComponent.value;
                }

                if (PlayerPrefs.HasKey(playerPrefsKey))
                {
                    return PlayerPrefs.GetFloat(playerPrefsKey);
                }

                throw new NullReferenceException(
                    "Slider entity wasn't yet generated and doesn't have corresponding player pref key");
            }
        }

        public UnityEvent<float> OnValueChanged => onValueChanged;

        public override GameObject Generate()
        {
            var sliderObject = Instantiate(prefab);
            sliderComponent = sliderObject.GetComponentInChildren<UnityEngine.UI.Slider>();
            var labelComponent = sliderObject.GetComponentInChildren<TMP_Text>();

            labelComponent.text = label;

            sliderComponent.minValue = minValue;
            sliderComponent.maxValue = maxValue;

            if (playerPrefsKey != string.Empty)
            {
                sliderComponent.value = PlayerPrefsHelper.GetPrefOrDefault("volume", 0.5f);
                sliderComponent.onValueChanged.AddListener((value) =>
                {
                    PlayerPrefs.SetFloat("volume", value);
                    PlayerPrefs.Save();
                });
            }
            
            sliderComponent.onValueChanged.AddListener(onValueChanged.Invoke);

            return sliderObject;
        }
    }
}