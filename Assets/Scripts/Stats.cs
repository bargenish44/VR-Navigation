using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class Stats { 
    public static ArrayList Path = new ArrayList();
    public static ArrayList Times = new ArrayList();
    public static float timer = 0;
    public static void CreateCsvFile()
    {
        Times.Add(timer);
        string data=System.DateTime.Now+ " -\n";
        for (int i = 0; i < Path.Count;i++)
        {
            data += Path[i]+" : Time - "+Times[i]+". ";
        }
        try
        {
            string path = Application.persistentDataPath + "/stats.csv";
            if (!File.Exists(path))
            {
                File.WriteAllText(path, data);
            }
            else
            {
                File.AppendAllText(path, "\n" + data);
                Debug.Log("creation : old modified");
            }
            // You can find the created folder in your phone's memory
        }
        catch (Exception e) { Debug.LogError ("csv problem : \n"+e); }
    }
}

