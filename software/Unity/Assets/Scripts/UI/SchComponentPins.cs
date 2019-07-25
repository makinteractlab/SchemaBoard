using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class SchComponentPins : MonoBehaviour
{
    private NetData netdata;
    private Command cmd;
    private HttpRequest http;

    public void Start() {
        setHttpRequestObject();
        setNetDataObject();

        this.GetComponent<Button>().onClick.AddListener(pinClick);
        cmd = new Command();
        // cmd.setUrls();
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
	}
    void pinClick() {
        initGlowIcon();
        int[] boardPins = new int[2];
        string pinName = this.name;
        string componentName = this.transform.parent.name;
        if(this.name.Contains("fconnector")) {
            pinName = pinName.Substring(1,pinName.Length-1);
        }
        componentName = componentName.Substring(4, componentName.Length-4);
        
        boardPins = netdata.getAllNetForPin(componentName, pinName);
        http.postJson(cmd.getUrl(), cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // ArrayList urls = new ArrayList(cmd.getUrls());
        // foreach(var url in urls) {
        //     http.postJson((string)url, cmd.multiPinOnOff(boardPins[0], boardPins[1]));
        // }
        //http.postJson(cmd.getUrl(), cmd.singlePinToggle(boardPinLineNumber));
        Debug.Log("============================= componentPinClick: " + this.name);
    }
    
    public void setNetDataObject()
    {
		netdata = GameObject.Find("NetData").GetComponent<NetData>();
		Debug.Log(netdata.name);
    }
}