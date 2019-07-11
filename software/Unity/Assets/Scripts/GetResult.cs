using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


public class GetResult : MonoBehaviour
{
    public NetData netData;
    JObject queryJsonResult;
    JObject querySchResult;

    public void setJsonQueryResult(string _result) {
        Debug.Log("+++++++++++++++++++++++++++++++++++++ setJsonQueryResult()");
        Debug.Log("result = " + _result);

        queryJsonResult = JObject.Parse(_result);
        netData.jsonDataReceivedEvent.Invoke(queryJsonResult);
    }

    public void setSchQueryResult(string _result) {
        Debug.Log("+++++++++++++++++++++++++++++++++++++ setSchQueryResult()");
        Debug.Log("result = " + _result);

        querySchResult = new JObject(parseSchFile(_result));
        netData.schDataReceivedEvent.Invoke(querySchResult);
    }

    private JObject parseSchFile(string _result) {
        JObject result = new JObject();
        
        result.Add("1","1");// sch file parser is needed
        return result;
    }
}