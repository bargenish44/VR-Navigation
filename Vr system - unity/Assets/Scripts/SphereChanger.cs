using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SphereChanger : MonoBehaviour
{

    GameObject tripod;

    private bool first = true;
    private string currentSphere = "";
    private string gameover = "GameOver";
    private string lastSphere;
    private TextManager textsEditor;
    private GameObject CurSphere;




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
        if (tripod == null) tripod = GameObject.Find("Tripod");
    }

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
            Stats.Times.Add(Mathf.Round(Stats.timer * 100f) / 100f);
            Stats.timer = 0;
            Stats.Path.Add(nextSphere.gameObject.name);
            string MSG = "CHANGE - THE PATH IS : ";
            for (int i = 0; i < Stats.Path.Count - 1; i++)
            {
                MSG += Stats.Path[i] + "THE Time IS : " + Stats.Times[i] + " , ";
            }
            MSG += Stats.Path[Stats.Path.Count - 1];
        }
        float newang = angle;
        Change(nextSphere, newang);
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

    void Change(Transform nextSphere, float newAng)
    {
        textsEditor.ChangePic(nextSphere.name);
        if (CurSphere != null) // First time
            CurSphere.SetActive(false);
        nextSphere.gameObject.SetActive(true);
        CurSphere = nextSphere.gameObject;
        //tripod.transform.Rotate(0, 180, 0); // Anchoring fix.
    }
}