import controlP5.*;
ControlP5 cp5;

int startTime= 0;
PrintWriter pw;
String fileName;
PFont font;
int counter=0;

PImage fritzingImg;
PImage schematicImg;
PImage imageToDisplay;

boolean schematicToFritzing;
String taskType;
String circuitType;
String selectedRefType;

boolean started;
ButtonBar selectCircuitType;
ButtonBar selectTaskType;
ButtonBar selectRefType;
ButtonBar startButtonBar;
ButtonBar finishedButtonBar;
PImage btfritz;
PImage btschematic;

void setup() {
  size(1440, 720);
  font = createFont("Georgia", 70);
  textFont(font);
  textAlign(CENTER, CENTER);
  started = false;
  //fritzingImg = loadImage("F146.jpg");
  //schematicImg = loadImage("S146.jpg");
  //imageToDisplay = schematicImg;
  btschematic = loadImage("btSchematic.png");
  btfritz = loadImage("btFritzing.png");
  
  selectInput("Select a file to process:", "fileSelected");
  showMenu();
}

void draw() {
  if(started) {
    image(imageToDisplay, 0, 0, imageToDisplay.width, imageToDisplay.height);
  } else {
    background(220);
    image(btschematic, 520, 370, btschematic.width*0.72, btschematic.height*0.72);
    image(btfritz, 720, 370, btfritz.width*0.72,btfritz.height*0.72);
  }
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

void showMenu()
{
  cp5 = new ControlP5(this);
  selectCircuitType = cp5.addButtonBar("circuitTypeBar")
  .setPosition(520,180)
  .setSize(400,50)
  .addItems(split("146 810 2971 3351", " "));
  
  selectCircuitType.onMove(new CallbackListener() {
    public void controlEvent(CallbackEvent ev) {
      ButtonBar circuitTypeBar = (ButtonBar)ev.getController();
    }
  });

  selectTaskType = cp5.addButtonBar("taskTypeBar")
  .setPosition(520,250)
  .setSize(400,50)
  .addItems(split("Build Debug", " "));
  
  selectTaskType.onMove(new CallbackListener() {
    public void controlEvent(CallbackEvent ev) {
      ButtonBar taskTypeBar = (ButtonBar)ev.getController();
    }
  });
  
  selectRefType = cp5.addButtonBar("refTypeBar")
  .setPosition(520,320)
  .setSize(400,50)
  .addItems(split("Schematic Fritzing", " "));
  
  selectRefType.onMove(new CallbackListener() {
    public void controlEvent(CallbackEvent ev) {
      ButtonBar refTypeBar = (ButtonBar)ev.getController();
    }
  });
  
  startButtonBar = cp5.addButtonBar("startTaskBar")
     .setPosition(620,580)
     .setSize(200,80)
     .addItems(split("Start the task", ":"));
  
  startButtonBar.onMove(new CallbackListener() {
    public void controlEvent(CallbackEvent ev) {
      ButtonBar startTaskBar = (ButtonBar)ev.getController();
    }
  });
  
  finishedButtonBar = cp5.addButtonBar("finishTaskBar")
     .setPosition(1240,0)
     .setSize(200,50)
     .addItems(split("Done", " "));
  
  finishedButtonBar.onMove(new CallbackListener() {
    public void controlEvent(CallbackEvent ev) {
      ButtonBar finishTaskBar = (ButtonBar)ev.getController();
    }
  });
  
  finishedButtonBar.hide();
}

void finishTaskBar(int _selected) {
  pw.println(circuitType + "," + taskType + ",EndTask," + (millis()-startTime));
  pw.flush();
  started = false;
  exit();
}

void startTaskBar(int _selected) {
  started = true;
  startTime = millis();
  selectCircuitType.hide();
  selectTaskType.hide();
  selectRefType.hide();
  startButtonBar.hide();
  finishedButtonBar.show();
  pw.println(circuitType + "," + taskType + ","+ selectedRefType + "," + (millis()-startTime));
  pw.flush();
}

void refTypeBar(int _refType) {
  switch(_refType) {
    case 0:  // schematic selected
    schematicToFritzing = true;
    selectedRefType = "schematic";
    imageToDisplay = schematicImg;
    break;
    case 1:
    schematicToFritzing = false;
    selectedRefType = "fritzing";
    imageToDisplay = fritzingImg;
    break;
  }
  //started = true;
  //startTime = millis();
}

void circuitTypeBar(int _circuitType) {
  switch(_circuitType) {
    case 0:
    circuitType = "146";
    break;
    case 1:
    circuitType = "810";
    break;
    case 2:
    circuitType = "2971";
    break;
    case 3:
    circuitType = "3351";
    break;
  }
  String fritzingFileName = "F"+circuitType+".jpg";
  String schematicFileName = "S"+circuitType+".jpg";
  fritzingImg = loadImage(fritzingFileName);
  schematicImg = loadImage(schematicFileName);
  println("circuitTypeBar clicked, item-value: " + circuitType);
}

void taskTypeBar(int _taskType) {
  switch(_taskType) {
    case 0:
    taskType = "Build";
    break;
    case 1:
    taskType = "Debug";
    break;
  }
  println("taskTypeBar clicked, item-value: " + taskType);
}

void mousePressed()
{
  if(started) {
    if(mouseX>1240 && mouseX<1440 && mouseY>0 && mouseY<50)
    {
      // done button area.
    } else {
      if(schematicToFritzing) {
        imageToDisplay = fritzingImg;
        schematicToFritzing = false; // for the next round
        pw.println(circuitType + "," + taskType + ",fritzing," + (millis()-startTime));
      } else {
        imageToDisplay = schematicImg;
        schematicToFritzing = true; // for the next round
        pw.println(circuitType + "," + taskType + ",schematic," + (millis()-startTime));
      }
      pw.flush();
    }
  }
}
