using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VrMenuGaze : MonoBehaviour
{
    public Image imgGaze;
    public UnityEvent GVRClick;
    [SerializeField]private float totalTime = 2;
    private bool gvrStatus = false;
    private float gvrTimer;

    private void Start()
    {
        imgGaze.fillAmount = 0;
    }
    void Update()
    {
        if (gvrStatus)
        {
            gvrTimer += Time.deltaTime;
            imgGaze.fillAmount = gvrTimer / totalTime;
        }
        if (gvrTimer >= totalTime) GVRClick.Invoke();
    }

    public void GVRon()
    {
        gvrStatus = true;
    }
    public void GVROff()
    {
        gvrStatus = false;
        gvrTimer = 0;
        imgGaze.fillAmount = 0;
    }
}

