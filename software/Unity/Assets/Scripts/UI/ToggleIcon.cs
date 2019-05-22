using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class ToggleIcon : MonoBehaviour {
    public static ToggleIcon instance;
	public Sprite resistorSymbol;
	public Sprite capacitorSymbol;
	public Sprite ledSymbol;
	public Sprite photoresistorSymbol;
	public Sprite vccSymbol;
	public Sprite inductorSymbol;
	public Sprite switchSymbol;
	public Sprite diodeSymbol;
	public Sprite zenerdiodeSymbol;
	public Sprite etcSymbol;

	public Sprite resistorImage;
	public Sprite capacitorImage;
	public Sprite ledImage;
	public Sprite photoresistorImage;
	public Sprite vccImage;
	public Sprite inductorImage;
	public Sprite switchImage;
	public Sprite diodeImage;
	public Sprite zenerdiodeImage;
	public Sprite etcImage;

	public Sprite onSprite;
	public Sprite offSprite;

	bool status;

	private string command = "";

    void Awake() {
        if (ToggleIcon.instance == null)
            ToggleIcon.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = true;
		this.GetComponent<Button>().onClick.AddListener(changeComponentIcon);
    }

	void changeComponentIcon() {
		//gameObject.SetActive(true);
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = onSprite;
			showSchematicSymbol(true);
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = offSprite;
			showSchematicSymbol(false);
			status = true;
		}
	}

	private void showSchematicSymbol(bool onoff) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("component");
		
		if(onoff) {
			foreach(GameObject componentObj in temp) {
				if( componentObj.name.Contains("LED") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = ledSymbol;
				} else if( componentObj.name.Contains("R") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = resistorSymbol;
				} else if( componentObj.name.Contains("C") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = capacitorSymbol;
				} else if( componentObj.name.Contains("L") && !componentObj.name.Contains("ED") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = inductorSymbol;
				} else if( componentObj.name.Contains("S") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = switchSymbol;
				} else if( componentObj.name.Contains("P") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = photoresistorSymbol;
				} else if( componentObj.name.Contains("D") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = diodeSymbol;
				} else if( componentObj.name.Contains("ZD") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = zenerdiodeSymbol;
				} else if( componentObj.name.Contains("VCC") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = vccSymbol;
				} else {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = etcSymbol;
				}
			}
		} else {
			foreach(GameObject componentObj in temp) {
				if( componentObj.name.Contains("LED") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = ledImage;
				} else if( componentObj.name.Contains("R") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = resistorImage;
				} else if( componentObj.name.Contains("C") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = capacitorImage;
				} else if( componentObj.name.Contains("L") && !componentObj.name.Contains("ED") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = inductorImage;
				} else if( componentObj.name.Contains("S") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = switchImage;
				} else if( componentObj.name.Contains("P") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = photoresistorImage;
				} else if( componentObj.name.Contains("D") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = diodeImage;
				} else if( componentObj.name.Contains("ZD") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = zenerdiodeImage;
				} else if( componentObj.name.Contains("VCC") ) {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = vccImage;
				} else {
					Util.getChildObject(componentObj.name, "Component").GetComponent<Button>().image.sprite = etcImage;
				}
			}
		}
	}
}