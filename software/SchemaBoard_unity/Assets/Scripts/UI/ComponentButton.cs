﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

public class ComponentButton : MonoBehaviour//, IPointerUpHandler, IPointerDownHandler//,IPointerEnterHandler
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
    public UnityAction<string> updateGlowIconAction;
    public IconToggleEvent updateGlowIconEvent;
    public UnityAction<string> resetComponentStateAction;
    public ResetAllStateEvent resetComponentStateEvent;
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
    // private ConstraintsHandler constraintsHandle;
	private Communication comm;
    private DrawVirtualWire wire;
    private ToggleIcon icon;
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
    private bool clicked;

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
        setToggleIconObject();
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
        //cmd.setUrls();
        this.GetComponent<Button>().onClick.AddListener(componentClick);
        clicked = false;
        updateGlowIconAction = new UnityAction<string>(updateGlowIcons);
        updateGlowIconEvent = new IconToggleEvent();
	    updateGlowIconEvent.AddListener(updateGlowIconAction);

        resetComponentStateAction = new UnityAction<string>(resetAllState);
        resetComponentStateEvent = new ResetAllStateEvent();
        resetComponentStateEvent.AddListener(resetComponentStateAction);
        
	}

    void Awake () {
        deleteConfirmPanel = DeleteConfirmPanel.Instance();
        resetAllYesAction = new UnityAction (resetAllYesFunction);
        resetAllCancelAction = new UnityAction (resetAllCancelFunction);

        editTogglePanel = EditTogglePanel.Instance();

        resetAllStateAction = new UnityAction (HandleDeleteMode);
    }

    public void initClickStatus() {
        clicked = false;
    }

    public bool isButtonClicked() {
        return clicked;
    }

    public void setButtonClicked(bool _state) {
        clicked = _state;
    }

    public void resetAllState(string _state) {
        initClickStatus();
        initComponentGlow();
    }

    public void updateGlowIcons(string _state) {
        if(clicked) {
            if(_state == "fritzing") {
                Util.getChildObject(this.transform.parent.name, "fritzing_glow").transform.localScale = new Vector3(1,1,1);
                Util.getChildObject(this.transform.parent.name, "schematic_glow").transform.localScale = new Vector3(0,0,0);
            } else {
                Util.getChildObject(this.transform.parent.name, "fritzing_glow").transform.localScale = new Vector3(0,0,0);
                Util.getChildObject(this.transform.parent.name, "schematic_glow").transform.localScale = new Vector3(1,1,1);
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

    public void setToggleIconObject() {
        icon = GameObject.Find("ToggleIcon").GetComponent<ToggleIcon>();
    }
	
	public void initComponentGlow() {
        if(icon.IsFritzingIcon())
                Util.getChildObject(this.transform.parent.name, "fritzing_glow").transform.localScale = new Vector3(0,0,0);
            else
                Util.getChildObject(this.transform.parent.name, "schematic_glow").transform.localScale = new Vector3(0,0,0);
    }

    private bool GlowToggle() {
        // bool result = clicked;
        if(clicked) {
            // on glow image
            if(icon.IsFritzingIcon())
                Util.getChildObject(this.transform.parent.name, "fritzing_glow").transform.localScale = new Vector3(0,0,0);
            else
                Util.getChildObject(this.transform.parent.name, "schematic_glow").transform.localScale = new Vector3(0,0,0);
            clicked = false;
        } else {
            if(icon.IsFritzingIcon())
                Util.getChildObject(this.transform.parent.name, "fritzing_glow").transform.localScale = new Vector3(1,1,1);
            else
                Util.getChildObject(this.transform.parent.name, "schematic_glow").transform.localScale = new Vector3(1,1,1);
            clicked = true;
        }

        showWires(clicked);
        if(clicked)
            EnterDeleteMode();
        else
            ExitDeleteMode(comm.connectedPinSprite, false);
        return clicked;
    }

    /*
    void componentClick() {
        int[] boardPins = new int[2];
        boardPins = netdata.getComponentPinsNet(this.transform.parent.name);
        http.postJson(comm.getUrl(), cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // ArrayList urls = new ArrayList(comm.getUrls());
        // foreach(var url in urls) {
        //     http.postJson((string)url, cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // }


        Wait (0.5f, () => {
             Debug.Log("0.3 seconds is lost forever");
        });

        // foreach(var url in urls) {
        //     http.postJson((string)url, cmd.singlePinBlink( Int32.Parse(netdata.getComponentFirstPinRowPosition(this.transform.parent.name)) ) );
        // }
        http.postJson(comm.getUrl(), cmd.singlePinBlink( Int32.Parse(netdata.getComponentFirstPinRowPosition(this.transform.parent.name)) ) );
    }
    */

    private void initPinGlow() {
        GameObject[] sch_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_schematic_pin");
        GameObject[] fritz_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_fritzing_pin");
        GameObject[] common_prefabs = GameObject.FindGameObjectsWithTag("manual_prefab_common_pin");
        
        foreach(var item in sch_prefabs) {
            // if(item.name.Contains("connector"))
                item.GetComponent<Image>().sprite = comm.DefaultPinSprite;
        }

        foreach(var item in fritz_prefabs) {
            // if(item.name.Contains("connector"))
                item.GetComponent<Image>().sprite = comm.DefaultPinSprite;
        }

        foreach(var item in common_prefabs) {
            // if(item.name.Contains("connector"))
                item.GetComponent<Image>().sprite = comm.DefaultPinSprite;
        }
    }
	
    void componentClick() {
        //int[] boardPins = new int[2];
        List<string> pins = new List<string>();
        string componentName = this.transform.parent.name;
        //componentName = componentName.Substring(4, componentName.Length-4);
        pins = netdata.getComponentPosition(componentName);

        if(GlowToggle()) {
            initPinGlow();
            if(comm.IsCompPinClicked()) {//if component pin clicked, then reset all 
                int[] boardPins = new int[2];
                boardPins = netdata.getMultiplePinsPosition(pins);
                http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));
                comm.setCompPinClicked(false);
            } else {
	            foreach(var pin in pins) {
	                http.postJson(comm.getUrl()+"/set", cmd.singlePinOn(Int16.Parse(pin)));
	                Wait (0.5f, () => {
	                    Debug.Log("0.5 seconds is lost forever");
	                });
                }
            }
            
            // boardPins = netdata.getComponentPinsNet(componentName); /
            // http.postJson(comm.getUrl(), cmd.multiPinOnOff(boardPins[0], boardPins[1])); /
            // ArrayList urls = new ArrayList(comm.getUrls());
            // foreach(var url in urls) {
            //     http.postJson((string)url, cmd.multiPinOnOff(boardPins[0], boardPins[1]));
            // }

            Wait (0.5f, () => {
                Debug.Log("0.5 seconds is lost forever");
            });

            // foreach(var url in urls) {
            //     http.postJson((string)url, cmd.singlePinBlink( Int32.Parse(netdata.getComponentFirstPinRowPosition(this.transform.parent.name)) ) );
            // }
            string firstPinPos = netdata.getComponentFirstPinRowPosition(componentName);
            if (firstPinPos != "") {
                string url = comm.getUrl()+"/set";
                http.postJson(comm.getUrl()+"/set", cmd.singlePinBlink( Int32.Parse(firstPinPos) ) );
            }
            else {
                Debug.Log("This Component is not included in the net.");
            }
            
        } else {
            foreach(var pin in pins) {
                http.postJson(comm.getUrl()+"/set", cmd.singlePinOff(Int16.Parse(pin)));
                Wait (0.5f, () => {
                    Debug.Log("0.5 seconds is lost forever");
                });
            }
        }
        Debug.Log("============================= componentClick: " + this.name);
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
        int seperator = targetComponentPinName.LastIndexOf("-");
        string componentPinName = targetComponentPinName.Substring(seperator+1, targetComponentPinName.Length-seperator-1);
        //Debug.Log("[pin.cs] 10 getTargetComponentPinName() " + componentPinName);
        return componentPinName;
    }

    private string getTargetComponentName(string targetComponentPinName)
    {
        int seperator = targetComponentPinName.LastIndexOf("-");
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
        comm.setComponentPin(transform.parent.name);
		// comm.setDeleteWireState(true, transform.parent.name);
        comm.setEditWireState(true, transform.parent.name);
        deleteState = true;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if( wireObj.name.Contains(transform.parent.name) )
            {
				int start = wireObj.name.IndexOf(":") + 1;
				int end = wireObj.name.IndexOf(",");
				string sourcePinName = wireObj.name.Substring(start, end - start);
				GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = DeleteModePinSprite;
            }
        }
    }

    public void ExitDeleteMode(Sprite _sprite, bool _delete)
    {
        //Debug.Log("[Component.cs]" + "Exit Delete Mode");
        // comm.setDeleteWireState(false, transform.parent.name);
        comm.setEditWireState(false);
        deleteState = false;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if( wireObj.name.Contains(transform.parent.name) )
            {
				int start = wireObj.name.IndexOf(":") + 1;
				int end = wireObj.name.IndexOf(",");
				string sourcePinName = wireObj.name.Substring(start, end - start);
				//Debug.Log("Component.cs - 500000 " + sourcePinName);
				GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = _sprite;
                // string pinName = wireObj.name.Substring(4, wireObj.name.Length-4-name.Length);
                // Debug.Log(pinName);
                // Button btTempPin = GameObject.Find(pinName).GetComponent<Button>();
                // btTempPin.image.sprite = sprite;
            }
        }
    }

    private void showWires(bool onoff) {
        // hide all wires
		GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in wireTemp) {
            if( wireObj.name.Contains(transform.parent.name) ) {
                LineRenderer lr = wireObj.GetComponent<LineRenderer>();
                lr.enabled = onoff;
            } else {
                LineRenderer lr = wireObj.GetComponent<LineRenderer>();
                lr.enabled = false;
            }
        }
	}
/*
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
	} */
}