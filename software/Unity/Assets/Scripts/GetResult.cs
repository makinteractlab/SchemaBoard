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
        Debug.Log(querySchResult.ToString());
        netData.schDataReceivedEvent.Invoke(querySchResult);
    }

    private JObject parseSchFile(string _result) {
        JObject result = new JObject();
        string[] resultStrings = _result.Split('\n');
        
        int count = 0;
        foreach(string line in resultStrings) {
            if(line.CompareTo("$Comp") == 0) {
                if(count<resultStrings.Length-3) {
                    string[] componentName = resultStrings[count+1].Split(' ');
                    string[] componentPos = resultStrings[count+3].Split(' ');
                    Debug.Log("name: " + componentName[2]);
                    Debug.Log("x: " + componentPos[1]);
                    Debug.Log("y: " + componentPos[2]);
                    //JObject tempJson = new JObject();
                    //tempJson.Add("name", componentName[2]);
                    //tempJson.Add("x", componentPos[1]);
                    //tempJson.Add("y", componentPos[2]);
                    //result.Add(tempJson);
                    //tempJson.RemoveAll();
                }
            } else if(line.CompareTo("Wire Wire Line") == 0) {
                // JObject tempJson = new JObject();
                // JObject rootJson = new JObject();
                if(count<resultStrings.Length-1) {
                    string[] wirePos = resultStrings[count+1].Split(' ');
                    Debug.Log("wire");
                    Debug.Log("x1: " + wirePos[0]);
                    Debug.Log("y1: " + wirePos[1]);
                    Debug.Log("x2: " + wirePos[2]);
                    Debug.Log("y2: " + wirePos[3]);
                    // tempJson.Add("x1", wirePos[0]);
                    // tempJson.Add("y1", wirePos[1]);
                    // tempJson.Add("x2", wirePos[2]);
                    // tempJson.Add("y2", wirePos[3]);
                    // rootJson.Add("wire", tempJson);
                    // result.Add(rootJson);
                    // tempJson.RemoveAll();
                    // rootJson.RemoveAll();
                }
            } else if(line.Contains("Connection ~")) {
                string[] connectionsPos = line.Split(' ');
                Debug.Log("connection");
                Debug.Log("x: " + connectionsPos[2]);
                Debug.Log("y: " + connectionsPos[3]);
            }
            count++;
        }
        return result;
    }
}