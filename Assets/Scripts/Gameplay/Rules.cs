using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Main rules of the game
    /// </summary>
    [CreateAssetMenu(menuName = "Gameplay/Rules")]
    public class Rules : ScriptableObject
    {
        public int FieldDimensions
        {
            get => Mathf.Clamp(fieldDimensions, 3, 15);
            set => fieldDimensions = Mathf.Clamp(value, 3, 15);
        }

        /// <summary>
        /// Size of playing field
        /// </summary>
        [SerializeField] private int fieldDimensions = 3;
    }
}