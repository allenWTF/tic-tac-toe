using DG.Tweening;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Advanced controller of canvas group
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupToggle : MonoBehaviour
    {
        [Header("Behaviour")] public bool visibleOnAwake;
        public bool disableOnHide = true;
        public bool setAsLastSiblingOnShow;

        [Header("Animation")] public bool animateFade;
        public float duration;
        public AnimationCurve fadeCurve;

        private CanvasGroup _group;

        private CanvasGroup Group
        {
            get
            {
                if (_group is null)
                {
                    _group = GetComponent<CanvasGroup>();
                }

                return _group;
            }
        }

        private bool visible;

        private void Awake()
        {
            if (visibleOnAwake)
            {
                Show(true);
            }
            else
            {
                Hide(true);
            }
        }

        /// <summary>
        /// Toggle canvas group
        /// </summary>
        public void Toggle()
        {
            if (visible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        /// <summary>
        /// Show canvas group
        /// </summary>
        /// <param name="immediately">If true - no fade in animation will be played</param>
        public void Show(bool immediately = false)
        {
            if (disableOnHide)
            {
                gameObject.SetActive(true);
                gameObject.SetActive(true);
            }

            if (animateFade && !immediately)
            {
                Fade(true);
            }
            else
            {
                Group.alpha = 1;
            }

            if (setAsLastSiblingOnShow)
            {
                transform.SetAsLastSibling();
            }

            Group.blocksRaycasts = true;
            visible = true;
        }

        /// <summary>
        /// Hide canvas group
        /// </summary>
        /// <param name="immediately">If true - no fade out animation will be played</param>
        public void Hide(bool immediately = false)
        {
            if (animateFade && !immediately)
            {
                Fade(false);
            }
            else
            {
                Group.alpha = 0;
                if (disableOnHide)
                {
                    gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
            }

            Group.blocksRaycasts = false;
            visible = false;
        }

        /// <summary>
        /// Fade animation
        /// </summary>
        /// <param name="fadeIn">If true - animation will be for fade in. Otherwise - for fade out</param>
        private void Fade(bool fadeIn)
        {
            if (fadeIn)
            {
                Group.DOFade(1.0f, duration).SetEase(fadeCurve).SetUpdate(true);
            }
            else
            {
                Group.DOFade(0.0f, duration).SetEase(fadeCurve).OnComplete(() =>
                {
                    if (disableOnHide)
                    {
                        gameObject.SetActive(false);
                        gameObject.SetActive(false);
                    }
                }).SetUpdate(true);
            }
        }
    }
}