using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
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
	public int[] getComponentPinsNet(string _component) {
		List<string> resultPins = new List<string>();
		int left = 0;
		int right = 1;
		int[] result = Enumerable.Repeat(0, 2).ToArray();
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();

		foreach(var item in componentsInCircuit[_component].getPins()){
			resultPins.Add(item.breadboardPosition);
		}

		foreach(var item in resultPins) boardBinary[int.Parse(item)-1] = '1';

		for(int i=0; i<16; i++)
			if(boardBinary[i] == '1') result[left] += (int)Math.Pow(2, i);

		for(int i=16; i<32; i++)
			if(boardBinary[i] == '1') result[right] += (int)Math.Pow(2, i-16);

		return result;
	}

	public string getComponentSinglePinPosition(string _component, string _pin) {
		string result = componentsInCircuit[_component].getPin(_pin).breadboardPosition;
		return result;
	}

	public string getComponentFirstPinPosition(string _component) {
		string result = componentsInCircuit[_component].getFirstPin().breadboardPosition;
		return result;
	}

	// public void addToResultPins(ref List<string> resultPins, string _component, string _pin) {

	// 	if(componentsInCircuit[_component].getPin(_pin).netElements.Count < 1) return;

	// 	foreach(var element in componentsInCircuit[_component].getPin(_pin).netElements) {	
	// 		resultPins.Add(componentsInCircuit[element.component].getPin(element.pinid).breadboardPosition);
	// 		addToResultPins(ref resultPins, element.component, element.pinid);
	// 	}
	// }

	public int[] getAllNetForPin(string _component, string _pin) {
		char[] boardBinary = Enumerable.Repeat('0', 32).ToArray();
		int left = 0;
		int right = 1;

		int[] result = Enumerable.Repeat(0, 2).ToArray();

		List<string> resultPins = new List<string>();
		resultPins.Add(componentsInCircuit[_component].getPin(_pin).breadboardPosition);

		// component pin의 net element에 들어있는 컴포넌트 핀의 breadboard pin 가져오기
		foreach(var element in componentsInCircuit[_component].getPin(_pin).netElementsAll) {	
			resultPins.Add(componentsInCircuit[element.component].getPin(element.pinid).breadboardPosition);
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
