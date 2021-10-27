using UnityEngine;

namespace Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<T>();
                    if (instances.Length != 0)
                    {
                        if (instances.Length == 1)
                        {
                            _instance = instances[0];
                            // _instance.Init();
                        }
                        else
                        {
                            Debug.Log($"More than one instance of {typeof(T)} found!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No instance of {typeof(T)} found!");
                    }
                }

                return _instance;
            }
        }
    }
}