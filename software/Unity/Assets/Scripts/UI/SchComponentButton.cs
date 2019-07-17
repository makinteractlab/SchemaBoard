using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

public class SchComponentButton : MonoBehaviour//, IPointerUpHandler, IPointerDownHandler//,IPointerEnterHandler
{
    // public Sprite DeleteModePinSprite;
    // public Sprite ConnectedPinSprite;
    // public Sprite DefaultPinSprite;
    // public Sprite DeleteModeComponentSprite;
    // public Sprite DefaultComponentSprite;
    // public Sprite onGoingButtonSprite;
    // public Sprite offButtonSprite;
    // public Sprite editOnSprite;
    // public Sprite editOffSprite;

    private DeleteConfirmPanel deleteConfirmPanel;
    private EditTogglePanel editTogglePanel;
    private UnityAction resetAllYesAction;
    private UnityAction resetAllCancelAction;
    private UnityAction resetAllStateAction;
	private Communication comm;
    private DrawVirtualWire wire;
    // private WifiConnection wifi;
    private HttpRequest http;
	// private bool deleteState;
    private NetData netdata;
    private Command cmd;
    private bool editOn;

    // private bool editButonActive;

    void Setup() {
        // editOn = false;
        // editButonActive = false;
    }

	public void Start()
	{
        setWireObject();
		setCommunicationObject();
        setHttpRequestObject();
        setNetDataObject();
        editOn = true;
        cmd = new Command();
        //cmd.setUrls();
        this.GetComponent<Button>().onClick.AddListener(componentClick);
	}

    void Awake () {
        // deleteConfirmPanel = DeleteConfirmPanel.Instance();
        // resetAllYesAction = new UnityAction (resetAllYesFunction);
        // resetAllCancelAction = new UnityAction (resetAllCancelFunction);
        // editTogglePanel = EditTogglePanel.Instance();
        // resetAllStateAction = new UnityAction (HandleDeleteMode);
    }

    public void Wait(float seconds, Action action){
		StartCoroutine(_wait(seconds, action));
	}
	IEnumerator _wait(float time, Action callback){
		yield return new WaitForSeconds(time);
		callback();
	}

    void componentClick() {
        int[] boardPins = new int[2];

        string componentName = this.transform.parent.name;
        componentName = componentName.Substring(3, componentName.Length-3);

        boardPins = netdata.getComponentPinsNet(componentName);
        http.postJson(cmd.getUrl(), cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // ArrayList urls = new ArrayList(cmd.getUrls());
        // foreach(var url in urls) {
        //     http.postJson((string)url, cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // }


        Wait (0.5f, () => {
             Debug.Log("0.3 seconds is lost forever");
        });

        // foreach(var url in urls) {
        //     http.postJson((string)url, cmd.singlePinBlink( Int32.Parse(netdata.getComponentFirstPinRowPosition(this.transform.parent.name)) ) );
        // }
        http.postJson(cmd.getUrl(), cmd.singlePinBlink( Int32.Parse(netdata.getComponentFirstPinRowPosition(componentName)) ) );
    }

    public void setHttpRequestObject() {
        http = GameObject.Find("HttpRequest").GetComponent<HttpRequest>();
    }

    public void setNetDataObject() {
		netdata = GameObject.Find("NetData").GetComponent<NetData>();
		Debug.Log(netdata.name);
    }

	public void setWireObject() {
        wire = GameObject.Find("DrawVirtualWires").GetComponent<DrawVirtualWire>();
    }

    public void setCommunicationObject()
    {
		comm = GameObject.Find("Communication").GetComponent<Communication>();
        //comm = temp.GetComponent<ComponentObject>().getCommunicationObject();
		//Debug.Log(comm.name);
    }

    public void editToggleWindow() {
        editTogglePanel.Choice(resetAllStateAction);
        editTogglePanel.setPosition(new Vector3(transform.position.x+150, transform.position.y+100, transform.position.z+10));
    }

    public void deleteOptionWindow() {
        //string title = "Reset " + transform.parent.name + " ?";
        deleteConfirmPanel.Choice (resetAllYesAction, resetAllCancelAction);
        deleteConfirmPanel.setTitle("Reset " + transform.parent.name + " ?");
        deleteConfirmPanel.setPosition(new Vector3(transform.position.x+200, transform.position.y+100, transform.position.z+10));
    }

    // void resetAllYesFunction () {
    //     wire.removeWireWithComponent(transform.parent.name);
    //     ExitDeleteMode(DefaultPinSprite, true);
    //     //Debug.Log("after exit deletemode in resetAllYesFunction()");

    //     //{"set":"X2", "on": 1, "value": [{"M":0, "B":1}, {"M":1, "B":2}]}

    //     comm.resetData();
    //     // Todo: arduino에게 변경된 Json 보내기 (connection, value가 reset 됨)
    //     // Notify value info ComponentDataHandler -> notify BoardDataHandler
    //     //                                        -> notify JsonHandler

    //     wire.resetBoardPinObj();
    //     wire.resetComponentPinObj();
    // }

    // void resetAllCancelFunction () {
    //     ExitDeleteMode(ConnectedPinSprite, false);
    //     comm.resetData();
    //     wire.resetBoardPinObj();
    //     wire.resetComponentPinObj();
    // }

    private string getTargetComponentPinName(string targetComponentPinName)
    {
        //string targetComponentPinName = comm.getTargetPin();
        int seperator = targetComponentPinName.IndexOf("-");
        string componentPinName = targetComponentPinName.Substring(seperator+1, targetComponentPinName.Length-seperator-1);
        //Debug.Log("[pin.cs] 10 getTargetComponentPinName() " + componentPinName);
        return componentPinName;
    }

    private string getTargetComponentName(string targetComponentPinName)
    {
        int seperator = targetComponentPinName.IndexOf("-");
        string componentName = targetComponentPinName.Substring(0, seperator);
        return componentName;
    }

    private GameObject getTargetComponentPinObject(string targetComponentPinName)
    {
        GameObject temp = GameObject.Find(getTargetComponentName(targetComponentPinName));
        GameObject resultObj = null;
        
        Transform[] children = temp.GetComponentsInChildren<Transform>();
        foreach(Transform obj in children)     
        {
            //Debug.Log("[pin.cs] 7 " + obj.name);
            if(obj.name == getTargetComponentPinName(targetComponentPinName)) {
                resultObj = obj.gameObject;
            }
        }
        //Debug.Log("[Pin.cs] 8 " + "getTargetComponentPinObject = " + resultObj.name);
        //Debug.Log("[Pin.cs] 9 " + "getTargetComponentPinObject = " + resultObj.transform.parent.name);
        return resultObj;
    }

	private GameObject getChildObject(string ParentObjectName, string ChildObjectName)
    {
        GameObject temp = GameObject.Find(ParentObjectName);
        GameObject resultObj = null;
        
        Transform[] children = temp.GetComponentsInChildren<Transform>();
        foreach(Transform obj in children)     
        {
            //Debug.Log("[pin.cs] 7 " + obj.name);
            if(obj.name == ChildObjectName) {
                resultObj = obj.gameObject;
            }
        }
        //Debug.Log("[Pin.cs] 88 " + "getChildObject = " + resultObj.name);
        //Debug.Log("[Pin.cs] 99 " + "getChildObject = " + resultObj.transform.parent.name);
        return resultObj;
    }

    // public void HandleDeleteMode() {
    //     if(editOn) {
    //         EnterDeleteMode();
    //         editOn = false;
    //     } else {
    //         ExitDeleteMode(ConnectedPinSprite, false);
    //         editOn = true;
    //     }
    // }

    // public void EnterDeleteMode()
    // {
    //     if(editTogglePanel.modalPanelObject.activeSelf) {
    //         GameObject.Find("EditToggleButton").GetComponent<Button>().image.sprite = editOnSprite;
    //     }

    //     //Debug.Log("[Component.cs]" + "Enter Delete Mode");
    //     //comm.setSourcePin(transform.parent.name); ==>?????
    //     comm.setComponentPin(transform.parent.name);
	// 	comm.setDeleteWireState(true, transform.parent.name);
    //     deleteState = true;
		
	// 	getChildObject(transform.parent.name, name).GetComponent<Button>().image.sprite = DeleteModeComponentSprite;

    //     GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
    //     foreach(GameObject wireObj in temp)
    //     {
    //         if( wireObj.name.Contains(transform.parent.name) )
    //         {
	// 			int start = wireObj.name.IndexOf(":") + 1;
	// 			int end = wireObj.name.IndexOf(",");
	// 			string sourcePinName = wireObj.name.Substring(start, end - start);
	// 			//Debug.Log("Component.cs - 500000 " + sourcePinName);
	// 			GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = DeleteModePinSprite;
	// 			// int seperator = wireObj.name.IndexOf(",");
	// 			// string targetComponentPinName = wireObj.name.Substring(seperator+1, wireObj.name.Length-seperator-1);
	// 			// getTargetComponentPinObject(targetComponentPinName).GetComponent<Button>().image.sprite = DeleteModePinSprite;
    //         }
    //     }
    // }

    // public void ExitDeleteMode(Sprite sprite, bool delete)
    // {
    //     if(editTogglePanel.modalPanelObject.activeSelf) {
    //         GameObject.Find("EditToggleButton").GetComponent<Button>().image.sprite = editOffSprite;
    //     }

    //     //Debug.Log("[Component.cs]" + "Exit Delete Mode");
    //     comm.setDeleteWireState(false, transform.parent.name);
    //     deleteState = false;
	// 	getChildObject(transform.parent.name, name).GetComponent<Button>().image.sprite = DefaultComponentSprite;

    //     GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
    //     foreach(GameObject wireObj in temp)
    //     {
    //         if( wireObj.name.Contains(transform.parent.name) )
    //         {
	// 			int start = wireObj.name.IndexOf(":") + 1;
	// 			int end = wireObj.name.IndexOf(",");
	// 			string sourcePinName = wireObj.name.Substring(start, end - start);
	// 			//Debug.Log("Component.cs - 500000 " + sourcePinName);
	// 			GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = sprite;
    //             // string pinName = wireObj.name.Substring(4, wireObj.name.Length-4-name.Length);
    //             // Debug.Log(pinName);
    //             // Button btTempPin = GameObject.Find(pinName).GetComponent<Button>();
    //             // btTempPin.image.sprite = sprite;
    //         }
    //     }
    // }
}