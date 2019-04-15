using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	}

	public void setNetData(Dictionary<string, _Component> _componentsInCircuit) {
		componentsInCircuit = new Dictionary<string, _Component>(_componentsInCircuit);
	}

	public Dictionary<string, _Component> getNetData() {
		return componentsInCircuit;
	}

}
