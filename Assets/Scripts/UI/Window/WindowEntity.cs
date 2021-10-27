using UnityEngine;
using UnityEngine.Events;

namespace UI.Window
{
    /// <summary>
    /// Single entity of auto-generated window
    /// </summary>
    public abstract class WindowEntity : ScriptableObject
    {
        [SerializeField] protected string label;
        [SerializeField] protected string playerPrefsKey;
        [SerializeField] protected GameObject prefab;

        /// <summary>
        /// Generate entity of window
        /// </summary>
        /// <returns></returns>
        public abstract GameObject Generate();
    }

    
    /// <summary>
    /// Interface for window entities that can represent some kind of value
    /// </summary>
    /// <typeparam name="T">Type of value which entity is representing</typeparam>
    public interface IValueEntity<T>
    {
        T Value { get; }
        UnityEvent<T> OnValueChanged { get; }
    }
}