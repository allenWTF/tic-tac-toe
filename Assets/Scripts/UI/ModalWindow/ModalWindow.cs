using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.ModalWindow
{
    [Serializable]
    public struct ModalWindowAction
    {
        public string description;
        public ModalWindowButtonStyle style;
        public Action action;
        public Sprite icon;

        /// <summary>
        /// Default Action constructor
        /// </summary>
        /// <param name="description">Action Description</param>
        /// <param name="action">On Click Action</param>
        public ModalWindowAction(string description, Action action)
        {
            this.description = description;
            style = ModalWindowButtonStyle.Default;
            this.action = action;
            icon = null;
        }

        /// <summary>
        /// Action constructor with specific style
        /// Available Styles:
        /// - Error
        /// - Cancel
        /// </summary>
        /// <param name="description">Action Description</param>
        /// <param name="style">Visual Style</param>
        /// <param name="action"></param>
        public ModalWindowAction(string description, ModalWindowButtonStyle style, Action action)
        {
            this.description = description;
            this.style = style;
            this.action = action;
            icon = null;
        }

        /// <summary>
        /// Action constructor with 24x24 icon
        /// </summary>
        /// <param name="description">Action Description</param>
        /// <param name="action">On Click Action</param>
        /// <param name="icon">On Click Action</param>
        public ModalWindowAction(string description, Action action, Sprite icon)
        {
            this.description = description;
            style = ModalWindowButtonStyle.Default;
            this.action = action;
            this.icon = icon;
        }

        /// <summary>
        /// Action constructor with 24x24 icon and specific style
        /// </summary>
        /// <param name="description">Action Description</param>
        /// <param name="style">Visual Style</param>
        /// <param name="action">On Click Action</param>
        /// <param name="icon">On Click Action</param>
        public ModalWindowAction(string description, ModalWindowButtonStyle style, Action action, Sprite icon)
        {
            this.description = description;
            this.style = style;
            this.action = action;
            this.icon = icon;
        }
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class ModalWindow : Singleton<ModalWindow>
    {
        [SerializeField] private GameObject window;

        [SerializeField] private TMP_Text windowHeader;

        [SerializeField] private TMP_Text windowMessage;

        [SerializeField] private TMP_InputField windowInput;

        [Space] [SerializeField] private GridLayoutGroup buttonsGrid;

        [SerializeField] private GameObject buttonPrefab;

        private CanvasGroupToggle groupToggle;
        private List<ModalWindowButton> buttons = new List<ModalWindowButton>();

        private void Awake()
        {
            groupToggle = GetComponent<CanvasGroupToggle>();
        }

        private void Start()
        {
            _ = instance;
            CloseImmediately();
        }

        /// <summary>
        /// Function to show modal window
        /// </summary>
        /// <param name="header">Title of your modal window.</param>
        /// <param name="message">Description of your modal window.</param>
        /// <param name="actions">Array of actions for modal window to show.</param>
        /// <param name="isHorizontal">"Whether your modal window will be displayed horizontally or not. 
        /// If you have more than two actions or more than one action with withCancel=true, then it will be still displayed vertically.</param>
        /// <param name="withCancel">Add cancel button to your modal window. Position will be determined automatically.</param>
        /// <param name="cancelAction">Action to invoke when cancel button is pressed</param>
        /// /// <param name="inputText">Default text to show in the input field</param>
        /// <param name="withInput">Presence of the input field in the window</param>
        /// <param name="contentType">Content type of input</param>
        public void Show(string header, string message, ModalWindowAction[] actions = null, bool isHorizontal = false,
            bool withCancel = true, Action cancelAction = null,
            bool withInput = false, string inputText = "",
            TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard)
        {
            Close();
            Open();

            //Configuring Text of the Window
            windowHeader.text = header;
            windowMessage.text = message;

            //Configuring Input Field
            if (withInput)
            {
                windowInput.transform.parent.gameObject.SetActive(true);
                windowInput.text = inputText;
                windowInput.contentType = contentType;
            }
            else
            {
                windowInput.transform.parent.gameObject.SetActive(false);
            }

            if (actions == null)
            {
                actions = new ModalWindowAction[0];
            }

            var actionsList = actions.ToList();

            if (withCancel)
            {
                var cancel = new ModalWindowAction
                {
                    description = "Отмена", style = ModalWindowButtonStyle.Cancel, action = cancelAction
                };

                actionsList.Add(cancel);
            }

            AllocateButtons(actionsList.Count);

            if (isHorizontal && actionsList.Count == 2)
            {
                buttonsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                buttonsGrid.constraintCount = 2;
                buttonsGrid.cellSize = new Vector2(buttonsGrid.GetComponent<RectTransform>().sizeDelta.x / 2,
                    buttonsGrid.cellSize.y);

                buttons[0].Init(actionsList[0], this, ButtonLocation.Left);
                buttons[1].Init(actionsList[1], this, ButtonLocation.Right);
            }
            else
            {
                buttonsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                buttonsGrid.constraintCount = 1;
                buttonsGrid.cellSize =
                    new Vector2(buttonsGrid.GetComponent<RectTransform>().sizeDelta.x, buttonsGrid.cellSize.y);

                for (int i = 0; i < actionsList.Count; i++)
                {
                    buttons[i].Init(actionsList[i], this);
                }
            }
        }

        public string GetInput()
        {
            return windowInput.text;
        }

        /// <summary>
        /// Show current modal window without animation
        /// </summary>
        public void ShowImmediately()
        {
            ShowWindow();
        }

        public void Close()
        {
            HideWindow();
        }

        public void Open()
        {
            ShowWindow();
        }

        private void ShowWindow()
        {
            transform.SetAsLastSibling();

            //Animation
            window.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            groupToggle.Show();
            window.transform.DOScale(1f, groupToggle.duration).SetEase(Ease.InBounce).SetUpdate(true);
        }

        private void HideWindow()
        {
            groupToggle.Hide();
        }

        private void CloseImmediately()
        {
            HideWindow();
        }

        /// <summary>
        /// Create ModalWindowButtons for all provided actions
        /// </summary>
        /// <param name="count"></param>
        private void AllocateButtons(int count)
        {
            var currentButtonsCount = buttons.Count;

            if (count > currentButtonsCount)
            {
                for (var i = 0; i < count - currentButtonsCount; i++)
                {
                    buttons.Add(Instantiate(buttonPrefab, buttonsGrid.transform).GetComponent<ModalWindowButton>());
                }
            }
            else if (count < currentButtonsCount)
            {
                for (var i = count; i < currentButtonsCount; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}