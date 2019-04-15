using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NetDataHandler handler = new NetDataHandler();
		handler.getNetData("D:/Works/Git/test/test_netlist.xml");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
