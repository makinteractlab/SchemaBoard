using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class HttpRequest : MonoBehaviour {
    public GetResult getResult;
    public void postJson(string url, string json) {
        //StartCoroutine(PostRequest("http://192.168.4.3", json));
		StartCoroutine(PostRequest(url, json));
    }

    public void getJson(string _url, string _json) {
        //StartCoroutine(PostRequest("http://192.168.4.3", json));
		StartCoroutine(GetRequestJson(_url, _json));
    }
    public void getSch(string _url, string _sch) {
        //StartCoroutine(PostRequest("http://192.168.4.3", json));
		StartCoroutine(GetRequestText(_url, _sch));
    }

    IEnumerator PostRequest(string url, string json)
    {
        Debug.Log("json = " + json);
        var www = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.chunkedTransfer = false;

        //Send the request then wait here until it returns
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
            Debug.Log("Error While Sending: " + www.error);
            StartCoroutine(PostRequest(url, json));
        } else {
            string result = www.downloadHandler.text;
            Debug.Log("Received: " + result);
            // queryResult.setQueryResult(result);
        }
    }

    IEnumerator GetRequestJson(string _url, string _data)
    {
        Debug.Log("json = " + _data);
        var www = new UnityWebRequest(_url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(_data);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.chunkedTransfer = false;

        //Send the request then wait here until it returns
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
            Debug.Log("Error While Sending: " + www.error);

            string result = "";
            
            // string path = @"D:/Works/Git/RetroBreadboard/software/SchematicParser/circuit2/circuit2.json";
            string path = @"D:/Works/Git/RetroBreadboard/software/BreadboardSimulator/data/circuit3.json";

            try {
                using (StreamReader reader = new StreamReader(path)) {
                    string line;
                    while((line = reader.ReadLine()) !=null) {
                        result += line+"\n";
                    }
                }
            } catch (Exception e) 
            {
                // Let the user know what went wrong.
                Debug.Log("The file could not be read:");
                Debug.Log(e.Message);
            }
            Debug.Log("JSON read: " + result);
            getResult.setJsonQueryResult(result);
        } else {
            string result = www.downloadHandler.text;
            Debug.Log("Received: " + result);
            getResult.setJsonQueryResult(result);
        }
    }

    IEnumerator GetRequestText(string _url, string _data)
    {
        Debug.Log("sch = " + _data);
        var www = new UnityWebRequest(_url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(_data);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.chunkedTransfer = false;

        //Send the request then wait here until it returns
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
            Debug.Log("Error While Sending: " + www.error);

            string result = "";
            
            // string path = @"D:/Works/Git/RetroBreadboard/software/SchematicParser/circuit2/circuit2.sch";
            string path = @"D:/Works/Git/RetroBreadboard/software/BreadboardSimulator/data/circuit3.sch";
            try {
                using (StreamReader reader = new StreamReader(path)) {
                    string line;
                    while((line = reader.ReadLine()) !=null) {
                        result += line+"\n";
                    }
                }
            } catch (Exception e) 
            {
                // Let the user know what went wrong.
                Debug.Log("The file could not be read:");
                Debug.Log(e.Message);
            }
            Debug.Log("JSON read: " + result);
            getResult.setSchQueryResult(result);
        } else {
            string result = www.downloadHandler.text;
            Debug.Log("Received: " + result);
            getResult.setSchQueryResult(result);
        }
    }
}