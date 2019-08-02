using UnityEngine;
// using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class TutorialPrevButton : MonoBehaviour {
    public static TutorialPrevButton instance;
	public ToggleTutorial tutorial;
	int index;

    void Awake() {
        if (TutorialPrevButton.instance == null)
            TutorialPrevButton.instance = this;
    }
    // Use this for initialization
    void Start() {
		this.GetComponent<Button>().onClick.AddListener(selectComponent);
    }

	public void selectComponent() {
		index = tutorial.index;
		index--;
		if(index < 0)
			index = tutorial.getComponentNameList().Count-1;
		tutorial.setSelectedComponent(index);
	}
}