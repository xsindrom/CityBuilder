using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Windows/Storage", order = 0)]
    public class UIBaseWindowsStorage : ScriptableObject
    {
        [SerializeField]
        private List<UIBaseWindow> windows = new List<UIBaseWindow>();
        public List<UIBaseWindow> Windows
        {
            get { return windows; }
        }
    }
}