using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public interface IUIItem<T>
    {
        T Source { get; set; }
    }
}