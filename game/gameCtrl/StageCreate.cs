using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//掛載於建立新關卡的物件中
public class StageCreate : MonoBehaviour
{
    public GameCtrl gameCtrl;
    public string[] eng = new string[10];
    public string[] chi = new string[10];
    public int playerHP = 20, enemyHP = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newStage()
    {
        SpellList.clearVocabulary();
        for(int i = 0; i < 10; i++)
        {
            SpellList.addVocabulary(eng[i], chi[i]);
        }
        gameCtrl.setHPMax(playerHP, enemyHP);
    }
}
