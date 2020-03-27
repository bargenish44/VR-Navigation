using System;
using System.Collections;
using System.Collections.Generic;

public class Graph
{
    public List<Vertex> _vertex = new List< Vertex > ();
    public Graph()
    {
        _vertex.Clear();
    }
    public Vertex GetVertexById(int Id)
    {
        foreach (Vertex v in this._vertex)
            if (v.ID == Id) return v;
        return null;
    }
    public string ToString()
    {
        string s = "";
        foreach(Vertex ver in _vertex)
        {
            s += ver.ToString()+"\n\n";
        }
        return s;
    }

}
public class Tuples
{
    private Vertex _verex;
    private double azimuth;

    public Vertex Verex { get => _verex; set => _verex = value; }
    public double Azimuth { get => azimuth; set => azimuth = value; }
    public Tuples(Vertex v,double azim)
    {
        _verex = new Vertex(v);
        azimuth = azim;
    }
}

public class Vertex
{
    private int id;
    private List<Tuples> nei = new List<Tuples>();

    public Vertex(int ID,string URL)
    {
        id = ID;
        Url = URL;
    }
    public Vertex(Vertex ot)
    {
        this.id = ot.id;
        this.nei.Clear();
        this.nei.AddRange(ot.nei);
    }
    public void addnei(Vertex v,double azimuth)
    {
        nei.Add(new Tuples(v,azimuth));
    }
    public List<Tuples> GetNei() { return nei; }
    public double Azimuth { get; set; }
    public string Url { get; set; }
    public int ID { get => id; set => id = value; }
    public string ToString()
    {
        string s = "ID : "+ID+" , PATH : "+Url+ " , Neighbors : ";
        foreach(Tuples t in nei)
        {
            s+= t.Verex.id +" : "+t.Azimuth+"\t";
        }
        return s;
    }
    /*
public static void Main(String[] args)
{
Graph graph = new Graph(4);
Vertex ver = new Vertex("first", 90);
Vertex ver2 = new Vertex("second", 180);
Vertex ver3 = new Vertex("third", 0);
Vertex ver4 = new Vertex("forth", 50);
ver.addnei(ver2);
ver.addnei(ver4);
ver2.addnei(ver3);
ver3.addnei(ver4);
graph.addcompleateVertex(ver);
graph.addcompleateVertex(ver2);
graph.addcompleateVertex(ver3);
graph.addcompleateVertex(ver4);
for(int l = 0; l < 4; l++)
{
for (int j = 0; j < 4; j++)
{
Console.Write(graph.azimuthvar[l, j]+",");
}
Console.WriteLine();
}
int i = 1;
Vertex temp;
while (true)
{
temp = graph.GetVertexById(i);
String str = "The neis is :";
foreach(Vertex v in temp.nei)
{
str += v.Id + ",";
}
Console.WriteLine(str);
String s = Console.ReadLine();
if (s.Equals("exit")) +return;
i = Convert.ToInt32(s);
while (i < 1 || i > graph.Vercount)
{
Console.WriteLine("Please choose num good number");
s = Console.ReadLine();
if (s.Equals("exit")) return;
i = Convert.ToInt32(s);
}

}
}
*/
}