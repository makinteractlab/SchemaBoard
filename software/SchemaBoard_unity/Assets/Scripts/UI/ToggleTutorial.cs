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
	private Sprite prevGlowIconSprite;
	private Sprite currGlowIconSprite;
	private Sprite firstComponentFritzingSprite;
	private Sprite firstComponentSchematicSprite;
	private Sprite lastComponentFritzingSprite;
	private Sprite lastComponentSchematicSprite;
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
	// bool travelComponent;
	public bool travelNet;

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

		if(icon.IsFritzingIcon()) {
			GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "fritzing_glow");
			firstComponentGlowIcon.GetComponent<Image>().sprite = firstComponentFritzingSprite;
			GameObject lastComponentGlowIcon = Util.getChildObject("SCH_"+components[componentCount-1], "fritzing_glow");
			lastComponentGlowIcon.GetComponent<Image>().sprite = lastComponentFritzingSprite;
		} else {
			GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "schematic_glow");
			firstComponentGlowIcon.GetComponent<Image>().sprite = firstComponentSchematicSprite;
			GameObject lastComponentGlowIcon = Util.getChildObject("SCH_"+components[componentCount-1], "schematic_glow");
			lastComponentGlowIcon.GetComponent<Image>().sprite = lastComponentSchematicSprite;
		}
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
		}

		if(index == componentCount-1 && prevButtonObj.clicked) {
			initAll();
		}

		if(index == componentCount && !prevButtonObj.clicked){
			initAll();
		}

		if(index == totalSteps-1) {
			GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
			foreach(var item in prefabButtons) {
				if(item.name.Contains("sch_")) {
					SchComponentButton button = item.GetComponent<SchComponentButton>();
					button.resetAllStateEvent.Invoke("schematic");
				}
			}
		}

		if(index >= componentCount) {
			// should start travel nets
			GameObject[] autoPrefabs = GameObject.FindGameObjectsWithTag("auto_prefab");

			int netIndex = index-componentCount;
			
			boardPins = netdata.getPositionForNet(nets[netIndex]);
			http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));
			
			// initAutoPinGlow();
			if(prevButtonObj.clicked) {
				// if(index == componentCount) {
				// 	initAll();
				// }
				if(netIndex < nets.Count-1) {
					// foreach(var item in nets[netIndex+1]) {
					for(int i=0; i<nets[netIndex+1].Count; i++) {
						Util.getChildObject("SCH_"+nets[netIndex+1][i][1], nets[netIndex+1][i][2]).GetComponent<Image>().sprite = prevSelectedPinSprite;
						// gnd랑 pwr이랑 아이콘 돌려놔야 함
						if(gndList.Count>0 && pwrList.Count>0) {
							if(netIndex+1 == nets.Count-2) {// ground net
								foreach(var element in gndList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							} else if(netIndex+1 == nets.Count-1) {//power net
								foreach(var element in pwrList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						} else if(gndList.Count>0 && pwrList.Count==0) {
							if(netIndex+1 == nets.Count-1) {// ground net
								foreach(var element in gndList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						} else if(gndList.Count==0 && pwrList.Count>0) {
							if(netIndex+1 == nets.Count-1) {//power net
								foreach(var element in pwrList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						}
					}
				}
				for(int i=0; i<nets[netIndex].Count; i++) {
				// foreach(var item in nets[netIndex]) {
					if(gndList.Count>0 && pwrList.Count>0) {
						if(netIndex == nets.Count-2) {// ground net
							foreach(var element in gndList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						} else if(netIndex == nets.Count-1) {//power net
							foreach(var element in pwrList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					} else if(gndList.Count>0 && pwrList.Count==0) {
						if(netIndex == nets.Count-1) {// ground net
							foreach(var element in gndList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					} else if(gndList.Count==0 && pwrList.Count>0) {
						if(netIndex == nets.Count-1) {//power net
							foreach(var element in pwrList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					}
					Util.getChildObject("SCH_"+nets[netIndex][i][1], nets[netIndex][i][2]).GetComponent<Image>().sprite = currSelectedPinSprite;
				}

			} else {
				if(netIndex > 0) {
					for(int i=0; i<nets[netIndex-1].Count; i++) {
					// foreach(var item in nets[netIndex-1]) {
						Util.getChildObject("SCH_"+nets[netIndex-1][i][1], nets[netIndex-1][i][2]).GetComponent<Image>().sprite = prevSelectedPinSprite;
						// gnd랑 pwr이랑 아이콘 돌려놔야 함
						if(gndList.Count>0 && pwrList.Count>0) {
							if(netIndex-1 == nets.Count-2) {// ground net
								foreach(var element in gndList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							} else if(netIndex == nets.Count-1) {//power net
								foreach(var element in pwrList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						} else if(gndList.Count>0 && pwrList.Count==0) {
							if(netIndex-1 == nets.Count-1) {// ground net
								foreach(var element in gndList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						} else if(gndList.Count==0 && pwrList.Count>0) {
							if(netIndex-1 == nets.Count-1) {//power net
								foreach(var element in pwrList) {
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = prevSelectedPinSprite;
								}
							}
						}
					}
				}
				for(int i=0; i<nets[netIndex].Count; i++) {
				// foreach(var item in nets[netIndex]) {
					if(gndList.Count>0 && pwrList.Count>0) {
						if(netIndex == nets.Count-2) {// ground net
							foreach(var element in gndList) {
								if(Util.getChildObject(element, "connector0"))
									Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						} else if(netIndex == nets.Count-1) {//power net
							foreach(var element in pwrList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					} else if(gndList.Count>0 && pwrList.Count==0) {
						if(netIndex == nets.Count-1) {// ground net
							foreach(var element in gndList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					} else if(gndList.Count==0 && pwrList.Count>0) {
						if(netIndex == nets.Count-1) {//power net
							foreach(var element in pwrList) {
								Util.getChildObject(element, "connector0").GetComponent<Image>().sprite = currSelectedPinSprite;
							}
						}
					}
					Util.getChildObject("SCH_"+nets[netIndex][i][1], nets[netIndex][i][2]).GetComponent<Image>().sprite = currSelectedPinSprite;
				}
			}
		} else {
			prevSelectedComponent = selectedComponent;
			selectedComponent = GameObject.Find("SCH_"+components[_index]);
			List<string> pins = new List<string>(netdata.getComponentPosition(components[_index]));
			
			boardPins = netdata.getMultiplePinsPosition(pins);
			http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));
			
			Wait (0.5f, () => {
				Debug.Log("0.5 seconds is lost forever");
			});

			string firstPinPos = netdata.getComponentFirstPinRowPosition(components[_index]);
			http.postJson(comm.getUrl()+"/set", cmd.singlePinBlink(Int32.Parse(firstPinPos)));
			
			// glow on
			if(icon.IsFritzingIcon()) {
				GameObject currGlowIcon = Util.getChildObject(selectedComponent.name, "fritzing_glow");
				currGlowIconSprite = currGlowIcon.GetComponent<Image>().sprite;
				currGlowIcon.transform.localScale = new Vector3(1,1,1);
				currGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;
				
				if(prevSelectedComponent) {
					GameObject prevGlowIcon = Util.getChildObject(prevSelectedComponent.name, "fritzing_glow");
					prevGlowIcon.GetComponent<Image>().sprite = prevGlowIconSprite;
					// prevGlowIcon.transform.localScale = new Vector3(0,0,0);
				}
				prevGlowIconSprite = currGlowIconSprite;
			} else {
				GameObject currGlowIcon = Util.getChildObject(selectedComponent.name, "schematic_glow");
				currGlowIconSprite = currGlowIcon.GetComponent<Image>().sprite;
				currGlowIcon.transform.localScale = new Vector3(1,1,1);
				currGlowIcon.GetComponent<Image>().sprite = selectedGlowIconSprite;

				if(prevSelectedComponent) {
					GameObject prevGlowIcon = Util.getChildObject(prevSelectedComponent.name, "schematic_glow");
					prevGlowIcon.GetComponent<Image>().sprite = prevGlowIconSprite;
					// prevGlowIcon.transform.localScale = new Vector3(0,0,0);
				}
				prevGlowIconSprite = currGlowIconSprite;
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
			stepInfo.transform.localScale = new Vector3(0,0,0);
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
			http.postJson(comm.getUrl()+"/set", cmd.resetAll());
			initAll();
			prevSelectedComponent = null;
		} else {	//tutorial on
			if(init) {
				data = netdata.getCurrentDebugSchematicData();
				foreach(var item in data) {
					components.Add(item.Key);
				}
				GameObject[] autoPrefabs = GameObject.FindGameObjectsWithTag("auto_prefab");
				foreach(var item in autoPrefabs) {
					if(item.name.Contains("GND")) {
						gndList.Add(item.name);
					} else if(item.name.Contains("PWR")) {
						pwrList.Add(item.name);
					}
				}
//List<dictionary<string,string>>
				nets = new List<List<string[]>>(netdata.getAllNetList());

				// if(gndList.Count > 0) {
				// 	nets.Add(netdata.getGndNet());
				// }
				// if(pwrList.Count > 0) {
				// 	nets.Add(netdata.getPwrNet());
				// }
				netCount = nets.Count;
				componentCount = components.Count;
				totalSteps = netCount + componentCount;
				init = false;
			}
			status = true;
			this.GetComponent<Image>().sprite = onSprite;
			showButtons(status);

			if(icon.IsFritzingIcon()) {
				GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "fritzing_glow");
				firstComponentFritzingSprite = firstComponentGlowIcon.GetComponent<Image>().sprite;
				GameObject lastComponentGlowIcon = Util.getChildObject("SCH_"+components[componentCount-1], "fritzing_glow");
				lastComponentFritzingSprite = lastComponentGlowIcon.GetComponent<Image>().sprite;
			} else {
				GameObject firstComponentGlowIcon = Util.getChildObject("SCH_"+components[0], "schematic_glow");
				firstComponentSchematicSprite = firstComponentGlowIcon.GetComponent<Image>().sprite;
				GameObject lastComponentGlowIcon = Util.getChildObject("SCH_"+components[componentCount-1], "schematic_glow");
				lastComponentSchematicSprite = lastComponentGlowIcon.GetComponent<Image>().sprite;
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