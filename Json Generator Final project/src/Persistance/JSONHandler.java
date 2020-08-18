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
import java.util.HashMap;
import org.json.simple.*;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;
import Logic.Map;
import Logic.Point;

public class JSONHandler {
	public static void Save(String Path,HashMap<Integer,Point> map, boolean ans, String projName, String navigationImg, String finalNavigationImg) 
			throws UnsupportedEncodingException, FileNotFoundException, IOException {
		
		try (Writer writer = new BufferedWriter(new OutputStreamWriter(
				new FileOutputStream(Path), "utf-8"))) {
			writer.write(CreateJSON(map, ans,projName,navigationImg,finalNavigationImg));
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

	private static String CreateJSON(HashMap<Integer,Point> map, boolean ans, String projName, String naviimg, String finalImg) { // create json stracture
		String str = "{\n";
		str += "\"ProjectName\": \"" +projName + "\",\n";
		if(!ans) { // phone version 
			naviimg = PhonePath(naviimg);
			finalImg = PhonePath(finalImg);
		}
		if(naviimg.equals("null")) naviimg ="";
		if(finalImg.equals("null")) finalImg ="";
		str += "\"NavigationImage\": \"" +naviimg + "\",\n";
		str += "\"FinalNavigationImage\": \"" +finalImg + "\",\n";
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
		return PhonePath + p[p.length-1];}
		catch (Exception e) {return "";}
	}
}