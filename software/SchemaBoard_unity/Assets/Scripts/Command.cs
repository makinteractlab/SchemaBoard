using UnityEngine;
using System.Collections;

public class Command {//: MonoBehaviour {
    // POST REQUEST
    // http://127.0.0.1:8081/set
    string url = "";// "http://10.0.1.62:8081/set";
    // string url = "http://10.0.1.77:8081/set";
    //ArrayList urls = new ArrayList();

    // public void setUrls() {
    //     for(int i=55; i<65; i++) {
    //         urls.Add("http://10.0.1."+ i + ":8081/set");
    //     }
    // }
    //string url = "http://10.0.1.57:8081/set";

    public string getFile(string _filename) {
        string result = "{\"cmd\": \"getFile\", \"name\": \"" + _filename + "\"}";
        return result;
    }

    public string getSchFile(string _filename) {
        string result = "{\"cmd\": \"getSchFile\", \"name\": \"" + _filename + "\"}";
        return result;
    }

    public string getJsonFile(string _filename) {
        string result = "{\"cmd\": \"getJsonFile\", \"name\": \"" + _filename + "\"}";
        return result;
    }
    
    public void setUrl(string _url) {
        url = _url;
    }

    // public void setUrls(ArrayList _urls) {
    //     urls = new ArrayList(_urls);
    // }

    public string getUrl() {
        return url;
    }

    // public ArrayList getUrls() {
    //     return urls;
    // }
    
    // RESET - ALL OFF
    // {"cmd": "reset"}
    public string resetAll() {
        string result = "{\"cmd\": \"reset\"}";
        return result;
    }

    // SINGLE PIN ON [1-32]
    // {"cmd": "on", "data": 1}
    public string singlePinOn(int _boardLineNumber){
        string result = "";
        result = "{\"cmd\": \"on\", \"data\": " + _boardLineNumber + "}\n";
        return result;
    }

    // SINGLE PIN OFF [1-32]
    // {"cmd": "off", "data": 1}
    public string singlePinOff(int _boardLineNumber){
        string result = "";
        result = "{\"cmd\": \"off\", \"data\": " + _boardLineNumber + "}\n";
        return result;
    }

    // SINGLE PIN TOGGLE [1-32]
    // {"cmd": "toggle", "data": 1}
    public string singlePinToggle(int _boardLineNumber){
        string result = "";
        result = "{\"cmd\": \"toggle\", \"data\": " + _boardLineNumber + "}\n";
        return result;
    }

    // SINGLE PIN BLINK [1-32]
    // {"cmd": "blink", "data": 1}
    public string singlePinBlink(int _boardLineNumber){
        string result = "";
        result = "{\"cmd\": \"blink\", \"data\": " + _boardLineNumber + "}\n";
        return result;
    }

    // SET ALL PINS to either be ON/OFF
    // {"cmd": "set", "left": 0, "right":7}
    public string multiPinOnOff(int _boardLeftLines, int _boardRightLines){
        string result = "";
        result = "{\"cmd\": \"set\", \"left\": " + _boardLeftLines + ", \"right\": " + _boardRightLines + "}\n";
        return result;
    }

    // SET ALL PINS to either be ON/OFF or to BLINK/NO_BLINK
    // {"cmd": "set", "left": 0, "right":7, "leftBlink":65535, "rightBlink":0}
    public string multiPinOnOffBlink(int _boardLeftOnOff, int _boardRightOnOff, int _boardLeftBlink, int _boardRightBlink){
        string result = "";
        result = "{\"cmd\": \"set\", \"left\": " + _boardLeftOnOff + ", \"right\": " + _boardRightOnOff + ", \"leftBlink\": " + _boardLeftBlink + ", \"rightBlink\": " + _boardRightBlink + "}\n";
        return result;
    }
}