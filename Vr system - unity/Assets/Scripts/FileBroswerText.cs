using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.SceneManagement;

public class FileBroswerText : MonoBehaviour
{
    string path;
    public FileBroswerText json;

    void Start()
    {
        string jsonTextFile = CheckJson();
        if (string.IsNullOrEmpty(jsonTextFile)) SceneManager.LoadScene("InsertJson");
        OpenExplorer(jsonTextFile);
    }

    public void OpenExplorer(string json)
    {
        Parser parser = new Parser();
        var a = parser.DeserializeJson(json);        Debug.Log(a.ToString());
        MapBuilder mapBuilder = (new GameObject("MapBuilder")).AddComponent<MapBuilder>();
        mapBuilder.BuildMap(a);
    }


    private string CheckJson()
    {
        string path = Application.persistentDataPath + "/Json/config.json";
        try
        {
            if (!Directory.Exists(Application.persistentDataPath + "/Json"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Json");
            }
            if (!Directory.Exists(Application.persistentDataPath + "/Pictures"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Pictures");
            }
        }
        catch (IOException ex){}
        if (System.IO.File.Exists(path))
        {
            var bytes = File.ReadAllBytes(path);
            var str = System.Text.Encoding.Default.GetString(bytes);
            return str;
        }
        else return ""; 
    }
}