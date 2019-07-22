using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{
    private static Dictionary<string, Object> cached = new Dictionary<string, Object>();


    public static T GetResource<T>(string path) where T: Object
    {

        if (cached.ContainsKey(path))
        {
            return cached[path] as T;
        }
        else
        {
            var resource = Resources.Load<T>(path);
            cached[path] = resource;
            return resource;
        }
    }
}
