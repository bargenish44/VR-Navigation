using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Parser;

class main
{
    
    public static void Main(string[] args)
    {
        Console.WriteLine("HI");
        Points points = JsonConvert.DeserializeObject<Points>(File.ReadAllText(@"config.json"));
        Console.WriteLine(points.ToString());
        Graph graph = new Graph();
       foreach (Point p in points.points)
        {
            Vertex ver = new Vertex(p.id,p.Picture);
            graph._vertex.Add(ver);
        }
        foreach(Vertex p in graph._vertex)
        {
            foreach(Point point in points.points)
            { 
                if(point.id == p.ID)
                {
                    foreach(Neighbor n in point.Neighbors)
                    {
                        p.addnei(graph.GetVertexById(n.PointID), n.Azimut);
                    }
                }
            }
        }
        Console.WriteLine(graph.ToString());
       /*
        int i = 1;
        Vertex temp;
        while (true)
        {
            temp = graph.GetVertexById(i);
            String str = "The neis is :";
            foreach (Vertex v in temp.nei)
            {
                str += v.Id + ",";
            }
            Console.WriteLine(str);
            String s = Console.ReadLine();
            if (s.Equals("exit")) return;
            i = Convert.ToInt32(s);
            while (i < 1 || i > graph.Vercount)
            {
                Console.WriteLine("Please choose num good number");
                s = Console.ReadLine();
                if (s.Equals("exit")) return;
                i = Convert.ToInt32(s);
            }

        }
        */
    }
}
