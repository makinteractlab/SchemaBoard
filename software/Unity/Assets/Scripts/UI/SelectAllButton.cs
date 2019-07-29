using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Newtonsoft.Json.Linq;

public class SelectAllButton : MonoBehaviour {
	public Sprite DefaultPinSprite;
	public Sprite AutoRefreshSprite;
	public Sprite ManualRefreshSprite;
	public WifiConnection wifi;
	// public UnityAction<JObject> dataReceivedAction;
	public DataReceivedEvent dataReceivedEvent;
	public PauseButton pauseButton;
	public Communication comm;
	public ToggleIcon icon;
	// public StatusButton status;

	private Command cmd;
    public HttpRequest http;
	public ToggleAutoManual modeToggleMenu;
	public NetData netData;
	public LoadNetUI loadNetUI;

	void Start () {
		gameObject.SetActive(true);
		cmd = new Command();
		//dataReceivedAction = new UnityAction<JObject>(reloadBoard);
        //dataReceivedEvent = new DataReceivedEvent();
        //dataReceivedEvent.AddListener(dataReceivedAction);
	}
	
	void Awake() {
	}

	void initGlowIcon() {
		GameObject[] schematic = GameObject.FindGameObjectsWithTag("schematic_glow");
		GameObject[] fritzing = GameObject.FindGameObjectsWithTag("fritzing_glow");

		if(icon.IsFritzingIcon()) {
			foreach(GameObject glow in fritzing) {
			glow.transform.localScale = new Vector3(0,0,0);
			}
		} else {
			foreach(GameObject glow in schematic) {
				glow.transform.localScale = new Vector3(0,0,0);
			}
		}

		

		GameObject[] prefabButtons = GameObject.FindGameObjectsWithTag("circuit_prefab_button");
        foreach(var item in prefabButtons) {
            if(item.GetComponent<SchComponentButton>().isButtonClicked()) {
                // item.GetComponent<SchComponentButton>().();
            }
        }
	}

	public void selectAll() {


		http.postJson(cmd.getUrl(), cmd.resetAll());
		initGlowIcon();
	}
}