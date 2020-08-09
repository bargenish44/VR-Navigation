using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VrGaze : MonoBehaviour
{
    public Image imgGaze;
    public float totalTime = 2;
    private bool gvrStatus ;
    private float gvrTimer;
    public string current = "";
    public string last = "";
    public float azimuth = 0f;
    private SphereChanger sphereChanger;
    private GameObject wantedsphere;
    private string lastsphere;
    private 
    void Start()
    {
        sphereChanger = GameObject.Find("SphereChanger").GetComponent<SphereChanger>();
    }

    void Update()
    {
        if (gvrStatus)
        {
            gvrTimer += Time.deltaTime;
            imgGaze.fillAmount = gvrTimer / totalTime;
        }
        if(imgGaze.fillAmount >= 1)moveSphere(); // 1 is full amount => move point
    }

    public void GVRon(string _current , float _azimuth,string lastsphere)
    {
        gvrStatus = true;
        current = _current;
        azimuth = _azimuth;
        this.lastsphere = lastsphere;
    }
    public void GVROff()
    {
        gvrStatus = false;
        gvrTimer = 0;
        imgGaze.fillAmount = 0;
    }
    public void moveSphere()
    {
        last = current;
        wantedsphere = GameObject.Find("Sphere" + current);
        sphereChanger.ChangeSphere(wantedsphere.transform, azimuth,lastsphere);
    }
}