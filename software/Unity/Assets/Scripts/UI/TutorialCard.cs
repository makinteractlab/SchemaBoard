using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	List<string[]> allPinsList;

	// Use this for initialization
	void Start () {
		index = 0;
		loadCircuitInfo(index);
		cmd = new Command();
		allPinsList = new List<string[]>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void loadCircuitInfo(int _step) {
		// bring components list in the order
		// bring net list in the order
		// pins list to on


	}

	

	void reflectToBreadboardLight(string[] _pins) {
		int[] boardPins = new int[2];

        boardPins = netdata.getMultiplePinsPosition(_pins);
        http.postJson(cmd.getUrl(), cmd.multiPinOnOff(boardPins[0], boardPins[1]));
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
		imageContents.setContent(_componentName, "center");
		// set description text
		tutorialText.text = "Place the" + _componentName;
	}
}
