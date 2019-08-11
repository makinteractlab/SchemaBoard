using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Vuforia;

public class DrawLine : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Communication comm;
    public Material material;
    private LineRenderer line;
    private Vector3 mousePosition;
    private Vector3 wireStartPosition;
    private Vector3 wireEndPosition;

    Vector2 startPoint;
    Vector2 endPoint;
    bool drag;

    public void Start()
    {
        setCommunicationObject();
        // lineRenderer = gameObject.AddComponent<LineRenderer>();
        // //lineRenderer.SetWidth(4, 4);
        // lineRenderer.startWidth = 4;
        // lineRenderer.endWidth = 4;
        // lineRenderer.enabled = false;
        // lineRenderer.material = material;
        // //lineRenderer.SetVertexCount(0);
        // lineRenderer.positionCount = 0;
    }

    public void setCommunicationObject(Communication obj)
    {
        comm = obj;
    }

    public void setCommunicationObject()
    {
		comm = GameObject.Find("Communication").GetComponent<Communication>();
    }

    void EnterPauseState()
    {
        //Communication comm = GameObject.Find("Communication").GetComponent<Communication>();
        comm.pauseButton.gameObject.SetActive(true);
		VuforiaRenderer.Instance.Pause(true);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        EnterPauseState();
        line = new GameObject("Wire" + ":" + this.transform.parent.name + "-" + name).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 6;
        line.endWidth = 6;

        // line.SetPosition(0, boardPinObj.transform.position);
        line.positionCount = 1;
        //comm.setSourcePin(name);
        // if(GameObject.Find(comm.getSourcePin())) {
        //     lineRenderer.SetPosition(0, GameObject.Find(name).transform.position);
        // } else {
        wireStartPosition = GetCurrentMousePosition();
        line.SetPosition(0, wireStartPosition);
        // }
    }

    public void OnDrag(PointerEventData eventData)
    {
        drag = true;
        wireEndPosition = GetCurrentMousePosition();
        line.enabled = true;
        //lineRenderer.SetVertexCount(2);
        line.positionCount = 2;
        line.SetPosition(1, wireEndPosition);
        line.tag = "instantwire";
        //lineRenderer.material.mainTextureScale = new Vector2(4, 4);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.enabled = false;
        //lineRenderer.SetVertexCount(0);
        line.positionCount = 0;
        drag = false;
        GameObject temp = GameObject.Find("Wire" + ":" + this.transform.parent.name + "-" + name);
        if(temp != null)
            Destroy(temp);
        line = null;
        //VuforiaRenderer.Instance.Pause(false);
    }

    private Vector3 GetCurrentMousePosition()
    {
        //float distance = 1500;//GameObject.Find(comm.getSourcePin()).transform.position.z;
        //float distance = Camera.main.nearClipPlane;
        float distance = Camera.main.transform.position.y - transform.position.y;
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}