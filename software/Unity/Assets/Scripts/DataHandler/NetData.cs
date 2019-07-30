using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nuclex.Support.Cloning;

public class NetData : MonoBehaviour {

	public UnityAction<JObject> jsonDataReceivedAction;
	public DataReceivedEvent jsonDataReceivedEvent;
	public UnityAction<JObject> schDataReceivedAction;
	public DataReceivedEvent schDataReceivedEvent;

	public HttpRequest http;
	public Communication comm;
	private Command cmd;
	public Sprite groundPinSprite;
	public Sprite vccPinSprite;
	NetDataHandler netHandler;
	public LoadNetUI netui;
	public LoadSchematicUI schematicUI;
	Dictionary<string, _Component> debugNetData;
	// Dictionary<string, _Component> buildNetData;
	Dictionary<string, _Component> initNetData;

	JObject schematicDrawingData;
	// Use this for initialization
	void Start () {
		//componentsInCircuit = new Dictionary<string, _Component>();
		jsonDataReceivedAction = new UnityAction<JObject>(setSchematicJsonData);
        jsonDataReceivedEvent = new DataReceivedEvent();
        jsonDataReceivedEvent.AddListener(jsonDataReceivedAction);

		schDataReceivedAction = new UnityAction<JObject>(setSchematicDrawingData);
        schDataReceivedEvent = new DataReceivedEvent();
        schDataReceivedEvent.AddListener(schDataReceivedAction);

		cmd = new Command();		
		//cmd.setUrls();

		schematicDrawingData = new JObject();
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	public Dictionary<string, _Component> getInitialSchematicData() {
		return initNetData;
	}

	public Dictionary<string, _Component> getCurrentDebugSchematicData() {
		return debugNetData;
	}

	// public Dictionary<string, _Component> getCurrentBuildSchematicData() {
	// 	return buildNetData;
	// }

	public void readSchematicData(string _fileName, NetDataHandler _netHandler)
	{
		//JObject netData = null;
		netHandler = _netHandler;
		
		// cmd.setUrl("http://10.0.1.77:8081");
		http.getJson(comm.getUrl()+"/get", cmd.getJsonFile(_fileName));
		http.getSch(comm.getUrl()+"/get", cmd.getSchFile(_fileName));
	}

	public void setSchematicDrawingData(JObject _data) {
		schematicUI.dataReceivedEvent.Invoke(_data);
	}

	public void setSchematicJsonData(JObject _data) {	
		debugNetData = new Dictionary<string, _Component>(netHandler.parseNetData(_data));
		//buildNetData = new Dictionary<string, _Component>(netHandler.getBuildNetData());
		initNetData = new Dictionary<string, _Component>(netHandler.getInitNetData());
		netui.dataReceivedEvent.Invoke(debugNetData);
	}

	// public void setAutoNetData(Dictionary<string, _Component> _data) {
	// 	buildNetData.Clear();
	// 	buildNetData = SerializationCloner.DeepFieldClone(_data);
	// }

	public void setInitialNetData() {
		debugNetData.Clear();
		debugNetData = SerializationCloner.DeepFieldClone(initNetData);
	}

	// public void copyAutoDataToManualData(){
	// 	setManualNetData(buildNetData);
	// }

	// public void copyManualDataToAutoData(){
	// 	setAutoNetData(buildNetData);
	// }

	// public void resetBuildNetData() {
	// 	buildNetData.Clear();
	// 	buildNetData = SerializationCloner.DeepFieldClone(initNetData);
	// }

	// public void resetdebugNetData() {
	// 	debugNetData.Clear();
	// 	debugNetData = SerializationCloner.DeepFieldClone(initNetData);
	// }

	public void syncNetData(string _componentName, string _componentPinName, string _boardPinName) {
		string pin = _componentPinName.Substring(_componentPinName.IndexOf('-')+1, _componentPinName.Length-_componentPinName.IndexOf('-')-1);

		// if(comm.getAutoState()) {		
			if(_boardPinName.Contains("init")) {
				debugNetData[_componentName].getPin(pin).breadboardRowPosition = "init";
			} else {
				debugNetData[_componentName].getPin(pin).breadboardRowPosition = Util.getChildObject(GameObject.Find(_boardPinName), "row").GetComponent<Text>().text;
				debugNetData[_componentName].getPin(pin).breadboardColPosition = Util.getChildObject(GameObject.Find(_boardPinName), "column").GetComponent<Text>().text;
			}
		// } else {
		// 	if(_boardPinName.Contains("init")) {
		// 		buildNetData[_componentName].getPin(pin).breadboardRowPosition = "init";
		// 	} else {
		// 		buildNetData[_componentName].getPin(pin).breadboardRowPosition = Util.getChildObject(GameObject.Find(_boardPinName), "row").GetComponent<Text>().text;
		// 		buildNetData[_componentName].getPin(pin).breadboardColPosition = Util.getChildObject(GameObject.Find(_boardPinName), "column").GetComponent<Text>().text;
		// 	}
		// }

		// debugNetData.ToList().ForEach(x => Debug.Log("debugNetData: " + x.Key + " pin1: " + x.Value.getPins()[0].breadboardRowPosition + " , pin2: " + x.Value.getPins()[1].breadboardRowPosition)); // debug
	}

	// public void setInitNetData(Dictionary<string, _Component> _componentsInCircuit) {
	// 	initNetData = new Dictionary<string, _Component>(_componentsInCircuit);
	// }
	
	public void recoverEmptyPosForPins() {
		foreach(var item in debugNetData) {
			foreach(var pin in item.Value.getPins()){
				if(pin.breadboardRowPosition == "init") {
					pin.breadboardRowPosition = initNetData[item.Key].getPin(pin.id).breadboardRowPosition;
					pin.breadboardColPosition = initNetData[item.Key].getPin(pin.id).breadboardColPosition;
				}
			}
		}
	}

	public Dictionary<string, List<string[]>> getComponentsAndPinsPosition() {
		Dictionary<string, List<string[]>> result = new Dictionary<string, List<string[]>>();
		List<string[]> pinsPosition;
		List<string[]> temp = new List<string[]>();
		foreach(var item in debugNetData) {
			foreach(var pin in item.Value.getPins()){
				temp.Add(new string[]{pin.id, pin.breadboardRowPosition});
			}
			pinsPosition = new List<string[]>(temp);
			result.Add(item.Key, pinsPosition);
			temp.Clear();
		}
		return result;
	}

	// public Dictionary <string, Dictionary<string,string>> getConnectedComponentAndPinsPosition(string _component, string _pin) {
	// 	Dictionary <string, Dictionary<string,string>> result = new Dictionary <string, Dictionary<string,string>>();
	// 	Dictionary<string,string> connectedComponentsPins = new Dictionary<string,string>();
	// 	foreach(var item in buildNetData) {
	// 		if(item.Key.Contains(_component)) {
	// 			foreach(var element in item.Value.getPin(_pin).getNetElementAll()) {
	// 				getBreadboardPosition(element.component,element.pinid);
	// 				connectedComponentsPins.Add(element.component, getBreadboardPosition(element.component,element.pinid));
	// 			}
	// 			result.Add("net:"+_component+"-"+_pin, );
	// 		}			
	// 	}
	// 	return result;
	// }

	public List<string> getComponentPosition(string _component) {
		List<string> resultPins = new List<string>();

		// if(comm.getAutoState()) {
			if(debugNetData.ContainsKey(_component)) {
				foreach(var item in debugNetData[_component].getPins()){
					resultPins.Add(item.breadboardRowPosition);
				}
			}
		// } else {
		// 	foreach(var item in buildNetData[_component].getPins()){
		// 		resultPins.Add(item.breadboardRowPosition);
		// 	}
		// }
		return resultPins;
	}
	
	public int[] getComponentPinsNet(string _component) {
		List<string> resultPins = new List<string>();
		int left = 0;
		int right = 1;
		int[] result = Enumerable.Repeat(0, 2).ToArray();
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();

		// if(comm.getAutoState()) {
		if(debugNetData.ContainsKey(_component)) {
			foreach(var item in debugNetData[_component].getPins()){
				resultPins.Add(item.breadboardRowPosition);
			}
		
			// } else {
			// 	foreach(var item in buildNetData[_component].getPins()){
			// 		resultPins.Add(item.breadboardRowPosition);
			// 	}
			// }

			foreach(var item in resultPins) {
				if(item.Contains("init")) continue;
				else boardBinary[int.Parse(item)-1] = '1';
			}

			for(int i=0; i<16; i++)
				if(boardBinary[i] == '1') result[left] += (int)Math.Pow(2, i);

			for(int i=16; i<32; i++)
				if(boardBinary[i] == '1') result[right] += (int)Math.Pow(2, i-16);
		}

		return result;
	}

	public string getComponentSinglePinRowPosition(string _component, string _pin) {
		string result = "";
		// if(comm.getAutoState()) {
		if(debugNetData.ContainsKey(_component)) {
			result = debugNetData[_component].getPin(_pin).breadboardRowPosition;
		}
		// } else {
		// 	result = buildNetData[_component].getPin(_pin).breadboardRowPosition;
		// }
		return result;
	}

	public string getComponentFirstPinRowPosition(string _component) {
		string result = "";
		// if(comm.getAutoState()) {
		if(debugNetData.ContainsKey(_component)) {
			result = debugNetData[_component].getFirstPin().breadboardRowPosition;
		}
		// } else {
		// 	result = buildNetData[_component].getFirstPin().breadboardRowPosition;
		// }
		return result;
	}

	// public void addToResultPins(ref List<string> resultPins, string _component, string _pin) {

	// 	if(componentsInCircuit[_component].getPin(_pin).netElements.Count < 1) return;

	// 	foreach(var element in componentsInCircuit[_component].getPin(_pin).netElements) {	
	// 		resultPins.Add(componentsInCircuit[element.component].getPin(element.pinid).breadboardPosition);
	// 		addToResultPins(ref resultPins, element.component, element.pinid);
	// 	}
	// }

	public int[] getMultiplePinsPosition(List<string> _pins) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		foreach(var pin in _pins) {
			if(pin.Contains("init")) continue;
			else boardBinary[int.Parse(pin)-1] = '1';
		}

		for(int i=0; i<16; i++)
			if(boardBinary[i] == '1') result[left] += (int)Math.Pow(2, i);

		for(int i=16; i<32; i++)
			if(boardBinary[i] == '1') result[right] += (int)Math.Pow(2, i-16);

		return result;
	}

	public int[] getAllNetForPin(string _component, string _pin, Sprite _selectedPinSprite) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string> resultPins = new List<string>();

		if(debugNetData.ContainsKey(_component)) {
			// component pin의 net element에 들어있는 컴포넌트 핀의 breadboard pin 가져오기
			foreach(var element in debugNetData[_component].getPin(_pin).netElementsAll) {	
				resultPins.Add(debugNetData[element.component].getPin(element.pinid).breadboardRowPosition);
				GameObject pin = Util.getChildObject(element.component, element.pinid);
				// pin.GetComponent<Button>().image.sprite = _selectedPinSprite;
				pin.GetComponent<Image>().sprite = _selectedPinSprite;
			}

			foreach(var item in resultPins) {
				if(item.Contains("init")) continue;
				else boardBinary[int.Parse(item)-1] = '1';
			}

			for(int i=0; i<16; i++)
				if(boardBinary[i] == '1') result[left] += (int)Math.Pow(2, i);

			for(int i=16; i<32; i++)
				if(boardBinary[i] == '1') result[right] += (int)Math.Pow(2, i-16);
		} else {
			result = Enumerable.Repeat(-1, 2).ToArray();
		}

		return result;
	}

	public int[] getAllNetForPin(string _component, string _pin) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string> resultPins = new List<string>();

		// if(comm.getAutoState()) {
			//resultPins.Add(debugNetData[_component].getPin(_pin).breadboardRowPosition);

		if(debugNetData.ContainsKey(_component)) {
			// component pin의 net element에 들어있는 컴포넌트 핀의 breadboard pin 가져오기
			foreach(var element in debugNetData[_component].getPin(_pin).netElementsAll) {	
				resultPins.Add(debugNetData[element.component].getPin(element.pinid).breadboardRowPosition);
			}
			// } else {
			// 	resultPins.Add(buildNetData[_component].getPin(_pin).breadboardRowPosition);

			// 	// component pin의 net element에 들어있는 컴포넌트 핀의 breadboard pin 가져오기
			// 	foreach(var element in buildNetData[_component].getPin(_pin).netElementsAll) {	
			// 		resultPins.Add(buildNetData[element.component].getPin(element.pinid).breadboardRowPosition);
			// 	}
			// }

			foreach(var item in resultPins) {
				if(item.Contains("init")) continue;
				else boardBinary[int.Parse(item)-1] = '1';
			}

			for(int i=0; i<16; i++)
				if(boardBinary[i] == '1') result[left] += (int)Math.Pow(2, i);

			for(int i=16; i<32; i++)
				if(boardBinary[i] == '1') result[right] += (int)Math.Pow(2, i-16);
		} else {
			result = Enumerable.Repeat(-1, 2).ToArray();
		}

		return result;
	}

	public void setColorGroundPins(string _component, string _pin) {
		if(initNetData.ContainsKey(_component)) {
			foreach(var element in initNetData[_component].getPin(_pin).netElementsAll) {
				GameObject groundPin = Util.getChildObject(element.component, element.pinid);
				groundPin.GetComponent<Button>().image.sprite = groundPinSprite;
			}
		}
	}

	public void setColorVccPins(string _component, string _pin) {
		if(initNetData.ContainsKey(_component)) {
			foreach(var element in initNetData[_component].getPin(_pin).netElementsAll) {
				GameObject groundPin = Util.getChildObject(element.component, element.pinid);
				groundPin.GetComponent<Button>().image.sprite = vccPinSprite;
			}
		}
	}

/* Recursive fuction version */
/*
	public void travelAllComponent(ref List<string> resultPins, string _component, string _pin) {
		//if(componentsInCircuit[_component].getPin(_pin).netElements.Count < 1) return;
		foreach(var component in componentsInCircuit) {
			foreach(var item in componentsInCircuit[component.Key].getPins()) {
				foreach(var element in item.netElements) {
					if(element.component.Contains(_component) && element.pinid.Contains(_pin)) {
						resultPins.Add(item.breadboardPosition);
						travelAllComponent(ref resultPins, component.Key, item.id);
					}
				}
			}
		}
	}

	public int[] getAllNetForPin(string _component, string _pin) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string> resultPins = new List<string>();
		resultPins.Add(componentsInCircuit[_component].getPin(_pin).breadboardPosition);

		// component pin의 net element에 들어있는 컴포넌트 핀의 breadboard pin 가져오기
		foreach(var element in componentsInCircuit[_component].getPin(_pin).netElements) {	
			resultPins.Add(componentsInCircuit[element.component].getPin(element.pinid).breadboardPosition);
			foreach(var item in componentsInCircuit[element.component].getPin(element.pinid).netElements) {
				resultPins.Add(componentsInCircuit[item.component].getPin(item.pinid).breadboardPosition);
			}
		}
		//recursive function이 필요한가?
		//addToResultPins(ref resultPins, _component, _pin);

		// component pin을 net element로 가지고 있는 Component pin들의 breadboard pin 가져오기
		travelAllComponent(ref resultPins, _component, _pin);
		
		//foreach(var item in componentsInCircuit[_component].getPins()) {
		// foreach(var component in componentsInCircuit) {
		// 	foreach(var item in componentsInCircuit[component.Key].getPins()) {
		// 		foreach(var element in item.netElements) {
		// 			if(element.component.Contains(_component) && element.pinid.Contains(_pin))
		// 				resultPins.Add(item.breadboardPosition);
		// 		}
		// 	}
		// }

		foreach(var item in resultPins) {
			if(item.Contains("init")) continue;
			else boardBinary[int.Parse(item)-1] = '1';
		}

		for(int i=0; i<16; i++)
			if(boardBinary[i] == '1') result[left] += (int)Math.Pow(2, i);

		for(int i=16; i<32; i++)
			if(boardBinary[i] == '1') result[right] += (int)Math.Pow(2, i-16);

		return result;
	}
	*/

	public string getComponentPowerNet(string _component, string _pin) {
		string result = "";
		return result;
	}
}
