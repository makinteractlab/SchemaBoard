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
	public GameObject prefabResistor;
	public GameObject prefabUniCapacitor;
	public GameObject prefabBiCapacitor;
	public GameObject prefabInductor;
	public GameObject prefabLed;
	public GameObject prefabSwitch;
	public GameObject prefabPhotoresistor;
	public GameObject prefabDiode;
	// public GameObject prefabZenerdiode;
	public GameObject prefabPwr;
	public GameObject prefabGnd;
	public GameObject prefabBattery;
	public GameObject prefabEtc;
    public RectTransform ParentPanel;
	//public BoardDataHandler board;

	public UnityAction<JObject> dataReceivedAction;
	public DataReceivedEvent dataReceivedEvent;

	public NetData netDataObj;
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

	private void initGlowIcon() {
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
		GameObject component = null;
		Dictionary<string, JObject> schematicData = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(_data.ToString());
		// Dictionary<string, JObject> schematicWireData = new Dictionary<string, JObject>();
		// Dictionary<string, JObject> schematicConnectionData = new Dictionary<string, JObject>();
		var pointList = new List<Vector2>();

		foreach(KeyValuePair<string, JObject> item in schematicData)
		{
			string componentName = Util.removeDigit(item.Key);
			
			string uiComponentName = "SCH_"+item.Key;
			//ComponentBase comp = ComponentFactory.Create(componentName, componentData);
			switch (componentName) {
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
				case "S":
					component = (GameObject)Instantiate(prefabSwitch);
					break;
				case "LDR":
					component = (GameObject)Instantiate(prefabPhotoresistor);
					break;
				case "D":
					component = (GameObject)Instantiate(prefabDiode);
					break;
				case "GND":
					component = (GameObject)Instantiate(prefabGnd);
					break;
				case "PWR":
					component = (GameObject)Instantiate(prefabPwr);
					break;
				case "BT":
					component = (GameObject)Instantiate(prefabBattery);
					break;
				case "wire":
					pointList.Add(new Vector2 ((float)item.Value["x1"]/5-Screen.width/2, -(float)item.Value["y1"]/5+Screen.height/2));
					pointList.Add(new Vector2 ((float)item.Value["x2"]/5-Screen.width/2, -(float)item.Value["y2"]/5+Screen.height/2));
					schematicWire.createWireObject(pointList, uiComponentName);
					pointList.Clear();
					component = null;
					break;
				case "connection":
					
					component = null;
					break;
				default:
					component = (GameObject)Instantiate(prefabEtc);
					break;
			}
			//Vector2 scale = new Vector2(ParentPanel.rect.width / Screen.width, ParentPanel.rect.height / Screen.height);

			if(component) {
				JObject position = item.Value;
				component.tag = "auto_prefab";
				component.name = uiComponentName;
				component.transform.SetParent(ParentPanel, false);
				component.transform.position = new Vector3((float)position["x"]/5, -(float)position["y"]/5, 0);
				component.transform.Translate(new Vector3(0,Screen.height,0));
				
				Debug.Log("+=+=+=+" + component.transform.position);
				component.transform.Rotate(0, (float)position["degree"], 0, Space.Self);
				if((string)position["flip"] == "x") {//x y
					component.transform.localScale = new Vector3(-1, 1, 1);
				} else if((string)position["flip"] == "y") {
					component.transform.localScale = new Vector3(1, -1, 1);
				} else {
					component.transform.localScale = new Vector3(1, 1, 1);
				}
			}
		}

		initGlowIcon();

		// Dictionary<string, _Component> netData = netDataObj.getInitialSchematicData();

		// foreach(KeyValuePair<string, _Component> item in netData)
		// {
		// 	List<_Pin> pins = item.Value.getPins();
		// 	foreach(var pin in pins) {
		// 		List<NetElement> netElement = pin.getNetElement();
		// 		//float dashSize = 4.0f;
		// 		foreach(var target in netElement) {
		// 			GameObject currentObject = GameObject.Find("SCH_"+item.Key);
		// 			GameObject targetObject = GameObject.Find("SCH_"+target.component);
		// 			schematicWire.createWireObject(getChildObject(currentObject, pin.id), getChildObject(targetObject, target.pinid));
		// 		}
		// 	}

		// 	if(item.Key.Contains("VCC") || item.Key.Contains("BT")) {
		// 		netDataObj.setColorGroundPins(item.Key, "connector0");
		// 		netDataObj.setColorVccPins(item.Key, "connector1");
		// 	}
		// }

		//modeToggleMenu.setAutoMode();
	}
}