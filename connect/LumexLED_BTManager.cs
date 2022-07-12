using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LumexLED_BTManager : MonoBehaviour
{
    private BTsocket lumexSoc;

    public static LumexLED_BTManager getNewLED_BTManager()
    {
        BTsocket.getNewBTsocket(Constants.bleLumexLED,
                new BTprofile(Constants.bleLumexLED, "0000****-0000-1000-8000-00805f9b34fb", "ffe0", "fff4", "fff1"));
        LumexLED_BTManager btManager = GameObject.Find(Constants.bleLumexLED).GetComponent<LumexLED_BTManager>();
        if(btManager == null)
            btManager = GameObject.Find(Constants.bleLumexLED).AddComponent<LumexLED_BTManager>();
        return btManager;
    }
    public IEnumerator connectLumexLED(System.Action successAct, System.Action<string> printLogAction = null)
    {
        if(PlayerPrefs.GetString("LumexLED_BLE_MAC").Length != 17)
        {
            PlayerPrefs.SetString("LumexLED_BLE_MAC", "54:4A:16:23:1E:08");
        }
        lumexSoc = BTsocket.getNewBTsocket(Constants.bleLumexLED,
            new BTprofile(Constants.bleLumexLED, "0000****-0000-1000-8000-00805f9b34fb", "ffe0", "fff4", "fff1"));
        if(!lumexSoc.bleActive)
        {                                
            lumexSoc.scan();
            if(printLogAction != null)
                StartCoroutine(printLog(printLogAction));
            yield return new WaitForSeconds(2f);
            while(!lumexSoc.bleActive)
            {                                
                lumexSoc.connect(PlayerPrefs.GetString("LumexLED_BLE_MAC"), 60f);
                yield return new WaitForSeconds(2f);
            }
            lumexSoc.subscribe();
            successAct();            
        }
    }
    private IEnumerator printLog(System.Action<string> printLogAction)
    {
        string log = "";
        while(true)
        {            
            if(lumexSoc.bleActive)
                log = "Connected!\n";
            else
                log = lumexSoc.BTLog;
            printLogAction(log);
            yield return 0;
        }
        
    }
       

}
