package Gui;

import javax.swing.*;

import jdk.nashorn.internal.parser.JSONParser;

public class JListExample			
{
	public static void main(String[] args)
	{
		/*JFileChooser fileChooser = new JFileChooser();
		fileChooser.setSelectedFile(new File("config.json"));
		int returnValue = fileChooser.showSaveDialog(null);
		if (returnValue == JFileChooser.APPROVE_OPTION) {
			String Picture = fileChooser.getSelectedFile().getAbsolutePath(); 
			String newPath = Picture.replaceAll("\\\\", "/");
			System.out.println(newPath);
		}*/
		ObjectMapper mapper = new ObjectMapper().registerModule(new JavaTimeModule());
		mapper.setDateFormat(new StdDateFormat());
	}
}