using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAddNet : MonoBehaviour {

	public NetData netdata;
	// Use this for initialization
	void Start () {
		//changeComponentIcon();
		this.GetComponent<Button>().onClick.AddListener(addNetPosition);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void addNetPosition() {
		netdata.removebbNetPosition("Pin10-5");
		netdata.addbbNetPosition("Pin15-5", "SP8", "connector1");
	}
}
