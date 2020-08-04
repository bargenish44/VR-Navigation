using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SimpleFileBrowser;

public class FileBroswerText : MonoBehaviour
{
    string path;
    public FileBroswerText json;

    void Start()
    {
        //#if UNITY_EDITOR  //Computer
        //          path = EditorUtility.OpenFilePanel("Please select json", "", "json");
        //        OpenExplorer();
        //#else
        //    path = Re
        //#endif
        var jsonTextFile = Resources.Load<TextAsset>("Text/config");
        OpenExplorer(jsonTextFile);
    }

    public void OpenExplorer(TextAsset json)
    {
        Parser parser = new Parser();
        var a = parser.DeserializeJson(json.ToString());
        MapBuilder mapBuilder = (new GameObject("MapBuilder")).AddComponent<MapBuilder>();
        mapBuilder.BuildMap(a);
        
    }
}