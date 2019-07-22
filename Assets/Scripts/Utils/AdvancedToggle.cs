using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdvancedToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private TMP_Text text;

    public Toggle Toggle
    {
        get { return toggle; }
    }

    public TMP_Text Text
    {
        get { return text; }
    }

}
