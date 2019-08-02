using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

public class ToggleTutorial : MonoBehaviour {
    public static ToggleTutorial instance;
	public HttpRequest http;
	public Communication comm;
	public NetData netdata;
	Command cmd;
	public Sprite onSprite;
	public Sprite offSprite;
	private Sprite prevGlowIconSprite;
	private Sprite currGlowIconSprite;
	private Sprite firstComponentSprite;
	public Sprite selectedGlowIconSprite;
	
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
	GameObject selectedComponent;
	GameObject prevSelectedComponent;
	Dictionary<string, _Component> data;
	List<string> components;
	bool init;
	public int index;

    void Awake() {
        if (ToggleTutorial.instance == null)
            ToggleTutorial.instance = this;
    }

    void Start() {
		status = false;
		//changeComponentIcon();
		this.GetComponent<Button>().onClick.AddListener(tutorial);
		showButtons(false);
		cmd = new Command();
		selectedComponent = null;
		components = new List<string>();
		init = true;
    }

	public List<string> getComponentNameList() {
		return components;
	}

	public void initSelectedComponent() {
		selectedComponent = null;
		// glow off
		// postJson for reset
	}

	public GameObject getSelectedComponent() {
		return selectedComponent;
	}

	void initAll() {
		GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
		foreach(var item in prefabButtons) {
			if(item.name.Contains("sch_")) {
				SchComponentButton button = item.GetComponent<SchComponentButton>();
				button.resetAllStateEvent.Invoke("schematic");
			}
		}

		initAutoPinGlow();
		comm.setSchCompPinClicked(false);

		GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "schematic_glow");
		firstComponentGlowIcon.GetComponent<Image>().sprite = firstComponentSprite;
	}

	public void setSelectedComponent(int _index) {
		index = _index;
		if(index == 0) {
			initAll();
		}
		prevSelectedComponent = selectedComponent;
		selectedComponent = GameObject.Find(components[_index]);
		List<string> pins = new List<string>(netdata.getComponentPosition(selectedComponent.name));

		int[] boardPins = new int[2];
		boardPins = netdata.getMultiplePinsPosition(pins);
		http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));
		
		Wait (0.5f, () => {
			Debug.Log("0.5 seconds is lost forever");
		});

		string firstPinPos = netdata.getComponentFirstPinRowPosition(selectedComponent.name);
		http.postJson(comm.getUrl()+"/set", cmd.singlePinBlink(Int32.Parse(firstPinPos)));
		// glow on
		if(icon.IsFritzingIcon()) {
			GameObject currGlowIcon = Util.getChildObject("SCH_"+selectedComponent.name, "fritzing_glow");
			currGlowIconSprite = currGlowIcon.GetComponent<Image>().sprite;
			currGlowIcon.transform.localScale = new Vector3(1,1,1);
			currGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;
			
			if(prevSelectedComponent) {
				GameObject prevGlowIcon = Util.getChildObject("SCH_"+prevSelectedComponent.name, "fritzing_glow");
				prevGlowIcon.GetComponent<Image>().sprite = prevGlowIconSprite;
				// prevGlowIcon.transform.localScale = new Vector3(0,0,0);
			}
			prevGlowIconSprite = currGlowIconSprite;
		} else {
			GameObject currGlowIcon = Util.getChildObject("SCH_"+selectedComponent.name, "schematic_glow");
			currGlowIconSprite = currGlowIcon.GetComponent<Image>().sprite;
			currGlowIcon.transform.localScale = new Vector3(1,1,1);
			currGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;

			if(prevSelectedComponent) {
				GameObject prevGlowIcon = Util.getChildObject("SCH_"+prevSelectedComponent.name, "schematic_glow");
				prevGlowIcon.GetComponent<Image>().sprite = prevGlowIconSprite;
				// prevGlowIcon.transform.localScale = new Vector3(0,0,0);
			}
			prevGlowIconSprite = currGlowIconSprite;
		}
	}

	public GameObject getPrevSelectedComponent() { 
		return prevSelectedComponent;
	}

	public void setPrevSelectedComponent(GameObject _obj) { 
		prevSelectedComponent = _obj;
	}

	public Dictionary<string, _Component> getSchematicData() {
		return data;
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

			//firstpin sprite

			GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "schematic_glow");
			firstComponentGlowIcon.GetComponent<Image>().sprite = firstComponentSprite;

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

		} else {	//tutorial on
			if(init) {
				data = netdata.getCurrentDebugSchematicData();
				foreach(var item in data) {
					components.Add(item.Key);
				}
				init = false;
			}
			status = true;
			this.GetComponent<Image>().sprite = onSprite;
			showButtons(status);

			GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "schematic_glow");
			firstComponentSprite = firstComponentGlowIcon.GetComponent<Image>().sprite;

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

			setSelectedComponent(0);
		}

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

	public void Wait(float seconds, Action action){
		StartCoroutine(_wait(seconds, action));
	}
	IEnumerator _wait(float time, Action callback){
		yield return new WaitForSeconds(time);
		callback();
	}
}