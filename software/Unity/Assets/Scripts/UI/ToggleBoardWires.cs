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

    void Awake()
    {
        if (ToggleBoardWires.instance == null)
            ToggleBoardWires.instance = this;
    }
    // Use this for initialization
    void Start()
    {
		status = true;
		this.GetComponent<Button>().onClick.AddListener(displayWires);
    }

	void displayWires() {
		//gameObject.SetActive(true);
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = onSprite;
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = offSprite;
			status = true;
		}
	}
}