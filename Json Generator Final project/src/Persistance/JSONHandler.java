package Persistance;

import java.io.BufferedWriter;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import java.io.Writer;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashMap;
import org.json.simple.*;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;
import Logic.Map;
import Logic.Point;

public class JSONHandler {
	public static void Save(String Path,HashMap<Integer,Point> map, boolean ans, String projName, String navigationImg, String finalNavigationImg, String startPoint, ArrayList<String> endPoints) 
			throws UnsupportedEncodingException, FileNotFoundException, IOException {

		try (Writer writer = new BufferedWriter(new OutputStreamWriter(
				new FileOutputStream(Path), "utf-8"))) {
			writer.write(CreateJSON(map, ans,projName,navigationImg,finalNavigationImg, startPoint, endPoints));
		} catch (IOException e) {e.printStackTrace();}
	}
	public static Map Load(String path) throws IOException, ParseException { // DeSearlize json
		Map map = new Map();
		String fileContent =  new String(Files.readAllBytes(Paths.get(path)));
		JSONParser parser = new JSONParser();
		JSONObject fileJson = (JSONObject) parser.parse(fileContent);
		String tranImg = (String)fileJson.get("NavigationImage");
		String FinaltranImg = (String)fileJson.get("FinalNavigationImage");
		map.setNavigationImg(tranImg);
		map.setFinalNavigationImg(FinaltranImg);
		try { // support JSON from previous versions.
		Long startPoint = (Long)fileJson.get("StartPoint");
		JSONArray endPoints = (JSONArray) fileJson.get("EndPoints");
		ArrayList<String> ends = new ArrayList<>();
		for(Object p : endPoints) {
			Long name =  (Long)p;
			ends.add(name+"");
		}
		map.setStartPoint(startPoint+"");
		map.setEndsPoint(ends);
		} catch(Exception e) {} 
		JSONArray points = (JSONArray) fileJson.get("Points");
		for(Object point : points) {
			JSONObject pointJson = (JSONObject)point;
			long id =  (long)pointJson.get("id");
			String pic = (String)pointJson.get("Picture");
			map.AddPointLoad(pic, (int)id);
			JSONArray texts = (JSONArray) pointJson.get("OptionalText");
			try {
				for(Object t : texts) {
					JSONObject OpText = (JSONObject)t;
					String text =  (String)OpText.get("text");
					double dur = (double) OpText.get("DurationInSeconds");
					double when = (double)OpText.get("whenToDisplay");
					map.AddOptionalText((int)id, text, (float)dur, (float)when);
				}}catch (NullPointerException e) {}
		}
		for(Object point : points) { // add neighbors
			JSONObject pointJson = (JSONObject)point;
			long id =  (long)pointJson.get("id");
			JSONArray neighbors = (JSONArray) pointJson.get("Neighbors");
			for(Object neighbor : neighbors) {
				JSONObject nei = (JSONObject)neighbor;
				long to =  (long)nei.get("PointID"); 
				double azimut = (double)nei.get("Azimut");
				map.AddNeighbor((int)id,(int) to,(float) azimut);
			}
		}
		return map;
	}

	private static String CreateJSON(HashMap<Integer,Point> map, boolean ans, String projName, String naviimg, String finalImg, String startPoint, ArrayList<String> endPoints) { // create json stracture
		String str = "{\n";
		str += "\"ProjectName\": \"" +projName + "\",\n";
		if(!ans) { // phone version 
			naviimg = PhonePath(naviimg);
			finalImg = PhonePath(finalImg);
		}
		naviimg = naviimg.replaceAll("\\\\", "/");
		finalImg = finalImg.replaceAll("\\\\", "/");
		if(naviimg.equals("null")) naviimg ="";
		if(finalImg.equals("null")) finalImg ="";
		str += "\"NavigationImage\": \"" +naviimg + "\",\n";
		str += "\"FinalNavigationImage\": \"" +finalImg + "\",\n";
		str += "\"StartPoint\": " +fixStartPoints(startPoint,map) + ",\n";
		str += "\"EndPoints\": [" +fixEndPoitns(endPoints,map)+ "],\n";
		str += "\"Points\": [";
		for(int i : map.keySet()) {
			str += map.get(i).toJson(ans) + ",";
		}
		if(map.size()>0)
			str = str.substring(0, str.length()-1);
		str += "]\n}";
		return str;
	}

	private static String PhonePath(String path) {
		if(path.isBlank()) return path;
		try {
			String PhonePath = "/storage/emulated/0/Android/data/com.Ariel.VrNavigation/files/Pictures/";
			String[] p = path.split("\\\\");
			if(p.length != 1)
				return PhonePath + p[p.length-1];
			else {
				p = path.split("/");
				return PhonePath + p[p.length-1];
			}
		}
		catch (Exception e) {return "";}
	}
	private static String fixEndPoitns(ArrayList<String> endPoints, HashMap<Integer,Point> map) {
		if(endPoints.isEmpty())
			return (String) (map.keySet().toArray()[map.size()-1]+"");
		String str = endPoints.toString();
		return str.substring(1, str.length()-1);
	}
	private static String fixStartPoints(String start, HashMap<Integer,Point> map) {
		if(start.isEmpty()) {
			return (String) (map.keySet().toArray()[0]+"");
		}
		return start;
	}
}