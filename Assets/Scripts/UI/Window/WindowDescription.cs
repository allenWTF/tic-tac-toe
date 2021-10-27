using System.Collections.Generic;
using UnityEngine;

namespace UI.Window
{
    /// <summary>
    /// Description of all entities that should be presented in auto generated window
    /// </summary>
    [CreateAssetMenu(menuName = "UI/Window/Description")]
    public class WindowDescription : ScriptableObject
    {
        [SerializeField]
        private List<WindowEntity> entities = new List<WindowEntity>();

        public List<WindowEntity> GetEntities() => entities;
    }
}