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
    private string gameover = "GameOver";
    //This ensures that we don't mash to change spheres
    private string lastSphere;
    private TextManager textsEditor;



    private void Start()
    {
        tripod = GameObject.Find("Tripod");
        textsEditor = GameObject.Find("TextEditor").GetComponent<TextManager>();
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

    //public void ChangeSphere(Transform nextSphere)
    public void ChangeSphere(Transform nextSphere, float angle, string last)
    {
        if (first)
        {
            first = false;
            currentSphere = nextSphere.name;
            Stats.Path.Add(nextSphere.gameObject.name);
            lastSphere = last;
            textsEditor = GameObject.Find("TextEditor").GetComponent<TextManager>();
        }
        else
        {
            Stats.Times.Add(Stats.timer);
            Stats.timer = 0;
            Stats.Path.Add(nextSphere.gameObject.name);
            string MSG = "CHANGE - THE PATH IS : ";
            for (int i = 0; i < Stats.Path.Count - 1; i++)
            {
                MSG += Stats.Path[i] + "THE Time IS : " + Stats.Times[i] + " , ";
            }
            MSG += Stats.Path[Stats.Path.Count - 1];
            Debug.Log(MSG);
        }
        Vector3 v = transform.rotation.eulerAngles;
        float newang = angle - 180;
        //tripod.transform.rotation = Quaternion.Euler(0, newang, 0);
        //tripod.transform.SetParent(nextSphere.transform);

        //tripod.transform.parent.rotation = Quaternion.Euler(0, newang, 0);
        StartCoroutine(FadeCamera(nextSphere, newang));
        //Stats.Path.Add(nextSphere.gameObject.name);
        //Stats.Times.Add(Stats.timer);

        //tripod.transform.LookAt(hotspot.transform);
        //tripod.transform.rotation.y += 180;
        //Debug.Log(tripod.transform.rotation.eulerAngles.ToString());
        //tripod.transform.Rotate(170, 0, 0);
        //tripod.transform.localRotation = Quaternion.Euler(v.x, v.y, v.z);
        if (nextSphere.name.Substring(6).Equals(lastSphere))
        {
            Stats.CreateCsvFile();
            StartCoroutine(DoneCoroutine());
        }
    }

    IEnumerator DoneCoroutine()
    {
        yield return new WaitForSeconds(2);
        tripod.GetComponent<SceneCtrl>().ChangeScene(gameover);
    }

    IEnumerator FadeCamera(Transform nextSphere, float newAng)
    {

        //Ensure we have a fader object
        if (m_Fader != null)
        {
            //Fade the Quad object in and wait 0.75 seconds
            StartCoroutine(FadeIn(0.75f, m_Fader.GetComponent<Renderer>().material));
            yield return new WaitForSeconds(0.75f);

            //Change the camera position
            Camera.main.transform.position = nextSphere.position;
            tripod.transform.rotation = Quaternion.Euler(0, newAng, 0);
            textsEditor.ChangePic(nextSphere.name);

            //Fade the Quad object out 
            StartCoroutine(FadeOut(0.75f, m_Fader.GetComponent<Renderer>().material));
            yield return new WaitForSeconds(0.75f);
        }
        else
        {
            //No fader, so just swap the camera position
            //tripod.transform.position = nextSphere.position;
            //tripod.transform.position = nextSphere.position;
            //Camera.main.transform.parent.rotation = Quaternion.Euler(0, newAng, 0);
            //newAng = 180;
            Camera.main.transform.parent.position = nextSphere.position;
            Camera.main.transform.parent.rotation = nextSphere.rotation;
            Camera.main.transform.parent.rotation = Quaternion.Euler(0, newAng, 0);
            Camera.main.transform.parent.localRotation = Quaternion.Euler(0, newAng, 0);
            tripod.transform.rotation = Quaternion.Euler(0, newAng, 0);
            tripod.transform.localRotation = Quaternion.Euler(0, newAng, 0);
            textsEditor.ChangePic(nextSphere.name);
            //Debug.LogError("The angle is : " + newAng);
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