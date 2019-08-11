using UnityEngine;
// using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class TutorialRestartButton : MonoBehaviour {
    public static TutorialRestartButton instance;
	public ToggleTutorial tutorial;
	int index;

    void Awake() {
        if (TutorialRestartButton.instance == null)
            TutorialRestartButton.instance = this;
    }
    // Use this for initialization
    void Start() {
		this.GetComponent<Button>().onClick.AddListener(selectComponent);
    }

	public void selectComponent() {
		tutorial.setSelectedComponent(0);
        tutorial.prevButtonObj.clicked = false;
	}
}