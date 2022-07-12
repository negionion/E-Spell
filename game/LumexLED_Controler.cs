using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumexLED_Controler : MonoBehaviour
{
    public static class LED_Command
    {
        public const string clear = "ATd0=()";
        public const string on = "ATf1=()";
        public const string off = "ATf0=()";
    }
    public BTsocket ledBTSoc;
    private static bool ledCmdLock = false;

    public static LumexLED_Controler getNewLED_Controler()
    {
        BTsocket.getNewBTsocket(Constants.bleLumexLED,
                new BTprofile(Constants.bleLumexLED, "0000****-0000-1000-8000-00805f9b34fb", "ffe0", "fff4", "fff1"));
        LumexLED_Controler controler = GameObject.Find(Constants.bleLumexLED).GetComponent<LumexLED_Controler>();
        if(controler == null)
            controler = GameObject.Find(Constants.bleLumexLED).AddComponent<LumexLED_Controler>();
        controler.ledBTSoc = GameObject.Find(Constants.bleLumexLED).GetComponent<BTsocket>();
        return controler;
    }

    public void ledATcommand(string command)
    {
        StartCoroutine(writeCommand(command));
    }
    private IEnumerator writeCommand(string command)
    {
        float TTL = Time.fixedTime;
        while(ledCmdLock)
        {
            if(!BTsocket.BLEisConnected(Constants.bleLumexLED) || (Time.fixedTime - TTL) >= 2f)
                yield break;
            yield return 0;
        }
        ledCmdLock = true;
        bool ledStat = false;
        //yield return StartCoroutine(clearDisplay());
        //ledStat = false;
        if(command.Length <= 20)    //MTU = 20 Bytes
            ledBTSoc.writeCharacteristic(command);
        else
        {
            ledBTSoc.writeCharacteristic(command.Substring(0,20));
        }
        while(!ledStat)
        {
            if(!ledBTSoc.bleActive)
                yield break;
            yield return 0;
            ledBTSoc.getReceiveText((data) =>
                {
                    if(data.Equals("E"))
                    {
                        ledStat = true;
                    }
                });            
        }
        ledCmdLock = false;
    }

    /*public IEnumerator clearDisplay()
    {
        bool ledStat = false;
        ledBTSoc.writeCharacteristic("ATd0=()");   //clear
        while(!ledStat)
        {            
            if(!ledBTSoc.bleActive)
                yield break;
            yield return 0;
            ledBTSoc.getReceiveText((data) =>
                {
                    if(data.Equals("E"))
                    {
                        ledStat = true;
                    }
                });
            
        }
    }*/
}
