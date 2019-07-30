using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class SchComponentPins : MonoBehaviour
{
    private Communication comm;
    private NetData netdata;
    private Command cmd;
    private HttpRequest http;
    private bool clicked;

    public void Start() {
        setHttpRequestObject();
        setNetDataObject();
        setCommunicationObject();

        this.GetComponent<Button>().onClick.AddListener(pinClick);
        cmd = new Command();
        clicked = false;
        // cmd.setUrls();
    }

    public void setCommunicationObject()
    {
		comm = GameObject.Find("Communication").GetComponent<Communication>();
        //comm = temp.GetComponent<ComponentObject>().getCommunicationObject();
		//Debug.Log(comm.name);
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
            if(item.GetComponent<SchComponentButton>().isButtonClicked()) {
                item.GetComponent<SchComponentButton>().initClickStatus();
            }
        }
	}
    // public bool isPinClicked() {
    //     return clicked;
    // }

    // public void initClickStatus() {
    //     clicked = false;
    // }

    void pinClick() {
        clicked = true;
        initGlowIcon();

        int[] boardPins = new int[2];
        string pinName = this.name;
        string componentName = this.transform.parent.name;
        if(this.name.Contains("fconnector")) {
            pinName = pinName.Substring(1,pinName.Length-1);
        }
        componentName = componentName.Substring(4, componentName.Length-4);
        
        boardPins = netdata.getAllNetForPin(componentName, pinName);

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
        comm.setSchCompPinClicked(true);
        Debug.Log("============================= componentPinClick: " + this.name);
    }
    
    public void setNetDataObject()
    {
		netdata = GameObject.Find("NetData").GetComponent<NetData>();
		Debug.Log(netdata.name);
    }
}