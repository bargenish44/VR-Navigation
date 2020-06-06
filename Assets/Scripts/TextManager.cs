﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class TextManager : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] public Dictionary<string, List<Parser.Optionaltext>> texts = new Dictionary<string, List<Parser.Optionaltext>>();
    [SerializeField] private TextMeshProUGUI textMPro;
    private bool TimerOn = false; 
    private float totalTime = 0;
    private Queue<Parser.Optionaltext> currentTexts = new Queue<Parser.Optionaltext>();

    void Start()
    {
        if (Panel == null) Panel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        textMPro = Panel.GetComponentInChildren<TextMeshProUGUI>();
        textMPro.text = "";
    }

    void Update()
    {
        if (TimerOn)
        {
            totalTime += Time.deltaTime;
        }
    }

    public void ChangePic(string name)
    {
        TimerOn = true;
        if(textMPro == null)textMPro = Panel.GetComponentInChildren<TextMeshProUGUI>();
        textMPro.text = "";
        currentTexts = new Queue<Parser.Optionaltext>();
        totalTime = 0;
        StopAllCoroutines();
        CopyTexts(name);
        StartCoroutine(ShowText());
    }

    private void CopyTexts(string name)
    {
        foreach (Parser.Optionaltext ot in texts[name])
        {
            currentTexts.Enqueue(ot);
        }
    }

    private IEnumerator ShowText()
    {
        while (currentTexts.Count > 0 && TimerOn)
        {
            if (totalTime >= currentTexts.Peek().whenToDisplay)
            {
                Parser.Optionaltext temp = currentTexts.Dequeue();
                textMPro.text = temp.text;
                yield return new WaitForSeconds(temp.DurationInSeconds);
                textMPro.text = "";
            }
            else yield return new WaitForSeconds(currentTexts.Peek().whenToDisplay - totalTime);
        }
        TimerOn = false;
        textMPro.text = "";
    }

    public void PrintTextDictionary()
    {
        if (texts.Count > 0)
        {
            foreach (var v in texts) {
                Debug.Log(v.Key);
                foreach (var c2 in v.Value)
                {
                    Debug.Log(c2.ToString());
                }
            }
        }
        else Debug.Log("its empty");
    }
}