using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseResources<T> : ScriptableObject where T:Object
{
    [SerializeField]
    protected List<T> resources = new List<T>();
    public List<T> Resources
    {
        get { return resources; }
    }
}
