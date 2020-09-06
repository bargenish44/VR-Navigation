using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Presentation
{
    public class GameOverText : MonoBehaviour
    {
        [SerializeField] private string path = "";
        public TextMeshProUGUI text;
        [SerializeField] private bool json = false;
        void Start()
        {
            if (!json)
            {
                if (path.Equals("")) path = "The path of the csv is : \n" + Application.persistentDataPath + "/stats.csv";
            }
            else
            {
                if (path.Equals("")) path = "Please insert a valid json, make sure it is called config.json to : \n" + Application.persistentDataPath + "/Json";
            }
            text.text = path;
        }
    }
};