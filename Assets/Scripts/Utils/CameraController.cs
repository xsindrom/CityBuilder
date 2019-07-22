using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    [SerializeField]
    private Camera mainCamera;
    public Camera MainCamera
    {
        get { return mainCamera; }
    }
}