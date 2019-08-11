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
    public UnityAction<string> updateGlowIconAction;
    public IconToggleEvent updateGlowIconEvent;
    public UnityAction<string> resetAllStateAction;
    public ResetAllStateEvent resetAllStateEvent;

    private DeleteConfirmPanel deleteConfirmPanel;
    private EditTogglePanel editTogglePanel;
    private UnityAction resetAllYesAction;
    private UnityAction resetAllCancelAction;
    // private UnityAction resetAllStateAction;
	private Communication comm;
    private ToggleIcon icon;
    private DrawVirtualWire wire;
    // private WifiConnection wifi;
    private HttpRequest http;
	// private bool deleteState;
    private NetData netdata;
    private Command cmd;
    private bool editOn;
    private bool clicked;
    // private bool editButonActive;

    void Setup() {
        // editOn = false;
        // editButonActive = false;
    }

	public void Start()
	{
        setWireObject();
		setCommunicationObject();
        setToggleIconObject();
        setHttpRequestObject();
        setNetDataObject();
        editOn = true;
        cmd = new Command();
        //cmd.setUrls();
        this.GetComponent<Button>().onClick.AddListener(componentClick);
        clicked = false;
        updateGlowIconAction = new UnityAction<string>(updateGlowIcons);
        updateGlowIconEvent = new IconToggleEvent();
	    updateGlowIconEvent.AddListener(updateGlowIconAction);

        resetAllStateAction = new UnityAction<string>(resetAllState);
        resetAllStateEvent = new ResetAllStateEvent();
        resetAllStateEvent.AddListener(resetAllStateAction);
	}

    void Awake () {
        // deleteConfirmPanel = DeleteConfirmPanel.Instance();
        // resetAllYesAction = new UnityAction (resetAllYesFunction);
        // resetAllCancelAction = new UnityAction (resetAllCancelFunction);
        // editTogglePanel = EditTogglePanel.Instance();
        // resetAllStateAction = new UnityAction (HandleDeleteMode);
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

        return clicked;
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
    
    public void componentClick() {
        //int[] boardPins = new int[2];
        List<string> pins = new List<string>();
        string componentName = this.transform.parent.name;
        componentName = componentName.Substring(4, componentName.Length-4);
        pins = netdata.getComponentPosition(componentName);

        if(GlowToggle()) {
            initAutoPinGlow();
            if(comm.IsSchCompPinClicked()) {//if component pin clicked, then reset all 
                int[] boardPins = new int[2];
                boardPins = netdata.getMultiplePinsPosition(pins);
                http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));
                comm.setSchCompPinClicked(false);
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
            // ArrayList urls = new ArrayList(cmd.getUrls());
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

    public void setToggleIconObject() {
        icon = GameObject.Find("ToggleIcon").GetComponent<ToggleIcon>();
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
}