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
	public ToggleAutoManual modeToggle;
	public Dictionary<string, _Component> debugNetData;
	// Dictionary<string, _Component> buildNetData;
	Dictionary<string, _Component> initNetData;
	Dictionary<string, List<string>> bbNetPosition = new Dictionary<string, List<string>>();
	List<string> occupiedRows;
	string bbPinNetName;

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
		bbPinNetName = "";
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	public List<List<string[]>> getAllNetList() {
		return netHandler.getAllNet();
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

	public List<string[]> getGndNet() {
		return netHandler.getGndNetList();
	}

	public List<string[]> getPwrNet() {
		return netHandler.getPwrNetList();
	}

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
		findOccupiedRows();
		setNetbbPins();
	}

	// public void setAutoNetData(Dictionary<string, _Component> _data) {
	// 	buildNetData.Clear();
	// 	buildNetData = SerializationCloner.DeepFieldClone(_data);
	// }

	public void setInitialNetData() {
		debugNetData.Clear();
		debugNetData = SerializationCloner.DeepFieldClone(initNetData);
	}

	public void syncNetData(string _componentName, string _componentPinName, string _boardPinName) {
		string pin = _componentPinName.Substring(_componentPinName.LastIndexOf('-')+1, _componentPinName.Length-_componentPinName.LastIndexOf('-')-1);

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

	public void findOccupiedRows() {
		occupiedRows = new List<string>();
		foreach(var item in debugNetData) {
			foreach(var pin in item.Value.getPins()) {
				if(!occupiedRows.Contains(pin.breadboardRowPosition))
					occupiedRows.Add(pin.breadboardRowPosition);
			}	
		}
	}
	
	public void setNetbbPins() {
		List<List<string[]>> allNetList = getAllNetList();
		string netName = "";
		foreach(var net in allNetList) {
			List<string> bbPins = new List<string>();
			
			foreach(var element in net) {
				netName = element[0];
				//if(!bbPins.Contains(debugNetData[element[1]].getPin(element[2]).breadboardRowPosition))
				bbPins.Add(debugNetData[element[1]].getPin(element[2]).breadboardRowPosition);
			}
			if(bbNetPosition.ContainsKey(netName)) {
				List<string> pins = bbNetPosition[netName];
				pins.AddRange(bbPins);
				bbNetPosition.Remove(netName);
			}
			bbNetPosition.Add(netName, bbPins);
		}
	}

/*
	public string getNetName(string _wirename) {
		string result = "";

		if(_wirename != "") {
			string first = _wirename.Substring(_wirename.IndexOf(':')+1,_wirename.IndexOf(',')-_wirename.IndexOf(':')-1);
			string last = _wirename.Substring(_wirename.IndexOf(',')+1, _wirename.Length-_wirename.IndexOf(',')-1);
			string firstComponentName = first.Substring(0,first.LastIndexOf('-'));
			string lastComponentName = last.Substring(0,last.LastIndexOf('-'));
			string component = "";
			string pin = "";
			if(firstComponentName.Contains("Pin")) {
				component = lastComponentName;
				pin = last.Substring(last.LastIndexOf('-')+1, last.Length-last.LastIndexOf('-')-1);
			} else {
				component = firstComponentName;
				pin = first.Substring(first.LastIndexOf('-')+1, first.Length-first.LastIndexOf('-')-1);
			}
			
			List<List<string[]>> allNetList = getAllNetList();
			//{netName, item["component"].ToString(), item["pin"].ToString()}
			foreach(var net in allNetList) {
				for(int i=0; i<net.Count; i++) {
					if(net[i][1].Contains(component) && net[i][2].Contains(pin)) {
						result = net[i][0];
						return result;
					}
				}
			}
		} else {
			Debug.Log("wire name is null");
		}
		return result;
	}
*/
	public void setNetName(string _name) {
		bbPinNetName = _name;
	}

	public string getbbPinWire(string _bbPinName) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
			string bbrow = _bbPinName.Substring(0, _bbPinName.IndexOf('-'));
            if( wireObj.name.Contains(_bbPinName.Substring(0, _bbPinName.IndexOf('-'))) )
            {
				return wireObj.name;
            }
        }
		return "";
	}

	public string getNetName() {
		string componentPinName = comm.getComponentPin();
		string component = componentPinName.Substring(0, componentPinName.IndexOf('-'));
		string pin = componentPinName.Substring(componentPinName.IndexOf('-')+1, componentPinName.Length-componentPinName.IndexOf('-')-1);

		List<List<string[]>> allNetList = getAllNetList();
		//{netName, item["component"].ToString(), item["pin"].ToString()}
		foreach(var net in allNetList) {
			for(int i=0; i<net.Count; i++) {
				if(net[i][1].Contains(component) && net[i][2].Contains(pin)) {
					bbPinNetName = net[i][0];
					return bbPinNetName;
				}
			}
		}
		return "";
	}

	public void addbbNetPosition(string _bbPinName) {
		string bbPinPos = "";
		string componentPinName = comm.getComponentPin();
		string component = componentPinName.Substring(0, componentPinName.IndexOf('-'));
		string pin = componentPinName.Substring(componentPinName.IndexOf('-')+1, componentPinName.Length-componentPinName.IndexOf('-')-1);

		List<List<string[]>> allNetList = getAllNetList();
		//{netName, item["component"].ToString(), item["pin"].ToString()}
		foreach(var net in allNetList) {
			for(int i=0; i<net.Count; i++) {
				if(net[i][1].Contains(component) && net[i][2].Contains(pin)) {
					bbPinNetName = net[i][0];
				}
			}
		}

		bbPinPos = (Util.getDigit( _bbPinName.Substring(0, _bbPinName.IndexOf('-'))).ToString());
		bbNetPosition[bbPinNetName].Add(bbPinPos);
	}

	public void removebbNetPosition(string _bbPinName) {
		string bbPinPos = (Util.getDigit( _bbPinName.Substring(0, _bbPinName.IndexOf('-'))).ToString());
		foreach(var item in bbNetPosition) {
			if(item.Value.Contains(bbPinPos)) {
				item.Value.Remove(bbPinPos);
				break;
			}
		}
	}

	public bool isOccupiedRow(string _bbPinName) {
		bool result = false;
		string pin = _bbPinName;
		int start = 0;
		int pos = pin.IndexOf("-");
		pin = pin.Substring(start, pos);

		if(occupiedRows.Contains( Util.getDigit(pin).ToString() ) ) {
			// allow connect to the same net
			bbPinNetName = getNetName();
			if(bbPinNetName != "") {
				if(bbNetPosition[bbPinNetName].Contains(Util.getDigit(pin).ToString())) {
					result = false;
					addbbNetPosition(_bbPinName);
				} else {
					result = true;
				}
			} else {
				result = true;
			}
		} else {
			result = false;
			addbbNetPosition(_bbPinName);
		}
		return result;
	}

	/*
	public bool isOccupiedRow(string _name) {
		bool result = false;
		string pin = _name;
		int start = 0;
		int pos = pin.IndexOf("-");
		pin = pin.Substring(start, pos);
		if(occupiedRows.Contains( Util.getDigit(pin).ToString() ) )
			result = true;
		else
			result = false;
		return result;
	} */

	public List<string> getOccupiedRows() {
		return occupiedRows;
	}

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

	public void addGroundPosition(string _name) {
		List<string[]> gndNetElements = netHandler.getGndNetList();

		_Component component = new _Component(_name, "");
		_Pin pin = new _Pin("connector0", "init", "1");
		for(int i=0; i<gndNetElements.Count; i++) {
			NetElement element = new NetElement(gndNetElements[i][1], gndNetElements[i][2]);
			pin.addNetElement(element);
			pin.addNetElementAll(element);
		}
		component.addPin(pin);
		debugNetData.Add(_name, component);

		if(gndNetElements!=null && gndNetElements.Count>0) {
			// debugNetData[_name].getPin("connector0").breadboardRowPosition = debugNetData[gndNetElements[gndNetElements.Count-1][1]].getPin(gndNetElements[gndNetElements.Count-1][2]).breadboardRowPosition;
			string test = debugNetData[gndNetElements[3][1]].getPin(gndNetElements[3][2]).breadboardRowPosition;
			debugNetData[_name].getPin("connector0").breadboardRowPosition = "1";//debugNetData[gndNetElements[0][1]].getPin(gndNetElements[0][2]).breadboardRowPosition;
		}
	}

	public void addPowerPosition(string _name) {
		List<string[]> pwrNetElements = netHandler.getPwrNetList();

		_Component component = new _Component(_name, "");
		_Pin pin = new _Pin("connector0", "init", "1");
		for(int i=0; i<pwrNetElements.Count; i++) {
			NetElement element = new NetElement(pwrNetElements[i][1], pwrNetElements[i][2]);
			pin.addNetElement(element);
			pin.addNetElementAll(element);
		}
		component.addPin(pin);
		debugNetData.Add(_name, component);

		if(pwrNetElements!=null && pwrNetElements.Count>0) {
			debugNetData[_name].getPin("connector0").breadboardRowPosition = debugNetData[pwrNetElements[0][1]].getPin(pwrNetElements[0][2]).breadboardRowPosition;
		}
	}

	public void addWireComponents() {
		int componentCount = 0;
		JArray componentsArray = (JArray)netHandler.getJsonNetData().GetValue("components");
		componentCount = componentsArray.Count;
		//add wires for schematic breadboard database
		for(int i=0; i<componentCount; i++) {
			string breadboardRowPosition = "";
			string breadboardColPosition = "";
			int row = 0;
			int col = 1;

			if( componentsArray[i]["label"].ToString().Contains("wire") ){
				_Component component = new _Component(componentsArray[i]["label"].ToString(), "");

				breadboardRowPosition = componentsArray[i]["connector"][0]["position"][row].ToString();
				breadboardColPosition = componentsArray[i]["connector"][0]["position"][col].ToString();
				_Pin pin1 = new _Pin("connector0", breadboardRowPosition, breadboardColPosition);

				breadboardRowPosition = componentsArray[i]["connector"][1]["position"][row].ToString();
				breadboardColPosition = componentsArray[i]["connector"][1]["position"][col].ToString();
				_Pin pin2 = new _Pin("connector1", breadboardRowPosition, breadboardColPosition);

				component.addPin(pin1);
				component.addPin(pin2);

				debugNetData.Add(componentsArray[i]["label"].ToString(), component);
			}
		}
	}

	public int[] getGndNet(ref List<GameObject> _pinsInNet) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string[]> gndNetElements = netHandler.getGndNetList();
		List<string> resultPins = new List<string>();

		// if(debugNetData.ContainsKey(_component)) {
			if(modeToggle.IsAutoMode()) {
			// component pin의 net element에 들어있는 컴포넌트 핀의 breadboard pin 가져오기
				for(int i=0; i<gndNetElements.Count; i++) {	
					resultPins.Add(debugNetData[gndNetElements[i][1]].getPin(gndNetElements[i][2]).breadboardRowPosition);
					GameObject spin = Util.getChildObject("SCH_"+gndNetElements[i][1], gndNetElements[i][2]);
					GameObject fpin = Util.getChildObject("SCH_"+gndNetElements[i][1], "f"+gndNetElements[i][2]);
					if(spin != null)
						_pinsInNet.Add(spin);
					if(fpin != null)
						_pinsInNet.Add(fpin);
				}
			} else {
				for(int i=0; i<gndNetElements.Count; i++) {	
					resultPins.Add(debugNetData[gndNetElements[i][1]].getPin(gndNetElements[i][2]).breadboardRowPosition);
					GameObject spin = Util.getChildObject(gndNetElements[i][1], gndNetElements[i][2]);
					GameObject fpin = Util.getChildObject(gndNetElements[i][1], "f"+gndNetElements[i][2]);
					if(spin != null)
						_pinsInNet.Add(spin);
					if(fpin != null)
						_pinsInNet.Add(fpin);
				}
			}

			foreach(var item in resultPins) {
				if(item.Contains("init")) continue;
				else boardBinary[int.Parse(item)-1] = '1';
			}

			for(int i=0; i<16; i++)
				if(boardBinary[i] == '1') result[left] += (int)Math.Pow(2, i);

			for(int i=16; i<32; i++)
				if(boardBinary[i] == '1') result[right] += (int)Math.Pow(2, i-16);
		// } else {
			// result = Enumerable.Repeat(-1, 2).ToArray();
		// }

		return result;
	}

	public int[] getPwrNet(ref List<GameObject> _pinsInNet) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string[]> pwrNetElements = netHandler.getPwrNetList();
		List<string> resultPins = new List<string>();

		// if(debugNetData.ContainsKey(_component)) {
			if(modeToggle.IsAutoMode()) {
			// component pin의 net element에 들어있는 컴포넌트 핀의 breadboard pin 가져오기
				for(int i=0; i<pwrNetElements.Count; i++) {	
					resultPins.Add(debugNetData[pwrNetElements[i][1]].getPin(pwrNetElements[i][2]).breadboardRowPosition);
					GameObject spin = Util.getChildObject("SCH_"+pwrNetElements[i][1], pwrNetElements[i][2]);
					GameObject fpin = Util.getChildObject("SCH_"+pwrNetElements[i][1], "f"+pwrNetElements[i][2]);
					if(spin != null)
						_pinsInNet.Add(spin);
					if(fpin != null)
						_pinsInNet.Add(fpin);
				}
			} else {
				for(int i=0; i<pwrNetElements.Count; i++) {	
					resultPins.Add(debugNetData[pwrNetElements[i][1]].getPin(pwrNetElements[i][2]).breadboardRowPosition);
					GameObject spin = Util.getChildObject(pwrNetElements[i][1], pwrNetElements[i][2]);
					GameObject fpin = Util.getChildObject(pwrNetElements[i][1], "f"+pwrNetElements[i][2]);
					if(spin != null)
						_pinsInNet.Add(spin);
					if(fpin != null)
						_pinsInNet.Add(fpin);
				}
			}

			foreach(var item in resultPins) {
				if(item.Contains("init")) continue;
				else boardBinary[int.Parse(item)-1] = '1';
			}

			for(int i=0; i<16; i++)
				if(boardBinary[i] == '1') result[left] += (int)Math.Pow(2, i);

			for(int i=16; i<32; i++)
				if(boardBinary[i] == '1') result[right] += (int)Math.Pow(2, i-16);
		// } else {
			// result = Enumerable.Repeat(-1, 2).ToArray();
		// }

		return result;
	}

	public int[] getAllNetForPin(string _component, string _pin, ref List<GameObject> _pinsInNet) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string> resultPins = new List<string>();

		if(debugNetData.ContainsKey(_component)) {
			if(modeToggle.IsAutoMode()) {
			// component pin의 net element에 들어있는 컴포넌트 핀의 breadboard pin 가져오기
				foreach(var element in debugNetData[_component].getPin(_pin).netElementsAll) {	
					resultPins.Add(debugNetData[element.component].getPin(element.pinid).breadboardRowPosition);
					GameObject spin = Util.getChildObject("SCH_"+element.component, element.pinid);
					GameObject fpin = Util.getChildObject("SCH_"+element.component, "f"+element.pinid);
					if(spin != null)
						_pinsInNet.Add(spin);
					if(fpin != null)
						_pinsInNet.Add(fpin);
				}
			} else {
				foreach(var element in debugNetData[_component].getPin(_pin).netElementsAll) {	
					resultPins.Add(debugNetData[element.component].getPin(element.pinid).breadboardRowPosition);
					GameObject spin = Util.getChildObject(element.component, element.pinid);
					GameObject fpin = Util.getChildObject(element.component, "f"+element.pinid);
					if(spin != null)
						_pinsInNet.Add(spin);
					if(fpin != null)
						_pinsInNet.Add(fpin);
				}
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

	public int[] getPositionForNet(List<string[]> _netElements) { //string _component, string _pin) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string> resultPins = new List<string>();

		for(int i=0; i<_netElements.Count; i++){
			if(debugNetData.ContainsKey(_netElements[i][1])) {
				resultPins.Add(debugNetData[_netElements[i][1]].getPin(_netElements[i][2]).breadboardRowPosition);
			}
		}

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

	public int[] getPositionForNet(Dictionary<string,string> _netElements) { //string _component, string _pin) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string> resultPins = new List<string>();

		foreach(var item in _netElements) {
			if(debugNetData.ContainsKey(item.Key)) {
				resultPins.Add(debugNetData[item.Key].getPin(item.Value).breadboardRowPosition);
			}
		}

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
