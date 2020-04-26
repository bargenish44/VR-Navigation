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
#if UNITY_EDITOR  //Computer
        path = EditorUtility.OpenFilePanel("Please select json", "", "json");
        OpenExplorer();
#else
        //Android device
        Debug.Log("Android Device");
        // Set filters (optional)
        // It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
        // if all the dialogs will be using the same filters
        FileBrowser.SetFilters(true, new FileBrowser.Filter("json"), new FileBrowser.Filter("json"));

        // Coroutine example
        StartCoroutine(ShowLoadDialogCoroutine());
#endif
    }

    public void OpenExplorer()
    {
        if (path != null)
        {
            Parser parser = new Parser();
            var a = parser.DeserializeJson(path);
            MapBuilder mapBuilder = (new GameObject("MapBuilder")).AddComponent<MapBuilder>();
            Debug.Log(a.ToString());
            mapBuilder.BuildMap(a);
        }
    }
    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);

        if (FileBrowser.Success)
        {
            // If a file was chosen, read its bytes via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
            foreach (byte b in bytes)
            {
                Debug.Log(b.ToString());
            } 
        }
    }
}
