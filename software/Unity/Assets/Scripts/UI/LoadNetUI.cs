using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine.UI;
using System.Xml;

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

	public NetData netDataObj;

	Dictionary<string, _Component> netData;

	public DrawNetWire netwire;
    private bool alreadyWired = false;

    public void Start() {
        setWireObject();
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

	public void setupNet(string filePath)
	{
		NetDataHandler handler = new NetDataHandler();
		netData = handler.getNetData(filePath);
		netDataObj.setNetData(netData);
		
		//string netName = filePath.Substring(filePath.LastIndexOf("xml/"), filePath.Length);
		//Text fritzingNameText = GameObject.Find("FritzingName").GetComponent<Text>();
		//fritzingNameText.text = netName;

		//int numberOfComponent = netData.Count;
		//Debug.Log("numberOfComponent = " + numberOfComponent);
		int count = 0;
		GameObject component = null;

		foreach(KeyValuePair<string, _Component> item in netData)
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
				case "P":
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
					component.transform.position = new Vector3(265,ParentPanel.transform.position.y+10,-80-count*110);
				} else {
					component.transform.position = new Vector3(415,ParentPanel.transform.position.y+10,-80-(count-4)*110);
				}
				component.transform.localScale = new Vector3(0.6f, 0.6f, 1);

				List<_Pin> pins = item.Value.getPins();

			}
			count ++;
		}

		foreach(KeyValuePair<string, _Component> item in netData)
		{
			List<_Pin> pins = item.Value.getPins();
			foreach(var pin in pins) {
				List<NetElement> netElement = pin.getNetElement();
				//float dashSize = 4.0f;
				foreach(var target in netElement) {
					GameObject currentObject = GameObject.Find(item.Key);
					GameObject targetObject = GameObject.Find(target.component);

					netwire.createWireObject(getChildObject(currentObject, pin.id), getChildObject(targetObject, target.pinid));
					Debug.Log("\n\n\n\n\nFrom: " + currentObject.name + "-" + pin.id + "\n\n\n\n\n");
					Debug.Log("\n\n\n\n\nTo: " + targetObject.name + "-" + target.pinid + "\n\n\n\n\n");
					//wire.setFromPinObj(getChildObject(currentObject, pin.id));
					//wire.setToPinObj(getChildObject(targetObject, target.pinid));
					//DottedLine.DottedLine.Instance.DrawDottedLine(currentObject.transform.position, targetObject.transform.position);
				}
			}
		}
	}
}