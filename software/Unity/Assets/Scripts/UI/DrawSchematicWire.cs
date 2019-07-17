using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DrawSchematicWire : MonoBehaviour
{
    private LineRenderer line;
    public Material material;
    private GameObject fromPinObj;
    private GameObject toPinObj;

    public LineTextureMode textureMode = LineTextureMode.Tile;
    public RectTransform ParentPanel;
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
            //int xOffset = 0;
            if(   GameObject.Find("Wire" + ":" + toPinObj.transform.parent.name + "-" + toPinObj.name + "," + fromPinObj.transform.parent.name + "-" + fromPinObj.name) == null
               && GameObject.Find("Wire" + ":" + fromPinObj.transform.parent.name + "-" + fromPinObj.name + "," + toPinObj.transform.parent.name + "-" + toPinObj.name) == null) {
                
                // string temp = "Wire" + ":" + fromPinObj.transform.parent.name + "-" + fromPinObj.name + "," + toPinObj.transform.parent.name + "-" + toPinObj.name;
                // if(temp == "Wire:R1-connector0,P1-connector1")
                //     toPinObj = Util.getChildObject(GameObject.Find("P1"), "connector1");
                line = new GameObject("Wire" + ":" + fromPinObj.transform.parent.name + "-" + fromPinObj.name + "," + toPinObj.transform.parent.name + "-" + toPinObj.name).AddComponent<LineRenderer>();
                line.transform.SetParent(ParentPanel,false);
                line.material = material;

                line.textureMode = textureMode;
                var distance = Vector3.Distance(fromPinObj.transform.position, toPinObj.transform.position);
                line.materials[0].mainTextureScale = new Vector3(distance, 1, 1);

                line.positionCount = 2;
                line.startWidth = 6;
                line.endWidth = 6;
                line.tag = "schwire";

                Debug.Log("\n\n\n\n\n ===== From: " + fromPinObj.transform.parent.name + "-" + fromPinObj.name + "\n\n\n\n\n");
				Debug.Log("\n\n\n\n\n ===== To: " + toPinObj.transform.parent.name + "-" + toPinObj.name + "\n\n\n\n\n");
                Debug.Log("\n\n\n\n\n ===== WireName: " + line.name + "\n\n\n\n\n");

                //Vector3 screenOffset = new Vector3(Screen.width/6, Screen.height/6, 0);
                line.SetPosition(0, fromPinObj.transform.position);
                line.SetPosition(1, toPinObj.transform.position);

                // int xOffset = 0;

                // if(fromPinObj.name.Contains("connector0")) xOffset = -3;
                // else if(fromPinObj.name.Contains("connector1")) xOffset = 3;
                // line.SetPosition(0, new Vector3(fromPinObj.transform.position.x+xOffset, fromPinObj.transform.position.y-5, fromPinObj.transform.position.z));

                // if(toPinObj.name.Contains("connector0")) xOffset = -3;
                // else if(toPinObj.name.Contains("connector1")) xOffset = 3;
                // line.SetPosition(1, new Vector3(toPinObj.transform.position.x+xOffset, toPinObj.transform.position.y-5, toPinObj.transform.position.z));

                line = null;
            }
        }
        resetToPinObj();
        resetFromPinObj();
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
        toPinObj = null;
    }

    public void resetFromPinObj()
    {
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