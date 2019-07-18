using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class ToggleAutoManual : MonoBehaviour {
    public static ToggleAutoManual instance;
	public Sprite autoSprite;
	public Sprite manualSprite;
	public LoadNetUI loadNetUI;
	public Communication comm;
	public TutorialCard card;

	bool status;
	bool manualMode;

	private string command = "";

    void Awake() {
        if (ToggleAutoManual.instance == null)
            ToggleAutoManual.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = false;
		this.GetComponent<Button>().onClick.AddListener(ModeChange);
		showManualUI(false);
		comm.setAutoState();
    }

	public bool isManualMode() {
		return manualMode;
	}

	public void setAutoMode() {
		status = false;
		ModeChange();
	}

	void ModeChange() {
		//gameObject.SetActive(true);
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = manualSprite;
			manualMode = true;
			showNetWire(true);
			showSchematicUI(false);
			showManualUI(true);
			loadNetUI.setupManualMode();
			comm.setManualState();
			// card.loadCircuitInfo(1);
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = autoSprite;
			manualMode = false;
			showNetWire(false);
			showSchematicUI(true);
			showManualUI(false);
			loadNetUI.setupAutoMode();
			comm.setAutoState();
			status = true;
		}
	}

	private void showManualUI(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("manual");
		
        foreach(GameObject obj in temp) {
			if(onoff) {
				obj.transform.localScale = new Vector3(1,1,1);
			} else {
				obj.transform.localScale = new Vector3(0,0,0);
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

	private void showSchematicUI(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("schematic");
		
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

	private void hideSchematicCircuit(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("schcomponent");
		foreach(GameObject obj in temp) {
			if(onoff) {
				obj.transform.localScale = new Vector3(1,1,1);
			} else {
				obj.transform.localScale = new Vector3(0,0,0);
			}
		}

		GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("schwire");
		foreach(GameObject obj in wireTemp) {
			if(onoff) {
				obj.transform.localScale = new Vector3(1,1,1);
			} else {
				obj.transform.localScale = new Vector3(0,0,0);
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