using UnityEngine;

public class VWJson : MonoBehaviour
{
    public string boardPin;
    public string boardPinLine;
    public string componentPin;
    public string ip;
    
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}