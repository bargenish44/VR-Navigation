using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereChanger : MonoBehaviour
{


    //This object should be called 'Fader' and placed over the camera
    GameObject m_Fader;

    GameObject tripod;

    private bool first = true;
    private string currentSphere = "";
    //This ensures that we don't mash to change spheres
    //bool changing = false;



    private void Start()
    {
        tripod = GameObject.Find("Tripod");
    }

    private void Update()
    {
        Stats.timer += Time.deltaTime;
    }
    void Awake()
    {

        //Find the fader object
        m_Fader = null;
        //m_Fader = GameObject.Find("Fader");

        //Check if we found something
        if (m_Fader == null)
            Debug.LogWarning("No Fader object found on camera.");
        if (tripod == null) tripod = GameObject.Find("Tripod");
    }


    public void ChangeSphere(Transform nextSphere,float angle)
    {
        if(first)
        {
            first = false;
            currentSphere = nextSphere.name;
            Stats.Path.Add(nextSphere.gameObject.name);
        }
        else
        {
            Stats.Times.Add(Stats.timer);
            Stats.timer = 0;
            Stats.Path.Add(nextSphere.gameObject.name);
            string MSG = "CHANGE - THE PATH IS : ";
            for (int i = 0; i < Stats.Path.Count-1; i++)
            {
                MSG += Stats.Path[i] + "THE Time IS : " + Stats.Times[i] + " , ";
            }
            MSG += Stats.Path[Stats.Path.Count - 1];
            Debug.LogError(MSG);
        }
        StartCoroutine(FadeCamera(nextSphere));
        //Stats.Path.Add(nextSphere.gameObject.name);
        //Stats.Times.Add(Stats.timer);

        Vector3 v = transform.rotation.eulerAngles;
        float newang = angle  - 180;
        //while (newang > 360) newang -= 360;
        //while (newang < 0) newang += 360;
        //Debug.Log("the new angle is : " + newang);
        tripod.transform.rotation = Quaternion.Euler(0, newang, 0);
        //tripod.transform.SetParent(nextSphere.transform);
        
        //tripod.transform.rotation = Quaternion.Euler(0, newang, 0);
        //float hotspotX;
        //float hotspotY;
        //float hotspotZ;
        //tripod.transform.LookAt(hotspot.transform);
        //tripod.transform.rotation.y += 180;
        //Debug.Log(tripod.transform.rotation.eulerAngles.ToString());
        //tripod.transform.Rotate(170, 0, 0);
        //tripod.transform.localRotation = Quaternion.Euler(v.x, v.y, v.z);
    }

    IEnumerator FadeCamera(Transform nextSphere)
    {

        //Ensure we have a fader object
        if (m_Fader != null)
        {
            //Fade the Quad object in and wait 0.75 seconds
            StartCoroutine(FadeIn(0.75f, m_Fader.GetComponent<Renderer>().material));
            yield return new WaitForSeconds(0.75f);

            //Change the camera position
            Camera.main.transform.parent.position = nextSphere.position;

            //Fade the Quad object out 
            StartCoroutine(FadeOut(0.75f, m_Fader.GetComponent<Renderer>().material));
            yield return new WaitForSeconds(0.75f);
        }
        else
        {
            //No fader, so just swap the camera position
            //Camera.main.transform.parent.position = nextSphere.position;
            //tripod.transform.position = nextSphere.position;
            tripod.transform.position = nextSphere.position;
        }
    }


        IEnumerator FadeOut(float time, Material mat)
        {
            //While we are still visible, remove some of the alpha colour
            while (mat.color.a > 0.0f)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - (Time.deltaTime / time));
                yield return null;
            }
        }


        IEnumerator FadeIn(float time, Material mat)
        {
            //While we aren't fully visible, add some of the alpha colour
            while (mat.color.a < 1.0f)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a + (Time.deltaTime / time));
                yield return null;
            }
        }


    }
