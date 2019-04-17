using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NetData : MonoBehaviour {

	Dictionary<string, _Component> componentsInCircuit;
	// Use this for initialization
	void Start () {
		componentsInCircuit = new Dictionary<string, _Component>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void syncNetData(string _componentName, string _componentPinName, string _boardPinName) {
		string pin = _componentPinName.Substring(_componentPinName.IndexOf('-')+1, _componentPinName.Length-_componentPinName.IndexOf('-')-1);
		componentsInCircuit[_componentName].getPin(pin).breadboardPosition = Util.getChildObject(GameObject.Find(_boardPinName), "LineNumber").GetComponent<Text>().text;
		Debug.Log("test");
	}

	public void setNetData(Dictionary<string, _Component> _componentsInCircuit) {
		componentsInCircuit = new Dictionary<string, _Component>(_componentsInCircuit);
	}

	public Dictionary<string, _Component> getNetData() {
		return componentsInCircuit;
	}
	public List<string> getComponentNet(string _component) {
		List<string> result = new List<string>();
		foreach(var item in componentsInCircuit[_component].getPins()){
			result.Add(item.breadboardPosition);
		}
		return result;
	}

	public string getComponentPinNet(string _component, string _pin) {
		string result = componentsInCircuit[_component].getPin(_pin).breadboardPosition;
		return result;
	}

	public string getComponentGroundNet(string _component, string _pin) {
		string result = "";
		return result;
	}

	public string getComponentPowerNet(string _component, string _pin) {
		string result = "";
		return result;
	}
}
