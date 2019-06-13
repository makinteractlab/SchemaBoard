using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DrawVirtualWire : MonoBehaviour
{
    private LineRenderer line;
    public Material material;
    private GameObject boardPinObj;
    private GameObject componentPinObj;
    public Sprite connectedPinSprite;
    public Sprite deletePinSprite;
    public Communication comm;

    public void Reset(LineRenderer lr)
    {
        //Debug.Log("[DrawVirtualWire.cs] " + lr.name);
        //lr.positionCount = 0;
        //lr.SetVertexCount(0);
        lr.enabled = false;
    }

    void Start() {
        boardPinObj = null;
        componentPinObj = null;
        //comm = GameObject.Find("Communication").GetComponent<Communication>();
        //EventMessenger
    }

    void Update()
    {
        if(boardPinObj != null && componentPinObj != null)
        {
            int xOffset = 0;
            // line = new GameObject("Wire" + ":" + boardPinObj.name + "," + componentPinObj.transform.parent.name + "-" + componentPinObj.name).AddComponent<LineRenderer>();
            // Debug.Log(line.name);

            // line.material = material;
            // line.positionCount = 2;
            // line.startWidth = 4;
            // line.endWidth = 4;
            // line.SetPosition(0, boardPinObj.transform.position);
            
            if(componentPinObj.name.Contains("connector0")) xOffset = -5;
            else if(componentPinObj.name.Contains("connector1")) xOffset = 5;

            Vector3 startPos = boardPinObj.transform.position;
            Vector3 endPos = new Vector3(componentPinObj.transform.position.x+xOffset, componentPinObj.transform.position.y-5, componentPinObj.transform.position.z);
            string wireObjectName = "Wire" + ":" + boardPinObj.name + "," + componentPinObj.transform.parent.name + "-" + componentPinObj.name;

            createWireObject(startPos, endPos, wireObjectName, boardPinObj.name);
            // line.SetPosition(1, new Vector3(componentPinObj.transform.position.x+xOffset, componentPinObj.transform.position.y-5, componentPinObj.transform.position.z));

            // line.tag = "wire";
            // line = null;
            // Button btTempPin = boardPinObj.GetComponent<Button>();
            // if(comm.getEditWireState()) {
            //     btTempPin.image.sprite = deletePinSprite;
            // } else {
            //     btTempPin.image.sprite = connectedPinSprite;
            // }
            resetComponentPinObj();
            resetBoardPinObj();
        }
    }

    public void createWireObject(Vector3 _startPos, Vector3 _endPos, string _wireObjectname, string _boardPinObjName) {
        line = new GameObject(_wireObjectname).AddComponent<LineRenderer>();
        Debug.Log(line.name);

        line.material = material;
        line.positionCount = 2;
        line.startWidth = 6;
        line.endWidth = 6;
        line.SetPosition(0, _startPos);
        line.SetPosition(1, _endPos);

        line.tag = "wire";
        line = null;
        Button btTempPin = GameObject.Find(_boardPinObjName).GetComponent<Button>();
        if(comm.getEditWireState()) {
            btTempPin.image.sprite = deletePinSprite;
        } else {
            btTempPin.image.sprite = connectedPinSprite;
        }
    }

    // void updateConnectionData(GameObject src, GameObject tgt) {
    //     src.GetComponent<Pin>().connectedTo = tgt;
    // }

    public void removeWireWithComponent(string targetComponent)
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        foreach(GameObject wireObj in temp)
        {
            if( wireObj.name.Contains(targetComponent) )
            {
                LineRenderer lr = wireObj.GetComponent<LineRenderer>();
                lr.enabled = false;
                //lr.SetVertexCount(0);
                lr.positionCount = 0;
                Destroy(wireObj);
            }
        }
        //GameObject compObjTemp = GameObject.Find(targetComponent);
        //Destroy(compObjTemp);
    }

    public void removeWire(string targetComponent, string  sourcePin)
    {
        //Debug.Log("[DrawVirtualWire.cs] " + "removeWire");
        GameObject[] temp = GameObject.FindGameObjectsWithTag("wire");
        GameObject targetTemp = GameObject.Find(targetComponent);

        foreach(GameObject wireObj in temp)
        {
            //Debug.Log("[DrawVirtualWire.cs] " + "foreach");
            //if there is a wire which is already connected to this target
            if( wireObj.name.Contains(targetComponent) && wireObj.name.Contains(sourcePin))
            {
                LineRenderer lr = wireObj.GetComponent<LineRenderer>();
                lr.enabled = false;
                //lr.SetVertexCount(0);
                lr.positionCount = 0;
                Destroy(wireObj);
            }
        }
        //Debug.Log("DrawVirtualWires.cs : removeWire() " + "target: " + targetComponent + "source: " + sourcePin);
    }

    public void setBoardPinObj(GameObject obj)
    {
        boardPinObj = obj;
        if(componentPinObj != null) {
            boardPinObj.GetComponent<Pin>().connectedTo = componentPinObj;
        }
    }

    public string setComponentPinObj(GameObject obj)
    {
        componentPinObj = obj;
        if(boardPinObj != null)
            boardPinObj.GetComponent<Pin>().connectedTo = componentPinObj;
        return boardPinObj.name;
    }

    public void resetComponentPinObj()
    {
        //Debug.Log("resetComponentPin");
        componentPinObj = null;
    }

    public void resetBoardPinObj()
    {
        //Debug.Log("resetBoardPin");
        boardPinObj = null;
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}