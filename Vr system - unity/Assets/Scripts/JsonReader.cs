using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Persistence
{
    public class JsonReader
    {

        //This function creates Pictures and Json librarys, and read the Json.
        public string GetJson()
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
            catch (IOException ex) { }
            if (System.IO.File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var str = System.Text.Encoding.Default.GetString(bytes);
                return str;
            }
            else return "";
        }
    }
};