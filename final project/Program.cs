using System;
using System.Collections;

public class Graph
{
    public ArrayList _vertex = new ArrayList();
    public double[,] azimuthvar;
    public Graph(int n)
    {
        Vercount = n;
        _vertex.Clear();
        azimuthvar = new double[Vercount, Vercount];
        for (int i = 0; i < Vercount; i++)
            for (int j = 0; j < Vercount; j++)
                azimuthvar[i, j] = -1;
    }
    public void addcompleateVertex(Vertex v)
    {
        _vertex.Add(v);
        foreach (Vertex o in v.nei)
        {
            azimuthvar[v.Id-1, o.Id-1] = v.Azimuth;
        }
    }
    public int Vercount { get; set; }
    public Vertex GetVertexById(int Id)
    {
        foreach (Vertex v in this._vertex)
            if (v.Id == Id) return v;
        return null;
    }

}

public class Vertex
{
    public static int k = 1;
    public ArrayList nei = new ArrayList();

    public Vertex(string URL, double azimuth)
    {
        Id = k;
        Url = URL;
        Azimuth = azimuth;
        k++;
    }
    public void addnei(Vertex v)
    {
        nei.Add(v);
        if(!v.nei.Contains(v))
            v.nei.Add(this);
    }
    public double Azimuth { get; set; }
    public int Id { get; set; }
    public string Url { get; set; }
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
    }
}