using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public struct Voca
{
    public string eng;
    public string chi;
    public Voca(string _eng, string _chi)
    {
        eng = _eng;
        chi = _chi;
    }
}
public class SpellList : MonoBehaviour
{
    public static bool listMode = false;

    private static bool isCustom = true;    //是否是自訂模式
    public static List<Voca> list = new List<Voca>();
    public GameObject listUI, listPanel, stagePanel, anaBtn;
    public Text uiTitle, listPanleTxtEng, listPanleTxtChi, listBtnTxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void swListMode()
    {
        listMode = !listMode;
        listPanel.SetActive(false);
        stagePanel.SetActive(false);
        listUI.SetActive(listMode);   

            
    }

    public void swStageMode()
    {
        anaBtn.SetActive(!anaBtn.activeSelf); 
        if(listMode)
        {
            listPanel.SetActive(false);
            listMode = false;
            isCustom = true;
            listUI.SetActive(false);
            stagePanel.SetActive(false);
            return;
        }
        stagePanel.SetActive(!stagePanel.activeSelf);
        listPanel.SetActive(false);
        listMode = false;
        listUI.SetActive(false);
    }

    public void swListPanel()
    {
        listPanel.SetActive(!listPanel.activeSelf);
        if(listPanel.activeSelf)
        {
            uiTitle.text = "闖關模式\n單字清單！";
            listBtnTxt.text = "關閉";
        }
        else
        {
            uiTitle.text = "新增闖關單字...\n請選擇單字！";
            listBtnTxt.text = "清單";
        }
        if(!isCustom)
            uiTitle.text = "關卡挑戰中！";
        showListToPanel();
    }

    public void showListToPanel()
    {
        listPanleTxtEng.text = "已選擇單字\n\n";
        listPanleTxtChi.text = list.Count + "\t個\n\n";
        foreach(Voca voca in list)
        {
            listPanleTxtEng.text += voca.eng + "\n";
            listPanleTxtChi.text += voca.chi + "\n";
        }
    }

    public void selStage(GameObject stage)
    {
        try
        {
            stage.GetComponent<StageCreate>().newStage();
            isCustom = false;
            swListMode();
            swListPanel();
        }
        catch(NullReferenceException e)
        {
            clearVocabulary();
            isCustom = true;
            swListMode();
            uiTitle.text = "新增闖關單字...\n請選擇單字！";
        }
    }

    public void popList()
    {
        if(SpellListPlay.isPlay || list.Count == 0 || !isCustom)
            return;
        list.RemoveAt(list.Count - 1);
        showListToPanel();
    }

    public void clearList()
    {
        if(SpellListPlay.isPlay || !isCustom)
            return;
        list.Clear();
        showListToPanel();
    }

    public static int listNum(string eng, string chi)
    {
        if(list.IndexOf(new Voca(eng, chi)) >= 0)
        {
            return list.IndexOf(new Voca(eng, chi)) + 1;
        }
        else
        {
            return -1;
        }
    }

    public static void addVocabulary(string eng, string chi = "")
    {
        if(!isCustom)
            return;
        list.Add(new Voca(eng, chi));
    }

    public static void clearVocabulary()
    {
        list.Clear();
    }


}
