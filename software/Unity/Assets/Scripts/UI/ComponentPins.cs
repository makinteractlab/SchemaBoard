using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class ComponentPins : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler//, IPointerClickHandler, IPointerUpHandler//, IBeginDragHandler, IDragHandler, IEndDragHandler, 
{
    public Communication comm;
    public DrawVirtualWire wire;
    public NetData netdata;
    private bool alreadyWired = false;
    private Command cmd;
    private HttpRequest http;

    public void Start() {
        setWireObject();
		setCommunicationObject();
        setHttpRequestObject();
        setNetDataObject();

        this.GetComponent<Button>().onClick.AddListener(componentPinClick);
        cmd = new Command();
    }

    public void setHttpRequestObject() {
        http = GameObject.Find("HttpRequest").GetComponent<HttpRequest>();
    }

    void componentPinClick() {
        //int boardPinLineNumber = int.Parse(netdata.getComponentPinNet(this.transform.parent.name, this.name));
        int[] boardPins = new int[2];

        boardPins = netdata.getAllNetForPin(this.transform.parent.name, this.name);
        http.postJson(cmd.getUrl(), cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        //http.postJson(cmd.getUrl(), cmd.singlePinToggle(boardPinLineNumber));
        //Debug.Log("============================= componentPinClick: " + this.name);
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
                compPin = componentPinName.Substring(0, 3);
                Debug.Log("compPin = " + compPin);
            }
            if(comm.getBoardPin() != null)
            {
                boardPinName = comm.getBoardPin();
                Debug.Log("boardPinName = " + boardPinName);
                boardPin = boardPinName.Substring(0, 3);
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
        //Debug.Log("pin name = " + name);
        bool result = false;
        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if( wireObj.name.Contains(name))
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