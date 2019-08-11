using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Newtonsoft.Json.Linq;

public class RefreshButton : MonoBehaviour {
	public Sprite DefaultPinSprite;
	public Sprite AutoRefreshSprite;
	public Sprite ManualRefreshSprite;
	public WifiConnection wifi;
	// public UnityAction<JObject> dataReceivedAction;
	public DataReceivedEvent dataReceivedEvent;
	public PauseButton pauseButton;
	public Communication comm;
	// public StatusButton status;

	private Command cmd;
    public HttpRequest http;
	public ToggleAutoManual modeToggleMenu;
	public NetData netData;
	public LoadNetUI loadNetUI;
//	public ConstraintsHandler constraintsHandle;
	private LoadConfirmPanel loadConfirmPanel;
	private UnityAction loadYesAction;
    private UnityAction loadCancelAction;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(true);
		cmd = new Command();
		//dataReceivedAction = new UnityAction<JObject>(reloadBoard);
        //dataReceivedEvent = new DataReceivedEvent();
        //dataReceivedEvent.AddListener(dataReceivedAction);
	}
	
	void Awake() {
		loadConfirmPanel = LoadConfirmPanel.Instance();
        loadYesAction = new UnityAction (refresh);
        loadCancelAction = new UnityAction (refreshCancel);
	}

	public void refreshCancel() {
		// Debug.Log("\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>refreshCancel!\n\n\n");
	}

	public void reloadBoard(string _filename) {
		Debug.Log("reloadBoard()");
		//after get board name, load the board json from internal storage.

		// string path = Application.persistentDataPath + "/xml/netlist.xml";
		// Debug.Log("path = " + path);
		//netUI.readSchematicData(_filename);
		netData.getInitialSchematicData();
		//send command to reset all connection
		//wifi.sendDataEvent.Invoke(Query.resetAllConnections);
		gameObject.SetActive(true);
		VuforiaRenderer.Instance.Pause(false);
	}
	
	private void initManualPinGlow() {
        GameObject[] sch_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_schematic_pin");
        GameObject[] fritz_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_fritzing_pin");
        GameObject[] common_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_common_pin");
        
        foreach(var item in sch_prefabs) {
            // if(item.name.Contains("connector"))
                item.GetComponent<Button>().image.sprite = comm.DefaultPinSprite;
        }

        foreach(var item in fritz_prefabs) {
            // if(item.name.Contains("connector"))
                item.GetComponent<Button>().image.sprite = comm.DefaultPinSprite;
        }

        foreach(var item in common_prefabs) {
            // if(item.name.Contains("connector"))
                item.GetComponent<Button>().image.sprite = comm.DefaultPinSprite;
        }
    }

	void initAutoPinGlow() {
        GameObject[] sch_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_schematic");
        GameObject[] fritz_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_fritzing");
        GameObject[] pin_prefabs = GameObject.FindGameObjectsWithTag("circuit_prefab_pin");
        
        foreach(var item in sch_prefabs) {
            if(item.name.Contains("connector"))
                item.GetComponent<Button>().image.sprite = comm.DefaultPinSprite;
        }

        foreach(var item in fritz_prefabs) {
            if(item.name.Contains("connector"))
                item.GetComponent<Button>().image.sprite = comm.DefaultPinSprite;
        }

        foreach(var item in pin_prefabs) {
            if(item.name.Contains("connector"))
                item.GetComponent<Button>().image.sprite = comm.DefaultPinSprite;
        }
		comm.setCompPinClicked(false);
		comm.setSchCompPinClicked(false);
    }

	void initGlowIcon() {
		GameObject[] schematic = GameObject.FindGameObjectsWithTag("schematic_glow");
		GameObject[] fritzing = GameObject.FindGameObjectsWithTag("fritzing_glow");

		foreach(GameObject glow in schematic) {
			glow.transform.localScale = new Vector3(0,0,0);
		}

		foreach(GameObject glow in fritzing) {
			glow.transform.localScale = new Vector3(0,0,0);
		}

		GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
        foreach(var item in prefabButtons) {
			if(item.name.Contains("sch_")) {
				if(item.GetComponent<SchComponentButton>().isButtonClicked()) {
					item.GetComponent<SchComponentButton>().initClickStatus();
				}
			} else {
				if(item.GetComponent<ComponentButton>().isButtonClicked()) {
					item.GetComponent<ComponentButton>().initClickStatus();
				}
			}
        }
	}

	public void refresh() {
		// Debug.Log("\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>yes pressed!\n\n\n");
		if(modeToggleMenu.IsManualMode()) {
			// gameObject.SetActive(false);
			// pauseButton.play();
			comm.setPopupState(false);
			// reloadBoard(comm.getCurrentFileName());
			
			//send command to reset all connection
			//wifi.sendDataEvent.Invoke(Query.resetAllConnections);
			// gameObject.SetActive(true);
			// VuforiaRenderer.Instance.Pause(false);
			netData.setInitialNetData();	// set init connection data
			ResetAllConnection();
			initManualPinGlow();
		} else {
			initAutoPinGlow();
		}

		http.postJson(comm.getUrl()+"/set", cmd.resetAll());
		initGlowIcon();
		// netUI.setupManualMode();
		// constraintsHandle.clearConstraintsDB();
		//send query to get board name
		// wifi.sendDataEvent.Invoke(Query.getBoardID);
		// status.dataWaitingEvent.Invoke(Query.getBoardID);
	}

	// popup을 띄우는 함수를 바로 콜하자
	public void refreshConfirmWindow() {
		// Debug.Log("\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> transform x y z " + transform.position.x + " , " +transform.position.y + " , " + transform.position.z + "\n\n\n");
		loadConfirmPanel.Choice (loadYesAction, loadCancelAction);
        loadConfirmPanel.setTitle("Reset All?");
        //loadConfirmPanel.setPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
	}

	private void ResetAllConnection() {
		loadNetUI.setupManualMode();
	}

	private void ResetAllComponents() {
		GameObject[] pinsTemp = GameObject.FindGameObjectsWithTag("pin");
		foreach(GameObject pinObj in pinsTemp) {
			pinObj.GetComponent<Button>().image.sprite = DefaultPinSprite;;
		}

		GameObject[] temp = GameObject.FindGameObjectsWithTag("manual_prefab");
        foreach(GameObject componentObj in temp)
        {
			//GameObject pinTemp = null;
			//pinTemp = GameObject.Find(componentObj.GetComponent<Component>().left.BoardPin);
			//pinTemp.GetComponent<Button>().image.sprite = defaultBoardPinSprite;
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