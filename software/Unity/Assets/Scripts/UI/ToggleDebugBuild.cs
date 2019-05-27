using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class ToggleDebugBuild : MonoBehaviour {
    public static ToggleDebugBuild instance;
	public Sprite debugSprite;
	public Sprite buildSprite;

	bool status;

	private string command = "";

    void Awake() {
        if (ToggleDebugBuild.instance == null)
            ToggleDebugBuild.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = false;
		this.GetComponent<Button>().onClick.AddListener(ModeChange);
    }

	void ModeChange() {
		//gameObject.SetActive(true);
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = buildSprite;
			showNetWire(false);
			showDebugMenu(false);
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = debugSprite;
			showNetWire(true);
			showDebugMenu(true);
			status = true;
		}
	}

	private void showDebugMenu(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("debugMenu");
        foreach(GameObject componentObj in temp) {
			if(onoff) {
				componentObj.transform.localScale = new Vector3(1,1,1);
			} else {
				componentObj.transform.localScale = new Vector3(0,0,0);
			}
        }
	}

	private void showNetWire(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("component");
        foreach(GameObject componentObj in temp) {
			GameObject[] netwireTemp = GameObject.FindGameObjectsWithTag("netwire");
			foreach(GameObject netwireObj in netwireTemp) {
				if( netwireObj.name.Contains(componentObj.name) ) {
					LineRenderer lr = netwireObj.GetComponent<LineRenderer>();
					lr.enabled = onoff;
				}
			}
        }
	}
}