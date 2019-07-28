using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

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
        string parsedResult = parseSchFile(_result);
        querySchResult = JObject.Parse(parsedResult);
        Debug.Log("result = " + querySchResult.ToString());
        netData.schDataReceivedEvent.Invoke(querySchResult);
    }

    private string parseSchFile(string _result) {
        JObject result = new JObject();
        string[] resultStrings = _result.Split('\n');
        
        int count = 0;
        int wireCount = 0;
        int connectionCount = 0;
        foreach(string line in resultStrings) {
            if(line.CompareTo("$Comp") == 0) {
                if(count<resultStrings.Length-9) {
                    string[] componentName = resultStrings[count+1].Split(' ');
                    string[] componentPos = resultStrings[count+3].Split(' ');
                    string[] compOrient = resultStrings[count+9].Split(' ');
                    string orient = "";
                    foreach(string item in compOrient) {
                        orient += item.Trim();
                    }
                    Debug.Log("name: " + componentName[2]);
                    Debug.Log("x: " + componentPos[1]);
                    Debug.Log("y: " + componentPos[2]);
                    Debug.Log("orient: " + orient);
                    
                    JObject tempJson = new JObject();
                    tempJson.Add("x", int.Parse(componentPos[1]));
                    tempJson.Add("y", int.Parse(componentPos[2]));

                    switch (orient) {
                        case "100-1":   // default
                            tempJson.Add("degree", 0);
                            tempJson.Add("flip", "none");
                            break;
                        case "1001":    // degree 0 mirror x
                            tempJson.Add("degree", 0);
                            tempJson.Add("flip", "x");
                            break;
                        case "-100-1":  // degree 0 mirror y
                            tempJson.Add("degree", 0);
                            tempJson.Add("flip", "y");
                            break;
                        case "0-1-10":  // degree 90
                            tempJson.Add("degree", 90);
                            tempJson.Add("flip", "none");
                            break;
                        case "0-110":   // degree 90 mirror x
                            tempJson.Add("degree", 90);
                            tempJson.Add("flip", "x");
                            break;
                        case "01-10":   // degree 90 mirror y
                            tempJson.Add("degree", 90);
                            tempJson.Add("flip", "y");
                            break;
                        case "-1001":   // degree 180
                            tempJson.Add("degree", 180);
                            tempJson.Add("flip", "none");
                            break;
                        case "0110":    // degree -90
                            tempJson.Add("degree", -90);
                            tempJson.Add("flip", "none");
                            break;
                    }

                    result.Add(componentName[2], tempJson);
                    //tempJson.RemoveAll();
                }
            } else if(line.CompareTo("Wire Wire Line") == 0) {
                JObject tempJson = new JObject();
                if(count<resultStrings.Length-1) {
                    string textLine = resultStrings[count+1];
                    string[] wirePos = textLine.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                    Debug.Log("wire");
                    Debug.Log("x1: " + int.Parse(wirePos[0]));
                    Debug.Log("y1: " + int.Parse(wirePos[1]));
                    Debug.Log("x2: " + int.Parse(wirePos[2]));
                    Debug.Log("y2: " + int.Parse(wirePos[3]));

                    wireCount++;

                    tempJson.Add("x1", int.Parse(wirePos[0]));
                    tempJson.Add("y1", int.Parse(wirePos[1]));
                    tempJson.Add("x2", int.Parse(wirePos[2]));
                    tempJson.Add("y2", int.Parse(wirePos[3]));
                    result.Add("wire" + wireCount, tempJson);
                    // tempJson.RemoveAll();
                }
            } else if(line.Contains("Connection ~")) {
                JObject tempJson = new JObject();
                string[] connectionsPos = line.Split(' ');
                Debug.Log("connection");
                Debug.Log("x: " + connectionsPos[2]);
                Debug.Log("y: " + connectionsPos[3]);
                connectionCount++;
                tempJson.Add("x", int.Parse(connectionsPos[2]));
                tempJson.Add("y", int.Parse(connectionsPos[3]));
                result.Add("connection" + connectionCount, tempJson);
            }
            count++;
        }
        return result.ToString();
    }
}