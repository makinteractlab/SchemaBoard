using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Newtonsoft.Json.Linq;

public class SelectAllButton : MonoBehaviour {
	public WifiConnection wifi;
	// public PauseButton pauseButton;
	public Communication comm; // component click state 변경
	public ToggleIcon icon;	// icon state에 따라 glow icon 변경
	
	private Command cmd;
    public HttpRequest http;
	public ToggleAutoManual modeToggleMenu;
	public NetData netData;
	// public LoadNetUI loadNetUI;

	void Start () {
		gameObject.SetActive(true);
		cmd = new Command();
	}
	
	void Awake() {
	}

	void onGlowIconAll() {
		GameObject[] schematic = GameObject.FindGameObjectsWithTag("schematic_glow");
		GameObject[] fritzing = GameObject.FindGameObjectsWithTag("fritzing_glow");

		if(icon.IsFritzingIcon()) {
			foreach(GameObject glow in fritzing) {
				glow.transform.localScale = new Vector3(1,1,1);
			}
		} else {
			foreach(GameObject glow in schematic) {
				glow.transform.localScale = new Vector3(1,1,1);
			}
		}

		GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
        foreach(var item in prefabButtons) {
            item.GetComponent<SchComponentButton>().setButtonClicked(true);
        }
	}

	public void selectAll() {
		onGlowIconAll();
		http.postJson(cmd.getUrl(), cmd.resetAll()); //send command to turn on all the pins
	}
}