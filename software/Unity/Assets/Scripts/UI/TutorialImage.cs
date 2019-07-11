using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialImage : MonoBehaviour {

	public Sprite battery;
	public Sprite resistor;
	public Sprite capacitor;
	public Sprite led;
	public Sprite button;
	public Sprite transistor;
	public Sprite diode;
	public Sprite inductor;
	public Sprite photoresistor;
	public Sprite piezo;
	public Sprite speaker;
	public Sprite chipset;
	public Sprite wire;
	public Sprite defaultImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setContent(string _name, string _position, int _componentPin) {
		Image content = this.GetComponent<Image>();

		if(_position.Contains("left"))
			content = GameObject.Find("leftImage").GetComponent<Image>();
		else if(_position.Contains("center"))
			content = GameObject.Find("centerImage").GetComponent<Image>();
		else if(_position.Contains("right"))
			content = GameObject.Find("rightImage").GetComponent<Image>();

		switch(_name) {
			case "R":
				content.sprite = resistor;
				break;
			case "C":
				content.sprite = capacitor;
				break;
			case "LED":
				content.sprite = led;
				break;
			case "S":
				content.sprite = button;
				break;
			case "Q":
				content.sprite = transistor;
				break;
			case "D":
				content.sprite = diode;
				break;
			case "L":
				content.sprite = inductor;
				break;
			case "LDR":
				content.sprite = photoresistor;
				break;
			case "J":
				content.sprite = piezo;
				break;
			case "SP":
				content.sprite = speaker;
				break;
			case "U":
				content.sprite = chipset;
				break;
			case "wire":
				content.sprite = chipset;
				break;
			default:
				content.sprite = defaultImage;
				break;
		}
	}

	public void setContent(string _name, string _position) {
		Image content = this.GetComponent<Image>();

		if(_position.Contains("left"))
			content = GameObject.Find("leftImage").GetComponent<Image>();
		else if(_position.Contains("center"))
			content = GameObject.Find("centerImage").GetComponent<Image>();
		else if(_position.Contains("right"))
			content = GameObject.Find("rightImage").GetComponent<Image>();

		switch(_name) {
			case "VCC":
			case "BT":
				content.sprite = battery;
				break;
			case "R":
				content.sprite = resistor;
				break;
			case "C":
				content.sprite = capacitor;
				break;
			case "LED":
				content.sprite = led;
				break;
			case "S":
				content.sprite = button;
				break;
			case "Q":
				content.sprite = transistor;
				break;
			case "D":
				content.sprite = diode;
				break;
			case "L":
				content.sprite = inductor;
				break;
			case "LDR":
				content.sprite = photoresistor;
				break;
			case "J":
				content.sprite = piezo;
				break;
			case "SP":
				content.sprite = speaker;
				break;
			case "U":
				content.sprite = chipset;
				break;
			case "wire":
				content.sprite = chipset;
				break;
			default:
				content.sprite = defaultImage;
				break;
		}
	}
}
