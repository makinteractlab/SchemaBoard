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

    public void Start() {
        setWireObject();
        setHttpRequestObject();
        setNetDataObject();
        setCommunicationObject();

        this.GetComponent<Button>().onClick.AddListener(componentPinClick);
        cmd = new Command();
        clicked = false;
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
    
    void componentPinClick() {
        //int boardPinLineNumber = int.Parse(netdata.getComponentPinNet(this.transform.parent.name, this.name));
        List<GameObject> resultPinsInNet = new List<GameObject>();
        clicked = true;
        initGlowIcon();
        int[] boardPins = new int[2];

        // boardPins = netdata.getAllNetForPin(this.transform.parent.name, this.name);
        // http.postJson(comm.getUrl()+"/set", cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // ArrayList urls = new ArrayList(cmd.getUrls());
        // foreach(var url in urls) {
        //     http.postJson((string)url, cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // }
        //http.postJson(comm.getUrl(), cmd.singlePinToggle(boardPinLineNumber));
        //Debug.Log("============================= componentPinClick: " + this.name);
		
        string pinName = this.name;
        string componentName = this.transform.parent.name;
        // 일단 모든 핀들을 default pin sprite로 돌려놓은 다음
        // 이 핀과 넷에 들어있는 나머지 핀들 색을 selected pin sprite로 바꾼다.
        initPinGlow();

        this.GetComponent<Image>().sprite = SelectedPinSprite;

        if(this.name.Contains("fconnector")) {
            pinName = pinName.Substring(1,pinName.Length-1);
        }
        componentName = componentName.Substring(4, componentName.Length-4);
        
        boardPins = netdata.getAllNetForPin(componentName, pinName, ref resultPinsInNet);
        foreach(var pin in resultPinsInNet) {
            pin.GetComponent<Image>().sprite = SelectedPinSprite;
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
            if (!comm.getDeleteWireState())
            {
                comm.setComponentPin(transform.parent.name + "-" + name);
                Debug.Log("OnPointerEnter() - button down - componentPin = " + comm.getComponentPin());
                wire.setComponentPinObj(gameObject);
            }
        } else
        {
            if(!comm.getDeleteWireState())
                comm.setComponentPin(transform.parent.name + "-" + name);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        this.transform.parent.GetComponent<Component>().setDragState(true);
        // VuforiaRenderer.Instance.Pause(false);
        if (Input.GetMouseButtonUp(0))
        {
            string componentPinName = null;
            string boardPinName = null;
            string boardPin = null;
            string compPin = null;

            if(comm.getComponentPin() != null)
            {
                componentPinName = comm.getComponentPin();
                Debug.Log("componentPinName = " + componentPinName);
                compPin = Util.removeDigit(componentPinName);
                Debug.Log("compPin = " + compPin);
            }
            if(comm.getBoardPin() != null)
            {
                boardPinName = comm.getBoardPin();
                Debug.Log("boardPinName = " + boardPinName);
                boardPin = Util.removeDigit(boardPinName);
                Debug.Log("boardPin = " + boardPin);
            }
            if((boardPinName == null) || (boardPinName == "")) {
                wire.resetBoardPinObj();
                wire.resetComponentPinObj();
                comm.resetData();
            } else {
                if( (compPin != boardPin) && !pinAlreadyWired(boardPinName) && !pinAlreadyWired(componentPinName))
                {
                    Debug.Log("come to right space...");
                    Debug.Log("boardPinName = " + boardPinName);
                    GameObject boardPinObj = GameObject.Find(boardPinName);
                    if(boardPinObj != null) {
                        Debug.Log("target object = " + boardPinObj.name);  /// pin 111 을 이상하게 찾는 듯 null은 아닌데 값이 이상함
                        comm.setBoardPin(boardPinName);
                        // comm.setBreadboardPinLine(boardPinObj.name);
                        wire.setBoardPinObj(boardPinObj);
                        // Todo: arduino에게 Json 보내기 (value 변경)
                        // Notify connected info ComponentDataHandler -> notify BoardDataHandler
                        //                                            -> notify JsonHandler
                        netdata.syncNetData(this.transform.parent.name, componentPinName, boardPinName);
                    } else {
                        Debug.Log("cannot find board pin object");
                    }
                } else {
                    wire.resetBoardPinObj();
                    wire.resetComponentPinObj();
                    comm.resetData();
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