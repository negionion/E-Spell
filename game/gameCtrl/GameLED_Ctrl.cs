using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLED_Ctrl : MonoBehaviour
{
    public LED_Ctrl ledCtrl;

    public HPbar playerHP, enemyHP;    //color建議<10，否則易超過MTU 20，或x0 + y0 + color的總位數必須 < 5
    // Start is called before the first frame update
    void Start()
    {   
        playerHP = new HPbar(ledCtrl, 29, 0, 24, 5, 4);
        enemyHP = new HPbar(ledCtrl, 0, 0, 24, 5, 32);
        /*
        hpBar = new HPbar(ledCtrl, 40, 0, 24, 7, 64);
        hpBar1 = new HPbar(ledCtrl, 0, 25, 24, 7, 40);
        StartCoroutine(hpBarTest());
        */
        /*ledCtrl.printAnime(LED_Ctrl.anime.printBG);
        ledCtrl.printAnime(LED_Ctrl.anime.playerAtk);*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public IEnumerator hpBarTest()
    {
        yield return new WaitForSeconds(5f);
        float hp = 1f;
        hpBar.printHP(hp, true);
        hpBar1.printHP(hp, true);
        do
        {
            hpBar.printHP(hp);
            hpBar1.printHP(hp);
            yield return new WaitForSeconds(2f);
            hp -= 0.1f;
        }while(hp >= 0f);
        hpBar.printHP(hp);
        hpBar1.printHP(hp);
    }*/



    public void ledAnime(LED_Ctrl.anime id)
    {
        ledCtrl.printAnime(id);
    }

    int ch = 1;
    public int chAct(bool defaultCh = false)  //切換led所選擇的動作，回傳目前動作id
    {
        if(defaultCh)   //default = 1
        {
            ledCtrl.printChActive(1,2);
            ledCtrl.printChActive(1,3);
            ch = 2;
            return  1;
        }
        if(ch > 3)
            ch = 1;
        if(ch == 1)
            ledCtrl.printChActive(1,3);
        else if(ch == 2)
            ledCtrl.printChActive(2,1);
        else if(ch == 3)
            ledCtrl.printChActive(3,2);
        ch++;

        return (ch - 1);
    }

    //在LED印出單字(遊戲模式)
    public void ledVocaHint(string str)
    {
        ledCtrl.setColor(100);
        ledCtrl.printStr(3, 1, str);
    }
}

