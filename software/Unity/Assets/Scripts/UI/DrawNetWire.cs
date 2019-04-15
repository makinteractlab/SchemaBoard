using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DrawNetWire : MonoBehaviour
{
    private LineRenderer line;
    public Material material;
    private GameObject fromPinObj;
    private GameObject toPinObj;

    public LineTextureMode textureMode = LineTextureMode.Tile;
    //public float tileAmount = 1.0f;

    public void Reset(LineRenderer lr)
    {
        //Debug.Log("[DrawVirtualWire.cs] " + lr.name);
        //lr.positionCount = 0;
        //lr.SetVertexCount(0);
        lr.enabled = false;
    }

    void Start() {
        fromPinObj = null; // board
        toPinObj = null; // component
        //EventMessenger
    }

    void Update()
    {
    }

    public void createWireObject(GameObject _fromPin, GameObject _toPin)
    {
        fromPinObj = _fromPin;
        toPinObj = _toPin;
        if((fromPinObj != null) && (toPinObj != null))
        {
            int xOffset = 0;
            if(   GameObject.Find("Wire" + ":" + toPinObj.transform.parent.name + "-" + toPinObj.name + "," + fromPinObj.transform.parent.name + "-" + fromPinObj.name) == null
               && GameObject.Find("Wire" + ":" + fromPinObj.transform.parent.name + "-" + fromPinObj.name + "," + toPinObj.transform.parent.name + "-" + toPinObj.name) == null) {
                line = new GameObject("Wire" + ":" + fromPinObj.transform.parent.name + "-" + fromPinObj.name + "," + toPinObj.transform.parent.name + "-" + toPinObj.name).AddComponent<LineRenderer>();
                line.material = material;

                line.textureMode = textureMode;
                var distance = Vector3.Distance(fromPinObj.transform.position, toPinObj.transform.position);
                line.materials[0].mainTextureScale = new Vector3(distance, 1, 1);

                line.positionCount = 2;
                line.startWidth = 4;
                line.endWidth = 4;
                line.tag = "netwire";

                line.SetPosition(0, fromPinObj.transform.position);
                if(toPinObj.name.Contains("connector0")) xOffset = -10;
                else if(toPinObj.name.Contains("connector1")) xOffset = 10;
                line.SetPosition(1, new Vector3(toPinObj.transform.position.x+xOffset, toPinObj.transform.position.y-5, toPinObj.transform.position.z));
                line = null;
            }
            resetToPinObj();
            resetFromPinObj();
        }
    }

    public void setFromPinObj(GameObject obj)
    {
        fromPinObj = obj;
    }

    public void setToPinObj(GameObject obj)
    {
        toPinObj = obj;
    }

    public void resetToPinObj()
    {
        //Debug.Log("resetComponentPin");
        toPinObj = null;
    }

    public void resetFromPinObj()
    {
        //Debug.Log("resetBoardPin");
        fromPinObj = null;
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