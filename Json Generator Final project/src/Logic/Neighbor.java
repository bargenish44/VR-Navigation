package Logic;


public class Neighbor {
	private int FromPointID;
	private int ToPointID;
	private float Azimut;

	public Neighbor(int from, int to , float az) {
		FromPointID = from;
		ToPointID = to;
		Azimut = az;
	}
	public int getFromPointID() {
		return FromPointID;
	}
	public void setFromPointID(int fromPointID) {
		FromPointID = fromPointID;
	}
	public int getToPointID() {
		return ToPointID;
	}
	public void setToPointID(int toPointID) {
		ToPointID = toPointID;
	}
	public float getAzimut() {
		return Azimut;
	}
	public void setAzimut(float azimut) {
		Azimut = azimut;
	}
	public String toString()
	{
		String s = "{ \"PointID\" :" + ToPointID + ",\n \"Azimut\" : " + Azimut + "}";
		return s;
	}
}