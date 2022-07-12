using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Analysis_hint_vocabulary : MonoBehaviour
{
    public GameObject panel;
    public GameObject stageBtn;
    public GameObject title;
    public Text eng, chi;
    public Image pieChart;
    public Text content;
    public static bool analysisFlag = false;
    private bool isDetail = false;
    public AWS_DynamoDB awsDDB;
    private int cnt = 0, hint = 0;
    private float result = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        awsDDB.putItem("unity", 0);
        Debug.Log(awsDDB.getUserID());
        //getItem("unity");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void analysisMode()
    {
        analysisFlag = !analysisFlag;
        stageBtn.SetActive(!analysisFlag);
        title.SetActive(analysisFlag);
    }

    public void analysisGet(string _vocabulary, string _chi)
    {
        panel.SetActive(true);
        isDetail = false;
        eng.text = _vocabulary;
        chi.text = _chi;
        content.text = "載入中...";
        awsDDB.getItem(_vocabulary, (data) => 
        {
            Debug.Log(data);
            result = analysisHV(data);
            if(result < 0)
            {
                content.text = "沒有資料！";
                pieChart.fillAmount = 0f;               
            }
            else
            {                
                pieChart.fillAmount = result;
                content.text = (int)(result * 100) + "%";
            }
        });
    }

    public void closePanel()
    {
        panel.SetActive(false);
        isDetail = false;
    }

/*
data example:
{"Count":2,"Items":[{"hintCnt":{"N":"1"},"time":{"S":"2019-10-10 20:06:08"},"vocabulary":{"S":"unity"},"userID":{"S":"player1"}},{"hintCnt":{"N":"1"},"time":{"S":"2019-10-11 13:28:06"},"vocabulary":{"S":"unity"},"userID":{"S":"player1"}}],"ScannedCount":2}

 */
    private float analysisHV(string _data)
    {
        string tmpData;
        float ans = 0;
        cnt = 0;
        hint = 0;
        tmpData = _data.Substring(_data.IndexOf("\"Count\":") + 8);
        cnt = Int32.Parse(tmpData.Substring(0, tmpData.IndexOf(",")));
        Debug.Log("使用次數:" + cnt);
        if(cnt <= 0)
        {
            return -1;
        }
        for(int i = 0; i < cnt; i++)
        {
            tmpData = tmpData.Substring(tmpData.IndexOf("\"hintCnt\":") + 10);
            tmpData = tmpData.Substring("{\"N\"}:\"".Length - 1);
            hint += Int32.Parse(tmpData.Substring(0, tmpData.IndexOf("\"")));
        }
        Debug.Log("幫助次數:" + hint);

        ans = 1.0f - ((float)hint / cnt);
        ans = Mathf.Clamp(ans, 0.0f, 1.0f);
        return ans;
    }

    public void detailShow()
    {
        if(result < 0)
        {
            content.text = "沒有資料！";  
            return;              
        }
        isDetail = !isDetail;
        if(isDetail)
        {
            content.text = "單字使用：" + cnt + " 次\n\n";
            content.text += "幫助：" + hint + " 次";
        }
        else
        {
            content.text = (int)(result * 100) + "%";
        }
    }

}
