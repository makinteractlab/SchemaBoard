using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class ToggleNetWires : MonoBehaviour {
    public static ToggleNetWires instance;
	public Sprite onSprite;
	public Sprite offSprite;

	bool status;

	private string command = "";

    void Awake() {
        if (ToggleNetWires.instance == null)
            ToggleNetWires.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = true;
		this.GetComponent<Button>().onClick.AddListener(displayNetWires);
    }

	void displayNetWires() {
		//gameObject.SetActive(true);
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = onSprite;
			showNetWires(true);
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = offSprite;
			showNetWires(false);
			status = true;
		}
	}

	private void showNetWires(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("component");
        foreach(GameObject componentObj in temp) {
			GameObject[] netwireTemp = GameObject.FindGameObjectsWithTag("netwire");
			foreach(GameObject netwireObj in netwireTemp) {
				if( netwireObj.name.Contains(componentObj.name) ) {
					LineRenderer lr = netwireObj.GetComponent<LineRenderer>();
					lr.enabled = onoff;
				}
			}
        }
	}
}