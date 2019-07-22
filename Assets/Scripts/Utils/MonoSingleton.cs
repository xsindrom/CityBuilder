using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static object lockObject = new object();
    private static bool isApplicationQuiting = false;

    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (isApplicationQuiting)
                return null;

            if (!instance)
            {
                var instances = FindObjectsOfType<T>();
                if (instances.Length > 0)
                {
                    lock (lockObject)
                    {
                        instance = instances[0];
                    }
                    for (int i = 1; i < instances.Length; i++)
                    {
                        Destroy(instances[i].gameObject);
                    }
                }
                if (!instance)
                {
                    lock (lockObject)
                    {
                        var newObject = new GameObject();
                        instance = newObject.AddComponent<T>();
                        newObject.name = typeof(T).Name;
                    }
                }
            }
            return instance;
        }
    }

    public static bool HasInstance
    {
        get { return instance; }
    }

    public static void CreateInstance(bool dontDestroyOnLoad = false)
    {
        var newObject = new GameObject();
        instance = newObject.AddComponent<T>();
        newObject.name = typeof(T).Name;
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(newObject);
        }
    }

    [SerializeField]
    protected bool dontDestroyOnLoad = false;

    protected void Awake()
    {
        if(Instance == this)
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(Instance.gameObject);
            }
            Init();
        }
    }

    protected virtual void OnApplicationQuit()
    {
        isApplicationQuiting = true;
    }

    protected virtual void OnApplicationPause(bool pause)
    {
        
    }

    protected virtual void OnDestroy()
    {
        
    }

    public virtual void Init()
    {

    }
}
