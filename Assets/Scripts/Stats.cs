using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class Stats { 
        //public static string Path { get; set; }
        //public static double times { get; set; }
    public static ArrayList Path = new ArrayList();
    public static ArrayList Times = new ArrayList();
    public static float timer = 0;
    public static void CreateCsvFile()
    {
        string data=System.DateTime.Now+ ":,";
        for (int i = 0; i < Path.Count;i++)
        {
            data += Path[i]+" . Times is : "+Times[i]+",";
        }
        string path = Application.persistentDataPath + "/stats.csv";
        Debug.Log(path);
        Debug.Log(data);
        try
        {
        //    saved2();
            if (!File.Exists(path))
            {
                File.WriteAllText(path, data);
                Debug.Log("new created");
            }
            else
            {
                File.AppendAllText(path, "\n" + data);
                Debug.Log("old modified");
            }
            // You can find the created folder in your phone's memory
        }
        catch (Exception e) { Debug.LogError (e); }
    }
}

