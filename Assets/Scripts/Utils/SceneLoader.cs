using System.Collections;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private LoadingMeta scenesLoadingMeta;

        private bool isLoading;

        /// <summary>
        /// Load scene by id
        /// </summary>
        /// <param name="id">Id of scene to be loaded</param>
        public void LoadScene(int id)
        {
            if (isLoading == false)
            {
                isLoading = true;
                var sceneLoadingMeta = scenesLoadingMeta.GetSceneLoadingMeta(ActiveSceneIndex(), id);
                StartCoroutine(EnableLoader(sceneLoadingMeta.label, sceneLoadingMeta.duration, id));
            }
        }

        /// <summary>
        /// Load next scene
        /// </summary>
        [UsedImplicitly]
        public void NexScene()
        {
            var curId = SceneManager.GetActiveScene().buildIndex;
            LoadScene(curId + 1);
        }

        /// <summary>
        /// Load previous scene
        /// </summary>
        [UsedImplicitly]
        public void PrevScene()
        {
            var curId = SceneManager.GetActiveScene().buildIndex;
            if (curId > 0)
            {
                LoadScene(curId - 1);
            }
        }
        
        /// <summary>
        /// Reload current scene
        /// </summary>
        public void ReloadScene()
        {
            int curId = SceneManager.GetActiveScene().buildIndex;
            LoadScene(curId);
        }

        private int ActiveSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// Activate loader before scene load
        /// </summary>
        /// <param name="label">Label of loader</param>
        /// <param name="duration">Loader show duration in milliseconds</param>
        /// <param name="id">Id of scene to load</param>
        /// <returns></returns>
        private IEnumerator EnableLoader(string label, float duration, int id)
        {
            Loader.instance.Show(label);

            const int fractionsCount = 20;
            for (var i = 0; i <= fractionsCount; i++)
            {
                Loader.instance.EditProgress((float) i / fractionsCount);
                yield return new WaitForSeconds(duration / 1000f / fractionsCount);
            }

            SceneManager.LoadSceneAsync(id);
            isLoading = false;
            Loader.instance.Close();
        }
    }
}