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
    //private GameObject wantedHotspot;
    private 
    // Start is called before the first frame update
    void Start()
    {
        sphereChanger = GameObject.Find("SphereChanger").GetComponent<SphereChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gvrStatus)
        {
            gvrTimer += Time.deltaTime;
            imgGaze.fillAmount = gvrTimer / totalTime;
        }
        if(imgGaze.fillAmount >= 1)moveSphere();
    }

    public void GVRon(string _current , float _azimuth,string lastsphere)
    {
        gvrStatus = true;
        current = _current;
        azimuth = _azimuth;
        this.lastsphere = lastsphere;
        //wantedHotspot = hotspot;
    }
    public void GVROff()
    {
        gvrStatus = false;
        gvrTimer = 0;
        imgGaze.fillAmount = 0;
        //Debug.Log("off$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
    }
    public void moveSphere()
    {
        last = current;
        //Debug.Log("Clicked");
        //Debug.Log("current is : " + current + "\nlast is : " + last + "\azimuth is : " + azimuth);
        wantedsphere = GameObject.Find("Sphere" + current);
        sphereChanger.ChangeSphere(wantedsphere.transform, azimuth,lastsphere);
    }
}
