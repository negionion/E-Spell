using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class AWStest : MonoBehaviour
{
    public Text debugLog;
    // Start is called before the first frame update
    void Start()
    {
        REST_API rest = REST_API.getNewREST_API(gameObject, "https://RESTful_API_address.amazonaws.com/v1/item-get/name");
        rest.addParam("t1", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
        rest.addParam("t2", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        rest.addParam("eng", "unityTest");
        rest.httpGET((data) => {debugLog.text += data;});
    }

    public IEnumerator awsGet(string _vocabulary)
    {
         using (UnityWebRequest webRequest = UnityWebRequest.Get("https://RESTful_API_address.amazonaws.com/v1/item-get/name?eng=" + _vocabulary + "&t1=" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " 00:00:00&t2=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            debugLog.text += webRequest.downloadHandler.text;
            Debug.Log(webRequest.downloadHandler.text);
        }
    }
    public IEnumerator awsPut()
    {
        DateTime.Now.ToShortTimeString();
         using (UnityWebRequest webRequest = UnityWebRequest.Get("https://RESTful_API_address.amazonaws.com/v1/item-put/name?eng=unity&hintCnt=3&time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            debugLog.text += webRequest.downloadHandler.text;
            Debug.Log(webRequest.downloadHandler.text);
        }
    }

    public void jsonToObj(string _json)
    {
        HintVocabulary obj = JsonUtility.FromJson<HintVocabulary>(_json);
        Debug.Log(obj.userID);
    }
}

class HintVocabulary
{
    public string userID, time, vocabulary;
    public int hintCnt;
}
