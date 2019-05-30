using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Vuforia;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;

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
	public Dropdown dropdown;
	private string selectedFileName;

	void Start() {
		dropdown.onValueChanged.AddListener(delegate {
			SelectFile(dropdown);
		});
	}

	void Awake() {
		loadConfirmPanel = LoadConfirmPanel.Instance();
        loadYesAction = new UnityAction (YesLoadFile);
        loadCancelAction = new UnityAction (CancelLoadFile);
	}

	void Update()
	{
	}

	public void CancelLoadFile() {
		// Debug.Log("\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>CancelLoadFile!\n\n\n");
	}

	public void YesLoadFile() {
		// Debug.Log("\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>yes pressed!\n\n\n");
		gameObject.SetActive(false);
		ResetAllComponents();
		pauseButton.play();
		comm.setPopupState(false);
		reloadBoard(GetSelectedFileName());
	}

	public void reloadBoard(string _filename) {
		Debug.Log("reloadBoard()");
		netUI.getSchematicData(_filename);
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
	}

	private void SelectFile(Dropdown _selectedFile)
	{
		int selectedFile = _selectedFile.value;
		switch(selectedFile) {
			case 0: {
				break;
			}
			case 1: {
				SetSelectedFileName("schematic1.json");
				refreshConfirmWindow();
				break;
			}
			case 2: {
				SetSelectedFileName("schematic2.json");
				refreshConfirmWindow();
				break;
			}
			case 3: {
				SetSelectedFileName("schematic3.json");
				refreshConfirmWindow();
				break;
			}
			case 4: {
				SetSelectedFileName("schematic4.json");
				refreshConfirmWindow();
				break;
			}
			case 5: {
				SetSelectedFileName("schematic5.json");
				refreshConfirmWindow();
				break;
			}
			case 6: {
				SetSelectedFileName("schematic6.json");
				refreshConfirmWindow();
				break;
			}
			case 7: {
				SetSelectedFileName("schematic7.json");
				refreshConfirmWindow();
				break;
			}
			case 8: {
				SetSelectedFileName("schematic8.json");
				refreshConfirmWindow();
				break;
			}
		}
	}
	
	private void ResetAllComponents() {
		GameObject[] pinsTemp = GameObject.FindGameObjectsWithTag("pin");
		foreach(GameObject pinObj in pinsTemp) {
			pinObj.GetComponent<Button>().image.sprite = DefaultPinSprite;;
		}

		GameObject[] temp = GameObject.FindGameObjectsWithTag("component");
        foreach(GameObject componentObj in temp)
        {
			GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("wire");
			foreach(GameObject wireObj in wireTemp)
			{
				if( wireObj.name.Contains(componentObj.name) )
				{
					LineRenderer lr = wireObj.GetComponent<LineRenderer>();
					lr.enabled = false;
					//lr.SetVertexCount(0);
					lr.positionCount = 0;
					Destroy(wireObj);
				}
			}

			GameObject[] netwireTemp = GameObject.FindGameObjectsWithTag("netwire");
			foreach(GameObject netwireObj in netwireTemp)
			{
				if( netwireObj.name.Contains(componentObj.name) )
				{
					LineRenderer lr = netwireObj.GetComponent<LineRenderer>();
					lr.enabled = false;
					//lr.SetVertexCount(0);
					lr.positionCount = 0;
					Destroy(netwireObj);
				}
			}
        }

		foreach(GameObject componentObj in temp)
        {
			Destroy(componentObj);
        }
	}
}