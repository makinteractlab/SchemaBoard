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
	public GameObject prefabCapacitor;
	public GameObject prefabInductor;
	public GameObject prefabLed;
	public GameObject prefabSwitch;
	public GameObject prefabPhotoresistor;
	public GameObject prefabDiode;
	public GameObject prefabZenerdiode;
	public GameObject prefabVcc;
	public GameObject prefabEtc;
    public RectTransform ParentPanel;
	//public BoardDataHandler board;

	public UnityAction<JObject> dataReceivedAction;
	public DataReceivedEvent dataReceivedEvent;

	public NetData netDataObj;
	public ToggleAutoManual modeToggleMenu;

	// Dictionary<string, _Component> netData;
	public Sprite DefaultPinSprite;
	public DrawSchematicWire schematicWire;

    public void Start() {
        setWireObject();
		dataReceivedAction = new UnityAction<JObject>(drawSchematic);
        dataReceivedEvent = new DataReceivedEvent();
        dataReceivedEvent.AddListener(dataReceivedAction);
    }

    public void setWireObject()
    {
        schematicWire = GameObject.Find("DrawSchematicWires").GetComponent<DrawSchematicWire>();
        //wire = temp.GetComponent<ComponentObject>().getWireObject();
		Debug.Log("\n\n\n\n++++++++++++++++++++++++ : " + schematicWire.name + "\n\n\n\n");
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
		Dictionary<string,int[]> schematicData = new Dictionary<string,int[]>();

		foreach(KeyValuePair<string, int[]> item in schematicData)
		{
			string componentName = Util.removeDigit(item.Key);
			
			string uiComponentName = item.Key;
			//ComponentBase comp = ComponentFactory.Create(componentName, componentData);
			switch (componentName) {
				case "R":
					component = (GameObject)Instantiate(prefabResistor);
					// getChildObject(component, "ValueText").GetComponent<Text>().text = Util.changeUnit(comp.value, componentName); //comp.value.ToString();
					break;
				case "C":
					component = (GameObject)Instantiate(prefabCapacitor);
					// getChildObject(component, "ValueText").GetComponent<Text>().text = Util.changeUnit(comp.value, componentName);//comp.value.ToString();
					break;
				case "L":
					component = (GameObject)Instantiate(prefabInductor);
					// getChildObject(component, "ValueText").GetComponent<Text>().text = Util.changeUnit(comp.value, componentName);//comp.value.ToString();
					break;
				case "LED":
					component = (GameObject)Instantiate(prefabLed);
					//getChildObject(component, "ValueText").GetComponent<Text>().text = comp.value.ToString();
					break;
				case "S":
					component = (GameObject)Instantiate(prefabSwitch);
					//getChildObject(component, "ValueText").GetComponent<Text>().text = comp.value.ToString();
					break;
				case "LDR":
					component = (GameObject)Instantiate(prefabPhotoresistor);
					//getChildObject(component, "ValueText").GetComponent<Text>().text = comp.value.ToString();
					break;
				case "D":
					component = (GameObject)Instantiate(prefabDiode);
					// getChildObject(component, "ValueText").GetComponent<Text>().text = Util.changeUnit(comp.value, componentName);
					break;
				case "ZD":
					component = (GameObject)Instantiate(prefabZenerdiode);
					// getChildObject(component, "ValueText").GetComponent<Text>().text = Util.changeUnit(comp.value, componentName);
					break;
				case "VCC":
				case "BT":
					component = (GameObject)Instantiate(prefabVcc);
					break;
				default:
					component = (GameObject)Instantiate(prefabEtc);
					// getChildObject(component, "ValueText").GetComponent<Text>().text = Util.changeUnit(comp.value, componentName);
					break;
			}
			
			if(component) {
				component.tag = "component";
				component.name = uiComponentName;
				component.transform.SetParent(ParentPanel, false);
				component.transform.position = new Vector3(item.Value[0],ParentPanel.transform.position.y+10,item.Value[1]);
				component.transform.localScale = new Vector3(1, 1, 1);
			}
		}
/*
		foreach(KeyValuePair<string, _Component> item in _netData)
		{
			List<_Pin> pins = item.Value.getPins();
			foreach(var pin in pins) {
				List<NetElement> netElement = pin.getNetElement();
				//float dashSize = 4.0f;
				foreach(var target in netElement) {
					GameObject currentObject = GameObject.Find(item.Key);
					GameObject targetObject = GameObject.Find(target.component);
					netwire.createWireObject(getChildObject(currentObject, pin.id), getChildObject(targetObject, target.pinid));
				}
			}

			if(item.Key.Contains("VCC") || item.Key.Contains("BT")) {
				netDataObj.setColorGroundPins(item.Key, "connector0");
				netDataObj.setColorVccPins(item.Key, "connector1");
			}
		}

		modeToggleMenu.setAutoMode();*/
	}
}