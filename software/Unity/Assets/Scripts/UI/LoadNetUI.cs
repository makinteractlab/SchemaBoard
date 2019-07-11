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

public class LoadNetUI : MonoBehaviour {
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

	public UnityAction<Dictionary<string, _Component>> dataReceivedAction;
	public DictionaryDataReceivedEvent dataReceivedEvent;

	public NetData netDataObj;
	public ToggleAutoManual modeToggleMenu;

	// Dictionary<string, _Component> netData;
	public Sprite DefaultPinSprite;
	public DrawNetWire netwire;
	public DrawVirtualWire virtualwire;
    private bool alreadyWired = false;

    public void Start() {
        setWireObject();
		dataReceivedAction = new UnityAction<Dictionary<string, _Component>>(setupNet);
        dataReceivedEvent = new DictionaryDataReceivedEvent();
        dataReceivedEvent.AddListener(dataReceivedAction);
    }

    public void setWireObject()
    {
        netwire = GameObject.Find("DrawNetWires").GetComponent<DrawNetWire>();
        //wire = temp.GetComponent<ComponentObject>().getWireObject();
		Debug.Log("\n\n\n\n++++++++++++++++++++++++ : " + netwire.name + "\n\n\n\n");
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

	public void readSchematicData(string _fileName) {
		NetDataHandler handler = new NetDataHandler();
		netDataObj.readSchematicData(_fileName, handler);
	}

	private void ResetAllConnectedWires() {
		GameObject[] pinsTemp = GameObject.FindGameObjectsWithTag("pin");
		foreach(GameObject pinObj in pinsTemp) {
			pinObj.GetComponent<Button>().image.sprite = DefaultPinSprite;;
		}

		// GameObject[] temp = GameObject.FindGameObjectsWithTag("component");
        // foreach(GameObject componentObj in temp)
        // {
			GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("wire");
			foreach(GameObject wireObj in wireTemp)
			{
				// if( wireObj.name.Contains(componentObj.name) )
				// {
					LineRenderer lr = wireObj.GetComponent<LineRenderer>();
					lr.enabled = false;
					//lr.SetVertexCount(0);
					lr.positionCount = 0;
					Destroy(wireObj);
				// }
			}
        // }
	}

	public void setupNet(Dictionary<string, _Component> _netData)
	{	
		//netData = handler.getNetData(filePath);
		//netData = netDataObj.getNetData(_filePath, handler);
		//netDataObj.setNetData(_netData);
		
		//string netName = filePath.Substring(filePath.LastIndexOf("xml/"), filePath.Length);
		//Text fritzingNameText = GameObject.Find("FritzingName").GetComponent<Text>();
		//fritzingNameText.text = netName;

		//int numberOfComponent = netData.Count;
		//Debug.Log("numberOfComponent = " + numberOfComponent);
		int count = 0;
		GameObject component = null;

		foreach(KeyValuePair<string, _Component> item in _netData)
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

				if(count < 4) {
					component.transform.position = new Vector3(-550,ParentPanel.transform.position.y+10,150-count*110);
				} else {
					component.transform.position = new Vector3(-300,ParentPanel.transform.position.y+10,150-(count-4)*110);
				}
				component.transform.localScale = new Vector3(1, 1, 1);

				List<_Pin> pins = item.Value.getPins();

			}
			count ++;
		}

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

		modeToggleMenu.setAutoMode();

		// if(modeToggleMenu.isBuildMode()) {
		// 	// auto complete connections
		// 	Vector3 startPos;
		// 	Vector3 endPos;
		// 	string wireObjectName;

		// 	foreach(KeyValuePair<string, _Component> item in _netData)
		// 	{
		// 		List<_Pin> pins = item.Value.getPins();
		// 		int index = 1;
		// 		foreach(var pin in pins) {
		// 			if(index > 5) index = 1;
		// 			string connectedPos = pin.getConnectedBreadboardPosition();
		// 			string boardPinObjName = "Pin" + connectedPos + index.ToString();
		// 			startPos = GameObject.Find(boardPinObjName).transform.position;
		// 			endPos = Util.getChildObject(item.Value.label, pin.id).transform.position;
		// 			wireObjectName = "Wire" + ":" + boardPinObjName + "," + item.Value.label + "-" + pin.id;
		// 			virtualwire.createWireObject(startPos, endPos, wireObjectName, boardPinObjName);
		// 			index ++;
		// 		}
		// 	}			
		// }
	}

	void ResetNetWires() {
		GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("netwire");
		foreach(GameObject wireObj in wireTemp)
		{
			LineRenderer lr = wireObj.GetComponent<LineRenderer>();
			lr.enabled = false;
			lr.positionCount = 0;
			Destroy(wireObj);
		}

		foreach(KeyValuePair<string, _Component> item in netDataObj.getInitialSchematicData())
		{
			List<_Pin> pins = item.Value.getPins();
			foreach(var pin in pins) {
				List<NetElement> netElement = pin.getNetElement();

				foreach(var target in netElement) {
					GameObject currentObject = GameObject.Find(item.Key);
					GameObject targetObject = GameObject.Find(target.component);
					netwire.createWireObject(getChildObject(currentObject, pin.id), getChildObject(targetObject, target.pinid));
				}
			}
		}
	}

	private Vector3 getComponentPinPosition(string componentName, string pinName)
    {
        Transform[] children = GameObject.Find(componentName).GetComponentsInChildren<Transform>();
        Vector3 result = Vector3.zero;
        foreach(Transform obj in children)     
        {
            if(obj.name == pinName) {
                result = obj.position;
            }
        }
        //Debug.Log("ComponentObject.cs - getTargetComponentPinPosition = " + result);
        return result;
    }
	
	void changeNetWiresPosition() {
		Vector3 wireEndPosition = new Vector3(0,0,0);
    	Vector3 wireStartPosition = new Vector3(0,0,0);
		GameObject[] netwires = GameObject.FindGameObjectsWithTag("netwire");
		GameObject[] components = GameObject.FindGameObjectsWithTag("component");

		foreach(GameObject wireObj in netwires)
		{
			foreach(GameObject componentObj in components) {
				string first = wireObj.name.Substring(wireObj.name.IndexOf(':')+1,wireObj.name.IndexOf(',')-wireObj.name.IndexOf(':')-1);
				string last = wireObj.name.Substring(wireObj.name.IndexOf(',')+1, wireObj.name.Length-wireObj.name.IndexOf(',')-1);
				string firstComponentName = first.Substring(0,first.IndexOf('-'));
				string lastComponentName = last.Substring(0,last.IndexOf('-'));

				if(String.Compare(lastComponentName, componentObj.name, true) == 0) // if this component is FromPin
				{
					if(last.Contains("connector0")) {
						wireEndPosition = getComponentPinPosition(componentObj.name, "connector0");
						wireEndPosition.x -= 5;
					} else if(last.Contains("connector1")) {
						wireEndPosition = getComponentPinPosition(componentObj.name, "connector1");
						wireEndPosition.x += 5;
					}
					LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
					wireLineRender.SetPosition(1, wireEndPosition);
				} else if(String.Compare(firstComponentName, componentObj.name) == 0) //else if this component is To Pin
				{
					if(first.Contains("connector0")) {
						wireStartPosition = getComponentPinPosition(componentObj.name, "connector0");
						wireStartPosition.x -= 5;
					}
					else if(first.Contains("connector1")) {
						wireStartPosition = getComponentPinPosition(componentObj.name, "connector1");
						wireStartPosition.x += 5;
					}
					LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
					wireLineRender.SetPosition(0, wireStartPosition);
				}
			}
		}
	}

	void changeConnectedWiresPosition() {
		Vector3 wireEndPosition = new Vector3(0,0,0);
    	Vector3 wireStartPosition = new Vector3(0,0,0);
		GameObject[] netwires = GameObject.FindGameObjectsWithTag("wire");
		GameObject[] components = GameObject.FindGameObjectsWithTag("component");

		foreach(GameObject wireObj in netwires)
		{
			foreach(GameObject componentObj in components) {
				string first = wireObj.name.Substring(0,wireObj.name.IndexOf(','));
				string last = wireObj.name.Substring(wireObj.name.IndexOf(','), wireObj.name.Length - wireObj.name.IndexOf(','));

				if(last.Contains(componentObj.name)) // if this component is FromPin
				{
					if(last.Contains("connector0")) {
						wireEndPosition = getComponentPinPosition(componentObj.name, "connector0");
						wireEndPosition.x -= 5;
					} else if(last.Contains("connector1")) {
						wireEndPosition = getComponentPinPosition(componentObj.name, "connector1");
						wireEndPosition.x += 5;
					}
					LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
					wireLineRender.SetPosition(1, wireEndPosition);
				} else if(first.Contains(componentObj.name)) //else if this component is To Pin
				{
					if(first.Contains("connector0")) {
						wireStartPosition = getComponentPinPosition(componentObj.name, "connector0");
						wireStartPosition.x -= 5;
					}
					else if(first.Contains("connector1")) {
						wireStartPosition = getComponentPinPosition(componentObj.name, "connector1");
						wireStartPosition.x += 5;
					}
					LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
					wireLineRender.SetPosition(0, wireStartPosition);
				}
			}
		}
	}

	public void setupAutoMode() {
		netDataObj.copyBuildDataToDebugData();
		//ResetAllConnectedWires();
		
		// GameObject[] temp = GameObject.FindGameObjectsWithTag("component");
		// int count = 0;
		// foreach(GameObject componentObj in temp) {
		// 	if(count<4) componentObj.transform.position = new Vector3(265, ParentPanel.transform.position.y+10, -80-count*110);
		// 	else componentObj.transform.position = new Vector3(415,ParentPanel.transform.position.y+10,-80-(count-4)*110);
		// 	count++;
		// }
		changeNetWiresPosition();
	}

	public void setupManualMode() {
		ResetAllConnectedWires();
		netDataObj.resetBuildNetData();
		if(modeToggleMenu.isManualMode()) {
			// auto complete connections
			Vector3 startPos;
			Vector3 endPos;
			string wireObjectName;

			GameObject[] temp = GameObject.FindGameObjectsWithTag("component");
			int count = 0;
        	foreach(GameObject componentObj in temp) {
				if(count<4) componentObj.transform.position = new Vector3(0,componentObj.transform.position.y,componentObj.transform.position.z);
				else componentObj.transform.position = new Vector3(100,componentObj.transform.position.y,componentObj.transform.position.z);
				count ++;
			}

			foreach(KeyValuePair<string, _Component> item in netDataObj.getCurrentBuildSchematicData())
			{
				List<_Pin> pins = item.Value.getPins();
				foreach(var pin in pins) {
					
					string connectedRowPos = pin.getConnectedBreadboardRowPosition();
					string connectedColPos = pin.getConnectedBreadboardColPosition();
					string boardPinObjName = "Pin" + connectedRowPos + "-" + connectedColPos;
					startPos = GameObject.Find(boardPinObjName).transform.position;
					endPos = Util.getChildObject(item.Value.label, pin.id).transform.position;
					wireObjectName = "Wire" + ":" + boardPinObjName + "," + item.Value.label + "-" + pin.id;
					virtualwire.createWireObject(startPos, endPos, wireObjectName, boardPinObjName);
				}
			}
		}
		changeNetWiresPosition();
	}
}