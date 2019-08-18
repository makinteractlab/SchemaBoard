using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine.UI;
using System.Xml;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class LoadSchematicUI : MonoBehaviour {
	public GameObject prefabOpAmp;
	public GameObject prefabRelay;
	public GameObject prefab8pinChip;
	public GameObject prefab16pinChip;
	public GameObject prefabConnector;
	public GameObject prefabResistor;
	public GameObject prefabUniCapacitor;
	public GameObject prefabBiCapacitor;
	public GameObject prefabInductor;
	public GameObject prefabLed;
	public GameObject prefabSwitch;
	public GameObject prefabPhotoresistor;
	public GameObject prefabDiode;
	// public GameObject prefabZenerdiode;
	public GameObject prefabTransistor;
	public GameObject prefabSpeaker;
	public GameObject prefabPwr;
	public GameObject prefabGnd;
	public GameObject prefabBattery;
	public GameObject prefabEtc;
    public RectTransform ParentPanel;
	//public BoardDataHandler board;

	public UnityAction<JObject> dataReceivedAction;
	public DataReceivedEvent dataReceivedEvent;

	public NetData netdata;
	public ToggleAutoManual modeToggleMenu;

	// Dictionary<string, _Component> netData;
	public DrawSchematicWire schematicWire;

	float distance = 3.0f;

    public void Start() {
        setWireObject();
		dataReceivedAction = new UnityAction<JObject>(drawSchematic);
        dataReceivedEvent = new DataReceivedEvent();
        dataReceivedEvent.AddListener(dataReceivedAction);
    }

	public void initGlowIcon() {
		GameObject[] schematic = GameObject.FindGameObjectsWithTag("schematic_glow");
		GameObject[] fritzing = GameObject.FindGameObjectsWithTag("fritzing_glow");

		foreach(GameObject glow in schematic) {
			glow.transform.localScale = new Vector3(0,0,0);
		}

		foreach(GameObject glow in fritzing) {
			glow.transform.localScale = new Vector3(0,0,0);
		}
	}

    public void setWireObject()
    {
        schematicWire = GameObject.Find("DrawSchematicWires").GetComponent<DrawSchematicWire>();
    }
	
	// Update is called once per frame
	void Update () {
	}

	private GameObject getChildObject(GameObject ParentObject, string ChildObjectName)
    {
        GameObject resultObj = null;
        
        Transform[] children = ParentObject.GetComponentsInChildren<Transform>();
        foreach(Transform obj in children)     
        {
            if(obj.name == ChildObjectName) {
                resultObj = obj.gameObject;
            }
        }
        return resultObj;
    }

	public void drawSchematic(JObject _data)
	{	
		int screenOffest = 120;
		GameObject component = null;
		GameObject connector = null;
		Dictionary<string, JObject> schematicData = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(_data.ToString());
		// Dictionary<string, JObject> schematicWireData = new Dictionary<string, JObject>();
		// Dictionary<string, JObject> schematicConnectionData = new Dictionary<string, JObject>();
		var pointList = new List<Vector2>();

		foreach(KeyValuePair<string, JObject> item in schematicData)
		{
			// string item.Key = "";
			string componentName = "";
			string uiComponentName = "";
			string gndName = "";
			string pwrName = "";
			int gndIndex = 100;
			int pwrIndex = 200;
			// Char r = '\r';
            // if (item.Key.Contains(r.ToString())) {
            //     item.Key = item.Key.Replace(r.ToString(), "");
            // }
			uiComponentName = "SCH_"+item.Key;
			componentName = Util.removeDigit(item.Key);
			
			//ComponentBase comp = ComponentFactory.Create(componentName, componentData);
			switch (componentName) {
				case "OPAMP":
					component = (GameObject)Instantiate(prefabOpAmp);
					break;
				case "R":
					component = (GameObject)Instantiate(prefabResistor);
					break;
				case "C":
					component = (GameObject)Instantiate(prefabUniCapacitor);
					break;
				case "CP":
					component = (GameObject)Instantiate(prefabBiCapacitor);
					break;
				case "L":
					component = (GameObject)Instantiate(prefabInductor);
					break;
				case "LED":
					component = (GameObject)Instantiate(prefabLed);
					break;
				case "SW":
					component = (GameObject)Instantiate(prefabSwitch);
					break;
				case "LDR":
					component = (GameObject)Instantiate(prefabPhotoresistor);
					break;
				case "Q":
					component = (GameObject)Instantiate(prefabTransistor);
					break;
				case "SP":
					component = (GameObject)Instantiate(prefabSpeaker);
					break;
				case "D":
					component = (GameObject)Instantiate(prefabDiode);
					break;
				case "GND":
					component = (GameObject)Instantiate(prefabGnd);
					gndName = "GND" + gndIndex;
					uiComponentName = "SCH_"+ gndName;
					gndIndex ++;
					break;
				case "GND?":
					component = (GameObject)Instantiate(prefabGnd);
					gndName = "GND" + gndIndex;
					uiComponentName = "SCH_"+ gndName;
					gndIndex ++;
					break;
				case "#GND":
					component = (GameObject)Instantiate(prefabGnd);
					gndName = "GND" + gndIndex;
					uiComponentName = "SCH_"+ gndName;
					gndIndex ++;
					break;
				case "#PWR":
					component = (GameObject)Instantiate(prefabPwr);
					pwrName = "PWR" + pwrIndex;
					uiComponentName = "SCH_"+ pwrName;
					pwrIndex++;
					break;
				case "PWR?":
					component = (GameObject)Instantiate(prefabPwr);
					pwrName = "PWR" + pwrIndex;
					uiComponentName = "SCH_"+ pwrName;
					pwrIndex++;
					break;
				case "PWR":
					component = (GameObject)Instantiate(prefabPwr);
					pwrName = "PWR" + pwrIndex;
					uiComponentName = "SCH_"+ pwrName;
					pwrIndex++;
					break;
				case "BT":
					component = (GameObject)Instantiate(prefabBattery);
					break;
				case "RELAY":
					component = (GameObject)Instantiate(prefabRelay);
					break;
				case "U":
				case "U-":
					string name = item.Key;
					int start;// = name.IndexOf("-")+1;
					int length;/// = name.Length-start;
					int chipType;// = int.Parse(item.Key.Substring(start,length));
					if(name.Contains("-")) {
						start = name.IndexOf("-")+1;
						length = name.Length-start;
						chipType = int.Parse(item.Key.Substring(start,length));
					} else
						chipType = 8;

					switch(chipType){
						case 6:
						component = (GameObject)Instantiate(prefabRelay);
						break;
						case 8:
						component = (GameObject)Instantiate(prefab8pinChip);
						break;
						case 16:
						component = (GameObject)Instantiate(prefab16pinChip);
						break;
						default:
						component = (GameObject)Instantiate(prefab8pinChip);
						break;
					}
					break;
				case "wire":
					pointList.Add(new Vector2 ((float)item.Value["x1"]/4.5f-Screen.width/2-screenOffest, -(float)item.Value["y1"]/4.5f+Screen.height/2+screenOffest));
					pointList.Add(new Vector2 ((float)item.Value["x2"]/4.5f-Screen.width/2-screenOffest, -(float)item.Value["y2"]/4.5f+Screen.height/2+screenOffest));
					schematicWire.createWireObject(pointList, uiComponentName);
					pointList.Clear();
					component = null;
					connector = null;
					break;
				case "connection":
					connector = (GameObject)Instantiate(prefabConnector);
					component = null;
					break;
				default:
					component = (GameObject)Instantiate(prefabEtc);
					break;
			}
			//Vector2 scale = new Vector2(ParentPanel.rect.width / Screen.width, ParentPanel.rect.height / Screen.height);
			if(connector) {
				JObject position = item.Value;
				connector.tag = "auto_prefab";
				connector.name = uiComponentName;
				connector.transform.SetParent(ParentPanel, false);
				connector.transform.position = new Vector3((float)position["x"]/4.5f-screenOffest, -(float)position["y"]/4.5f+screenOffest, 0);
				connector.transform.Translate(new Vector3(0,Screen.height,0));
			}
			if(component) {
				JObject position = item.Value;
				component.tag = "auto_prefab";
				component.name = uiComponentName;
				component.transform.SetParent(ParentPanel, false);
				component.transform.position = new Vector3((float)position["x"]/4.5f-screenOffest, -(float)position["y"]/4.5f+screenOffest, 0);
				component.transform.Translate(new Vector3(0,Screen.height,0));
				
				Debug.Log("+=+=+=+" + component.transform.position);
				component.transform.Rotate(0, 0, (float)position["degree"], Space.Self);
				if((string)position["flip"] == "x") {//x y
					component.transform.localScale = new Vector3(-1, 1, 1);
				} else if((string)position["flip"] == "y") {
					component.transform.localScale = new Vector3(1, -1, 1);
				} else {
					component.transform.localScale = new Vector3(1, 1, 1);
				}

				if(!(componentName.Contains("PWR")||componentName.Contains("GND"))) {
					string title = netdata.debugNetData[uiComponentName.Replace("SCH_", "")].getValue();
					Util.getChildObject(component.name, "title").GetComponent<Text>().text = title;
				}
			}

			if(item.Key.Contains("GND")) {
				netdata.addGroundPosition(gndName);
			} else if(item.Key.Contains("PWR")) {
				netdata.addPowerPosition(pwrName);
				//netdata.debugNetData[pwrName].setValue((string)item.Value["value"]);
			}
		}
		netdata.addWireComponents();

		initGlowIcon();
		GameObject[] temp = GameObject.FindGameObjectsWithTag("circuit_prefab_fritzing");
		foreach(GameObject componentObj in temp) {
			componentObj.transform.localScale = new Vector3(0,0,0);
		}

		component = null;
		connector = null;
	}
}