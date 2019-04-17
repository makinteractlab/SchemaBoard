using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Vuforia;

public class Component : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler//, IPointerEnterHandler//, IPointerUpHandler
{
    private Vector3 wireEndPosition;
    private Vector3 wireStartPosition;
    private bool drag;
    
    public void Awake()
    {
    }

    public void Setup()
    {
    }

	public void Start()
    {
        drag = true;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        EnterPauseState();
    }

    private Vector3 getCurrentComponentPinPosition(string pinName)
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        Vector3 result = Vector3.zero;
        foreach(Transform obj in children)     
        {
            if(obj.name == pinName) {
                result = obj.position;
            }
        }
        //Debug.Log("ComponentObject.cs - getTargetComponentPinPosition = " + result);
        return result;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //drag = true;
        if(drag) {
            transform.position = new Vector3(GetCurrentMousePosition().x, transform.position.y, GetCurrentMousePosition().z);

            GameObject[] wires = GameObject.FindGameObjectsWithTag("wire");
            foreach(GameObject wireObj in wires)
            {
                if(wireObj.name.Contains(name))
                {
                    if(wireObj.name.Contains("connector0")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector0");
                        wireEndPosition.x -= 5;
                    }
                    else if(wireObj.name.Contains("connector1")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector1");
                        wireEndPosition.x += 5;
                    }
                    LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
                    wireLineRender.SetPosition(1, wireEndPosition);
                }
            }

            GameObject[] netwires = GameObject.FindGameObjectsWithTag("netwire");
            foreach(GameObject wireObj in netwires)
            {
                
                string first = wireObj.name.Substring(0,wireObj.name.IndexOf(','));
                string last = wireObj.name.Substring(wireObj.name.IndexOf(','), wireObj.name.Length - wireObj.name.IndexOf(','));
                //if(wireObj.name.Contains(name))
                if(last.Contains(name)) // if this component is FromPin
                {
                    if(last.Contains("connector0")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector0");
                        wireEndPosition.x -= 5;
                    } else if(last.Contains("connector1")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector1");
                        wireEndPosition.x += 5;
                    }
                    LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
                    wireLineRender.SetPosition(1, wireEndPosition);
                } else if(first.Contains(name)) //else if this component is To Pin
                {
                    if(first.Contains("connector0")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector0");
                        wireStartPosition.x -= 5;
                    }
                    else if(first.Contains("connector1")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector1");
                        wireStartPosition.x += 5;
                    }
                    LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
                    wireLineRender.SetPosition(0, wireStartPosition);
                }
            }
        }
    }
	
    public void setDragState(bool state) {
        drag = state;
    }

    public bool getDragState() {
        return drag;
    }

    void EnterPauseState()
    {
        Communication comm = GameObject.Find("Communication").GetComponent<Communication>();
        comm.pauseButton.gameObject.SetActive(true);
		VuforiaRenderer.Instance.Pause(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //drag = false;
        //VuforiaRenderer.Instance.Pause(false);
    }

    private Vector3 GetCurrentMousePosition()
    {
        //float distance = 1100;
        //float distance = Camera.main.nearClipPlane;
        float distance = Camera.main.transform.position.y - transform.position.y;
        
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}