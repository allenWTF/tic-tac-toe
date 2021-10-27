using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [RequireComponent(typeof(CanvasGroupToggle))]
    public class Loader : Singleton<Loader>
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private TMP_Text percentage;
        [SerializeField] private Image progressBar;

        private CanvasGroupToggle cgToggle;

        private void Awake()
        {
            cgToggle = GetComponent<CanvasGroupToggle>();
        }

        /// <summary>
        /// Show loader on screen
        /// </summary>
        /// <param name="text">Loader initial label</param>
        /// <param name="progress">Loader initial progress</param>
        public void Show(string text, float progress = 0)
        {
            EditLabel(text);
            EditProgress(progress);
            cgToggle.Show();
        }

        /// <summary>
        /// Edit loader label
        /// </summary>
        /// <param name="labelText">New label</param>
        public void EditLabel(string labelText)
        {
            label.text = labelText;
        }

        /// <summary>
        /// Edit loader current progress
        /// </summary>
        /// <param name="progress">New progress</param>
        public void EditProgress(float progress)
        {
            percentage.text = $"{progress * 100:0}%";
            progressBar.fillAmount = progress;
        }

        public void Close()
        {
            cgToggle.Hide();
        }
    }
}