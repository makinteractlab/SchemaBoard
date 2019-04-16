using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if(wireObj.name.Contains(transform.parent.name))
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
            string first = wireObj.name.Substring(0,wireObj.name.IndexOf(','));
            string last = wireObj.name.Substring(wireObj.name.IndexOf(','), wireObj.name.Length - wireObj.name.IndexOf(','));
            if(last.Contains(transform.parent.name)) // if this component is FromPin
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
            } else if(first.Contains(transform.parent.name)) //else if this component is To Pin
            {
                if(wireObj.name.Contains("connector0")) {
                    wireStartPosition = leftPinObject.transform.position;
                    wireStartPosition.x -= 5;
                }
                else if(wireObj.name.Contains("connector1")) {
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
