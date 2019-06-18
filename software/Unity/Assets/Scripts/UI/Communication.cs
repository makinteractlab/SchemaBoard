using UnityEngine;
//using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Communication : MonoBehaviour
{
    public VWJson vw;
    private int key = 1;
    private float time;
    private bool deleteState;
    private bool dragState;
    private bool popupState;
    private bool editWireState;

    private string component;

    public Button pauseButton;
    public Image freezeAll;

    private string selectedJsonFileName;

    void Start()
    {
        deleteState = false;
        dragState = false;
        popupState = false;
        editWireState = false;
    }

    public string getCurrentFileName() {
        return selectedJsonFileName;
    }

    public void setCurrentFileName(string _selectedJsonFileName) {
        selectedJsonFileName = _selectedJsonFileName;
    }

    public GameObject getPauseButton() {
        return pauseButton.gameObject;
    }
    
    public bool getPopupState()
    {
        return popupState;
    }

    public void setPopupState(bool state)
    {
        popupState = state;
    }

    public bool getDragState()
    {
        return dragState;
    }

    public void setDragState(bool state)
    {
        dragState = state;
    }

    public void setDeleteWireState(bool state, string name)
    {
        deleteState = state;
        component = name;
    }

    public string getComponentInDeleteState()
    {
        return component;
    }

    public void setEditWireState(bool state) {
        editWireState = state;
    }
    public bool getEditWireState() {
        return editWireState;
    }

    public bool getDeleteWireState()
    {
        return deleteState;
    }

    public void setStartTime(float t)
    {
        time = t;
    }

    public float getStartTime()
    {
        return time;
    }

    // public void setBreadboardPinLine(string _breadboardPinLine)
    // {
    //     vw.boardPinLine = _breadboardPinLine;
    // }

    // public string getBreadboardPinLine()
    // {
    //     return vw.boardPinLine;
    // }

    public void setBoardPin(string pin)
    {
        vw.boardPin = pin;
    }

    public string getBoardPin()
    {
        return vw.boardPin;
    }

    public void setComponentPin(string pin)
    {
        vw.componentPin = pin;
    }

    public string getComponentPin()
    {
        return vw.componentPin;
    }

    public void resetData()
    {
        vw.boardPin = null;
        vw.componentPin = null;
    }

    public void setBreadboardIP(string _ip)
    {
        vw.ip = _ip;
    }

    public string getBreadboardIP()
    {
        return vw.ip;
    }
}