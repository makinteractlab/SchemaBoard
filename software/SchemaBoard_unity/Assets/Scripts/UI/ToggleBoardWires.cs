using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class ToggleBoardWires : MonoBehaviour {
    public static ToggleBoardWires instance;
	public Sprite onSprite;
	public Sprite offSprite;
	bool status;

    void Awake() {
        if (ToggleBoardWires.instance == null)
            ToggleBoardWires.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = true;
		this.GetComponent<Button>().onClick.AddListener(displayWires);
    }

	void displayWires() {
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = onSprite;
			showWires(true);
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = offSprite;
			showWires(false);
			status = true;
		}
	}

	private void showWires(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("manual_prefab");
        foreach(GameObject componentObj in temp) {
			GameObject[] wireTemp = GameObject.FindGameObjectsWithTag("wire");
			foreach(GameObject wireObj in wireTemp) {
				if( wireObj.name.Contains(componentObj.name) ) {
					LineRenderer lr = wireObj.GetComponent<LineRenderer>();
					lr.enabled = onoff;
				}
			}
        }
	}
}