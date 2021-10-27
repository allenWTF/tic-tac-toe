using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace Gameplay
{
    public enum CellState
    {
        Empty,
        Cross,
        Circle
    }

    /// <summary>
    /// Cell of playing field
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class Cell : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Sprite crossSprite;
        [SerializeField] private Sprite circleSprite;

        private CellState state = CellState.Empty;
        private Image img;

        public CellUnityEvent onCellClick = new CellUnityEvent();

        private void Awake()
        {
            img = GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (state == CellState.Empty)
            {
                onCellClick.Invoke(this);
            }
        }

        public CellState GetState() => state;

        public void SetState(CellState newState)
        {
            state = newState;
            switch (state)
            {
                case CellState.Empty:
                    return;
                case CellState.Circle:
                    img.sprite = circleSprite;
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
                    break;
                case CellState.Cross:
                    img.sprite = crossSprite;
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
                    break;
            }
        }
    }
}