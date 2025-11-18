using System;
using UnityEngine;

namespace Helpers
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public static T I
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
                return instance;
            }
        }
    }
}