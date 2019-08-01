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
                string first = wireObj.name.Substring(wireObj.name.IndexOf(':')+1,wireObj.name.IndexOf(',')-wireObj.name.IndexOf(':')-1);
				string last = wireObj.name.Substring(wireObj.name.IndexOf(',')+1, wireObj.name.Length-wireObj.name.IndexOf(',')-1);
				string firstComponentName = first.Substring(0,first.LastIndexOf('-'));
				string lastComponentName = last.Substring(0,last.LastIndexOf('-'));

                //if(wireObj.name.Contains(name)) // should be fixed!
                if(String.Compare(lastComponentName, name, true) == 0 || String.Compare(firstComponentName, name) == 0)
                {
                    if(wireObj.name.Contains("connector0")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector0");
                    } else if(wireObj.name.Contains("connector1")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector1");
                    } else if(wireObj.name.Contains("connector2")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector2");
                    } else if(wireObj.name.Contains("connector3")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector3");
                    } else if(wireObj.name.Contains("connector4")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector4");
                    } else if(wireObj.name.Contains("connector5")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector5");
                    } else if(wireObj.name.Contains("connector6")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector6");
                    } else if(wireObj.name.Contains("connector7")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector7");
                    } else if(wireObj.name.Contains("connector8")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector8");
                    } else if(wireObj.name.Contains("connector9")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector9");
                    } else if(wireObj.name.Contains("connector10")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector10");
                    } else if(wireObj.name.Contains("connector11")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector11");
                    } else if(wireObj.name.Contains("connector12")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector12");
                    } else if(wireObj.name.Contains("connector13")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector13");
                    } else if(wireObj.name.Contains("connector14")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector14");
                    } else if(wireObj.name.Contains("connector15")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector15");
                    }
                    LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
                    wireLineRender.SetPosition(1, wireEndPosition);
                }
            }

            GameObject[] netwires = GameObject.FindGameObjectsWithTag("netwire");
            foreach(GameObject wireObj in netwires)
            {
                string first = wireObj.name.Substring(wireObj.name.IndexOf(':')+1,wireObj.name.IndexOf(',')-wireObj.name.IndexOf(':')-1);
				string last = wireObj.name.Substring(wireObj.name.IndexOf(',')+1, wireObj.name.Length-wireObj.name.IndexOf(',')-1);
				string firstComponentName = first.Substring(0,first.LastIndexOf('-'));
				string lastComponentName = last.Substring(0,last.LastIndexOf('-'));


                //if(wireObj.name.Contains(name))
                if(String.Compare(lastComponentName, name, true) == 0) // if this component is FromPin
                {
                    if(last.Contains("connector0")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector0");
                    } else if(last.Contains("connector1")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector1");
                    } else if(last.Contains("connector2")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector2");
                    } else if(last.Contains("connector3")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector3");
                    } else if(last.Contains("connector4")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector4");
                    } else if(last.Contains("connector5")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector5");
                    } else if(last.Contains("connector6")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector6");
                    } else if(last.Contains("connector7")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector7");
                    } else if(last.Contains("connector8")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector8");
                    } else if(last.Contains("connector9")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector9");
                    } else if(last.Contains("connector10")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector10");
                    } else if(last.Contains("connector11")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector11");
                    } else if(last.Contains("connector12")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector12");
                    } else if(last.Contains("connector13")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector13");
                    } else if(last.Contains("connector14")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector14");
                    } else if(last.Contains("connector15")) {
                        wireEndPosition = getCurrentComponentPinPosition("connector15");
                    }
                    LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
                    wireLineRender.SetPosition(1, wireEndPosition);
                } else if(String.Compare(firstComponentName, name) == 0) //else if this component is To Pin
                {
                    if(first.Contains("connector0")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector0");
                    } else if(first.Contains("connector1")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector1");
                    } else if(first.Contains("connector2")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector2");
                    } else if(first.Contains("connector3")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector3");
                    } else if(first.Contains("connector4")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector4");
                    } else if(first.Contains("connector5")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector5");
                    } else if(first.Contains("connector6")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector6");
                    } else if(first.Contains("connector7")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector7");
                    } else if(first.Contains("connector8")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector8");
                    } else if(first.Contains("connector9")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector9");
                    } else if(first.Contains("connector10")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector10");
                    } else if(first.Contains("connector11")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector11");
                    } else if(first.Contains("connector12")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector12");
                    } else if(first.Contains("connector13")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector13");
                    } else if(first.Contains("connector14")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector14");
                    } else if(first.Contains("connector15")) {
                        wireStartPosition = getCurrentComponentPinPosition("connector15");
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