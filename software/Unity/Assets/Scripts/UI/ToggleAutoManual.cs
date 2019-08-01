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
	public NetData netData;
	public HttpRequest http;
	Command cmd;

	bool autoMode;
	bool manualMode;
	string mode;
	bool init;

	//private string command = "";

    void Awake() {
        if (ToggleAutoManual.instance == null)
            ToggleAutoManual.instance = this;
    }
    // Use this for initialization
    void Start() {
		autoMode = true;
		this.GetComponent<Button>().onClick.AddListener(ModeChange);
		showManualMenu(false);
		comm.setAutoState();
		cmd = new Command();
		init = true;
    }

	public bool IsManualMode() {
		return manualMode;
	}

	public bool IsAutoMode() {
		return autoMode;
	}

	public void setAutoMode() {
		autoMode = false;
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
		if(autoMode) { // toggle autoMod --> manualMode
			gameObject.GetComponent<Button>().image.sprite = manualSprite;
			manualMode = true;
			showSchematicMenu(false);
			showManualMenu(true);
			loadNetUI.setupManualMode();
			comm.setManualState();
			// card.loadCircuitInfo(1);
			showAutoPrefabs(false);
			showManualPrefabs(true);
			autoMode = false;
			mode = "manual";

			GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
			foreach(var item in prefabButtons) {
				if(item.name.Contains("Component")) {
					ComponentButton manualbutton = item.GetComponent<ComponentButton>();
					manualbutton.resetComponentStateEvent.Invoke(mode); // manual>auto
				}
			}
			initAutoPinState();
		} else {
			// if some pins disconnected in manual mode, restore previous pins for that
			netData.recoverEmptyPosForPins();
			gameObject.GetComponent<Button>().image.sprite = autoSprite;
			manualMode = false;
			autoMode = true;
			mode = "auto";
			//showNetWire(false);
			showSchematicMenu(true);
			showManualMenu(false);
			loadNetUI.setupAutoMode();
			comm.setAutoState();
			showAutoPrefabs(true);
			showManualPrefabs(true);

			if(!init) {
				GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
				foreach(var item in prefabButtons) {
					if(item.name.Contains("sch_")) {
						SchComponentButton autobutton = item.GetComponent<SchComponentButton>();
						autobutton.resetAllStateEvent.Invoke(mode); // auto>manual
					}
				}
				// initManualPinState(); // need to fix!!
			} else {
				init = false;
			}
		}
		http.postJson(comm.getUrl()+"/set", cmd.resetAll());
	}

	private void initManualPinState() {
        GameObject[] sch_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_schematic_pin");
        GameObject[] fritz_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_fritzing_pin");
        GameObject[] common_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_common_pin");
        
        foreach(var item in sch_prefabs) {
            ComponentPins manualpin = item.GetComponent<ComponentPins>();
			manualpin.resetAllStateEvent.Invoke(mode); // manual>auto
        }

        foreach(var item in fritz_prefabs) {
			ComponentPins manualpin = item.GetComponent<ComponentPins>();
			manualpin.resetAllStateEvent.Invoke(mode); // manual>auto
        }

        foreach(var item in common_prefabs) {
            ComponentPins manualpin = item.GetComponent<ComponentPins>();
			manualpin.resetAllStateEvent.Invoke(mode); // manual>auto
        }
    }

	private void initAutoPinState() {
        GameObject[] sch_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_schematic");
        GameObject[] fritz_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_fritzing");
        GameObject[] pin_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_pin");
        
        foreach(var item in sch_prefabs) {
            if(item.name.Contains("connector")) {
				SchComponentPins pin = item.GetComponent<SchComponentPins>();
				pin.resetAllStateEvent.Invoke(mode); // auto>manual
			}
        }

        foreach(var item in fritz_prefabs) {
            if(item.name.Contains("connector")) {
				SchComponentPins pin = item.GetComponent<SchComponentPins>();
				pin.resetAllStateEvent.Invoke(mode); // auto>manual
			}
        }

        foreach(var item in pin_prefabs) {
            if(item.name.Contains("connector")) {
				SchComponentPins pin = item.GetComponent<SchComponentPins>();
				pin.resetAllStateEvent.Invoke(mode); // auto>manual
			}
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