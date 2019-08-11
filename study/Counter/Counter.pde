int startTime= 0;
PrintWriter pw;
String fileName;
PFont font;
int counter=0;

void setup() {
	size(400, 400);	
	font = createFont("Georgia", 70);
  	textFont(font);
	textAlign(CENTER, CENTER);

	selectInput("Select a file to process:", "fileSelected");
}

void draw() 
{
	if (startTime > 0)
	{
		background(0, 255, 0);
		text(counter, width/2, height/2);

	}else{
		background(255,0,0);
	}
	fill(255);
}


void fileSelected(File selection) {
  if (selection == null) {
    println("Window was closed or the user hit cancel.");
    exit();
  } else {
    fileName= selection.getAbsolutePath();
    println(fileName);
    pw= createWriter (fileName);
  }
}

void mousePressed()
{
	counter++;
	pw.println(counter+","+(millis()-startTime));
	pw.flush();
}

void keyPressed()
{
	if (startTime>0) return;
	if (key == ' ') startTime= millis();
}