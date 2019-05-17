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

public class ComponentButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler//,IPointerEnterHandler
{
    public Sprite DeleteModePinSprite;
    public Sprite ConnectedPinSprite;
    public Sprite DefaultPinSprite;
    public Sprite DeleteModeComponentSprite;
    public Sprite DefaultComponentSprite;
    public Sprite onGoingButtonSprite;
    public Sprite offButtonSprite;
    public Sprite editOnSprite;
    public Sprite editOffSprite;
    // public Sprite refreshingVoltmeter;
    //private ModalPanel modalPanel;
    // private UnityAction resistorSaveAction;
    // private UnityAction resistorCancelAction;
    private DeleteConfirmPanel deleteConfirmPanel;
//    private SelectSingleValuePanel settingAwgPanel;
//    private SelectValuePanel selectCapacitorValuePanel;
//    private SelectValuePanel selectInductorValuePanel;
//    private SliderValuePanel setResistorValuePanel;
//    private VoltmeterPanel readVoltmeterPanel;
    private EditTogglePanel editTogglePanel;
    private UnityAction resetAllYesAction;
    private UnityAction resetAllCancelAction;
    private UnityAction resetAllStateAction;
    //private UnityAction constraintsSaveAction;
    // private UnityAction capacitorSaveAction;
    // private UnityAction capacitorCancelAction;
    // private UnityAction inductorSaveAction;
    // private UnityAction inductorCancelAction;
    // private UnityAction awgOnAction;
    // private UnityAction awgCloseAction;
    // private UnityAction adcRefreshAction;
    // private float componentValue;
//    private ConstraintsHandler constraintsHandle;
	private Communication comm;
    private DrawVirtualWire wire;
    private WifiConnection wifi;
    private HttpRequest http;
	private bool deleteState;
    private NetData netdata;
    private Command cmd;
    // private string unit;
    // private string awgIP;
    // bool awgOn;
    // private JObject awgData;

    // bool resistorWindowDrag;
    // bool capacitorWindowDrag;
    // bool inductorWindowDrag;

    // int capacitorToggleValue;
    // int inductorToggleValue;
    // int awgToggleValue;
    // int constraintsSelectedValue;

    private bool editOn;
    //private bool voltmeterPop;

    private bool editButonActive;

    // private List<string> constraintsComponentList;

    void Setup() {
        // resistorWindowDrag = false;
        // capacitorWindowDrag = false;
        // inductorWindowDrag = false;

        // capacitorToggleValue = 0;
        // inductorToggleValue = 0;
        // constraintsSelectedValue = 0;

        editOn = false;
        //voltmeterPop = false;
        editButonActive = false;
        
    }

	public void Start()
	{
        setWireObject();
		setCommunicationObject();
        //setWifiObject();
        setHttpRequestObject();
        setNetDataObject();
        // setConstraintsHandleObject();

        //awgData = new JObject();
        //awgData.Add("frequency", 0.0);
        //awgData.Add("amplitude", 0.0);
        //awgData.Add("dcOffset", 0.0);
        //awgData.Add("selectedToggle", 0);
        //awgOn = true;
        editOn = true;
        // closeActivePopup();
        // BUG!
        //GameObject.Find("EditToggleButton").GetComponent<Button>().onClick.AddListener(HandleDeleteMode);
        cmd = new Command();
        this.GetComponent<Button>().onClick.AddListener(componentClick);
	}

    void Awake () {
        deleteConfirmPanel = DeleteConfirmPanel.Instance();
        resetAllYesAction = new UnityAction (resetAllYesFunction);
        resetAllCancelAction = new UnityAction (resetAllCancelFunction);

        editTogglePanel = EditTogglePanel.Instance();

        resetAllStateAction = new UnityAction (HandleDeleteMode);
    }

    // public void setWifiObject() {
    //     wifi = GameObject.Find("WifiConnection").GetComponent<WifiConnection>();
    // }

    void componentClick() {
        int[] boardPins = new int[2];
        boardPins = netdata.getComponentPinsNet(this.transform.parent.name);
        http.postJson(cmd.getUrl(), cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        http.postJson(cmd.getUrl(), cmd.singlePinBlink( Int32.Parse(netdata.getComponentFirstPinPosition(this.transform.parent.name)) ) );
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

    // public void setConstraintsHandleObject() {
    //     // constraintsHandle = GameObject.Find("ConstraintsHandler").GetComponent<ConstraintsHandler>();
    // }

    public void setCommunicationObject()
    {
		comm = GameObject.Find("Communication").GetComponent<Communication>();
        //comm = temp.GetComponent<ComponentObject>().getCommunicationObject();
		//Debug.Log(comm.name);
    }
    
    // public void closeActivePopup()
    // {
    //     if(setResistorValuePanel.modalPanelObject.activeSelf) {
    //         GameObject.Find("SliderEditButton").GetComponent<Button>().image.sprite = editOffSprite;
    //         ExitDeleteMode(ConnectedPinSprite, false);
    //         setResistorValuePanel.ClosePanel();
    //     }
    //     if(selectCapacitorValuePanel.modalPanelObject.activeSelf) {
    //         GameObject.Find("SelectEditButton").GetComponent<Button>().image.sprite = editOffSprite;
    //         ExitDeleteMode(ConnectedPinSprite, false);
    //         selectCapacitorValuePanel.ClosePanel();
    //     }
    //     if(selectInductorValuePanel.modalPanelObject.activeSelf) {
    //         GameObject.Find("SelectEditButton").GetComponent<Button>().image.sprite = editOffSprite;
    //         ExitDeleteMode(ConnectedPinSprite, false);
    //         selectInductorValuePanel.ClosePanel();
    //     }
    //     if(settingAwgPanel.modalPanelObject.activeSelf) {
    //         GameObject.Find("AwgEditButton").GetComponent<Button>().image.sprite = editOffSprite;
    //         ExitDeleteMode(ConnectedPinSprite, false);
    //         settingAwgPanel.ClosePanel();
    //     }
    // }

    //  Send to the Modal Panel to set up the Buttons and Functions to call
    // public void pop () {
    //     //Debug.Log("poped");
    //     if(!comm.getPopupState() && !comm.getDeleteWireState()){
    //         if(transform.parent.name.Contains("ADC")) {
    //                 //closeActivePopup();
    //                 readAdcValueWindow();
    //                 //comm.setPopupState(true);
    //         } else if(transform.parent.GetComponent<Component>().fixedComponent) {
    //             //closeActivePopup();
    //             //Debug.Log("editToggleWindow");
    //             editToggleWindow();
    //             //comm.setPopupState(true);
    //         } else {
    //             if(transform.parent.name.Contains("resistor")) {
    //                 closeActivePopup();
    //                 setResistorValueWindow();
    //                 //comm.setPopupState(true);
    //                 resistorWindowDrag = true;
    //             } else if(transform.parent.name.Contains("capacitor")) {
    //                 closeActivePopup();
    //                 setCapacitorValueWindow();
    //                 //comm.setPopupState(true);
    //                 capacitorWindowDrag = true;
    //             } else if(transform.parent.name.Contains("inductor")) {
    //                 closeActivePopup();
    //                 setInductorValueWindow();
    //                 //comm.setPopupState(true);
    //                 inductorWindowDrag = true;
    //             } else if(transform.parent.name.Contains("AWG")) {
    //                 closeActivePopup();
    //                 setAwgValueWindow();
    //                 //comm.setPopupState(true);
    //             }
    //         }
    //     }
    // }

    public void editToggleWindow() {
        editTogglePanel.Choice(resetAllStateAction);
        editTogglePanel.setPosition(new Vector3(transform.position.x+150, transform.position.y+100, transform.position.z+10));
    }

    // private int findItemsInList(List<string> list, string toFind) {
    //     for (int i = 0; i < list.Count; i++) 
    //     if (list[i] == toFind) return i;
    //     return -1;
    // }

    public void deleteOptionWindow() {
        //string title = "Reset " + transform.parent.name + " ?";
        deleteConfirmPanel.Choice (resetAllYesAction, resetAllCancelAction);
        deleteConfirmPanel.setTitle("Reset " + transform.parent.name + " ?");
        deleteConfirmPanel.setPosition(new Vector3(transform.position.x+200, transform.position.y+100, transform.position.z+10));
    }

    void resetAllYesFunction () {
        wire.removeWireWithComponent(transform.parent.name);
        ExitDeleteMode(DefaultPinSprite, true);
        //Debug.Log("after exit deletemode in resetAllYesFunction()");

        //{"set":"X2", "on": 1, "value": [{"M":0, "B":1}, {"M":1, "B":2}]}

        comm.resetData();
        // Todo: arduino에게 변경된 Json 보내기 (connection, value가 reset 됨)
        // Notify value info ComponentDataHandler -> notify BoardDataHandler
        //                                        -> notify JsonHandler

        wire.resetBoardPinObj();
        wire.resetComponentPinObj();
    }

    void resetAllCancelFunction () {
        ExitDeleteMode(ConnectedPinSprite, false);
        comm.resetData();
        wire.resetBoardPinObj();
        wire.resetComponentPinObj();
    }

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

    public void HandleDeleteMode() {
        if(editOn) {
            EnterDeleteMode();
            editOn = false;
        } else {
            ExitDeleteMode(ConnectedPinSprite, false);
            editOn = true;
        }
    }

    public void EnterDeleteMode()
    {
        if(editTogglePanel.modalPanelObject.activeSelf) {
            GameObject.Find("EditToggleButton").GetComponent<Button>().image.sprite = editOnSprite;
        }

        //Debug.Log("[Component.cs]" + "Enter Delete Mode");
        //comm.setSourcePin(transform.parent.name); ==>?????
        comm.setComponentPin(transform.parent.name);
		comm.setDeleteWireState(true, transform.parent.name);
        deleteState = true;
		
		getChildObject(transform.parent.name, name).GetComponent<Button>().image.sprite = DeleteModeComponentSprite;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if( wireObj.name.Contains(transform.parent.name) )
            {
				int start = wireObj.name.IndexOf(":") + 1;
				int end = wireObj.name.IndexOf(",");
				string sourcePinName = wireObj.name.Substring(start, end - start);
				//Debug.Log("Component.cs - 500000 " + sourcePinName);
				GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = DeleteModePinSprite;
				// int seperator = wireObj.name.IndexOf(",");
				// string targetComponentPinName = wireObj.name.Substring(seperator+1, wireObj.name.Length-seperator-1);
				// getTargetComponentPinObject(targetComponentPinName).GetComponent<Button>().image.sprite = DeleteModePinSprite;
            }
        }
    }

    public void ExitDeleteMode(Sprite sprite, bool delete)
    {
        if(editTogglePanel.modalPanelObject.activeSelf) {
            GameObject.Find("EditToggleButton").GetComponent<Button>().image.sprite = editOffSprite;
        }

        //Debug.Log("[Component.cs]" + "Exit Delete Mode");
        comm.setDeleteWireState(false, transform.parent.name);
        deleteState = false;
		getChildObject(transform.parent.name, name).GetComponent<Button>().image.sprite = DefaultComponentSprite;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if( wireObj.name.Contains(transform.parent.name) )
            {
				int start = wireObj.name.IndexOf(":") + 1;
				int end = wireObj.name.IndexOf(",");
				string sourcePinName = wireObj.name.Substring(start, end - start);
				//Debug.Log("Component.cs - 500000 " + sourcePinName);
				GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = sprite;
                // string pinName = wireObj.name.Substring(4, wireObj.name.Length-4-name.Length);
                // Debug.Log(pinName);
                // Button btTempPin = GameObject.Find(pinName).GetComponent<Button>();
                // btTempPin.image.sprite = sprite;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("*****ComponentButton down");
        comm.setStartTime(Time.time);
    }

	public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
        {
            float releasedTime = Time.time;
            float pressedTime = comm.getStartTime();
            //Debug.Log("[pin.cs] " + "pressed time = " + pressedTime);
            //Debug.Log("[pin.cs] " + "release time = " + releasedTime);

			if ((releasedTime - pressedTime) > 0.3)
			{
				//long press
				if(!comm.getDeleteWireState() && !comm.getPopupState())
					EnterDeleteMode();
			} else {
				//short press
				if(deleteState) {
					deleteOptionWindow();
				} else {
                    //Debug.Log("component Name = " + transform.parent.name);
                    // if(transform.parent.name.Contains("ADC")) {
                    //     //wifi.sendDataEvent.Invoke(Query.getVoltage);
                    //     if(voltmeterPop) {
                    //         readVoltmeterPanel.ClosePanel();
                    //         voltmeterPop = false;
                    //     } else {
                    //         pop();
                    //         // if(GameObject.Find("ReadVoltageButton")) {
                    //         //     GameObject.Find("ReadVoltageButton").GetComponent<Button>().image.sprite = refreshingVoltmeter;
                    //         // }
                    //         voltmeterPop = true;
                    //     }
                    // } else 
                    //if(transform.parent.GetComponent<Component>().fixedComponent) {
                        //Debug.Log("fixed");
                        if(editButonActive) {
                            editTogglePanel.ClosePanel();
                            editButonActive = false;
                        } else {
                            //pop();
                            editButonActive = true;
                        }
                        //HandleDeleteMode();
                    //} //else pop();
				}
			}
		}
	}
}