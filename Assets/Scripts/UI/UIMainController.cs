using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    public class UIMainController : MonoSingleton<UIMainController>
    {
        private UIBaseWindowsStorage storage;
        public UIBaseWindowsStorage Storage
        {
            get
            {
                if (!storage)
                {
                    storage = ResourceManager.GetResource<UIBaseWindowsStorage>(GameConstants.PATH_WINDOWS_STORAGE);
                }
                return storage;
            }
        }

        [SerializeField]
        private RectTransform windowsRoot;
        private Dictionary<string, UIBaseWindow> windows = new Dictionary<string, UIBaseWindow>();

        public event Action<UIBaseWindow> OnWindowAdded;
        public event Action<string> OnWindowRemoved;
        public event Action OnWindowsLoaded;

        public override void Init()
        {
            base.Init();
            for(int i = 0; i < Storage.Windows.Count; i++)
            {
                var window = Storage.Windows[i];
                if (!windows.ContainsKey(window.Id))
                {
                    var clone = Instantiate(window, windowsRoot);
                    windows.Add(clone.Id, clone);
                    clone.Init();
                    clone.gameObject.SetActive(clone.WindowState == WindowState.Opened);
                }
            }
            OnWindowsLoaded?.Invoke();
        }

        public UIBaseWindow GetWindow(string id)
        {
            return windows.ContainsKey(id)? windows[id] : null;
        }

        public T GetWindow<T>(string id) where T: UIBaseWindow
        {
            return windows.ContainsKey(id) ? windows[id] as T: null;
        }

        public void AddWindow(UIBaseWindow window)
        {
            if (!window)
                return;

            if (!windows.ContainsKey(window.Id))
            {
                var clone = Instantiate(window, windowsRoot);
                windows[window.Id] = clone;
                OnWindowAdded?.Invoke(clone);
                clone.Init();
            }
        }

        public void RemoveWindow(UIBaseWindow window)
        {
            if (!window)
                return;

            if (windows.ContainsKey(window.Id))
            {
                windows.Remove(window.Id);
                OnWindowRemoved?.Invoke(window.Id);
                Destroy(window.gameObject);
            }
        }
    }
}