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
	public LoadNetUI loadNetUI;
	public Communication comm;
	public TutorialCard card;

	bool status;
	bool buildMode;

	private string command = "";

    void Awake() {
        if (ToggleDebugBuild.instance == null)
            ToggleDebugBuild.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = false;
		this.GetComponent<Button>().onClick.AddListener(ModeChange);
		showBuildMenu(false);
		comm.setDebugState();
    }

	public bool isBuildMode() {
		return buildMode;
	}

	public void setDebugMode() {
		status = false;
		ModeChange();
	}

	void ModeChange() {
		//gameObject.SetActive(true);
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = buildSprite;
			buildMode = true;
			showNetWire(false);
			showDebugMenu(false);
			showBuildMenu(true);
			loadNetUI.setupBuildMode();
			comm.setBuildState();
			card.loadCircuitInfo(1);
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = debugSprite;
			buildMode = false;
			showNetWire(true);
			showDebugMenu(true);
			showBuildMenu(false);
			loadNetUI.setupDebugMode();
			comm.setDebugState();
			status = true;
		}
	}

	private void showBuildMenu(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("buildMode");
		
        foreach(GameObject componentObj in temp) {
			if(onoff) {
				componentObj.transform.localScale = new Vector3(1,1,1);
			} else {
				componentObj.transform.localScale = new Vector3(0,0,0);
			}
        }

		// if(onoff) {
		// 	GameObject.Find("buildBackground").transform.localScale = new Vector3(0,0,0);
		// 	GameObject.Find("debugBackground").transform.localScale = new Vector3(1,1,1);
		// } else {
		// 	GameObject.Find("debugBackground").transform.localScale = new Vector3(0,0,0);
		// 	GameObject.Find("buildBackground").transform.localScale = new Vector3(1,1,1);
		// }
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

		// if(onoff) {
		// 	GameObject.Find("buildBackground").transform.localScale = new Vector3(0,0,0);
		// 	GameObject.Find("debugBackground").transform.localScale = new Vector3(1,1,1);
		// } else {
		// 	GameObject.Find("debugBackground").transform.localScale = new Vector3(0,0,0);
		// 	GameObject.Find("buildBackground").transform.localScale = new Vector3(1,1,1);
		// }
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