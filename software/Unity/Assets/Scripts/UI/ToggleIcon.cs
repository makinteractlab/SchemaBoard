using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class ToggleIcon : MonoBehaviour {
    public static ToggleIcon instance;
	public Sprite resistorSymbol;
	public Sprite capacitorSymbol;
	public Sprite ledSymbol;
	public Sprite photoresistorSymbol;
	public Sprite vccSymbol;
	public Sprite inductorSymbol;
	public Sprite switchSymbol;
	public Sprite diodeSymbol;
	public Sprite zenerdiodeSymbol;
	public Sprite etcSymbol;

	public Sprite resistorImage;
	public Sprite capacitorImage;
	public Sprite ledImage;
	public Sprite photoresistorImage;
	public Sprite vccImage;
	public Sprite inductorImage;
	public Sprite switchImage;
	public Sprite diodeImage;
	public Sprite zenerdiodeImage;
	public Sprite etcImage;

	public Sprite onSprite;
	public Sprite offSprite;

	public ToggleAutoManual toggleMode;

	bool status;

	private string command = "";

    void Awake() {
        if (ToggleIcon.instance == null)
            ToggleIcon.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = false;
		//changeComponentIcon();
		this.GetComponent<Button>().onClick.AddListener(changeComponentIcon);
    }

	public bool IsFritzingIcon() {
		return status;
	}

	public void changeComponentIcon() {
		//gameObject.SetActive(true);
		// if(toggleMode.IsManualMode()) {
		// 	if(status) {
		// 		gameObject.GetComponent<Button>().image.sprite = onSprite;
		// 		showSchematicSymbol(true);
		// 		status = false;
		// 	} else {
		// 		gameObject.GetComponent<Button>().image.sprite = offSprite;
		// 		showSchematicSymbol(false);
		// 		status = true;
		// 	}
		// } else {
			string mode = "";
			if(status) {	//schematic
				gameObject.GetComponent<Button>().image.sprite = onSprite;
				showFritzingCircuitComponent(false);
				showSchematicCircuitComponent(true);
				status = false;
				mode = "schematic";
			} else {	//fritzing
				gameObject.GetComponent<Button>().image.sprite = offSprite;
				showSchematicCircuitComponent(false);
				showFritzingCircuitComponent(true);
				status = true;
				mode = "fritzing";
			}
		// }
		GameObject[] components = GameObject.FindGameObjectsWithTag("auto_prefab");
		foreach(var item in components) {
			SchComponentButton button = Util.getChildObject(item.name, "sch_button").GetComponent<SchComponentButton>();
			button.updateGlowIconEvent.Invoke(mode);
		}
	}

	private void showSchematicCircuitComponent(bool on) {
		GameObject[] prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_schematic");
		if(on) {
			foreach(GameObject prefab in prefabs) {
				prefab.transform.localScale = new Vector3(1,1,1);
			}
		} else {
			foreach(GameObject componentObj in prefabs) {
				componentObj.transform.localScale = new Vector3(0,0,0);
			}
		}
	}

	private void showFritzingCircuitComponent(bool on) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("circuit_prefab_fritzing");
		if(on) {
			foreach(GameObject componentObj in temp) {
				componentObj.transform.localScale = new Vector3(1,1,1);
			}
		} else {
			foreach(GameObject componentObj in temp) {
				componentObj.transform.localScale = new Vector3(0,0,0);
			}
		}
	}

	private void showSchematicSymbol(bool on) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("manual_prefab");
		
		if(on) {
			foreach(GameObject componentObj in temp) {
				string componentName = Util.removeDigit(componentObj.name);
				switch(componentName) {
					case "LED":
						Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = ledSymbol;
						break;
					case "R":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = resistorSymbol;
						break;
					case "C":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = capacitorSymbol;
						break;
					case "L":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = inductorSymbol;
						break;
					case "S":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = switchSymbol;
						break;
					case "LDR":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = photoresistorSymbol;
						break;
					case "D":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = diodeSymbol;
						break;
					case "ZD":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = zenerdiodeSymbol;
						break;
					case "VCC":
					case "BT":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = vccSymbol;
						break;
					default:
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = etcSymbol;
						break;
				}
			}
		} else {
			foreach(GameObject componentObj in temp) {
				string componentName = Util.removeDigit(componentObj.name);
				switch(componentName) {
					case "LED":
						Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = ledImage;
						break;
					case "R":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = resistorImage;
						break;
					case "C":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = capacitorImage;
						break;
					case "L":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = inductorImage;
						break;
					case "S":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = switchImage;
						break;
					case "LDR":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = photoresistorImage;
						break;
					case "D":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = diodeImage;
						break;
					case "ZD":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = zenerdiodeImage;
						break;
					case "VCC":
					case "BT":
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = vccImage;
						break;
					default:
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = etcImage;
						break;
				}
			}
		}
	}
}