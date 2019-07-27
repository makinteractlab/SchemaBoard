using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Vuforia;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
public class SelectFileDropdown : MonoBehaviour {

	public LoadNetUI netUI;
	public Sprite DefaultPinSprite;
	public UnityAction<JObject> dataReceivedAction;
	public DataReceivedEvent dataReceivedEvent;
	public Communication comm;
	public PauseButton pauseButton;

	private LoadConfirmPanel loadConfirmPanel;
	private UnityAction loadYesAction;
    private UnityAction loadCancelAction;
	public Dropdown m_Dropdown;
	List<string> m_DropOptions;
	private string selectedFileName;

	private bool init;

	private ArrayList fileList;

	void Start() {
		m_Dropdown.onValueChanged.AddListener(delegate {
			SelectFile(m_Dropdown);
		});
		init = true;
		
		showSchematicMenu(true);
		showCommonMenu(false);
		showTutorialUI(false);
		showManualMenu(false);
		
		LoadFileList();
	}

	void Awake() {
		loadConfirmPanel = LoadConfirmPanel.Instance();
        loadYesAction = new UnityAction (YesLoadFile);
        loadCancelAction = new UnityAction (CancelLoadFile);
	}

	void Update()
	{
	}

	private void LoadFileList() {
		m_DropOptions = new List<string>();
		string path = @"/storage/emulated/0/Android/data/com.kaist.netvisualizer/files/fileList.txt";
		
		try {
			using (StreamReader reader = new StreamReader(path)) {
				string line;
				while((line = reader.ReadLine()) !=null) {
					m_DropOptions.Add(line);
				}
			}
		} catch (Exception e) 
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
			string localpath = @"D:/Works/Git/RetroBreadboard/software/Unity/Assets/Scripts/data/fileList.txt";
			try {
				using (StreamReader reader = new StreamReader(localpath)) {
					string line;
					while((line = reader.ReadLine()) !=null) {
						m_DropOptions.Add(line);
					}
				}
			} catch (Exception ec) {
				Debug.Log("The local file could not be read:");
            	Debug.Log(ec.Message);
			}
        }

		m_Dropdown.AddOptions(m_DropOptions);
	}
	
	public void CancelLoadFile() {
		if(init) {
			showSchematicMenu(true);
			showCommonMenu(false);
			showTutorialUI(false);
			showManualMenu(false);
		}
	}

	private void showManualMenu(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("manual");
		
        foreach(GameObject manualObject in temp) {
			if(onoff) {
				manualObject.transform.localScale = new Vector3(1,1,1);
			} else {
				manualObject.transform.localScale = new Vector3(0,0,0);
			}
        }
	}

	private void showTutorialUI(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("tutorial");
		
        foreach(GameObject tutorialObject in temp) {
			if(onoff) {
				tutorialObject.transform.localScale = new Vector3(1,1,1);
			} else {
				tutorialObject.transform.localScale = new Vector3(0,0,0);
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

	private void showCommonMenu(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("commonMenu");
		
        foreach(GameObject menuObject in temp) {
			if(onoff) {
				menuObject.transform.localScale = new Vector3(1,1,1);
			} else {
				menuObject.transform.localScale = new Vector3(0,0,0);
			}
        }
	}

	public void YesLoadFile() {
		// Debug.Log("\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>yes pressed!\n\n\n");
		if(!init)
			ResetAllComponents();
		showCommonMenu(true);
		showSchematicMenu(true);
		showManualMenu(false);
		showTutorialUI(false);
		gameObject.SetActive(false);
		pauseButton.play();
		comm.setPopupState(false);
		reloadBoard(GetSelectedFileName());
		init = false;
	}

	public void reloadBoard(string _filename) {
		Debug.Log("reloadBoard()");
		netUI.readSchematicData(_filename);
		gameObject.SetActive(true);
		VuforiaRenderer.Instance.Pause(false);
	}

	public void refreshConfirmWindow() {
		loadConfirmPanel.Choice (loadYesAction, loadCancelAction);
        loadConfirmPanel.setTitle("Load the schematic");
	}

	public string GetSelectedFileName() {
		return selectedFileName;
	}

	public void SetSelectedFileName(string _selectedFileName) {
		selectedFileName = _selectedFileName;
		comm.setCurrentFileName(_selectedFileName);
	}

	// private string solver(string _fritzfilename) {
	// 	// execute python solver with python3 _fritzfilename outputfilepath/outputfilename
	// 	return outputfilename;
	// }
	private void SelectFile(Dropdown _selectedFile)
	{
		int selectedFile = _selectedFile.value;
		// ArrayList options = new ArrayList(m_DropOptions.ToArray());
		// SetSelectedFileName((string)options[selectedFile]);
		if(selectedFile > 0) {
			SetSelectedFileName(m_DropOptions[selectedFile-1]);
			refreshConfirmWindow();
		}

		// switch(selectedFile) {
		// 	case 0: {
		// 		break;
		// 	}
		// 	case 1: {
		// 		SetSelectedFileName("schematic1");
		// 		refreshConfirmWindow();
		// 		break;
		// 	}
		// 	case 2: {
		// 		SetSelectedFileName("schematic2.json");
		// 		refreshConfirmWindow();
		// 		break;
		// 	}
		// 	case 3: {
		// 		SetSelectedFileName("schematic3.json");
		// 		refreshConfirmWindow();
		// 		break;
		// 	}
		// 	case 4: {
		// 		SetSelectedFileName("schematic4.json");
		// 		refreshConfirmWindow();
		// 		break;
		// 	}
		// 	case 5: {
		// 		SetSelectedFileName("schematic5.json");
		// 		refreshConfirmWindow();
		// 		break;
		// 	}
		// 	case 6: {
		// 		SetSelectedFileName("schematic6.json");
		// 		refreshConfirmWindow();
		// 		break;
		// 	}
		// 	case 7: {
		// 		SetSelectedFileName("schematic7.json");
		// 		refreshConfirmWindow();
		// 		break;
		// 	}
		// 	case 8: {
		// 		SetSelectedFileName("schematic8.json");
		// 		refreshConfirmWindow();
		// 		break;
		// 	}
		// }
	}
	
	private void ResetAllComponents() {
		GameObject[] pinsTemp = GameObject.FindGameObjectsWithTag("pin");
		foreach(GameObject pinObj in pinsTemp) {
			pinObj.GetComponent<Button>().image.sprite = DefaultPinSprite;;
		}

		GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("wire");
		foreach(GameObject wireObj in wireTemp) {
			LineRenderer lr = wireObj.GetComponent<LineRenderer>();
			lr.enabled = false;
			//lr.SetVertexCount(0);
			lr.positionCount = 0;
			Destroy(wireObj);
		}

		GameObject[] netwireTemp = GameObject.FindGameObjectsWithTag("netwire");
		foreach(GameObject netwireObj in netwireTemp) {
			LineRenderer lr = netwireObj.GetComponent<LineRenderer>();
			lr.enabled = false;
			//lr.SetVertexCount(0);
			lr.positionCount = 0;
			Destroy(netwireObj);
		}

		GameObject[] schwireTemp = GameObject.FindGameObjectsWithTag("schwire");
		foreach(GameObject schwireObj in schwireTemp) {
				UnityEngine.UI.Extensions.UILineRenderer lr = schwireObj.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>();
				lr.enabled = false;
				lr.Points = null;
				Destroy(schwireObj);
		}
		GameObject[] manualPrefabs = GameObject.FindGameObjectsWithTag("manual_prefab");
		foreach(GameObject componentObj in manualPrefabs) {
			Destroy(componentObj);
        }

		GameObject[] autoPrefabs = GameObject.FindGameObjectsWithTag("auto_prefab");
		foreach(GameObject componentObj in autoPrefabs) {
			Destroy(componentObj);
        }
	}
}