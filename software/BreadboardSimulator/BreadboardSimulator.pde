import http.*;

SimpleHTTPServer server;
BreadBoard br;
final int PIN_SIZE= 40;
final int FONT_SIZE= 20;


void setup() 
{
	size(600, 700); 
	initServer();

	br= new BreadBoard (PIN_SIZE);
}


void draw()
{
	background(255);
	br.draw();
}



void initServer()
{
	// Create a server listening on port 8081
  	server = new SimpleHTTPServer(this, 8081); 
  	DynamicResponseHandler postResponder = new DynamicResponseHandler(new PostRequest(), "application/json");
  	DynamicResponseHandler getResponder = new DynamicResponseHandler(new GetRequest(), "application/json");

  	server.createContext("set", postResponder);
  	server.createContext("get", getResponder);
}



class PostRequest extends ResponseBuilder {
  
  public String getResponse(String requestBody) 
  {
    JSONObject json = parseJSONObject(requestBody);
    // println(json);

    JSONObject res= new JSONObject();


    if (json.getString("cmd").equals("on"))
    	br.setOn(json.getInt("data"));
    else if (json.getString("cmd").equals("off"))
    	br.setOff(json.getInt("data"));
    else if (json.getString("cmd").equals("toggle"))
    	br.toggle(json.getInt("data"));
    else if (json.getString("cmd").equals("blink"))
    	br.blink(json.getInt("data"));
    else if (json.getString("cmd").equals("reset"))
    	br.reset();
    else if (json.getString("cmd").equals("set"))
    {
      int left = json.getInt("left",0);
      int right = json.getInt("right",0);
      int leftBlink = json.getInt("leftBlink",0);
      int rightBlink = json.getInt("rightBlink",0);
      if (leftBlink==0 && rightBlink==0)
        br.set (left, right);
      else
        br.set (left, right, leftBlink, rightBlink);
    }
    else
    {
      res.setString("ack", "Wrong command");
      return res.toString();
    }

    // all ok
    res.setString("ack", "ok");
    return res.toString();
  }
}


class GetRequest extends ResponseBuilder {
  
  public String getResponse(String requestBody) 
  {
    JSONObject json = parseJSONObject(requestBody);
    JSONObject res= new JSONObject();

    if (json.getString("cmd").equals("getFile"))
    {
    	String filename= json.getString("name");
    	File f = new File(dataPath(filename));
  		if(!f.exists()) return "";
  
      if(filename.contains("xml")) {
  		  XML xml= loadXML(filename);
        println("xml: " + filename);
  		  return xml.toString();
      } else if(filename.contains("json")) {
        println("json: " + filename);
        res = loadJSONObject(filename);
        println(res.toString());
        return res.toString();
      }
      
    }
    return "";
  }
}
