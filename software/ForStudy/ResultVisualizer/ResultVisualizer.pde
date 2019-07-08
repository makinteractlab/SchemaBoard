
float MAX_VALUE= 60*15;
String [] build = {"P2","P3","P4","P5"};
String [] debug = {"P2","P3","P4","P5"};

color FRITZING = color(169, 242, 236);  //skyblue
color SCHEMATIC = color(221, 183, 247);
color FINISH= color(255, 0, 255);
//color SW_PROGRAMMING= color(221, 183, 247);

//color HW_TEST= color(66, 203, 244);
//color SW_TEST= color(215, 124, 255);

//color SW_DEBUGGING= color(203, 84, 255);
//color HW_DEBUGGING= color(71, 129, 255);

//// only in arduino
//color SW_COMPILE= color(255, 140, 233);
//color SW_DOWNLOADING= color(255, 30, 212);

//color HW_MODIFYING = color(159, 152, 226);


int USERS= 4;

float lineHeight;
int lineWidth;
int spacer= 50;


PFont font;


void setup()
{
	size (1800, 300);
	noLoop();
	lineHeight= height/2/USERS;
	lineWidth= width-spacer;
	font = loadFont("font.vlw");

	int [] timeSlots = {0, 10*60, 20*60, 30*60, 40*60}; // minutes in seconds

	// build
	// get data (inefficinetly)
	ArrayList<User> users= new ArrayList<User>();
	for (String user: build)
	{
		users.add (new User (user, loadStrings("build.csv")));
	}

	// draw
	background(255);
	// DRAW BAKGROUND
	stroke(200);
	for (int i=0; i<timeSlots.length; i++)
	{
		float x= spacer + timeSlots[i] / MAX_VALUE * lineWidth;
		line (x, 0, x, height);
	}

	noStroke();
	for (int i=0; i<users.size(); i++)
	{
		drawUser(users.get(i), spacer, (lineHeight*2)*i, lineHeight+5);
	}
	save("build.png");



	// debug

	// get data (inefficinetly)
	users= new ArrayList<User>();
	for (String user: debug)
	{
		users.add (new User (user, loadStrings("debug.csv")));
	}

	// draw
	background(255);
	background(255);
	// DRAW BAKGROUND
	stroke(200);
	for (int i=0; i<timeSlots.length; i++)
	{
		float x= spacer + timeSlots[i] / MAX_VALUE * lineWidth;
		line (x, 0, x, height);
	}
	noStroke();
	for (int i=0; i<users.size(); i++)
	{
		drawUser(users.get(i), spacer, (lineHeight*2)*i, lineHeight+5);
	}
	save("debug.png");


	// done
	System.exit(0);
}



void drawUser(User u, float x, float y, float height)
{
	
	textFont(font, 20);
	fill(0);
	textAlign(LEFT, CENTER);
	text(u.name, 0, y+height/2);


	for (Data d: u.data)
	{
		float start= d.start / MAX_VALUE * lineWidth;
		float duration= d.duration / MAX_VALUE * lineWidth;
		color c= d.col;
		fill(c);
		rect (x+start, y, duration, height);
	}
}



class User
{
	String name;
	ArrayList<Data> data;

	User (String name, String[] allData)
	{
		data= new ArrayList<Data>();
		this.name= name;

		for (String s: allData)
		{
			String d[] = splitTokens(s, ",");
	
			if (d[0].equals(name))
			{
				int start= parseInt(d[1])/1000;
				int duration= parseInt(d[2])/1000;
				String type= d[3];
				data.add (new Data (start, duration, type));
			} 
		}
	}
}

class Data
{	
	public color col;
	public int start, duration;

	Data(int s, int d, String type)
	{
		start= s;
		duration= d;
		col= 0; // default black

		if (type.equals("fritzing")) col= FRITZING;
		else if (type.equals("schematic")) col= SCHEMATIC; 
		//else if (type.equals("HW_TEST")) col= HW_TEST; 
		//else if (type.equals("SW_PROGRAMMING")) col= SW_PROGRAMMING; 
		//else if (type.equals("SW_TEST")) col= SW_TEST; 
		//else if (type.equals("SW_DEBUGGING")) col= SW_DEBUGGING; 
		//else if (type.equals("HW_DEBUGGING")) col= HW_DEBUGGING;
  //  else if (type.equals("SW_COMPILE")) col=SW_COMPILE;
  //  else if (type.equals("SW_DOWNLOADING")) col=SW_DOWNLOADING;
  //  else if (type.equals("HW_MODIFYING")) col=HW_MODIFYING;
	}
}
