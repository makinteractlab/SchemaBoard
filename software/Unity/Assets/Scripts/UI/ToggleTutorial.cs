using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class ToggleTutorial : MonoBehaviour {
    public static ToggleTutorial instance;
	public HttpRequest http;
	public Communication comm;
	Command cmd;
	public Sprite onSprite;
	public Sprite offSprite;
	public ToggleIcon icon;
	public Image background;
	public Button prevButton;
	public Button nextButton;
	public Button restartButton;
	public Button refreshButton;
	public Button selectAllButton;
	public Button autoManualButton;
	public Dropdown selectFileDropdown;
	bool status;

    void Awake() {
        if (ToggleTutorial.instance == null)
            ToggleTutorial.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = false;
		//changeComponentIcon();
		this.GetComponent<Button>().onClick.AddListener(tutorial);
		showButtons(false);
		cmd = new Command();
    }

	void showButtons(bool _onoff) {
		if(_onoff) {
			prevButton.transform.localScale = new Vector3(1,1,1);
			nextButton.transform.localScale = new Vector3(1,1,1);
			restartButton.transform.localScale = new Vector3(1,1,1);
			background.transform.localScale = new Vector3(1,1,1);
			// background.GetComponent<SpriteRenderer>().sortingOrder = 0;
			// background.GetComponent<SpriteRenderer>().sortingLayerName = "TutorialLayer";
			refreshButton.transform.localScale = new Vector3(0,0,0);
			selectAllButton.transform.localScale = new Vector3(0,0,0);
			autoManualButton.transform.localScale = new Vector3(0,0,0);
			selectFileDropdown.transform.localScale = new Vector3(0,0,0);
		} else {
			prevButton.transform.localScale = new Vector3(0,0,0);
			nextButton.transform.localScale = new Vector3(0,0,0);
			restartButton.transform.localScale = new Vector3(0,0,0);
			background.transform.localScale = new Vector3(0,0,0);
			refreshButton.transform.localScale = new Vector3(1,1,1);
			selectAllButton.transform.localScale = new Vector3(1,1,1);
			autoManualButton.transform.localScale = new Vector3(1,1,1);
			selectFileDropdown.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void tutorial() {
		if(status) {	//tutorial off
			status = false;
			this.GetComponent<Image>().sprite = offSprite;
			showButtons(status);
		} else {	//tutorial on
			status = true;
			this.GetComponent<Image>().sprite = onSprite;
			showButtons(status);
		}

		http.postJson(comm.getUrl()+"/set", cmd.resetAll());

		GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
		foreach(var item in prefabButtons) {
			if(item.name.Contains("sch_")) {
				SchComponentButton button = item.GetComponent<SchComponentButton>();
				button.resetAllStateEvent.Invoke("schematic");
			}
		}

		// initComponentGlow();
		initAutoPinGlow();
		comm.setSchCompPinClicked(false);

		// GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
		// foreach(var item in prefabButtons) {
		// 		if(item.name.Contains("sch_")) {
		// 			SchComponentButton button = item.GetComponent<SchComponentButton>();
		// 			button.resetAllStateEvent.Invoke("schematic");
		// 		}
		// }
	}

	public void initComponentGlow() {
		GameObject[] glowIcons;
        if(icon.IsFritzingIcon()) {
			glowIcons = GameObject.FindGameObjectsWithTag("fritzing_glow");
		} else {
			glowIcons = GameObject.FindGameObjectsWithTag("schematic_glow");
		}

		foreach(var item in glowIcons) {
			item.transform.localScale = new Vector3(0,0,0);
		}
    }

	private void initAutoPinGlow() {
        GameObject[] sch_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_schematic");
        GameObject[] fritz_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_fritzing");
        GameObject[] pin_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_pin");
        
        foreach(var item in sch_prefabs) {
            if(item.name.Contains("connector"))
                item.GetComponent<Image>().sprite = comm.DefaultPinSprite;
        }

        foreach(var item in fritz_prefabs) {
            if(item.name.Contains("connector"))
                item.GetComponent<Image>().sprite = comm.DefaultPinSprite;
        }

        foreach(var item in pin_prefabs) {
            if(item.name.Contains("connector"))
                item.GetComponent<Image>().sprite = comm.DefaultPinSprite;
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
}