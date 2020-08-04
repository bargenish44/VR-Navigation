package Logic;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.HashMap;
import javax.swing.JFileChooser;
import Persistance.JSONHandler;

public class Map {
	private HashMap<Integer,Point> map = new HashMap<>();  // key = id , val = Point;

	public HashMap<Integer, Point> getMap() {
		return map;
	}
	
	public void AddPoint(String url) {
		Point temp = new Point(url);
		map.put(temp.getId(), temp);
	}
	public void RemovePoint(int id) {
		if(map.get(id) == null) throw new IllegalArgumentException("Point doesn't exist");
		for(Point h : map.values())
		{
			try {
				h.RemoveNeighbor(id);
			}catch (IllegalArgumentException i) {}
		}
		map.remove(id);
	}
	public void EditPoint(int id , String newUrl) {
		if(map.get(id) == null) throw new IllegalArgumentException("Point doesn't exist");
		map.get(id).setPicture(newUrl);
	}
	public void AddNeighbor(int from,int to,float azimut) {
		Point from_Point = map.get(from);
		if(from_Point == null)
			throw new IllegalArgumentException("From point doesn't exist");
		if(map.get(to) == null)
			throw new IllegalArgumentException("To point doesn't exist");
		if(to == from)
			throw new IllegalArgumentException("A point cannot be a neighbor of its own");
		from_Point.AddNeighbor(to, azimut);
	}
	public void RemoveNeighbor(int from,int to) {
		Point from_Point = map.get(from);
		if(from_Point == null)
			throw new IllegalArgumentException("From point doesn't exist");
		from_Point.RemoveNeighbor(to);
	}
	public void EditNeighbor(int from, int to, float az) {
		Point from_Point = map.get(from);
		if(from_Point == null)
			throw new IllegalArgumentException("From point doesn't exist");
		from_Point.EditNeighbor(to, az);
	}
	public int AddOptionalText(int PointId, String text , float dur, float when) {
		Point from_Point = map.get(PointId);
		if(from_Point == null)
			throw new IllegalArgumentException("From point doesn't exist");
		return from_Point.AddOptionalText(text, dur, when);
	}
	public void RemoveOptionalText(int PointId, int textId) {
		Point from_Point = map.get(PointId);
		if(from_Point == null)
			throw new IllegalArgumentException("From point doesn't exist");
		from_Point.RemoveOptionalText(textId);
	}
	public void EditOptionalText(int PointId, int textId, String text, float dur, float when) {
		Point from_Point = map.get(PointId);
		if(from_Point == null)
			throw new IllegalArgumentException("From point doesn't exist");
		from_Point.EditOptionalText(textId, text, dur, when);
	}

	public void ExportJSON(String path) throws UnsupportedEncodingException, FileNotFoundException, IOException {
		JSONHandler.Save(path, map);
	}
	
	public Map ImportJSON(String path) throws IOException { 
		return JSONHandler.Load(path);
	}
	
	public void ClearMap() {
		map.clear();
	}
}
