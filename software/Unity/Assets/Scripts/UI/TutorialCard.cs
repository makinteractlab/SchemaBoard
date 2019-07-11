using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialCard : MonoBehaviour {

	public TutorialImage imageContents;
	public Text tutorialTitle;
	public Text tutorialText;
	public Button prevButton;
	public Button nextButton;
	public NetData netdata;
	public HttpRequest http;
	private Command cmd;
	private int index;
	Dictionary<string, List<string[]>> tutorialData;

	List<string> resultPins;
	Dictionary<int, string> steps;
	List<List<string>> layers; // layer
	List<string> layer; // step, pins[]
	int stepsCount;

	// Use this for initialization
	void Start () {
		index = 1;
		// loadCircuitInfo(index);
		cmd = new Command();
		cmd.setUrls();
		tutorialData = new Dictionary<string, List<string[]>>();
		resultPins = new List<string>();
		steps = new Dictionary<int, string>();
		layers = new List<List<string>>();
		layer = new List<string>();

		prevButton.GetComponent<Button>().onClick.AddListener(prevButtonClick);
		nextButton.GetComponent<Button>().onClick.AddListener(nextButtonClick);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void prevButtonClick() {
		index --;
		if (index <= 1) {
			index = 1;
			prevButton.GetComponent<Button>().image.transform.localScale = new Vector3(0,0,0);
			nextButton.GetComponent<Button>().image.transform.localScale = new Vector3(1,1,1);
		} else {
			prevButton.GetComponent<Button>().image.transform.localScale = new Vector3(1,1,1);
			nextButton.GetComponent<Button>().image.transform.localScale = new Vector3(1,1,1);
		}

		loadCircuitInfo(index);
	}

	void nextButtonClick() {
		index ++;
		if(index >= stepsCount){
			index = stepsCount;
			nextButton.GetComponent<Button>().image.transform.localScale = new Vector3(0,0,0);
			prevButton.GetComponent<Button>().image.transform.localScale = new Vector3(1,1,1);
		} else {
			nextButton.GetComponent<Button>().image.transform.localScale = new Vector3(1,1,1);
			prevButton.GetComponent<Button>().image.transform.localScale = new Vector3(1,1,1);
		}

		loadCircuitInfo(index);
	}

// tutorialData
// Dictionary <key: Component Name, value: list [component's pin id, breadboard position]>
// ex: resistor <R1, [connector0, 12] [connector1, 22]>

// R1, connector0, 12
// R1, connector1, 22
// U1, connector0, 2
// U1, connector1, 4
// U1, connector2, 10

// let's make step's info
// ex step 1 R1
	public void loadCircuitInfo(int _step) {

		foreach(var item in netdata.getComponentsAndPinsPosition()) {
			if(!tutorialData.ContainsKey(item.Key)) {
				tutorialData.Add(item.Key, item.Value);
				steps.Add(index, item.Key);
				foreach(var elements in item.Value) {
					layer.Add(index.ToString());
					layer.Add(elements[1]);
					layers.Add(layer);
				}
				index ++;
			}
		}
		
		setContents(_step, steps[_step]);

		foreach(var item in layers) {
			if(Int32.Parse(item[0]) == _step) {
				for(int i=1; i<item.Count; i++) {
					resultPins.Add(item[i]);
					//Debug.Log("=====================> resultPins["+i+"] = " + item[i]);
				}
			}
		}

		reflectToBreadboardLight(resultPins);

		// foreach(var item in netdata.getComponentsAndPinsPosition()) {
		// 	foreach(var pin in item.Value) {
		// 		// pin의 net에 있는 component와 연결된 pin 가져오기
		// 		foreach(var element in netdata.getConnectedComponentAndPinsPosition(item.Key, index.ToString())) {
		// 			tutorialData.Add(element.Key, element.Value);
		// 		}
		// 		index ++;
		// 	}
		// }

		stepsCount = tutorialData.Count;
		resultPins.Clear();
	}

	void reflectToBreadboardLight(List<string> _breadboardpins) {
		int[] boardPins = new int[2];

        boardPins = netdata.getMultiplePinsPosition(_breadboardpins);
        //http.postJson(cmd.getUrl(), cmd.multiPinOnOff(boardPins[0], boardPins[1]));
		ArrayList urls = new ArrayList(cmd.getUrls());
        foreach(var url in urls) {
            http.postJson((string)url, cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        }
	}

	void setContents(int _step, string _leftCompName, string _rightCompName, int _leftCompPin, int _rightCompPin) {	// _type: component or wire
		// set title
		tutorialTitle.text = "Step " + _step.ToString();
		// set content image
		imageContents.setContent(_leftCompName, "left", _leftCompPin);
		imageContents.setContent(_rightCompName, "right", _rightCompPin);
		imageContents.setContent("wire", "center", -1);
		// set description text
		tutorialText.text = "Connect between " + _leftCompName + "'s pin " + _leftCompPin.ToString() + " and " + _rightCompName + "'s pin " + _rightCompPin.ToString();
	}

	void setContents(int _step, string _componentName) {	// _type: component or wire
		// set title
		tutorialTitle.text = "Step " + _step.ToString();
		// set content image
		
		// set description text
		string componentName = "";
		string name = Util.removeDigit(_componentName);
		Debug.Log("orig name = " + _componentName);
		Debug.Log("name = " + name);

		imageContents.setContent(name, "center");

		switch(name) {
			case "VCC":
			case "BT":
				componentName = "battery";
				break;
			case "R":
				componentName = "resistor";
				break;
			case "C":
				componentName = "capacitor";
				break;
			case "LED":
				componentName = "led";
				break;
			case "S":
				componentName = "button";
				break;
			case "Q":
				componentName = "transistor";
				break;
			case "D":
				componentName = "diode";
				break;
			case "L":
				componentName = "inductor";
				break;
			case "LDR":
				componentName = "photoresistor";
				break;
			case "J":
				componentName = "piezo";
				break;
			case "SP":
				componentName = "speaker";
				break;
			case "U":
				componentName = "chipset";
				break;
			case "wire":
				componentName = "chipset";
				break;
			default:
				componentName = "component";
				break;
		}
		tutorialText.text = "Place the " + componentName;
	}
}
