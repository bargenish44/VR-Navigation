﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnMouseDown()
    {
        Debug.Log("Goodbye cruel world!");
        Stats.CreateCsvFile();
        Debug.Log("Done");
    }
}