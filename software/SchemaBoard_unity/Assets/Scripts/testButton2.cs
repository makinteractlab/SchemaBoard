using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testButton2 : MonoBehaviour {

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
		netdata.removebbNetPosition("Pin11-5");
		netdata.addbbNetPosition("Pin16-5", "CP6", "connector0");
	}
}
