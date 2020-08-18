using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugMode : MonoBehaviour
{
    public bool DebugOn;
    void Start()
    {
        try
        {
            DebugOn = GameObject.Find("DebugMode").GetComponent<debug>().DebugOn;
        }
        catch (NullReferenceException e) { gameObject.SetActive(false); }
    }

    void Update()
    {
        string debug = "current azimth : " + Camera.main.transform.eulerAngles.y + "\n";
        debug += "Camera rotation : " + Camera.main.transform.rotation;

        if (DebugOn)
            Camera.main.transform.Find("Canvas").Find("Debug").GetComponentInChildren<TextMeshProUGUI>().SetText(debug);
    }
}