package Logic;

public class Optionaltext {
	private int id;
	static int counter = 0;
	private String text;
	private float DurationInSeconds;
	private float whenToDisplay;

	public Optionaltext(String t, float dur, float when) {
		setId((++counter));
		text = t;
		DurationInSeconds = dur;
		whenToDisplay = when;
	}

	public String getText() {
		return text;
	}
	public void setText(String text) {
		this.text = text;
	}
	public float getDurationInSeconds() {
		return DurationInSeconds;
	}
	public void setDurationInSeconds(float durationInSeconds) {
		DurationInSeconds = durationInSeconds;
	}
	public float getWhenToDisplay() {
		return whenToDisplay;
	}
	public void setWhenToDisplay(float whenToDisplay) {
		this.whenToDisplay = whenToDisplay;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String toString()
	{
		String s = "{\"text\" : \"" + text + "\", \n\"DurationInSeconds\" : " + DurationInSeconds + ", \n\"whenToDisplay\" : " + whenToDisplay + "}";
		return s;
	}
}