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

import Logic.Map;
import Logic.Point;

public class JSONHandler {
	public static void Save(String Path,HashMap<Integer,Point> map) throws UnsupportedEncodingException, FileNotFoundException, IOException {
		try (Writer writer = new BufferedWriter(new OutputStreamWriter(
				new FileOutputStream(Path), "utf-8"))) {
			writer.write(CreateJSON(map));
		} catch (IOException e) {e.printStackTrace();}
	}
	public static Map Load(String path) throws IOException { // DeSearlize json
		return null;
		//return new String(Files.readAllBytes(Paths.get(path))); 
	}

	private static String CreateJSON(HashMap<Integer,Point> map) { // create json stracture
		String str = "{\n";
		str += "\"Points\": [";
		for(int i : map.keySet()) {
			str += map.get(i).toString() + ",";
		}
		if(map.size()>0)
			str = str.substring(0, str.length()-1);
		str += "]\n}";
		return str;
	}
}
