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
		showManualMenu(false);
		comm.setAutoState();
    }

	public bool IsManualMode() {
		return manualMode;
	}

	public void setAutoMode() {
		status = false;
		ModeChange();
	}

	void showAutoPrefabs(bool show) {
		GameObject[] prefabs = GameObject.FindGameObjectsWithTag("auto_prefab");
		GameObject[] wires = GameObject.FindGameObjectsWithTag("schwire");
		if(show) {
			foreach(var item in prefabs) {
				item.transform.localScale = new Vector3(1,1,1);
			}
			foreach(var item in wires) {
				item.transform.localScale = new Vector3(1,1,1);
			}
		} else {
			foreach(var item in prefabs) {
				item.transform.localScale = new Vector3(0,0,0);
			}
			foreach(var item in wires) {
				item.transform.localScale = new Vector3(0,0,0);
			}
		}
	}

	void showManualPrefabs(bool show) {
		GameObject[] prefabs = GameObject.FindGameObjectsWithTag("manual_prefab");
		GameObject[] netwires = GameObject.FindGameObjectsWithTag("netwire");
		GameObject[] wires = GameObject.FindGameObjectsWithTag("wire");
		if(show) {
			foreach(var item in prefabs) {
				item.transform.localScale = new Vector3(1,1,1);
			}
			foreach(var item in netwires) {
				item.transform.localScale = new Vector3(1,1,1);
			}
			foreach(var item in wires) {
				item.transform.localScale = new Vector3(1,1,1);
			}
		} else {
			foreach(var item in prefabs) {
				item.transform.localScale = new Vector3(0,0,0);
			}
			foreach(var item in netwires) {
				item.transform.localScale = new Vector3(0,0,0);
			}
			foreach(var item in wires) {
				item.transform.localScale = new Vector3(0,0,0);
			}
		}
	}

	void ModeChange() {
		//gameObject.SetActive(true);
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = manualSprite;
			manualMode = true;
			showSchematicMenu(false);
			showManualMenu(true);
			loadNetUI.setupManualMode();
			comm.setManualState();
			// card.loadCircuitInfo(1);
			showAutoPrefabs(false);
			showManualPrefabs(true);
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = autoSprite;
			manualMode = false;
			//showNetWire(false);
			showSchematicMenu(true);
			showManualMenu(false);
			loadNetUI.setupAutoMode();
			comm.setAutoState();
			showAutoPrefabs(true);
			showManualPrefabs(true);
			status = true;
		}
	}

	private void showManualMenu(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("manual");
		
        foreach(GameObject obj in temp) {
			if(onoff) {
				obj.transform.localScale = new Vector3(1,1,1);
			} else {
				obj.transform.localScale = new Vector3(0,0,0);
			}
        }
	}

	private void showSchematicMenu(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("schematic");
		
        foreach(GameObject componentObj in temp) {
			if(onoff) {
				componentObj.transform.localScale = new Vector3(1,1,1);
			} else {
				componentObj.transform.localScale = new Vector3(0,0,0);
			}
        }
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
		GameObject[] temp = GameObject.FindGameObjectsWithTag("manual_prefab");
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