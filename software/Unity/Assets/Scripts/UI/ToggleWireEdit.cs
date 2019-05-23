using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWireEdit : MonoBehaviour {
	public static ToggleWireEdit instance;
	public Sprite onSprite;
	public Sprite offSprite;

	public Sprite deleteModePinSprite;
	public Sprite connectedPinSprite;

	private Communication comm;

	bool status;

    void Awake() {
        if (ToggleWireEdit.instance == null)
            ToggleWireEdit.instance = this;
    }
    // Use this for initialization
    void Start() {
		status = false;
		comm = GameObject.Find("Communication").GetComponent<Communication>();
		this.GetComponent<Button>().onClick.AddListener(deleteMode);
    }

	void deleteMode() {
		if(status) {
			gameObject.GetComponent<Button>().image.sprite = onSprite;
			EnterDeleteMode();
			status = false;
		} else {
			gameObject.GetComponent<Button>().image.sprite = offSprite;
			ExitDeleteMode();
			status = true;
		}
	}


	public void EnterDeleteMode()
    {
		comm.setEditWireState(true);

        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            // if( wireObj.name.Contains(transform.parent.name) )
            // {
				int start = wireObj.name.IndexOf(":") + 1;
				int end = wireObj.name.IndexOf(",");
				string sourcePinName = wireObj.name.Substring(start, end - start);
				//Debug.Log("Component.cs - 500000 " + sourcePinName);
				GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = deleteModePinSprite;
				// int seperator = wireObj.name.IndexOf(",");
				// string targetComponentPinName = wireObj.name.Substring(seperator+1, wireObj.name.Length-seperator-1);
				// getTargetComponentPinObject(targetComponentPinName).GetComponent<Button>().image.sprite = DeleteModePinSprite;
            // }
        }
    }

	public void ExitDeleteMode()
    {
		comm.setEditWireState(false);

        // deleteState = false;
		// getChildObject(transform.parent.name, name).GetComponent<Button>().image.sprite = DefaultComponentSprite;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            // if( wireObj.name.Contains(transform.parent.name) )
            // {
				int start = wireObj.name.IndexOf(":") + 1;
				int end = wireObj.name.IndexOf(",");
				string sourcePinName = wireObj.name.Substring(start, end - start);
				//Debug.Log("Component.cs - 500000 " + sourcePinName);
				GameObject.Find(sourcePinName).GetComponent<Button>().image.sprite = connectedPinSprite;
                // string pinName = wireObj.name.Substring(4, wireObj.name.Length-4-name.Length);
                // Debug.Log(pinName);
                // Button btTempPin = GameObject.Find(pinName).GetComponent<Button>();
                // btTempPin.image.sprite = sprite;
            // }
        }
    }
}
