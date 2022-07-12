using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellListPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isPlay = false;  //表示闖關模式是否開啟
    public Text uiTitle, playTxt, countdownTxt;
    public GameObject delayMask, listPanel;
    [SerializeField]
    private Spell spell;

    private Coroutine startCorou, playModeCorou;

    public GameCtrl game;
    void Start()
    {
        isPlay = false;

        Debug.Log(1 % 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void swListPlay()
    {
        if(!BTsocket.BLEisConnected(Constants.bleMicroBit))
            return;
        if(SpellList.list.Count <= 0)
            return;
        listPanel.SetActive(false);
        isPlay = !isPlay;        
        if(isPlay)
        {
            SpellList.listMode = false;
            uiTitle.text = "闖關模式！\n";
            playTxt.text = "ll";
            startCorou = StartCoroutine(startPlay());
            delayMask.SetActive(true);
        }
        else
        {
            SpellList.listMode = true;
            uiTitle.text = "新增闖關單字...\n請選擇單字！";
            playTxt.text = "▶";
            StopCoroutine(startCorou);
            spell.spellPlayClose();
            delayMask.SetActive(false);
            if(playModeCorou != null)
            {
                StopCoroutine(playModeCorou);
                game.gameOver();
            }
        }
    }

    private IEnumerator startPlay()
    {        
        for(int i = 3; i > 0; i--)
        {
            countdownTxt.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        delayMask.SetActive(false);
        if(!BTsocket.BLEisConnected(Constants.bleLumexLED))
        {
            GameCtrl.isGame = false;
            playModeCorou = StartCoroutine(playMode());     //無燈板,連續拼字模式            
        }
        else
        {
            playModeCorou = StartCoroutine(gameMode());     //有燈板,遊戲模式
        }
    }
    private IEnumerator playMode()
    {
        int cnt = 0;
        while(cnt < SpellList.list.Count)
        {
            //Debug.Log(SpellList.list[cnt].eng);
            
            spell.spellInitial(SpellList.list[cnt].eng, SpellList.list[cnt].chi);
            while(!spell.isSuccess())
            {
                yield return 0;
            }
            yield return new WaitForSeconds(5f);
            spell.spellPlayClose();            
            //等待下個單字準備完成
            listPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            listPanel.SetActive(false);
            cnt++;
        }
        yield return new WaitForSeconds(1f);
        swListPlay();
    }

    private IEnumerator gameMode()
    {
        int cnt = 0, rnd = 0;
        int[] rndAry = new int[SpellList.list.Count];
        for(int i = 0; i < rndAry.Length; i++)
        {
            rndAry[i] = Random.Range(0, rndAry.Length);
            for(int j = 0; j < i; j++)
            {
                if(rndAry[i] == rndAry[j])
                {
                    i--;
                    break;
                }
            }
        }

        game.gameStart();
        yield return new WaitForSecondsRealtime(2f);
        while(cnt < (SpellList.list.Count * 3))   //限定回合數為單字集 * 3
        {            
            rnd = rndAry[cnt % rndAry.Length];
            game.turnStart(SpellList.list[rnd].eng);
            yield return new WaitForSecondsRealtime(2f);
            game.swVocabularyHint(true);
            listPanel.SetActive(true);
            //等待選取動作，並顯示目標單字
            yield return StartCoroutine(chActMode());
            //yield return new WaitForSecondsRealtime(10f);
            game.swVocabularyHint(false);
            //Debug.Log(SpellList.list[cnt].eng);
            spell.spellInitial(SpellList.list[rnd].eng, SpellList.list[rnd].chi);
            listPanel.SetActive(false);
            while(!spell.isSuccess())
            {
                yield return 0;
            }
            //播放LDM動畫
            game.playAnime();
            yield return StartCoroutine(game.waitAnimePlay());
            yield return new WaitForSecondsRealtime(1f);
            //傷害計算
            game.computeDmg(spell.hintCnt);

            //檢查是否已結束遊戲
            if(game.checkGameOver() != 0)
            {
                yield return new WaitForSecondsRealtime(3f);
                if(game.checkGameOver() == 1)
                    game.gameWin();
                else if(game.checkGameOver() == -1)
                    game.gameLose();
                yield return new WaitForSecondsRealtime(10f);
                swListPlay();
            }


            yield return new WaitForSeconds(3f);
            spell.spellPlayClose();            
            //等待下個單字準備完成
            listPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            listPanel.SetActive(false);
            cnt++;
        }
        yield return new WaitForSeconds(1f);
        swListPlay();
    }

    private IEnumerator chActMode()
    {
        int flag = 0;
        while(flag == 0)
        {
            flag = game.readChAct();
            yield return 0;
        }
       //Step.1 將list切3組，計算攻擊組的總字數 * 0.8 = 敵方hp，防禦+回血+20 = 敵方總攻擊量(加入亂數)
       //Step.2 依據選擇的動作隨機選擇該組單字
    }
}
