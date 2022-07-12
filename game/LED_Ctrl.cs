using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LED_Ctrl : MonoBehaviour
{
    //private float timing = 0f, timer = 1f;
    //private int tmpCnt = 0;
    private LumexLED_Controler ledCtrl;

    private string str1, str2;
    private bool hintSyncLock = false;

    void Start()
    {
        if((ledCtrl = LumexLED_Controler.getNewLED_Controler()) == null)
            return;        
        ledCtrl.ledATcommand(LumexLED_Controler.LED_Command.on);       
        ledCtrl.ledATcommand("ATf2=(1)");
        ledCtrl.ledATcommand("ATef=(100)"); 
        ledCtrl.ledATcommand(LumexLED_Controler.LED_Command.clear);

        hintSyncLock = false;
        //--------------------------------------------------------------------
        //printHPbar(0,0,20,10,1);
    }
    void Update()
    {        
        //if(ledCtrl == null)
            //return;

        /*timing += Time.deltaTime;
        if(timing >= timer)
        {
            timing = 0f;
            tmpCnt++;
            ledCtrl.ledATcommand(LumexLED_Controler.LED_Command.clear);
            ledCtrl.ledATcommand("AT83=(0,0,test1" + tmpCnt.ToString() + ")");
            ledCtrl.ledATcommand("AT83=(8,0,test2" + tmpCnt.ToString() + ")");
        }*/
    }

    public void writeLine(string lineStr1, string lineStr2 = "")
    {
        ledCtrl.ledATcommand("ATef=(100)");
        ledCtrl.ledATcommand(LumexLED_Controler.LED_Command.clear);
        ledCtrl.ledATcommand("AT83=(0,0," + lineStr1 + ")");
        if(lineStr2.Length > 0)
        {
            ledCtrl.ledATcommand("AT83=(8,0," + lineStr2 + ")");
        }
    }
    public void clearDisplay()
    {
        ledCtrl.ledATcommand(LumexLED_Controler.LED_Command.clear);
        //-------------------------------------
        //printHPbar(0,0,20,10,100);
    }

    public void setLine(string _s1, string _s2 = "")
    {
        str1 = _s1;
        str2 = _s2;
    }

    public IEnumerator printLine()
    {
        if(hintSyncLock)
            yield break;
        setColor(100);
        hintSyncLock = true;
        writeLine(str1, str2);
        yield return new WaitForSecondsRealtime(5f);
        clearDisplay();
        hintSyncLock = false;
    }

    public void setColor(uint color)    //設置LED預設顏色，0=滅,1=藍,4=綠,32=紅,100=橘黃
    {
        ledCtrl.ledATcommand(String.Format("ATef=({0})", color));
    }

    //在LDM印出7*6的字串 delayClear:延遲一段時間後在原處清除
    public void printStr(uint row, uint col, string str, float delayClearTime = 0f)
    {
        ledCtrl.ledATcommand(String.Format("AT81=({0},{1},{2})", row, col, str));
        if(delayClearTime > 1f)
        {
            StartCoroutine(delayCall(() => 
            {ledCtrl.ledATcommand(String.Format("AT81=({0},{1},{2})", row, col, "        "));},
            delayClearTime));  //印出8字空字串來清除
        }
    }
    private IEnumerator delayCall(System.Action delayAct, float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        delayAct();
    }

    public void printHPbar(uint x0, uint y0, uint with, uint high, uint color, float hp, bool printSide = true)
    {
        //因為座標從0開始，因此(長/寬)都需要多減1
        if(with < 5 || high < 5)
            return;
        string cmdStr = "";
        uint x1 = x0 + with - 1, y1 = y0 + high - 1;
        if(hp > 1)  hp = 1;
        else if(hp < 0) hp = 0;
        if(printSide)
        {
            cmdStr = String.Format("AT91=({0},{1},{2},{3},{4})", x0, y0, x1, y1, color);
            ledCtrl.ledATcommand(cmdStr);
        }
        //改無條件進位，確保血量不會憑空消失，以外框尺寸為主
        if(hp <= 0f)
        {
            cmdStr = String.Format("AT92=({0},{1},{2},{3},0)", x0 + 2, y0 + 2, x0 + with - 3, y0 + high - 3);
            ledCtrl.ledATcommand(cmdStr);
            return;
        }
        x1 = (uint)Math.Ceiling(x0 + ((with - 4) * hp)) + 1;
        y1 = y0 + high - 3;
        cmdStr = String.Format("AT92=({0},{1},{2},{3},0)", x0 + 2, y0 + 2, x0 + with - 3, y0 + high - 3);
        ledCtrl.ledATcommand(cmdStr);
        cmdStr = String.Format("AT92=({0},{1},{2},{3},{4})", x0 + 2, y0 + 2, x1, y1, color);
        ledCtrl.ledATcommand(cmdStr);
    }

    public void printPage(int pageNum)
    {
        ledCtrl.ledATcommand(String.Format("ATfc=({0})", pageNum));
    }

    public void printChActive(int ch = 0, int disCh = 0)
    {
        switch(ch)
        {
            case 1:
                ledCtrl.ledATcommand("AT91=(55,0,63,8,5)");
            break;
            case 2:
                ledCtrl.ledATcommand("AT91=(55,11,63,19,5)");
            break;
            case 3:
                ledCtrl.ledATcommand("AT91=(55,22,63,30,5)");
            break;
        }
        switch(disCh)
        {
            case 1:
                ledCtrl.ledATcommand("AT91=(55,0,63,8,0)");
                break;
            case 2:
                ledCtrl.ledATcommand("AT91=(55,11,63,19,0)");
                break;
            case 3:
                ledCtrl.ledATcommand("AT91=(55,22,63,30,0)");
                break;
        }
    }

//--------------------------Anime-----------------------------
    public enum anime
    {
        printBG = 1,
        playerAtk, playerDef, playerRec, enemyAtk, enemyAtkHint, enemySuffer
    }

    public Coroutine printAnime (anime ani)
    {
        Coroutine coroutine = null;
        switch(ani)
        {
            case anime.printBG:
                ledCtrl.ledATcommand("ATfc=(4)");
                break;
            case anime.playerAtk:
                coroutine = StartCoroutine(pAtk());
                break;
            case anime.playerDef:
                coroutine = StartCoroutine(pDef());
                break;
            case anime.playerRec:
                coroutine = StartCoroutine(pRec());
                break;
            case anime.enemyAtk:
                coroutine = StartCoroutine(eAtk());
                break;
            case anime.enemyAtkHint:
                coroutine = StartCoroutine(eAtkHint());
                break;
            case anime.enemySuffer:
                coroutine = StartCoroutine(eSuffer());
                break;
        }
        return coroutine;
    }

    public void clearAnime()
    {
        ledCtrl.ledATcommand("AT92=(22,5,32,22,0)");
        ledCtrl.ledATcommand("AT99=(22,14,9,0)");
    }

    private IEnumerator pAtk()
    {
        ledCtrl.ledATcommand("ATef=(4)");
        ledCtrl.ledATcommand("AT94=(31,11,1,4)");
        yield return new WaitForSecondsRealtime(1f);
        ledCtrl.ledATcommand("AT94=(26,11,2,4)");
        yield return new WaitForSecondsRealtime(1f);
        ledCtrl.ledATcommand("AT81=(1,4,*)");
        yield return new WaitForSecondsRealtime(3f);
        clearAnime();
    }
    private IEnumerator pDef()
    {
        ledCtrl.ledATcommand("ATef=(4)");
        ledCtrl.ledATcommand("AT92=(28,10,30,20,4)");
        yield return new WaitForSecondsRealtime(8f);
        clearAnime();
    }

    private IEnumerator pRec()
    {
        ledCtrl.ledATcommand("ATef=(4)");
        ledCtrl.ledATcommand("AT97=(29,7,3,4)");
        yield return new WaitForSecondsRealtime(2f);
        ledCtrl.ledATcommand("AT97=(29,9,1,0)");
        yield return new WaitForSecondsRealtime(1f);
        ledCtrl.ledATcommand("AT97=(29,8,2,0)");
        yield return new WaitForSecondsRealtime(1f);
        ledCtrl.ledATcommand("AT97=(29,7,3,0)");
        clearAnime();
    }

    private IEnumerator eAtk()
    {
        ledCtrl.ledATcommand("ATef=(1)");
        ledCtrl.ledATcommand("AT90=(22,10,25,7,1)");
        yield return new WaitForSecondsRealtime(0.8f);
        ledCtrl.ledATcommand("AT90=(24,14,27,14,1)");
        yield return new WaitForSecondsRealtime(0.8f);
        ledCtrl.ledATcommand("AT90=(23,12,26,10,1)");
        yield return new WaitForSecondsRealtime(0.8f);
        ledCtrl.ledATcommand("AT94=(30,9,2,1)");
        yield return new WaitForSecondsRealtime(2f);
        clearAnime();
    }

    private IEnumerator eAtkHint()
    {
        ledCtrl.ledATcommand("ATef=(1)");
        ledCtrl.ledATcommand("AT92=(2,7,3,9,32)");
        ledCtrl.ledATcommand("AT90=(2,11,3,11,32)");
        yield return new WaitForSecondsRealtime(2f);
        
    }

    public void eAtkHintClose()
    {
        ledCtrl.ledATcommand("AT92=(2,7,3,11,0)");
    }

    private IEnumerator eSuffer()
    {
        ledCtrl.ledATcommand("ATef=(1)");
        ledCtrl.ledATcommand("AT95=(16,16,5,0)");
        yield return new WaitForSecondsRealtime(3f);
        ledCtrl.ledATcommand("AT95=(16,16,5,1)");
        ledCtrl.ledATcommand("AT90=(15,15,17,15,0)");
        ledCtrl.ledATcommand("AT90=(8,20,16,20,0)");
    }
    
}
