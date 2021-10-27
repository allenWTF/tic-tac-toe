using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Stores data about loadings from scene to scene such as duration and loader label
    /// </summary>
    [CreateAssetMenu(menuName = "Scenes loading meta")]
    public class LoadingMeta : ScriptableObject
    {
        [SerializeField] private List<SceneLoadingMeta> data = new List<SceneLoadingMeta>();

        public SceneLoadingMeta GetSceneLoadingMeta(int fromId, int toId)
        {
            return data.Find(d => d.fromId == fromId && d.toId == toId);
        }
    }

    [Serializable]
    public struct SceneLoadingMeta
    {
        public int fromId;
        public int toId;
        public string label;
        public float duration;
    }
}