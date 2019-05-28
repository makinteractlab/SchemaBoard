using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vuforia;

public class Pin : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler// required interface when using the OnPointerEnter method.
{
    public Communication comm;
    public DrawVirtualWire wire;
    public NetData netdata;
    //public BezierCurve wire;
    //Do this when the cursor enters the rect area of this selectable UI object.

    private DeleteConfirmPanel deleteConfirmPanel;
    private UnityAction deleteYesAction;
    private UnityAction deleteCancelAction;
    public Sprite ConnectedPinSprite;
    public Sprite DefaultPinSprite;
    public GameObject connectedTo;

    void Start() {
        connectedTo = null;
        setNetDataObject();
    }

    void Awake () {
        deleteConfirmPanel = DeleteConfirmPanel.Instance();
        deleteYesAction = new UnityAction (DeleteYesFunction);
        deleteCancelAction = new UnityAction (DeleteCancleFunction);
    }

    public void setNetDataObject()
    {
		netdata = GameObject.Find("NetData").GetComponent<NetData>();
		Debug.Log(netdata.name);
    }

     public void deleteOptionWindow() {
        deleteConfirmPanel.Choice (deleteYesAction, deleteCancelAction);
        deleteConfirmPanel.setTitle("Delete this Wire?");
        deleteConfirmPanel.setPosition(new Vector3(transform.position.x+150, transform.position.y, transform.position.z));
    }

    public Component getComponentObject(string _name) {
        return GameObject.Find(_name).GetComponent<Component>();
    }

    void DeleteYesFunction () {
        //ExitDeleteMode(DefaultPinSprite, true);
        GameObject.Find(name).GetComponent<Button>().image.sprite = DefaultPinSprite; 
        //string connectedComponent = connectedTo.transform.parent.name;
        //comm.setDeleteWireState(false, "");
        //comm.setTargetPin(name); ====???????
        //comm.setBoardPin(name);


        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if( wireObj.name.Contains(name) )
            {
                LineRenderer lr = wireObj.GetComponent<LineRenderer>();
                lr.enabled = false;
                lr.positionCount = 0;
                Destroy(wireObj);
            }
        }
        //wire.removeWire(connectedComponent, name);
        comm.resetData();
        wire.resetBoardPinObj();
        wire.resetComponentPinObj();
        connectedTo = null;
    }

    void DeleteCancleFunction () {
        ExitDeleteMode(ConnectedPinSprite, false);
        comm.setDeleteWireState(false, "");
        comm.resetData();
        wire.resetBoardPinObj();
        wire.resetComponentPinObj();
    }

    public bool PinsInDeleteState()
    {
        bool active = false;
        
        if(comm.getEditWireState()) {
            active = true;
        }

        return active;

        // GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");

        // foreach(GameObject wireObj in temp)
        // {
        //     if( wireObj.name.Contains(component) )
        //     {
        //         int start = wireObj.name.IndexOf(":") + 1;
        //         int end = wireObj.name.IndexOf(",");
        //         string pinName = wireObj.name.Substring(start, end - start);
        //         Debug.Log("pinName? " + pinName);
        //         if(pinName == name) {
        //             active = true;
        //         }
        //     }
        // }
        // return active;
    }

	private GameObject getComponentObject(string ParentObjectName, string ChildObjectName)
    {
        GameObject temp = GameObject.Find(ParentObjectName);
        GameObject resultObj = null;
        
        Transform[] children = temp.GetComponentsInChildren<Transform>();
        foreach(Transform obj in children)     
        {
            if(obj.name == "Component") {
                resultObj = obj.gameObject;
            }
        }
        //Debug.Log("[Pin.cs] 88 " + "getComponentObject = " + resultObj.name);
        //Debug.Log("[Pin.cs] 99 " + "parent = " + resultObj.transform.parent.name);
        return resultObj;
    }

    public void ExitDeleteMode(Sprite sprite, bool delete)
    {
        string component = comm.getComponentInDeleteState();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        if(delete) {
            string connectedComponentName = connectedTo.transform.parent.name;
            GameObject componentButton = getComponentObject(connectedComponentName, connectedTo.name);
            componentButton.GetComponent<ComponentButton>().ExitDeleteMode(ConnectedPinSprite, delete);
            GameObject.Find(name).GetComponent<Button>().image.sprite = sprite;
        } else {
            string connectedComponentName = connectedTo.transform.parent.name;
            GameObject componentButton = getComponentObject(connectedComponentName, connectedTo.name);
            componentButton.GetComponent<ComponentButton>().ExitDeleteMode(sprite, delete);
        }   
    }

    public void setCommunicationObject(Communication obj)
    {
        comm = obj;
    }

    public void setWireObject(DrawVirtualWire obj)
    {
        wire = obj;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // VuforiaRenderer.Instance.Pause(true);
        if (Input.GetMouseButtonDown(0)) {
            //Debug.Log("[pin.cs] " + "mouse button down: " + name);
            comm.setStartTime(Time.time);

            if (!comm.getDeleteWireState())
            {
                comm.setBoardPin(name);
                GameObject temp = GameObject.Find(name);
                if(temp != null)
                    wire.setBoardPinObj(temp);
                else Debug.Log("cannot find board pin object");
            }
        } else {
            if(!comm.getDeleteWireState())
                comm.setBoardPin(name);
        }
    }

    private bool pinAlreadyWired(string name)
    {
        //string targetComponentPinName = comm.getComponentPin();

        bool result = false;
        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if( wireObj.name.Contains(name))
            {
                result = true;
            }
        }
        return result;
    }

    private string getTargetComponentPinName(string targetComponentPinName)
    {
        int seperator = targetComponentPinName.IndexOf("-");
        string componentPinName = targetComponentPinName.Substring(seperator+1, targetComponentPinName.Length-seperator-1);
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
            if(obj.name == getTargetComponentPinName(targetComponentPinName)) {
                resultObj = obj.gameObject;
                break;
            }
        }
        return resultObj;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // VuforiaRenderer.Instance.Pause(false);
        if (Input.GetMouseButtonUp(0))
        {
            float releasedTime = Time.time;
            float pressedTime = comm.getStartTime();

            string boardPinName = null;
            string componentPinName = null;
            string boardPin = null;
            string compPin = null;

            boardPinName = comm.getBoardPin();
            if(boardPinName != null)
                boardPin = boardPinName.Substring(0, 3);

            componentPinName = comm.getComponentPin();
            if(componentPinName != null)
                compPin = componentPinName.Substring(0, 3);

            if(comm.getEditWireState())
            {
                //deleteOptionWindow();
                DeleteYesFunction();

                string targetName = null;
                string targetComponentName = null;
                string targetComponentPinName = null;

                GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("wire");
                foreach(GameObject wireObj in wireTemp) {
                    if( wireObj.name.Contains(this.transform.name) ) {
                        string wireName = wireObj.name;
                        //"Wire" + ":" + boardPinObj.name + "," + componentPinObj.transform.parent.name + "-" + componentPinObj.name
                        int compSeperator = wireName.IndexOf(",");
                        targetName = wireName.Substring(compSeperator+1, wireName.Length-compSeperator-1);

                        int compPinSeperator = targetName.IndexOf("-");
                        targetComponentName = targetName.Substring(0, compPinSeperator);
                        targetComponentPinName = targetName.Substring(compPinSeperator+1, targetName.Length-compPinSeperator-1);
                    }
                }
                netdata.syncNetData(targetComponentName, targetComponentPinName, "init");
            } else if((componentPinName == null) || (componentPinName == "")) {
                wire.resetBoardPinObj();
                wire.resetComponentPinObj();
                comm.resetData();
            } else {   // drag released
                if (boardPin == "Pin" && compPin == "Pin")
                {
                    if(!comm.getEditWireState()) {
                        wire.resetBoardPinObj();
                        wire.resetComponentPinObj();
                        comm.resetData();
                    }
                }
                else if( (boardPin != compPin) && !pinAlreadyWired(componentPinName) && !pinAlreadyWired(boardPinName))
                {
                    string wireStartBoardPin = "";
                    comm.setComponentPin(componentPinName);
                    wireStartBoardPin = wire.setComponentPinObj(getTargetComponentPinObject(componentPinName));
                    // Todo: arduino에게 Json 보내기 (value 변경)
                    // Notify connected info ComponentDataHandler -> notify BoardDataHandler
                    //                                            -> notify JsonHandler
                    netdata.syncNetData(getTargetComponentPinObject(componentPinName).transform.parent.name, componentPinName, wireStartBoardPin);
                } else {
                    wire.resetBoardPinObj();
                    wire.resetComponentPinObj();
                    comm.resetData();
                }
            }
        }
    }
}