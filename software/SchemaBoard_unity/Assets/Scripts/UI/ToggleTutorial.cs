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
	public TutorialPrevButton prevButtonObj;
	Command cmd;
	public Sprite onSprite;
	public Sprite offSprite;
	// private Sprite prevGlowIconSprite;
	// private Sprite currGlowIconSprite;
	public Sprite glowIconSprite;
	// private Sprite firstComponentFritzingSprite;
	// private Sprite firstComponentSchematicSprite;
	// private Sprite lastComponentFritzingSprite;
	// private Sprite lastComponentSchematicSprite;
	public Sprite selectedGlowIconSprite;
	public Sprite prevSelectedPinSprite;
	public Sprite currSelectedPinSprite;
	
	public ToggleIcon icon;
	public Image background;
	public Button prevButton;
	public Button nextButton;
	public Button restartButton;
	public Button refreshButton;
	public Button selectAllButton;
	public Button autoManualButton;
	public Dropdown selectFileDropdown;
	public Text stepInfo;
	bool status;
	GameObject selectedComponent;
	GameObject prevSelectedComponent;
	Dictionary<string, _Component> data;
	// List<Dictionary<string, string>> nets;
	List<List<string[]>> nets;
	List<string> components;
	List<string> gndList;
	List<string> pwrList;
	bool init;
	public int index;
	public int totalSteps;
	public int componentCount;
	public int netCount;
	public int wireCount;
	// bool travelComponent;
	public bool travelNet;

	public UnityAction<string> updateGlowIconAction;
    public IconToggleEvent updateGlowIconEvent;

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
		gndList = new List<string>();
		pwrList = new List<string>();
		init = true;
		travelNet = false;
		totalSteps = 0;

		updateGlowIconAction = new UnityAction<string>(updateGlowIcons);
        updateGlowIconEvent = new IconToggleEvent();
	    updateGlowIconEvent.AddListener(updateGlowIconAction);
    }

	public void updateGlowIcons(string _state) {
		GameObject[] fritzingGlow = GameObject.FindGameObjectsWithTag("fritzing_glow");
		GameObject[] schematicGlow = GameObject.FindGameObjectsWithTag("schematic_glow");
		
		foreach(var icon in fritzingGlow) {
			icon.GetComponent<Image>().sprite = glowIconSprite;
			icon.transform.localScale = new Vector3(0,0,0);
		}
		foreach(var icon in schematicGlow) {
			icon.GetComponent<Image>().sprite = glowIconSprite;
			icon.transform.localScale = new Vector3(0,0,0);
		}

		if(index <= componentCount) {
			if(prevButtonObj.clicked) {
				if(_state == "fritzing") {
					foreach(var icon in fritzingGlow) {
						for(int i=index; i>=0; i--) {
							if(icon.transform.parent.name.CompareTo("SCH_"+components[i])==0) {
								if(i == index) {
									icon.GetComponent<Image>().sprite = selectedGlowIconSprite;
								}
								icon.transform.localScale = new Vector3(1,1,1);
							}
						}
					}
				} else {
					foreach(var icon in schematicGlow) {
						for(int i=index; i>=0; i--) {
							if(icon.transform.parent.name.CompareTo("SCH_"+components[i])==0) {
								if(i == index) {
									icon.GetComponent<Image>().sprite = selectedGlowIconSprite;
								}
								icon.transform.localScale = new Vector3(1,1,1);
							}
						}
					}
				}
			} else {
				if(_state == "fritzing") {
					foreach(var icon in fritzingGlow) {
						for(int i=0; i<=index; i++) {
							if(icon.transform.parent.name.CompareTo("SCH_"+components[i])==0) {
								if(i == index) {
									icon.GetComponent<Image>().sprite = selectedGlowIconSprite;
								}
								icon.transform.localScale = new Vector3(1,1,1);
							}
						}
					}
				} else {
					foreach(var icon in schematicGlow) {
						for(int i=0; i<=index; i++) {
							if(icon.transform.parent.name.CompareTo("SCH_"+components[i])==0) {
								if(i == index) {
									icon.GetComponent<Image>().sprite = selectedGlowIconSprite;
								}
								icon.transform.localScale = new Vector3(1,1,1);
							}
						}
					}
				}
			}
		}
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
	}

	public void setSelectedComponent(int _index) {
		int[] boardPins = new int[2];
		index = _index;
		if(index == 0) {
			stepInfo.text = "Start: Step 1";
		} else if(index == totalSteps-1) {
			stepInfo.text = "Done: Final Step";			
		} else {
			stepInfo.text = "Step " + (index+1);
		}

		if(index == 0) {
			initAll();
			prevSelectedComponent = null;
			selectedComponent = null;
			if(icon.IsFritzingIcon()) {
				GameObject selectedComponentGlowIcon = Util.getChildObject("SCH_"+components[index], "fritzing_glow");
				selectedComponentGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;
			} else {
				GameObject selectedComponentGlowIcon = Util.getChildObject("SCH_"+components[index], "schematic_glow");
				selectedComponentGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;
			}
		} else if(index == componentCount-1 && prevButtonObj.clicked) {
			initAll();
			if(icon.IsFritzingIcon()) {
				GameObject selectedComponentGlowIcon = Util.getChildObject("SCH_"+components[componentCount-wireCount-1], "fritzing_glow");
				selectedComponentGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;
			} else {
				GameObject selectedComponentGlowIcon = Util.getChildObject("SCH_"+components[componentCount-wireCount-1], "schematic_glow");
				selectedComponentGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;
			}
		} else if(index == componentCount-wireCount && !prevButtonObj.clicked){
			initAll();
			if(icon.IsFritzingIcon()) {
				GameObject selectedComponentGlowIcon = Util.getChildObject("SCH_"+components[index-1], "fritzing_glow");
				selectedComponentGlowIcon.GetComponent<Image>().sprite = glowIconSprite;
			} else {
				GameObject selectedComponentGlowIcon = Util.getChildObject("SCH_"+components[index-1], "schematic_glow");
				selectedComponentGlowIcon.GetComponent<Image>().sprite = glowIconSprite;
			}
		} else if(index == totalSteps-1) {
			GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
			foreach(var item in prefabButtons) {
				if(item.name.Contains("sch_")) {
					SchComponentButton button = item.GetComponent<SchComponentButton>();
					button.resetAllStateEvent.Invoke("schematic");
				}
			}
		}

		if(index >= componentCount) { // should start travel nets
			GameObject[] autoPrefabs = GameObject.FindGameObjectsWithTag("auto_prefab");

			int netIndex = index-componentCount;

			if(index < totalSteps-1)
				stepInfo.text += "\n\nLet's check every component is connected correctly.";
			
			boardPins = netdata.getPositionForNet(nets[netIndex]);
			http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));
			
			if(prevButtonObj.clicked) {
				if(netIndex < nets.Count-1) {
					// foreach(var item in nets[netIndex+1]) {
					for(int i=0; i<nets[netIndex+1].Count; i++) {
						Util.getChildObject("SCH_"+nets[netIndex+1][i][1], nets[netIndex+1][i][2]).GetComponent<Image>().sprite = prevSelectedPinSprite;
						// gnd랑 pwr이랑 아이콘 돌려놔야 함
						if(nets[netIndex+1][0][0].Contains("GND")) {
							if(gndList.Count>0) {
								foreach(var element in gndList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						} else if(nets[netIndex+1][0][0].Contains("3V") || nets[netIndex+1][0][0].Contains("9V")) {
							if(pwrList.Count>0) {
								foreach(var element in pwrList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						}
					}
				}
				for(int i=0; i<nets[netIndex].Count; i++) {
					Util.getChildObject("SCH_"+nets[netIndex][i][1], nets[netIndex][i][2]).GetComponent<Image>().sprite = currSelectedPinSprite;
					if(nets[netIndex][0][0].Contains("GND")) {
						if(gndList.Count>0) {
							foreach(var element in gndList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					} else if(nets[netIndex][0][0].Contains("3V") || nets[netIndex][0][0].Contains("9V")) {
						if(pwrList.Count>0) {
							foreach(var element in pwrList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					}
				}

			} else {
				if(netIndex > 0) {
					for(int i=0; i<nets[netIndex-1].Count; i++) {
						Util.getChildObject("SCH_"+nets[netIndex-1][i][1], nets[netIndex-1][i][2]).GetComponent<Image>().sprite = prevSelectedPinSprite;
						// gnd랑 pwr이랑 아이콘 돌려놔야 함
						if(nets[netIndex-1][0][0].Contains("GND")) {
							if(gndList.Count>0) {
								foreach(var element in gndList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						} else if(nets[netIndex-1][0][0].Contains("3V") || nets[netIndex-1][0][0].Contains("9V")) {
							if(pwrList.Count>0) {
								foreach(var element in pwrList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						}
					}
				}
				for(int i=0; i<nets[netIndex].Count; i++) {
					Util.getChildObject("SCH_"+nets[netIndex][i][1], nets[netIndex][i][2]).GetComponent<Image>().sprite = currSelectedPinSprite;
					if(nets[netIndex][0][0].Contains("GND")) {
						if(gndList.Count>0) {
							foreach(var element in gndList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					} else if(nets[netIndex][0][0].Contains("3V") || nets[netIndex][0][0].Contains("9V")){
						if(pwrList.Count>0) {
							foreach(var element in pwrList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					}
				}
			}
		} else {	// travel components
			prevSelectedComponent = selectedComponent;
			selectedComponent = GameObject.Find("SCH_"+components[_index]);
			List<string> pins = new List<string>(netdata.getComponentPosition(components[_index]));

			boardPins = netdata.getMultiplePinsPosition(pins);
			http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));

			Wait (0.5f, () => {
				Debug.Log("0.5 seconds is lost forever");
			});

			// string firstPinPos = netdata.getComponentFirstPinRowPosition(components[_index]);
			// http.postJson(comm.getUrl()+"/set", cmd.singlePinBlink(Int32.Parse(firstPinPos)));
			
			if(selectedComponent.name.Contains("wire")) {
				stepInfo.text += "\n\nConnect wire (" + Util.getDigit(selectedComponent.name) + "/" + wireCount + ")\n" +"where the light is on";
			} else {
				string firstPinPos = netdata.getComponentFirstPinRowPosition(components[_index]);
				http.postJson(comm.getUrl()+"/set", cmd.singlePinBlink(Int32.Parse(firstPinPos)));
			// glow on				
				string value = netdata.debugNetData[components[index]].getValue();
				string componentName = netdata.debugNetData[components[index]].label;
				componentName = Util.removeDigit(componentName);
				switch(componentName) {
					case "R":
						componentName = "resistor";
						break;
					case "C":
					case "CP":
						componentName = "capacitor";
						break;
					case "L":
						componentName = "inductor";
						break;
					case "LED":
						value = "LED";
						componentName = "";
						break;
					case "SW":
						value = "switch";
						componentName = "";
						break;
					case "LDR":
						value = "photoresistor";
						componentName = "";
						break;
					case "Q":
						componentName = "transistor";
						break;
					case "SP":
						value = "speaker";
						componentName = "";
						break;
					case "D":
						componentName = "diode";
						break;
					case "GND":
						componentName = "ground";
						value = "";
						break;
					case "PWR":
						componentName = "power";
						break;
					case "BT":
						componentName = "battery";
						break;
					case "RERAY":
						value = "relay";
						componentName = "";
						break;
					case "U":
					case "U-":
						componentName = "";
						break;
				}
				stepInfo.text += "\n\nPlace the " + value + " " + componentName;
			
				if(icon.IsFritzingIcon()) {
					GameObject currGlowIcon = Util.getChildObject(selectedComponent.name, "fritzing_glow");
					// currGlowIconSprite = currGlowIcon.GetComponent<Image>().sprite;
					currGlowIcon.transform.localScale = new Vector3(1,1,1);
					currGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;
					
					if(prevSelectedComponent && !prevSelectedComponent.name.Contains("wire")) {
						GameObject prevGlowIcon = Util.getChildObject(prevSelectedComponent.name, "fritzing_glow");
						prevGlowIcon.GetComponent<Image>().sprite = glowIconSprite;
						// prevGlowIcon.transform.localScale = new Vector3(0,0,0);
					}
					// prevGlowIconSprite = glowIconSprite; //currGlowIconSprite;
				} else {
					GameObject currGlowIcon = Util.getChildObject(selectedComponent.name, "schematic_glow");
					// currGlowIconSprite = currGlowIcon.GetComponent<Image>().sprite;
					currGlowIcon.transform.localScale = new Vector3(1,1,1);
					currGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;
					
					if(prevSelectedComponent && !prevSelectedComponent.name.Contains("wire")) {
						GameObject prevGlowIcon = Util.getChildObject(prevSelectedComponent.name, "schematic_glow");
						prevGlowIcon.GetComponent<Image>().sprite = glowIconSprite;
						// prevGlowIcon.transform.localScale = new Vector3(0,0,0);
					}
					// prevGlowIconSprite = glowIconSprite; //currGlowIconSprite;
				}
			}
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
			stepInfo.transform.localScale = new Vector3(1,1,1);
			background.transform.localScale = new Vector3(1,1,1);
			refreshButton.transform.localScale = new Vector3(0,0,0);
			selectAllButton.transform.localScale = new Vector3(0,0,0);
			autoManualButton.transform.localScale = new Vector3(0,0,0);
			selectFileDropdown.transform.localScale = new Vector3(0,0,0);
		} else {
			prevButton.transform.localScale = new Vector3(0,0,0);
			nextButton.transform.localScale = new Vector3(0,0,0);
			restartButton.transform.localScale = new Vector3(0,0,0);
			stepInfo.transform.localScale = new Vector3(0,0,0);
			background.transform.localScale = new Vector3(0,0,0);
			refreshButton.transform.localScale = new Vector3(1,1,1);
			selectAllButton.transform.localScale = new Vector3(1,1,1);
			autoManualButton.transform.localScale = new Vector3(1,1,1);
			selectFileDropdown.transform.localScale = new Vector3(1,1,1);
		}
	}

	public bool isInTutorial() {
		return status;
	}

	public void tutorial() {
		if(status) {	//tutorial off
			status = false;
			this.GetComponent<Image>().sprite = offSprite;
			showButtons(status);
			http.postJson(comm.getUrl()+"/set", cmd.resetAll());
			initAll();
			prevSelectedComponent = null;
		} else {	//tutorial on
			components.Clear();
			wireCount = 0;
			// if(init) {
			data = netdata.getCurrentDebugSchematicData();
			foreach(var item in data) {
				if(item.Key.Contains("U")) {
					components.Add(item.Key);
				}
			}
			foreach(var item in data) {
				if(item.Key.Contains("U")){
					Debug.Log("chip is already added");
				} else {
					components.Add(item.Key);
				}
			}
			GameObject[] autoPrefabs = GameObject.FindGameObjectsWithTag("auto_prefab");
			foreach(var item in autoPrefabs) {
				if(item.name.Contains("GND")) {
					gndList.Add(item.name);
				} else if(item.name.Contains("PWR")) {
					pwrList.Add(item.name);
				}
			}

			nets = new List<List<string[]>>(netdata.getAllNetList());

			netCount = nets.Count;
			componentCount = components.Count;

			foreach(var component in components) {
				if(component.Contains("wire"))
					wireCount++;
			}

			totalSteps = netCount + componentCount;
				// init = false;
			// }
			status = true;
			this.GetComponent<Image>().sprite = onSprite;
			showButtons(status);

			// if(icon.IsFritzingIcon()) {
			// 	GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "fritzing_glow");
			// 	firstComponentFritzingSprite = firstComponentGlowIcon.GetComponent<Image>().sprite;
			// 	GameObject lastComponentGlowIcon = Util.getChildObject("SCH_"+components[componentCount-wireCount-1], "fritzing_glow");
			// 	lastComponentFritzingSprite = lastComponentGlowIcon.GetComponent<Image>().sprite;
			// } else {
			// 	GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "schematic_glow");
			// 	firstComponentSchematicSprite = firstComponentGlowIcon.GetComponent<Image>().sprite;
			// 	GameObject lastComponentGlowIcon = Util.getChildObject("SCH_"+components[componentCount-wireCount-1], "schematic_glow");
			// 	lastComponentSchematicSprite = lastComponentGlowIcon.GetComponent<Image>().sprite;
			// }
			

			http.postJson(comm.getUrl()+"/set", cmd.resetAll());

			GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
			foreach(var item in prefabButtons) {
				if(item.name.Contains("sch_")) {
					SchComponentButton button = item.GetComponent<SchComponentButton>();
					button.resetAllStateEvent.Invoke("schematic");
				}
			}

			initComponentGlow();
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