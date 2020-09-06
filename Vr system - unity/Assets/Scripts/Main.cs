using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Presentation;
using Persistence;
using Logic;

using UnityEngine.SceneManagement;
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Parser parser = new Parser();
        Points points = parser.ReadPoints();
        if(points == null) SceneManager.LoadScene("InsertJson");
        GameObject.Find("MapBuilder").GetComponent<MapBuilder>().BuildMap(points);
    }
}
