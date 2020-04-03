using Newtonsoft.Json;
using System;
using System.IO;


class Parser
{

    public class Points
    {
        public Point[] points { get; set; }
        public string ToString()
        {
            string s = "";
            foreach(Point p in points)
            {
                s += p.ToString()+"\n";
            }
            return s;
        }
    }

    public class Point
    {
        public int id { get; set; }
        public string Picture { get; set; }
        public Neighbor[] Neighbors { get; set; }
        public string ToString()
        {
            string s = "ID : " + id + "\nPATH : " + Picture+"\nNeighbors : ";
            foreach(Neighbor n in Neighbors)
            {
                s += n.ToString();
            }
            return s;
        }
    }

    public class Neighbor
    {
        public int PointID { get; set; }
        public int Azimut { get; set; }
        public string ToString()
        {
            string s = "[ID : " + PointID + " , Azimuth : " + Azimut+"]";
            return s;
        }
        public Points DeserializeJson()
        {
            Points points = JsonConvert.DeserializeObject<Points>(File.ReadAllText(@"config.json"));
            return points;
        }
    }
    
    /*static void Main(string[] args)
    {
        Points points = JsonConvert.DeserializeObject<Points>(File.ReadAllText(@"config.json"));
        Console.WriteLine(points.ToString());
    }*/
    

}

