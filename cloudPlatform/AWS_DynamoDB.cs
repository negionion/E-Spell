using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AWS_DynamoDB : MonoBehaviour
{
    public const string prefsIDKey = "AWS_DDB_userID";
    private string userID = "player1";
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString(prefsIDKey).Length == 0)
        {
            PlayerPrefs.SetString(prefsIDKey, "player1");
        }
        else
        {
            getUserID();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getUserID()
    {
        userID = PlayerPrefs.GetString(prefsIDKey);
        return userID;
    }

    public void getItem(string _vocabulary, System.Action<string> responseAct = null)
    {
        getUserID();
        REST_API rest = REST_API.getNewREST_API(gameObject, "https://RESTful_API_address.amazonaws.com/v1/item-get/" + userID);
        rest.addParam("t1", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
        rest.addParam("t2", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        rest.addParam("eng", _vocabulary);
        rest.httpGET((data) => {
            if(responseAct != null)
                responseAct(data);
            });
    }

    public void putItem(string _vocabulary, int _hintCnt, System.Action<string> responseAct = null)
    {
        getUserID();
        REST_API rest = REST_API.getNewREST_API(gameObject, "https://RESTful_API_address.amazonaws.com/v1/item-put/" + userID);
        rest.addParam("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        rest.addParam("hintCnt", _hintCnt.ToString());
        rest.addParam("eng", _vocabulary);
        rest.httpGET((data) => {
            if(responseAct != null)
                responseAct(data);
            });
    }
}
