using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
//using Newtonsoft.Json.Linq;

public class TutorialPrevButton : MonoBehaviour {
    public static TutorialPrevButton instance;

	public Sprite onSprite;
	public Sprite offSprite;
	public ToggleIcon icon;
	// public Image background;
	public Button prevButton;
	public Button nextButton;
	public Button restartButton;
	bool status;

    void Awake() {
        if (TutorialPrevButton.instance == null)
            TutorialPrevButton.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = false;
		//changeComponentIcon();
		this.GetComponent<Button>().onClick.AddListener(prevComponent);
    }

	void showGlowIcon() {
	}

	public void prevComponent() {
		if(status) {	//tutorial off
			status = false;
			this.GetComponent<Image>().sprite = offSprite;
			showGlowIcon();
		}
	}
}