using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Newtonsoft.Json.Linq;

public class RefreshButton : MonoBehaviour {
	public LoadNetUI netUI;
	public Sprite DefaultPinSprite;
	public WifiConnection wifi;
	public UnityAction<JObject> dataReceivedAction;
	public DataReceivedEvent dataReceivedEvent;
	public PauseButton pauseButton;
	public Communication comm;
	public StatusButton status;
//	public ConstraintsHandler constraintsHandle;

	private LoadConfirmPanel loadConfirmPanel;
	private UnityAction loadYesAction;
    private UnityAction loadCancelAction;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(true);
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
		netUI.getSchematicData(_filename);

		//send command to reset all connection
		//wifi.sendDataEvent.Invoke(Query.resetAllConnections);
		gameObject.SetActive(true);
		VuforiaRenderer.Instance.Pause(false);
	}
	
	public void refresh() {
		// Debug.Log("\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>yes pressed!\n\n\n");
		gameObject.SetActive(false);
		ResetAllComponents();
		pauseButton.play();
		comm.setPopupState(false);
		reloadBoard("schematic1.json");
		// constraintsHandle.clearConstraintsDB();
		//send query to get board name
		// wifi.sendDataEvent.Invoke(Query.getBoardID);
		// status.dataWaitingEvent.Invoke(Query.getBoardID);
	}

	// popup을 띄우는 함수를 바로 콜하자
	public void refreshConfirmWindow() {
		// Debug.Log("\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> transform x y z " + transform.position.x + " , " +transform.position.y + " , " + transform.position.z + "\n\n\n");
		loadConfirmPanel.Choice (loadYesAction, loadCancelAction);
        loadConfirmPanel.setTitle("Load the schematic");
        //loadConfirmPanel.setPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
	}

	//for test
	// public void refresh() {
	// 	ResetAllComponents();
	// 	pauseButton.play();
	// 	string path = @"/storage/emulated/0/Android/data/com.kaist.virtualcomponent/files/Json/102.json";
	// 	boardUI.setupBoard(path);
	// }

	private void ResetAllComponents() {
		GameObject[] pinsTemp = GameObject.FindGameObjectsWithTag("pin");
		foreach(GameObject pinObj in pinsTemp) {
			pinObj.GetComponent<Button>().image.sprite = DefaultPinSprite;;
		}

		GameObject[] temp = GameObject.FindGameObjectsWithTag("component");
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