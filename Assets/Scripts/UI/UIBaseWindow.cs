using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    public enum WindowState
    {
        Opened,
        Closed
    }

    public class UIBaseWindow : MonoBehaviour
    {
        [SerializeField]
        protected string id;
        [SerializeField]
        protected WindowState windowState;

        public virtual string Id
        {
            get { return id; }
        }

        public WindowState WindowState
        {
            get { return windowState; }
            set
            {
                if(windowState != value)
                {
                    windowState = value;
                    OnWindowStateChanged?.Invoke(windowState);
                }
            }
        }

        public event Action<WindowState> OnWindowStateChanged;

        public virtual void Init()
        {

        }

        public virtual void OpenWindow()
        {
            gameObject.SetActive(true);
            WindowState = WindowState.Opened;
        }

        public virtual void CloseWindow()
        {
            gameObject.SetActive(false);
            WindowState = WindowState.Closed;
        }

    }
}