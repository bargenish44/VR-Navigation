package Logic;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.HashMap;
import javax.swing.JFileChooser;

import org.json.simple.parser.ParseException;

import Persistance.JSONHandler;
import sun.rmi.transport.Endpoint;

public class Map {
	private HashMap<Integer,Point> map = new HashMap<>();  // key = id , val = Point;
	private HashMap<String,Point> nickToPoint = new HashMap<>();	// key = nickName , val = Point;
	private String navigationImg = "";
	private String finalNavigationImg = "";
	private String startPoint = "";
	private ArrayList<String> endsPoint = new ArrayList<>();

	public HashMap<Integer, Point> getMap() {
		return map;
	}

	public HashMap<String,Point> getNickToPoint() {
		return nickToPoint;
	}

	public String getNavigationImg() {
		return navigationImg;
	}

	public void setNavigationImg(String navImg) {
		navigationImg = navImg;
	}

	public String getFinalNavigationImg() {
		return finalNavigationImg;
	}

	public void setFinalNavigationImg(String finalNavImg) {
		this.finalNavigationImg = finalNavImg;
	}

	public void AddPoint(String url) {
		Point temp = new Point(url);
		map.put(temp.getId(), temp);
		nickToPoint.put(temp.getNickName(), temp);
	}
	public void AddPointLoad(String url,int id) {
		Point temp = new Point(url,id);
		map.put(temp.getId(), temp);
		nickToPoint.put(temp.getNickName(), temp);
	}
	public void RemovePoint(int id) {
		if(map.get(id) == null) throw new IllegalArgumentException("Point doesn't exist");
		for(Point h : map.values())
		{
			try {
				h.RemoveNeighbor(id);
			}catch (IllegalArgumentException i) {}
		}
		nickToPoint.remove(map.get(id).getNickName());
		map.remove(id);
	}
	public void EditPoint(int id , String newUrl) {
		if(map.get(id) == null) throw new IllegalArgumentException("Point doesn't exist");
		map.get(id).setPicture(newUrl);
		String[] path = newUrl.split("\\\\");
		nickToPoint.put(path[path.length-1], map.get(id));
		nickToPoint.remove(map.get(id).getNickName());
		map.get(id).setNickName(path[path.length-1]);
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

	public void ExportJSON(String path,boolean ans, String projName) throws UnsupportedEncodingException, FileNotFoundException, IOException {
		JSONHandler.Save(path, map, ans, projName, navigationImg, finalNavigationImg, startPoint, endsPoint);
	}

	public Map ImportJSON(String path) throws IOException, ParseException { 
		return JSONHandler.Load(path);
	}

	public void ClearMap() {
		map.clear();
		nickToPoint.clear();
		navigationImg ="";
		finalNavigationImg = "";
		startPoint = "";
		endsPoint.clear();
	}

	public ArrayList<Integer> GetTextsID(){
		ArrayList<Integer> textsID = new ArrayList<>();
		for(int pID : map.keySet()) {
			Point p = map.get(pID);
			for(int tID : p.getOptionalTexts().keySet()) {
				Optionaltext ot = p.getOptionalTexts().get(tID);
				textsID.add(ot.getId());
			}
		}
		return textsID;
	}

	public String getStartPoint() {
		return startPoint;
	}

	public void setStartPoint(String startPoint) {
		this.startPoint = startPoint;
	}

	public ArrayList<String> getEndsPoint() {
		return endsPoint;
	}
	
	public void setEndsPoint(ArrayList<String> points) {
		endsPoint = points;
	}
}
