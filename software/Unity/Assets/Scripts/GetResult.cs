using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


public class GetResult : MonoBehaviour
{
    public NetData netData;
    JObject queryResult;

    public void setQueryResult(string result) {
        Debug.Log("setQueryResult()");
        Debug.Log("result = " + result);

        queryResult = JObject.Parse(result);
        netData.dataReceivedEvent.Invoke(queryResult);
    }
}