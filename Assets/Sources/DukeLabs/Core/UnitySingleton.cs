using UnityEngine;

namespace DukeLabs.Core
{
    public class UnitySingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (!IsInitialized)
                {
                    _instance = FindObjectOfType<T>() ?? new GameObject(typeof(T).FullName).AddComponent<T>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        public static bool IsInitialized
        {
            get { return (_instance != null); }
        }

        void Awake()
        {
            if (_instance != null && this != _instance)
            {
                Destroy(this.gameObject);
                return;
            }
            Initialize();
        }

        protected virtual void Initialize(){}
    }
}