using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModalWindow
{
    [Serializable]
    public enum ButtonLocation
    {
        Default,
        Left,
        Right
    }

    [Serializable]
    public enum ModalWindowButtonStyle
    {
        Default,
        Error,
        Cancel
    }


    [RequireComponent(typeof(Button))]
    public class ModalWindowButton : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Button button;

        [SerializeField] private TMP_Text buttonText;

        [SerializeField] private Image buttonImage;

        [SerializeField] private Image buttonIcon;

        [Header("Images")] [SerializeField] private Sprite defaultSprite;

        [SerializeField] private Sprite horizontalLeftSprite;

        [SerializeField] private Sprite horizontalRightSprite;

        private Action action;

        public void Init(ModalWindowAction buttonAction, ModalWindow window, ButtonLocation location = 0)
        {
            // Rest text to prevent style caching 
            buttonText.text = string.Empty;

            switch (buttonAction.style)
            {
                case ModalWindowButtonStyle.Default:
                    buttonText.text = buttonAction.description;
                    break;
                case ModalWindowButtonStyle.Error:
                    buttonText.text = string.Concat("<style=ModalError>", buttonAction.description, "</style>");
                    break;
                case ModalWindowButtonStyle.Cancel:
                    buttonText.text = string.Concat("<style=ModalCancel>", buttonAction.description, "</style>");
                    break;
            }

            if (buttonAction.icon != null)
            {
                buttonIcon.gameObject.SetActive(true);
                buttonIcon.sprite = buttonAction.icon;

                var rt = buttonText.gameObject.GetComponent<RectTransform>();
                rt.offsetMin = new Vector2(64, rt.offsetMin.y);
                rt.offsetMax = new Vector2(-64, rt.offsetMax.y);
            }
            else
            {
                buttonIcon.gameObject.SetActive(false);
            }

            action = buttonAction.action;
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(() => window.Close());
            button.onClick.AddListener(Execute);

            switch (location)
            {
                case ButtonLocation.Default:
                    buttonImage.sprite = defaultSprite;
                    break;
                case ButtonLocation.Left:
                    buttonImage.sprite = horizontalLeftSprite;
                    break;
                case ButtonLocation.Right:
                    buttonImage.sprite = horizontalRightSprite;
                    break;
            }

            Show();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Execute()
        {
            action?.Invoke();
        }
    }
}