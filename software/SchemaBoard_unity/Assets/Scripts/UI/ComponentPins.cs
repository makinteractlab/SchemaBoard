using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ComponentPins : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler//, IPointerClickHandler, IPointerUpHandler//, IBeginDragHandler, IDragHandler, IEndDragHandler, 
{
    public UnityAction<string> resetAllStateAction;
    public ResetAllStateEvent resetAllStateEvent;
    public Sprite SelectedPinSprite;
    public Sprite DefaultPinSprite;
    private Communication comm;
    public DrawVirtualWire wire;
    private NetData netdata;
    private bool alreadyWired = false;
    private Command cmd;
    private HttpRequest http;
    private bool clicked;
    private bool wireOn;

    public void Start() {
        setWireObject();
        setHttpRequestObject();
        setNetDataObject();
        setCommunicationObject();

        this.GetComponent<Button>().onClick.AddListener(componentPinClick);
        cmd = new Command();
        clicked = false;
        wireOn = false;
        // cmd.setUrls();
        resetAllStateAction = new UnityAction<string>(resetAllState);
        resetAllStateEvent = new ResetAllStateEvent();
        resetAllStateEvent.AddListener(resetAllStateAction);

        SelectedPinSprite = comm.SelectedPinSprite;
        DefaultPinSprite = comm.DefaultPinSprite;
    }

    public void resetAllState(string _mode) {
        initClickStatus();
        initGlow();
    }

    void initClickStatus() {
        clicked = false;
        wireOn = false;
    }

    void initGlow() {
        this.GetComponent<Image>().sprite = comm.DefaultPinSprite;
    }

    public void setHttpRequestObject() {
        http = GameObject.Find("HttpRequest").GetComponent<HttpRequest>();
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

        //component click state should be updated
        GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
        foreach(var item in prefabButtons) {
            if(item.name.Contains("Component")) {
                if(item.GetComponent<ComponentButton>().isButtonClicked()) {
                    item.GetComponent<ComponentButton>().initClickStatus();
                }
            }
        }
	}

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
    
    public void ExitDeleteMode(Sprite _sprite, bool _delete)
    {
        //Debug.Log("[Component.cs]" + "Exit Delete Mode");
        // comm.setDeleteWireState(false, transform.parent.name);
        comm.setEditWireState(false);
        // deleteState = false;

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

    public void EnterDeleteMode()
    {
        comm.setComponentPin(transform.parent.name + "-" + name);
		// comm.setDeleteWireState(true, transform.parent.name);
        comm.setEditWireState(true, transform.parent.name);
        // deleteState = true;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if(wireObj.name.Contains(transform.parent.name) && wireObj.name.Contains(name))
            {
				int start = wireObj.name.IndexOf(":") + 1;
				int end = wireObj.name.IndexOf(",");
				string sourcePinName = wireObj.name.Substring(start, end - start);
				GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = comm.DeleteModePinSprite;
            }
        }
    }

    private void showWires(bool onoff) {
        // hide all wires
		GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in wireTemp) {
            if( wireObj.name.Contains(transform.parent.name) && wireObj.name.Contains(name)) {
                LineRenderer lr = wireObj.GetComponent<LineRenderer>();
                lr.enabled = onoff;
            }
        }
	}

    private void showWires(bool onoff, List<GameObject> _resultPinsInNet) {
        // hide all wires
		GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("wire");
        
        foreach(GameObject wireObj in wireTemp) {
            foreach(var pins in _resultPinsInNet) {
                if( wireObj.name.Contains(pins.transform.parent.name) && wireObj.name.Contains(pins.name)) {
                    LineRenderer lr = wireObj.GetComponent<LineRenderer>();
                    lr.enabled = onoff;
                }
                // else {
                //     LineRenderer lr = wireObj.GetComponent<LineRenderer>();
                //     lr.enabled = false;
                // }
            }
        }
	}

    void componentPinClick() {
        comm.setComponentPin(null);
        wire.resetBoardPinObj();
        wire.resetComponentPinObj();
        comm.resetData();

        List<GameObject> resultPinsInNet = new List<GameObject>();
        clicked = true;
        initGlowIcon();
        int[] boardPins = new int[2];
		
        string pinName = this.name;
        string componentName = this.transform.parent.name;


        // 일단 모든 핀들을 default pin sprite로 돌려놓은 다음
        // 이 핀과 넷에 들어있는 나머지 핀들 색을 selected pin sprite로 바꾼다.
        initPinGlow();

        this.GetComponent<Image>().sprite = comm.SelectedPinSprite;

        if(this.name.Contains("fconnector")) {
            pinName = pinName.Substring(1,pinName.Length-1);
        }
        
        GameObject[] manualPrefabs = GameObject.FindGameObjectsWithTag("manual_prefab");

        if(this.transform.parent.name.Contains("GND")) {
            boardPins = netdata.getGndNet(ref resultPinsInNet);
            foreach(var item in manualPrefabs) {
                if(item.name.Contains("GND")) {
                    Util.getChildObject(item.name, "connector0").GetComponent<Image>().sprite = comm.SelectedPinSprite;
                }
            }
        } else if(this.transform.parent.name.Contains("PWR")) {
            boardPins = netdata.getPwrNet(ref resultPinsInNet);
            foreach(var item in manualPrefabs) {
                if(item.name.Contains("PWR")) {
                    Util.getChildObject(item.name, "connector0").GetComponent<Image>().sprite = comm.SelectedPinSprite;
                }
            }
        } else {
            boardPins = netdata.getAllNetForPin(componentName, pinName, ref resultPinsInNet);
        }

        foreach(var pin in resultPinsInNet) {
            pin.GetComponent<Image>().sprite = comm.SelectedPinSprite;
        }

        if(boardPins[0] > 0 || boardPins[1] > 0) {
            http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        } else {
            Debug.Log("This Component is not included in the net.");
        }
        // ArrayList urls = new ArrayList(cmd.getUrls());
        // foreach(var url in urls) {
        //     http.postJson((string)url, cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // }
        //http.postJson(comm.getUrl(), cmd.singlePinToggle(boardPinLineNumber));
        if(wireOn) {
            ExitDeleteMode(comm.connectedPinSprite, false);
            showWires(false, resultPinsInNet);
            initPinGlow();
            wireOn = false;
            // comm.setCompPinClicked(false);
        } else {
            EnterDeleteMode();
            showWires(true, resultPinsInNet);
            wireOn = true;
            // comm.setCompPinClicked(true);
        }

        comm.setCompPinClicked(true);
        Debug.Log("============================= componentPinClick: " + this.name);
    }
    
    public void setNetDataObject()
    {
		netdata = GameObject.Find("NetData").GetComponent<NetData>();
		Debug.Log(netdata.name);
    }

    public void setCommunicationObject()
    {
		comm = GameObject.Find("Communication").GetComponent<Communication>();
        //comm = temp.GetComponent<ComponentObject>().getCommunicationObject();
		Debug.Log(comm.name);
    }

    public void setWireObject()
    {
        wire = GameObject.Find("DrawVirtualWires").GetComponent<DrawVirtualWire>();
        //wire = temp.GetComponent<ComponentObject>().getWireObject();
		Debug.Log(wire.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.transform.parent.GetComponent<Component>().setDragState(false);
            comm.setComponentPin(transform.parent.name + "-" + name);
            Debug.Log("OnPointerEnter() - button down - componentPin = " + comm.getComponentPin());
            wire.setComponentPinObj(gameObject);
        } else {
            comm.setComponentPin(transform.parent.name + "-" + name);
        }

        this.GetComponent<Image>().sprite = comm.SelectedPinSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = comm.DefaultPinSprite;
        Debug.Log("OnPointerUp");
        
        this.transform.parent.GetComponent<Component>().setDragState(true);
        // VuforiaRenderer.Instance.Pause(false);
        if (Input.GetMouseButtonUp(0))
        {
            string componentPinName = null;
            string boardPinName = null;
            string boardPin = null;
            string compPin = null;

            componentPinName = comm.getComponentPin();
            boardPinName = comm.getBoardPin();
            compPin = Util.removeDigit(componentPinName);
            boardPin = Util.removeDigit(boardPinName);

            if(boardPinName == null) {  // click
                comm.setComponentPin(null);
                wire.resetBoardPinObj();
                wire.resetComponentPinObj();
                comm.resetData();
            } else { // dragging (component pin -> pin drag)
                if(netdata.isOccupiedRow(boardPinName)) { // pin이 occupied row에 있는 거면 
                    wire.resetBoardPinObj();
                    wire.resetComponentPinObj();
                    comm.resetData();
                } else {
                    if( (compPin != boardPin) && !pinAlreadyWired(boardPinName) && !pinAlreadyWired(componentPinName)) {
                        GameObject boardPinObject = GameObject.Find(boardPinName);
                        if(boardPinObject) {
                            comm.setBoardPin(boardPinName);
                            wire.setBoardPinObj(boardPinObject);
                            netdata.syncNetData(transform.parent.name, name, boardPinName);
                        }
                    } else {
                        wire.resetBoardPinObj();
                        wire.resetComponentPinObj();
                        comm.resetData();
                    }
                }
            }
        }
    }

    private bool pinAlreadyWired(string name)
    {
        //Debug.Log("===============> pin name = " + name);
        bool result = false;
        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            string first = wireObj.name.Substring(wireObj.name.IndexOf(':')+1,wireObj.name.IndexOf(',')-wireObj.name.IndexOf(':')-1);
            string last = wireObj.name.Substring(wireObj.name.IndexOf(',')+1, wireObj.name.Length-wireObj.name.IndexOf(',')-1);
            // string firstComponentName = first.Substring(0,first.IndexOf('-'));
            // string firstPin = first.Substring(first.IndexOf('-')+1, first.Length-first.IndexOf('-')-1);
            // string lastComponentName = last.Substring(0,last.IndexOf('-'));
            // string lastPin = last.Substring(last.IndexOf('-')+1, last.Length-last.IndexOf('-')-1);

            // Debug.Log("========================================>");
            // Debug.Log("first = " + first);
            // Debug.Log("last = " + last);
            // Debug.Log("first component = " + firstComponentName);
            // Debug.Log("last component = " + lastComponentName);
            // Debug.Log("first pin = " + firstPin);
            // Debug.Log("last pin = " + lastPin);
            // Debug.Log("========================================>");

            // if(wireObj.name.Contains(name))
            if ( String.Compare(first, name, true) == 0 || String.Compare(last, name, true) == 0 )
            {
                Debug.Log("already wired");
                result = true;
            }
        }
        return result;
    }

    public Component getComponentObject(string _name) {
        return GameObject.Find(_name).GetComponent<Component>();
    }
}