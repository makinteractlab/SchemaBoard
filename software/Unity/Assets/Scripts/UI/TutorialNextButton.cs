using UnityEngine;
// using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class TutorialNextButton : MonoBehaviour {
    public static TutorialNextButton instance;
	public ToggleTutorial tutorial;
	int index;

    void Awake() {
        if (TutorialNextButton.instance == null)
            TutorialNextButton.instance = this;
    }
    // Use this for initialization
    void Start() {
		this.GetComponent<Button>().onClick.AddListener(selectComponent);
    }

	public void selectComponent() {
		index = tutorial.index;
		index++;
		if(index > tutorial.totalSteps-1) {
			index = 0;
		}
		tutorial.setSelectedComponent(index);
	}
}