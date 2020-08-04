using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverText : MonoBehaviour
{
    [SerializeField] private string path = "";
    public TextMeshProUGUI text;
    void Start()
    {
        if (path.Equals("")) path = "The path of the csv is : \n" + Application.persistentDataPath + "/stats.csv";
        text.text = path;
    }
}
