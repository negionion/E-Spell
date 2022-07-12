using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
public class REST_API : MonoBehaviour
{   
    private class RequestParams
    {
        private string name, value;
        public RequestParams(string _n, string _v)
        {
            name = _n;
            value = _v;
        }
        public string getParam()
        {
            return name + "=" + value;
        }
        public override string ToString()
        {
            return getParam();
        }      
    }
    private List<RequestParams> requestParams = new List<RequestParams>();
    private string url;
    public static REST_API getNewREST_API(GameObject parent, string _url)
    {
        REST_API restApi;
        if(parent.GetComponent<REST_API>() != null)
        {
            restApi = parent.GetComponent<REST_API>();
        }
        else
        {
            restApi = parent.AddComponent<REST_API>();
        }        
        restApi.url = _url + "?";
        return restApi;
    }

    public void addParam(string _name, string _value)
    {
        requestParams.Add(new RequestParams(_name, _value));
    }

    public string getRequestURL()
    {
        string tmp = url;
        foreach(RequestParams param in requestParams)
        {
            tmp += param + "&";
        }
        tmp = tmp.Substring(0, tmp.Length - 1);
        return tmp;
    }

    public void httpGET(System.Action<string> responseAction, string _url = "")
    {
        if(_url.Length > 0)
        {
            StartCoroutine(webGet(_url, responseAction)); //自訂url，不使用物件參數
        }
        else
        {
            StartCoroutine(webGet(getRequestURL(), responseAction));  //使用物件參數的url
        }
    }

    public IEnumerator webGet(string _url, System.Action<string> responseAction)
    {
         using (UnityWebRequest webRequest = UnityWebRequest.Get(_url))
        {
            yield return webRequest.SendWebRequest();
            //Debug.Log(webRequest.downloadHandler.text);
            if(responseAction != null && webRequest.isDone)
            {
                responseAction(webRequest.downloadHandler.text);
            }
        }
    }

    public override string ToString()
    {
        return getRequestURL();
    }
}
