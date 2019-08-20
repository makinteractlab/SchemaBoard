using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class ToggleIcon : MonoBehaviour {
    public static ToggleIcon instance;

	public Sprite onSprite;
	public Sprite offSprite;

	public ToggleAutoManual toggleMode;

	bool status;

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
		GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
		foreach(var item in prefabButtons) {
			if(toggleMode.IsAutoMode()) {
				if(item.name.Contains("sch_")) {
					SchComponentButton button = item.GetComponent<SchComponentButton>();
					button.updateGlowIconEvent.Invoke(mode);
				}
			} else {
				if(item.name.Contains("Component")) {
					ComponentButton manaulbutton = item.GetComponent<ComponentButton>();
					manaulbutton.updateGlowIconEvent.Invoke(mode);
				}
			}
		}
		GameObject.Find("TutorialToggle").GetComponent<ToggleTutorial>().updateGlowIconEvent.Invoke(mode);
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
}