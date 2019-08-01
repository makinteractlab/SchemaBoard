using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateButton : MonoBehaviour {
	private Vector3 wireEndPosition;
    private Vector3 wireStartPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Rotate() {
        GameObject leftPinObject = getChildObject(transform.parent.name, "connector0");
        GameObject RightPinObject = getChildObject(transform.parent.name, "connector1");

        //rotate component with pins
        gameObject.transform.parent.Rotate(new Vector3 (0, 0, 1), 90);

        //only pins are rotate around the components
        // Vector3 targetPos = getChildObject(transform.parent.name, "Component").transform.position;
		// leftPinObject.transform.RotateAround(targetPos, new Vector3 (0, 1, 0), 90);
		// RightPinObject.transform.RotateAround(targetPos, new Vector3 (0, 1, 0), 90);
		// gameObject.transform.RotateAround(targetPos, new Vector3 (0, 1, 0), 90);    //90 button

		GameObject[] wires = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in wires)
        {
            string first = wireObj.name.Substring(wireObj.name.IndexOf(':')+1,wireObj.name.IndexOf(',')-wireObj.name.IndexOf(':')-1);
            string last = wireObj.name.Substring(wireObj.name.IndexOf(',')+1, wireObj.name.Length-wireObj.name.IndexOf(',')-1);
            string firstComponentName = first.Substring(0,first.LastIndexOf('-'));
            string lastComponentName = last.Substring(0,last.LastIndexOf('-'));

            if(String.Compare(lastComponentName, transform.parent.name, true) == 0 || String.Compare(firstComponentName, transform.parent.name) == 0)
            {
                if(wireObj.name.Contains("connector0")) {
                    wireEndPosition = leftPinObject.transform.position;
                    wireEndPosition.x -= 5;
                }
                else if(wireObj.name.Contains("connector1")) {
                    wireEndPosition = RightPinObject.transform.position;
                    wireEndPosition.x += 5;
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

            if(String.Compare(lastComponentName, transform.parent.name, true) == 0) // if this component is FromPin
            {
                if(last.Contains("connector0")) {
                    wireEndPosition = leftPinObject.transform.position;
                    wireEndPosition.x -= 5;
                }
                else if(last.Contains("connector1")) {
                    wireEndPosition = RightPinObject.transform.position;
                    wireEndPosition.x += 5;
                }
                LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
                wireLineRender.SetPosition(1, wireEndPosition);
            } else if(String.Compare(firstComponentName, transform.parent.name) == 0) //else if this component is To Pin
            {
                if(first.Contains("connector0")) {
                    wireStartPosition = leftPinObject.transform.position;
                    wireStartPosition.x -= 5;
                }
                else if(first.Contains("connector1")) {
                    wireStartPosition = RightPinObject.transform.position;
                    wireStartPosition.x += 5;
                }
                LineRenderer wireLineRender = wireObj.GetComponent<LineRenderer>();
                wireLineRender.SetPosition(0, wireStartPosition);
            }
        }
    }

	private GameObject getChildObject(string ParentObjectName, string ChildObjectName)
    {
        GameObject temp = GameObject.Find(ParentObjectName);
        GameObject resultObj = null;
        
        Transform[] children = temp.GetComponentsInChildren<Transform>();
        foreach(Transform obj in children)     
        {
            //Debug.Log("[pin.cs] 7 " + obj.name);
            if(obj.name == ChildObjectName) {
                resultObj = obj.gameObject;
            }
        }
        //Debug.Log("[Pin.cs] 88 " + "getChildObject = " + resultObj.name);
        //Debug.Log("[Pin.cs] 99 " + "getChildObject = " + resultObj.transform.parent.name);
        return resultObj;
    }
}
