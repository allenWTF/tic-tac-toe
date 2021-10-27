using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(SceneLoader))]
    public class ApplicationStartHelper : MonoBehaviour
    {
        private SceneLoader sceneLoader;
        private void Start()
        {
            sceneLoader = GetComponent<SceneLoader>();
            sceneLoader.NexScene();
        }
    }
}
