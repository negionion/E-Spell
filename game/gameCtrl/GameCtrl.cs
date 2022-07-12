using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour
{
    private BTsocket mbBLE;
    public GameLED_Ctrl led;

    private string voca;

    public static bool isGame = false;
    private int chActID = 0, playerDmg = 0, enemyDmg = 0, enemyAtk = 0, playerHPMax = 0, enemyHPMax = 0;
    private bool enemyBash = false;
    private float animePlayTime = 0f;

    public int actID{
        get{
            return chActID;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public IEnumerator delayAct(System.Action action, float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        action();
    }


    public Text log;
    public int readChAct()  //讀取microbit回傳的操作，選擇遊戲動作 -1:失敗, 0:選擇動作, 1:確定動作
    {
        if(!BTsocket.BLEisConnected(Constants.bleLumexLED) || !BTsocket.BLEisConnected(Constants.bleMicroBit))
        {
            return -1;        
        }
        if(mbBLE == null)
            mbBLE = GameObject.Find(Constants.bleMicroBit).GetComponent<BTsocket>();
        bool sel = false;
        log.text = "log\n";
        mbBLE.getReceiveText((data) =>
        {
            data = data.Substring(0, data.Length - 1);  //去掉標尾'#'
            log.text += data;
            if(data.Equals("!!!btnA"))
            {
                chActID = led.chAct();
            }else if(data.Equals("!!!btnB"))
            {
                //決定動作
                sel = true;
            }
        });
        if(sel)
            return 1;
        else
            return 0;
    }

    //設定怪物與玩家hp上限
    public void setHPMax(int pHP = 20, int eHP = 20)
    {
        playerHPMax = pHP;
        enemyHPMax = eHP;
    }

    //遊戲開始，初始化
    public void gameStart()
    {
        Random.InitState(System.Guid.NewGuid().GetHashCode() + (int)Time.time);
        isGame = true;
        led.ledAnime(LED_Ctrl.anime.printBG);
        led.playerHP.printHP(1f);
        led.enemyHP.printHP(1f);
    }

    //遊戲結束，重置
    public void gameOver()
    {
        isGame = false;
        led.ledCtrl.clearDisplay();
    }

    public int checkGameOver()
    {
        if(led.enemyHP.nowHP <= 0)
        {
            led.ledVocaHint("Win!!!");
            return 1;
        }
        else if(led.playerHP.nowHP <= 0)
        {
            led.ledVocaHint("Lose...");
            return -1;
        }
        else
            return 0;
    }

    //回合開始，重置LED狀態
    public void turnStart(string _voca)
    {
        voca = _voca;
        led.ledAnime(LED_Ctrl.anime.printBG);               
        enemyAtk = voca.Length - 1;
        playerDmg = 0;
        enemyDmg = 0;
        enemyBash = false;  
        StartCoroutine(delayAct(() => 
        {
            led.playerHP.printHP(led.playerHP.nowHP);
            led.enemyHP.printHP(led.enemyHP.nowHP);
            chActID = led.chAct(true);
            enemyAtkHint();
        }, 0.5f));       
    }
    
    //顯示單字提示
    public void swVocabularyHint(bool sw)
    {
        if(sw)
        {
            led.ledVocaHint(voca);
            log.text += voca;
        }
        else
        {
            led.ledVocaHint("        ");
            log.text += "clear";
        }

    }
    
    //怪物重攻擊提示
    public void enemyAtkHint()
    {
        if(Random.Range(0, 10) < 3)
        {
            enemyBash = true;
            led.ledAnime(LED_Ctrl.anime.enemyAtkHint);
        }
        else
        {
            enemyBash = false;
            led.ledCtrl.eAtkHintClose();
        }
    }



    //LDM動畫
    public void playAnime()
    {
        //led.playerHP.printHP(led.playerHP.nowHP - 0.1f);
        //led.enemyHP.printHP(led.enemyHP.nowHP - 0.1f);
        switch(chActID)
        {
            case 1:
                animePlayTime = 11f;
                led.ledAnime(LED_Ctrl.anime.playerAtk);
                StartCoroutine(delayEnemyAct(LED_Ctrl.anime.enemySuffer, 3f));
                StartCoroutine(delayEnemyAct(LED_Ctrl.anime.enemyAtk, 6f));
                break;
            case 2:
                animePlayTime = 8f;
                led.ledAnime(LED_Ctrl.anime.playerDef);
                StartCoroutine(delayEnemyAct(LED_Ctrl.anime.enemyAtk, 3f));
                break;
            case 3:
                animePlayTime = 9f;
                led.ledAnime(LED_Ctrl.anime.playerRec);
                StartCoroutine(delayEnemyAct(LED_Ctrl.anime.enemyAtk, 5f));
                break;
        }
        
    }

    public IEnumerator delayEnemyAct(LED_Ctrl.anime enemyAct, float dalayTime)
    {
        yield return new WaitForSecondsRealtime(dalayTime);
        led.ledAnime(enemyAct);
    }

    public IEnumerator waitAnimePlay()
    {
        yield return new WaitForSecondsRealtime(animePlayTime);
    }

    //計算該回合傷害，並顯示在LDM上
    public void computeDmg(int _hintCnt)
    {
        int turnAtk = Random.Range(enemyAtk - 2, enemyAtk + 3); //預設怪物攻擊參數±2
        switch(chActID)
        {
            case 1:
                enemyDmg = (voca.Length - _hintCnt) + Random.Range(0, 3);
                if(enemyBash)
                    playerDmg = turnAtk * 2;
                else
                    playerDmg = turnAtk;
                led.playerHP.printHP(((led.playerHP.nowHP * (float)playerHPMax) - playerDmg) / (float)playerHPMax);
                led.enemyHP.printHP(((led.enemyHP.nowHP * (float)enemyHPMax) - enemyDmg) / (float)enemyHPMax);
                break;
            case 2:
                enemyDmg = 0;
                if(enemyBash)
                    playerDmg = turnAtk * 2;
                else
                    playerDmg = turnAtk;
                playerDmg -= (int)((voca.Length - _hintCnt) * 2f);
                playerDmg = playerDmg < 0 ? 0 : playerDmg;
                led.playerHP.printHP(((led.playerHP.nowHP * (float)playerHPMax) - playerDmg) / (float)playerHPMax);
                led.enemyHP.printHP(((led.enemyHP.nowHP * (float)enemyHPMax) - enemyDmg) / (float)enemyHPMax);
                break;
            case 3:
                enemyDmg = 0;
                if(enemyBash)
                    playerDmg = turnAtk * 2;
                else
                    playerDmg = turnAtk;
                playerDmg -= (voca.Length - _hintCnt) + Random.Range(0, 3);                
                led.playerHP.printHP(((led.playerHP.nowHP * (float)playerHPMax) - playerDmg) / (float)playerHPMax);
                led.enemyHP.printHP(((led.enemyHP.nowHP * (float)enemyHPMax) - enemyDmg) / (float)enemyHPMax);
                break;
        }
    }

    public void gameWin()
    {
        led.ledCtrl.printPage(5);
    }

    public void gameLose()
    {
        led.ledCtrl.printPage(6);
    }


}
