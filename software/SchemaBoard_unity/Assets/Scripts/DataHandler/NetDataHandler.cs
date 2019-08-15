using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml;
using System;
using Nuclex.Support.Cloning;

public class _Pin {
	public string id;
	public string breadboardRowPosition;
	public string breadboardColPosition;

	public List<NetElement> netElements = new List<NetElement>();

	public List<NetElement> netElementsAll = new List<NetElement>();

	public _Pin (string _id, string _breadboardRowPosition, string _breadboardColPosition) {
		this.id = _id;
		this.breadboardRowPosition = _breadboardRowPosition;
		this.breadboardColPosition = _breadboardColPosition;
	}

	public string getConnectedBreadboardRowPosition() {
		return breadboardRowPosition;
	}

	public string getConnectedBreadboardColPosition() {
		return breadboardColPosition;
	}

	public void addNetElement(NetElement ne) {
		this.netElements.Add(ne);
	}

	public List<NetElement> getNetElement() {
		return netElements;
	}

	public void addNetElementAll(NetElement ne) {
		this.netElementsAll.Add(ne);
	}

	public List<NetElement> getNetElementAll() {
		return netElementsAll;
	}
}

public class NetElement {
	public string component;
	public string pinid;
	public NetElement (string _component, string _pinid) {
		this.component = _component;
		this.pinid = _pinid;
	}
}

public class _Component {
	public string label;
	//public string title;
	public List<_Pin> pins = new List<_Pin>();
	
	public _Component(string _label) {
		this.label = _label;
		//this.title = _title;
	}

	public void addPin(_Pin pin) {
		this.pins.Add(pin);
	}

	public _Pin getPin(string _pinid) {
		_Pin result = null;
		foreach(var pin in pins) {
			if(pin.id.Contains(_pinid)) {
				result = pin;
				break;
			}
		}
		return result;		
	}

	public _Pin getFirstPin() {
		_Pin result = null;
		foreach(var pin in pins) {
			if(pin.id == "connector0") {
				result = pin;
				break;
			}
		}
		
		return result;		
	}

	public List<_Pin> getPins() {
		return pins;
	}
}

public class NetDataHandler {
	//List<_Component> components = new List<_Component>();
	Dictionary<string, _Component> debugNetData = new Dictionary<string, _Component>();
	// Dictionary<string, _Component> buildNetData = new Dictionary<string, _Component>();
	Dictionary<string, _Component> initNetData = new Dictionary<string, _Component>();

	// Dictionary<string, string> gndNetElements = new Dictionary<string, string>();
	List<string[]> gndNetElementsList = new List<string[]>();
	// Dictionary<string, string> pwrNetElements = new Dictionary<string, string>();
	List<string[]> pwrNetElementsList = new List<string[]>();
	// Dictionary<string, string> netElements = new Dictionary<string, string>();
	List<string[]> netElementsList = new List<string[]>();
	// List<Dictionary<string, string>> allNetList = new List<Dictionary<string, string>>();
	List<List<string[]>> allNet = new List<List<string[]>>();

	string log = "";
	Dictionary<string, ArrayList> connections = new Dictionary<string, ArrayList>();
	Dictionary<string, ArrayList> allConnections = new Dictionary<string, ArrayList>();
	Dictionary<string, ArrayList> componentNameAndPins = new Dictionary<string, ArrayList>();
	// public Dictionary<string, _Component> getNetData(string _filePath)
	// {
	// 	JObject netData = null;
	// 	XmlDocument doc = new XmlDocument();
	// 	string xmlContent = System.IO.File.ReadAllText(_filePath);
	// 	doc.LoadXml(xmlContent);
	// 	doc.RemoveChild(doc.FirstChild);
	// 	var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);
	// 	json = json.Replace("@", "");
	// 	netData = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
	// 	parseNetData(netData);
	// 	return componentsInCircuit;
	// }

	// public List<Dictionary<string, string>> getAllNetList() {
	// 	return allNetList;
	// }
	// public Dictionary<string,string> getGndNet() {
	// 	return gndNetElements;
	// }

	// public Dictionary<string,string> getPwrNet() {
	// 	return pwrNetElements;
	// }

	public List<List<string[]>> getAllNet() {
		return allNet;
	}
	public List<string[]> getGndNetList() {
		return gndNetElementsList;
	}

	public List<string[]> getPwrNetList() {
		return pwrNetElementsList;
	}

	public Dictionary<string, _Component> getInitNetData() {
		return initNetData;
	}

	// public Dictionary<string, _Component> getBuildNetData() {
	// 	return buildNetData;
	// }

	public Dictionary<string, _Component> getDebugNetData() {
		return debugNetData;
	}

	public Dictionary<string, _Component> parseNetData(JObject _netData)
	{
		int netCount = 0;
		int componentCount = 0;

		string noneFormattedString = _netData.ToString(Newtonsoft.Json.Formatting.None);
		noneFormattedString = noneFormattedString.Replace("\\\"", "\"");
		JObject data = JObject.Parse(noneFormattedString);

		JArray netArray = (JArray)data.GetValue("net");
		JArray componentsArray = (JArray)data.GetValue("components");

		netCount = netArray.Count;
		componentCount = componentsArray.Count;

		ArrayList connectionKeys = new ArrayList();
		Dictionary<string, ArrayList> netComponent = new Dictionary<string, ArrayList>();
		Dictionary<string, ArrayList> netComponentAll = new Dictionary<string, ArrayList>();

		for(int i=0; i<netCount; i++) {
			int connectorCount = 0;
			JArray connectorArray = (JArray)((JObject)netArray[i]).GetValue("connector");
			connectorCount = connectorArray.Count;

			string netName = ((JObject)netArray[i]).GetValue("name").ToString();

			foreach(var item in connectorArray) {
				String[] netComponentPin = {netName, item["component"].ToString(), item["pin"].ToString()};
				netElementsList.Add(netComponentPin);
				// netElements.Add(item["component"].ToString(), item["pin"].ToString());
			}
			// Dictionary<string,string> element = new Dictionary<string,string>(netElements);
			List<string[]> netElementsListCopy = new List<string[]> (netElementsList);

			if(netName.CompareTo("GND") == 0) {
				foreach(var item in connectorArray) {
					String[] gndComponentPin = {netName, item["component"].ToString(), item["pin"].ToString()};
					gndNetElementsList.Add(gndComponentPin);
					// gndNetElements.Add(item["component"].ToString(), item["pin"].ToString());
				}
			} else if(netName.Contains("3V")) {
				foreach(var item in connectorArray) {
					String[] pwrComponentPin = {netName, item["component"].ToString(), item["pin"].ToString()};
					pwrNetElementsList.Add(pwrComponentPin);
					// pwrNetElements.Add(item["component"].ToString(), item["pin"].ToString());
				}
			} else {
				allNet.Add(netElementsListCopy);
				// netElements.Clear();
				netElementsList.Clear();
			}

			for(int j=0; j<connectorCount; j++) {
				//string title = ((JObject)connectorArray[j])["part"]["title"].ToString();
				string label = ((JObject)connectorArray[j])["component"].ToString();
				
				string breadboardRowPosition = "";
				string breadboardColPosition = "";
				int row = 0;
				int col = 1;

				for(int k=0; k<componentCount; k++) {
					if( componentsArray[k]["label"].ToString().Equals(label) ) {
						int compPinNumber = Util.getDigit(((JObject)connectorArray[j])["pin"].ToString());
						JArray componentPins =  (JArray)((JObject)componentsArray[k]).GetValue("connector");
						for(int e=0; e<componentPins.Count; e++) {
							if( Util.getDigit((string)componentsArray[k]["connector"][e]["id"]) == compPinNumber) {
								breadboardRowPosition = componentsArray[k]["connector"][e]["position"][row].ToString();
								breadboardColPosition = componentsArray[k]["connector"][e]["position"][col].ToString();
								break;
							}
						}
						break;
					}
				}
				
				string[] pinid = {((JObject)connectorArray[j])["pin"].ToString(), breadboardRowPosition, breadboardColPosition};
				
				if(!componentNameAndPins.ContainsKey(label)){
					componentNameAndPins.Add(label, new ArrayList());
					componentNameAndPins[label].Add(pinid);
				} else {
					componentNameAndPins[label].Add(pinid);
				}

				string key = label + "-" + pinid[0];

				if(!netComponent.ContainsKey(key)) {
					netComponent[key] = new ArrayList();
				}
				if(!netComponentAll.ContainsKey(key)) {
					netComponentAll[key] = new ArrayList();
				}
				connectionKeys.Add(key);
			}
			
			foreach(KeyValuePair<string, ArrayList> entry in netComponent) {
				//자신과 연결된 다음 컴포넌트만 net element로 추가한다 for net wires visualization in unity
				for(int j=0; j<connectionKeys.Count-1; j++){
					if(entry.Key == (string)connectionKeys[j]){
						entry.Value.Add((string)connectionKeys[j+1]);
					}
				}

				if(!connections.ContainsKey(entry.Key))
					connections.Add(entry.Key, entry.Value);
			}

			foreach(KeyValuePair<string, ArrayList> entry in netComponentAll) {
				//자기 자신이 아니면 자신의 넷 안에 있는 모든 컴포넌트를 connection으로 추가한다.
				foreach(var item in connectionKeys) {
					//if(entry.Key != (string)item)
						entry.Value.Add((string)item);
				}

				if(!allConnections.ContainsKey(entry.Key))
					allConnections.Add(entry.Key, entry.Value);
			}
			
			connectionKeys.Clear();
			netComponent.Clear();	// netComponent는 componentNameAndPins를 만들기 위한 data
			netComponentAll.Clear();
		}

		foreach(KeyValuePair<string, ArrayList> item in componentNameAndPins) {
			_Component component = new _Component(item.Key);

			foreach(string[] pinInfo in item.Value) {
				component.addPin(new _Pin(pinInfo[0], pinInfo[1], pinInfo[2]));
			}

			foreach(KeyValuePair<string, ArrayList> entry in connections) {
				foreach(string[] pinInfo in item.Value){
					if(entry.Key.Contains(component.label+"-"+pinInfo[0])) {
						foreach(var arrItem in entry.Value) {
							string strItem = arrItem.ToString();
							int pos = strItem.LastIndexOf("-");
							string comp = strItem.Substring(0,pos);
							string pinid = strItem.Substring(pos+1, strItem.Length-pos-1);
							if(component.getPin(pinInfo[0]) != null)
								component.getPin(pinInfo[0]).addNetElement(new NetElement(comp, pinid));
						}
					}
				}
			}

			// breadboard에서 component pin click시 모든 연결된 pin들을 display하기 위해 따로 데이터를 추가함
			foreach(KeyValuePair<string, ArrayList> entry in allConnections) {
				foreach(string[] pin in item.Value){
					if(entry.Key.Contains(component.label+"-"+pin[0])) {
						foreach(var arrItem in entry.Value) {
							string strItem = arrItem.ToString();
							int pos = strItem.LastIndexOf("-");
							string comp = strItem.Substring(0,pos);
							string pinid = strItem.Substring(pos+1, strItem.Length-pos-1);
							if(component.getPin(pin[0]) != null)
								component.getPin(pin[0]).addNetElementAll(new NetElement(comp, pinid));
						}
					}
				}
			}
			
			// dictionary에 component add 하기
			if (!debugNetData.ContainsKey(component.label))
				debugNetData.Add(component.label, component);
			if (!initNetData.ContainsKey(component.label)) {
				_Component newComponent = SerializationCloner.DeepFieldClone(component);
				initNetData.Add(component.label, newComponent);
			}
			// if (!buildNetData.ContainsKey(component.label)) {
			// 	_Component newComponent = SerializationCloner.DeepFieldClone(component);
			// 	buildNetData.Add(component.label, newComponent);
			// }
			//Debug.Log("done");
		}

		return debugNetData;
	}
}