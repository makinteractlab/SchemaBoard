using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.EventSystems;

public class CoverTouch : MonoBehaviour, IPointerDownHandler
{
	public Communication comm;
	GameObject arCam;
	GameObject arMarker;
	// Use this for initialization
	void Start () {
		arCam = GameObject.Find("ARCamera");
		arMarker = GameObject.Find("ImageTarget");
	}

	// Update is called once per frame
	void Update () {
		float dist = calDistance();

		if(dist>1100 && dist<1150) {
			comm.pauseButton.gameObject.SetActive(true);
			VuforiaRenderer.Instance.Pause(true);
		}
	}

	public Transform ARMarker;   //attach ar marker gameObject here
	float calDistance() {
		// Debug.Log("Distance is:"+dist);
		return Vector3.Distance(arCam.transform.position, arMarker.transform.position);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		comm.pauseButton.gameObject.SetActive(true);
		VuforiaRenderer.Instance.Pause(true);
	}
}
