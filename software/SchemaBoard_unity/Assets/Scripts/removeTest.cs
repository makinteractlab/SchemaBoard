using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class removeTest : MonoBehaviour {
	public NetData netdata;
	// Use this for initialization
	void Start () {
		//changeComponentIcon();
		this.GetComponent<Button>().onClick.AddListener(removeNetPosition);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void removeNetPosition() {
		netdata.removebbNetPosition("Pin15-5");
	}
}
