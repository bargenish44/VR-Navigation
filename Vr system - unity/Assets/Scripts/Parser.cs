using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class Parser
{
    public class Points
    {
        public List<Point> points = new List<Point>();
        public string ToString()
        {
            string s = "";
            foreach (Point p in points)
            {
                s += p.ToString() + "\n";
            }
            return s;
        }
    }

    public class Point
    {
        public int id { get; set; }
        public string Picture { get; set; }
        public List<Neighbor> Neighbors = new List<Neighbor>();
        public List<Optionaltext> OptionalText = new List<Optionaltext>();
        public string ToString()
        {
            string s = "ID : " + id + "\nPATH : " + Picture + "\nNeighbors : ";
            foreach (Neighbor n in Neighbors)
            {
                s += n.ToString();
            }
            s += "\nTexts : ";
            foreach (Optionaltext o in OptionalText)
            {
                s += o.ToString();
            }
            return s;
        }
    }

    public class Neighbor
    {
        public int PointID { get; set; }
        public float Azimut { get; set; }
        public string ToString()
        {
            string s = "[ID : " + PointID + " , Azimuth : " + Azimut + "]";
            return s;
        }
    }
    [Serializable]
    public class Optionaltext
    {
        public string text { get; set; }
        public float DurationInSeconds { get; set; }
        public float whenToDisplay { get; set; }
        public string ToString()
        {
            string s = "[Text : " + text + " , Duration : " + DurationInSeconds + " , after : " + whenToDisplay + "]";
            return s;
        }

    }

    public Points DeserializeJson(string JsonText)
    {
        try
        {
            Points points = JsonConvert.DeserializeObject<Points>(JsonText);
            SortByText(ref points);
            return points;
        }
        catch (Exception e) { SceneManager.LoadScene("InsertJson"); }
        return null;
    }

    private void SortByText(ref Points p)
    {
        foreach (Point points in p.points)
        {
            points.OptionalText = points.OptionalText.OrderBy(o => o.whenToDisplay).ToList<Optionaltext>();
        }
    }
}