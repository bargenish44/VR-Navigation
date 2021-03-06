package Gui;

import java.awt.*;
import java.io.File;
import java.io.FileFilter;
import java.io.IOException;
import java.net.URL;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;

import javax.imageio.ImageIO;
import java.awt.event.*;
import java.awt.geom.Rectangle2D;
import java.awt.image.BufferedImage;
import javax.swing.*;
import javax.swing.border.TitledBorder;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import javax.swing.filechooser.FileNameExtensionFilter;

import Logic.Map;
import Logic.Neighbor;
import Logic.Optionaltext;


public class MyFrame implements ActionListener {

	private JMenuItem clear, load, about, Export_Json, addPicture, removePicture, editPicture, addNeighbor, 
	removeNeighbor, editNeighbor, addText, editText, removeText, editNavImg, editFinalNavImg, showNavImg, showFinalNavImg,
	clearNavImg, clearFinalNavImg, markAsStartPoint, markAsEndPoints, unmarkEndPoints; 
	private JMenuBar menubar;
	private JMenu menu, menu2, menu3, menu4;
	private Image img;
	private int width, hight;
	private JFrame frame;
	private ImagePanel panel;
	private Map map = new Map();
	private int selectedImageIndex;
	private int selectedTextIndex;
	private ArrayList<String> picturesLabels = new ArrayList();
	private ArrayList<String> textsLabels = new ArrayList(); // holds all current picture OptionalTexts
	private ArrayList<Integer> textsIDs = new ArrayList(); // holds OpttionalTexts IDs.
	private DefaultListModel listModel = new DefaultListModel<>();
	private DefaultListModel TextListModel = new DefaultListModel<>();
	private JList list = new JList();
	private JList textList = new JList();
	private String PicName = "Picture ";
	private String textName = "Text : ";
	private JScrollPane leftPanel;
	private JScrollPane rightPanel;
	private JSplitPane Mainpanel;
	private boolean listChanged = false;
	private boolean TextListChanged = false;
	private String NeighborName = "Neighbor to : ";
	private BufferedImage arrow;
	private String neiPicPath = "/arrow.jpg";
	private int ImageSizeWidh = 30;
	private int ImageSizeHeight = 30;
	private int rotationFix= 180;
	private String choose = "";
	private String editNeiStr = "edit neighbor";
	private String addNeiStr = "add neighbor";
	private int neiToEdit = -1;
	private String computerOption = "Computer", phoneOption = "Phone";

	public static void main(String[] args) {
		new MyFrame();
	}
	/**
	 * Defult constractor.
	 */
	public MyFrame() {
		try {
			img = null;
			URL url = MyFrame.class.getResource(neiPicPath);
			arrow = ImageIO.read(url);
			frame = new JFrame("Json Generator");
			menubar = new JMenuBar();
			menu = new JMenu("Help");
			menubar.add(menu);
			about = new JMenuItem("About");
			about.addActionListener(this);
			menu.add(about);
			menu2 = new JMenu("Option");
			load = new JMenuItem("Load");
			load.addActionListener(this);
			menu2.add(load);
			clear = new JMenuItem("Clear");
			clear.addActionListener(this);
			menu2.add(clear);
			Export_Json = new JMenuItem("Export Json");
			Export_Json.addActionListener(this);
			menu2.add(Export_Json);
			menubar.add(menu2);
			menu3 = new JMenu("Add/Remove");
			addPicture = new JMenuItem("Add picture/s");
			addPicture.addActionListener(this);
			menu3.add(addPicture);
			removePicture = new JMenuItem("Remove picture");
			removePicture.addActionListener(this);
			menu3.add(removePicture);
			editPicture = new JMenuItem("Edit picture");
			editPicture.addActionListener(this);
			menu3.add(editPicture);
			addNeighbor = new JMenuItem("Add neighbor");
			addNeighbor.addActionListener(this);
			menu3.add(addNeighbor);
			removeNeighbor = new JMenuItem("Remove neighbor");
			removeNeighbor.addActionListener(this);
			menu3.add(removeNeighbor); 
			editNeighbor = new JMenuItem("Edit neighbor");
			editNeighbor.addActionListener(this);
			menu3.add(editNeighbor);
			addText = new JMenuItem("Add text");
			addText.addActionListener(this);
			menu3.add(addText);
			removeText = new JMenuItem("Remove text");
			removeText.addActionListener(this);
			menu3.add(removeText); 
			editText = new JMenuItem("Edit text");
			editText.addActionListener(this);
			menu3.add(editText);
			menubar.add(menu3);
			menu4 = new JMenu("Optional");
			showNavImg = new JMenuItem("Show navigation image path");
			showNavImg.addActionListener(this);
			menu4.add(showNavImg);
			showFinalNavImg = new JMenuItem("Show final navigation image path");
			showFinalNavImg.addActionListener(this);
			menu4.add(showFinalNavImg);
			editNavImg = new JMenuItem("Edit navigation image");
			editNavImg.addActionListener(this);
			menu4.add(editNavImg);
			editFinalNavImg = new JMenuItem("Edit final navigation image");
			editFinalNavImg.addActionListener(this);
			menu4.add(editFinalNavImg);
			clearNavImg = new JMenuItem("Clear navigation image");
			clearNavImg.addActionListener(this);
			menu4.add(clearNavImg);
			clearFinalNavImg = new JMenuItem("Clear final navigation image");
			clearFinalNavImg.addActionListener(this);
			menu4.add(clearFinalNavImg);
			markAsStartPoint = new JMenuItem("Mark as start point");
			markAsStartPoint.addActionListener(this);
			menu4.add(markAsStartPoint);
			markAsEndPoints = new JMenuItem("Mark as end point");
			markAsEndPoints.addActionListener(this);
			menu4.add(markAsEndPoints);
			unmarkEndPoints = new JMenuItem("Unmark as end point");
			unmarkEndPoints.addActionListener(this);
			menu4.add(unmarkEndPoints);
			menubar.add(menu4);
			frame.setJMenuBar(menubar);
			frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
			frame.setLayout(new BorderLayout());
			panel = new ImagePanel(img);
			list.addListSelectionListener(new ListSelectionListener() { // selectPictureList

				@Override
				public void valueChanged(ListSelectionEvent e) {
					if (!e.getValueIsAdjusting()) {
						try {
							selectedImageIndex = map.getNickToPoint().get(picturesLabels.get(list.getSelectedIndex())).getId();
							if(! (selectedImageIndex < 0 || map.getMap().get(selectedImageIndex) == null))
								img = ImageIO.read(new File(map.getMap().get(selectedImageIndex).getPicture()));
							panel.setPic(img);
							TextListChanged = true;
							updateDisplay();
						} catch (IOException e1) {JOptionPane.showMessageDialog(null, "Invalid picture path");}
						catch (IndexOutOfBoundsException e2) {}
					}
				}
			});
			list.setModel(listModel);

			textList.addListSelectionListener(new ListSelectionListener() { // selectTextList

				@Override
				public void valueChanged(ListSelectionEvent e) {
					if (!e.getValueIsAdjusting()) {
						try {
							selectedTextIndex = textsIDs.get(textList.getSelectedIndex());
						} catch (Exception e1) {
						}
					}
				}
			});
			textList.setModel(TextListModel);
			leftPanel = new JScrollPane(textList);
			rightPanel = new JScrollPane(list);
			rightPanel.setBorder (BorderFactory.createTitledBorder (BorderFactory.createEtchedBorder (),
					"Picture list",
					TitledBorder.CENTER,
					TitledBorder.TOP));
			leftPanel.setBorder (BorderFactory.createTitledBorder (BorderFactory.createEtchedBorder (),
					"Optional Text liat",
					TitledBorder.CENTER,
					TitledBorder.TOP));
			JSplitPane temp = new JSplitPane( JSplitPane.VERTICAL_SPLIT, 
					leftPanel, rightPanel );
			Mainpanel = new JSplitPane( JSplitPane.HORIZONTAL_SPLIT, 
					temp, panel);
			Container contentPane = frame.getContentPane();
			frame.setSize(350,200);
			frame.add(Mainpanel);
			frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
			frame.setVisible(true);
			frame.pack();
			frame.setLocationRelativeTo(null);
			frame.setVisible(true);

		} catch (IOException e2) {
			e2.printStackTrace();
		}
	}

	class ImagePanel extends JPanel implements MouseListener {

		private static final long serialVersionUID = 1L;
		private Image img;

		public ImagePanel(String img) {
			this(new ImageIcon(img).getImage());
			this.addMouseListener(this);
		}

		public ImagePanel(Image img) {
			this.img = img;
			this.addMouseListener(this);
		}

		public void setPic(Image pic) {
			this.img = pic;
			repaint();
		}

		@Override
		public void invalidate() {
			super.invalidate();
			width = getWidth();
			hight = getHeight();
		}

		@Override
		public Dimension getPreferredSize() {
			return img == null ? new Dimension(200, 200) : new Dimension(img.getWidth(this), img.getHeight(this));
		}

		@Override
		public void paintComponent(Graphics g) {
			super.paintComponent(g);
			g.drawImage(img, 0, 0, this.getWidth(), this.getHeight(), null);
			Logic.Point selectedPoint = map.getMap().get(selectedImageIndex);
			try {
				for(int nei : selectedPoint.getNeighbors().keySet()) { // draws each neighbor with a text.
					Neighbor n = selectedPoint.getNeighbors().get(nei);
					int fixedX = (int)((n.getAzimut() + rotationFix) * this.getWidth() / (2 * rotationFix));
					g.drawImage(arrow, fixedX - (ImageSizeWidh/2) , this.getHeight()/2, ImageSizeWidh, ImageSizeHeight, null);

					String str = NeighborName + n.getToPointID()+ " ";
					Color textColor = Color.WHITE;
					Color bgColor = Color.BLACK;

					FontMetrics fm = g.getFontMetrics();
					Rectangle2D rect = fm.getStringBounds(str, g);

					int y = this.getHeight()/2;
					g.setColor(bgColor);
					fixedX -= ImageSizeWidh; // Positions the string to be in the middle of the top of the image.
					g.fillRect(fixedX,
							y - fm.getAscent(),
							(int) rect.getWidth(),
							(int) rect.getHeight());

					g.setColor(textColor);
					g.drawString(str, fixedX, y);
				}
			}catch (NullPointerException e) {}
		}

		@Override
		public void mouseClicked(MouseEvent e) {
			if (choose.equals(addNeiStr)) {
				int to = -1;
				int x = e.getX();
				int from = selectedImageIndex;
				try {
					to = Integer.parseInt( JOptionPane.showInputDialog("Neighbor Id :"));
				}catch (Exception ex) {}
				if(to != -1) 
					try {
						map.AddNeighbor(from, to, (float)(x * rotationFix * 2 / this.getWidth()) - rotationFix );
					}catch (Exception e2) {JOptionPane.showMessageDialog(null, e2.getMessage());}
				frame.repaint();
			}
			if (choose.equals(editNeiStr)) {
				int x = e.getX();
				int from = selectedImageIndex;
				try {
					map.EditNeighbor(from, neiToEdit, (float)(x * rotationFix * 2 / this.getWidth()) - rotationFix );
				}catch (Exception e2) {JOptionPane.showMessageDialog(null, e2.getMessage());}
				choose = "";
				neiToEdit = -1;
				frame.repaint();
			}
		}

		@Override
		public void mouseEntered(MouseEvent e) {}

		@Override
		public void mouseExited(MouseEvent e) {}

		@Override
		public void mousePressed(MouseEvent e) {}

		@Override
		public void mouseReleased(MouseEvent e) {}
	}

	private void updateDisplay() {
		try {
			if(listChanged) {
				picturesLabels = new ArrayList<>();
				for(int key : map.getMap().keySet()) {
					picturesLabels.add(map.getMap().get(key).getNickName());
				}
				listModel.clear();
				for(int i=0;i<picturesLabels.size();i++) {
					String pointName = picturesLabels.get(i);
					String start="";
					String end = "";
					if((map.getNickToPoint().get(pointName).getId()+"").equals(map.getStartPoint())) 
						start = " | Start point ";
					if(map.getEndsPoint().contains(map.getNickToPoint().get(pointName).getId()+"")) {
						end = " | End point ";
					}
					listModel.addElement("ID : "+map.getNickToPoint().get(pointName).getId()+"  |  "+picturesLabels.get(i)+start+end);
				}
				listChanged = false;
			}
			if(TextListChanged) {
				textsLabels= new ArrayList<>();
				for (int text : map.getMap().get(selectedImageIndex).getOptionalTexts().keySet()) {
					Optionaltext tmp = map.getMap().get(selectedImageIndex).getOptionalTexts().get(text);
					textsLabels.add(textName + tmp.getText() + "\nDuration : "+ tmp.getDurationInSeconds()+ "\nWhen : "+tmp.getWhenToDisplay());
				}
				TextListModel.clear();
				for(int i=0;i<textsLabels.size();i++) {
					TextListModel.addElement(textsLabels.get(i));
				}
				TextListChanged = false;
			}
		}catch (NullPointerException e) {}
	}

	@Override
	public void actionPerformed(ActionEvent e) {

		//Help Button
		if (e.getSource() == about) {
			JOptionPane.showMessageDialog(null,"First, you have to add all the points,\r\n" + 
					"you also can remove points, edit points (change the point picture path).\r\n" + 
					"Then you can connect pictures crossover by add picture and click on the crossover spot,\r\n" + 
					"or edit a neighbor's crossover spot, or delete neighbor.\r\n" + 
					"Also, you have the option to add text that will show in the navigation system,\r\n" + 
					"edit text (edit text/duration/when to display) or even delete a text.\r\n" + 
					"After you did create the map,\r\n" + 
					"You can export the JSON to phone version (with phone paths),\r\n" + 
					"or to a computer version that allows you to edit the JSON.\r\n" + 
					"Also, you have the option to load an existing JSON or clear your map.\r\n" +
					"You can edit the navigation arrows, put it blank if you want the default icons. \r\n" +
					"You can select one starting point and several endpoints, the default is that the first point "
					+ "is the beginning and the last point is the last.","About", JOptionPane.PLAIN_MESSAGE);
			choose = "";
		}

		//--Options Menu--
		//Load Json Button
		if (e.getSource() == load) {
			JFileChooser fileChooser = new JFileChooser();
			int returnValue = fileChooser.showOpenDialog(null);
			if (returnValue == JFileChooser.APPROVE_OPTION)
				try {
					map.ClearMap();
					listModel.clear();
					TextListModel.clear();
					img = null;
					panel.setPic(img);
					map = map.ImportJSON(fileChooser.getSelectedFile().getAbsolutePath());
					textsIDs.clear();
					textsIDs = map.GetTextsID();
					listChanged = true;
					TextListChanged = true;
				} catch (Exception e1) {
					JOptionPane.showMessageDialog(null, "Error occurred during import JSON");
					System.out.println(e1.getMessage());
				}
			choose = "";
		}

		//Export json
		if (e.getSource() == Export_Json) {
			choose = "";
			JFileChooser fileChooser = new JFileChooser();
			fileChooser.setSelectedFile(new File("config.json"));
			int returnValue = fileChooser.showSaveDialog(null);
			if (returnValue == JFileChooser.APPROVE_OPTION)
				try {
					boolean ans = true;
					String[] options = new String[] {computerOption, phoneOption};
					int response = JOptionPane.showOptionDialog(null, "Choose path version", "Export JSON", 
							JOptionPane.DEFAULT_OPTION, JOptionPane.PLAIN_MESSAGE,
							null, options, options[0]);
					if(options[response].equals(phoneOption))
						ans = false;
					String projName = JOptionPane.showInputDialog("Insert project name :");
					map.ExportJSON(fileChooser.getSelectedFile().getAbsolutePath(),ans,projName); 
					JOptionPane.showMessageDialog(null, "File saved at : " + fileChooser.getSelectedFile().getAbsolutePath());
					if(!ans)
						JOptionPane.showMessageDialog(null, "Copy the pictures to : /storage/emulated/0/Android/data/com.Ariel.VrNavigation/files/Pictures/");
				} catch (IOException e1) {
					JOptionPane.showMessageDialog(null, "Error occurred during export JSON");
				}
		}

		//Clear Map
		if (e.getSource() == clear) {
			map.ClearMap();
			listModel.clear();
			TextListModel.clear();
			choose = "";
			img = null;
			panel.setPic(img);
			textsIDs.clear();
		}
		//--Optional Menu--
		if(e.getSource() == showNavImg) {
			if(map.getNavigationImg().equals(""))
				JOptionPane.showMessageDialog(null, "The path is : default");
			else JOptionPane.showMessageDialog(null, "The path is : "+ map.getNavigationImg());
		}
		if(e.getSource() == showFinalNavImg) {
			if(map.getFinalNavigationImg().equals(""))
				JOptionPane.showMessageDialog(null, "The path is : default");
			else JOptionPane.showMessageDialog(null, "The path is : "+ map.getFinalNavigationImg());
		}
		if(e.getSource() == editNavImg ||e.getSource() == editFinalNavImg) {
			JFileChooser fileChooser = new JFileChooser();
			fileChooser.setAcceptAllFileFilterUsed(false);
			fileChooser.addChoosableFileFilter(new FileNameExtensionFilter(
					"Image files", ImageIO.getReaderFileSuffixes()));
			int returnValue = fileChooser.showOpenDialog(null);
			if (returnValue == JFileChooser.APPROVE_OPTION) {
				if(e.getSource() == editNavImg) map.setNavigationImg(fileChooser.getSelectedFile().getAbsolutePath());
				else map.setFinalNavigationImg(fileChooser.getSelectedFile().getAbsolutePath());
			}
		}
		if(e.getSource() == clearNavImg) map.setNavigationImg("");
		if(e.getSource() == clearFinalNavImg) map.setFinalNavigationImg("");

		if(e.getSource() == markAsStartPoint) {
			System.out.println(map.getEndsPoint().toString());
			System.out.println(selectedImageIndex+"");
			if(map.getEndsPoint().contains(selectedImageIndex+""))
				JOptionPane.showMessageDialog(null,"An end point cannot be a starting point.");
			else {
				map.setStartPoint(selectedImageIndex+"");
				listChanged = true; // update list
			}
		}
		if(e.getSource() == markAsEndPoints) {
			if(map.getStartPoint().equals(selectedImageIndex+""))
				JOptionPane.showMessageDialog(null, "A starting point cannot be an endpoint.");
			else {
				map.getEndsPoint().add(selectedImageIndex+ "");
				listChanged = true; // update list
				Collections.sort(map.getEndsPoint());
			}
		}
		if(e.getSource() == unmarkEndPoints) {
			if(map.getEndsPoint().contains(selectedImageIndex+ ""))
				map.getEndsPoint().remove(selectedImageIndex+ "");
			listChanged = true; // update list
		}
		//--Add/Remove Menu--
		//Picture Manager
		//Add Picture
		if (e.getSource() == addPicture) {
			JFileChooser fileChooser = new JFileChooser();
			fileChooser.setMultiSelectionEnabled(true);
			fileChooser.setAcceptAllFileFilterUsed(false);
			fileChooser.addChoosableFileFilter(new FileNameExtensionFilter(
					"Image files", ImageIO.getReaderFileSuffixes()));
			int returnValue = fileChooser.showOpenDialog(null);
			if (returnValue == JFileChooser.APPROVE_OPTION) {
				File[] files = fileChooser.getSelectedFiles();
				for(File f : files) {
					map.AddPoint(f.getAbsolutePath());
				}
			}
			choose = "";
			listChanged = true;
		}

		//Remove Picture
		if(e.getSource() == removePicture) {
			choose = "";
			img = null;
			try {
				map.RemovePoint(selectedImageIndex);
			}catch (IllegalArgumentException e2) {JOptionPane.showMessageDialog(null, e2.getMessage());}
			listChanged = true; // update list
		}

		//Edit Picture
		if (e.getSource() == editPicture) {
			JFileChooser fileChooser = new JFileChooser();
			fileChooser.setAcceptAllFileFilterUsed(false);
			fileChooser.addChoosableFileFilter(new FileNameExtensionFilter(
					"Image files", ImageIO.getReaderFileSuffixes()));
			int returnValue = fileChooser.showOpenDialog(null);
			if (returnValue == JFileChooser.APPROVE_OPTION) {
				try {
					map.EditPoint(selectedImageIndex,fileChooser.getSelectedFile().getAbsolutePath());
					listChanged = true;
				}catch (IllegalArgumentException e2) {JOptionPane.showMessageDialog(null, e2.getMessage());}
			}
			choose = "";
		}
		//Neighbor Manager
		//Add Neighbor
		if (e.getSource() == addNeighbor)
			choose = addNeiStr;

		//Remove Neighbor
		if (e.getSource() == removeNeighbor) {
			choose = "";
			try {
				map.RemoveNeighbor(selectedImageIndex,Integer.parseInt( JOptionPane.showInputDialog("Neighbor Id :")));
			}catch (IllegalArgumentException e2) {JOptionPane.showMessageDialog(null, e2.getMessage());}
			catch (Exception ex) {ex.printStackTrace();}
		}

		//Edit Neighbor
		if (e.getSource() == editNeighbor) { 
			choose = editNeiStr;
			try {
				neiToEdit = Integer.parseInt( JOptionPane.showInputDialog("Neighbor Id : "));
				if (map.getMap().get(selectedImageIndex).getNeighbors().get(neiToEdit) == null) {
					JOptionPane.showMessageDialog(null, "Neighbor doesn't exist");
					choose = "";
				}
				else JOptionPane.showMessageDialog(null, "Click on the new position");
			} catch (Exception ex) {ex.printStackTrace();}
		}

		//Text Manager
		//Add Text
		if(e.getSource() == addText) { 
			choose = "";
			try {
				String text = JOptionPane.showInputDialog("Insert text :");
				float dur = Float.parseFloat( JOptionPane.showInputDialog("Text duration :"));
				float when = Float.parseFloat( JOptionPane.showInputDialog("When to display the text :"));
				int ind = map.AddOptionalText(selectedImageIndex, text, dur, when);
				textsIDs.add(ind);
				TextListChanged = true;
			}catch (NumberFormatException e1) {JOptionPane.showMessageDialog(null, "Illegal input");}
			catch (IllegalArgumentException ex) {JOptionPane.showMessageDialog(null, ex.getMessage());}
		}
		//Remove Text
		if(e.getSource() == removeText) {
			choose = "";
			try {
				map.RemoveOptionalText(selectedImageIndex, selectedTextIndex);
				textsIDs.remove(textList.getSelectedIndex());
				TextListChanged = true; // update list
			}catch (IllegalArgumentException ex) {JOptionPane.showMessageDialog(null, ex.getMessage());}
		}
		//Edit Text
		if(e.getSource() == editText) {
			try {
				String text = JOptionPane.showInputDialog("Insert text :");
				float dur = Float.parseFloat( JOptionPane.showInputDialog("Text duration :"));
				float when = Float.parseFloat( JOptionPane.showInputDialog("When to display the text :"));
				map.EditOptionalText(selectedImageIndex, selectedTextIndex, text, dur, when);
				TextListChanged = true;
			}catch (NumberFormatException e1) {JOptionPane.showMessageDialog(null, "Illegal input");}
			catch (IllegalArgumentException ex) {JOptionPane.showMessageDialog(null, ex.getMessage());}
		}

		updateDisplay();
		frame.repaint();
	}
}