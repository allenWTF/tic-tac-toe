using UnityEngine;

namespace UI.Window
{
    /// <summary>
    /// Auto generated UI window
    /// </summary>
    [RequireComponent(typeof(CanvasGroupToggle))]
    [RequireComponent(typeof(RectTransform))]
    public class Window : MonoBehaviour
    {
        [SerializeField] private WindowDescription windowDescription;

        private CanvasGroupToggle cgToggle;
        private RectTransform window;

        private void Awake()
        {
            cgToggle = GetComponent<CanvasGroupToggle>();
            window = GetComponent<RectTransform>();
            
            Generate();
        }

        /// <summary>
        /// Generate window based on window description
        /// </summary>
        private void Generate()
        {
            var entities = windowDescription.GetEntities();
            foreach (var e in entities)
            {
                e.Generate().transform.SetParent(window, false);
            }
        }
    }
}