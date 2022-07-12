using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PhonicsAnalysis : MonoBehaviour
{
    private static bool isInit = false;
    private static readonly char[] ruleC = {'b', 'c', 'd', 'f', 'g',
                                       'h', 'j', 'k', 'l', 'm',
                                       'n', 'p', 'q', 'r', 's',
                                       't', 'v', 'w', 'x', 'y', 'z'};
    private static readonly char[] ruleM = {'a', 'e', 'i', 'o', 'u'};

    private static Dictionary<string, List<string>> ruleLongM = new Dictionary<string, List<string>>();//長母音，分析後用雙母音表示ex:aa ee
    private static Dictionary<string, List<string>> ruleP = new Dictionary<string, List<string>>();    //主音在前
    private static Dictionary<string, List<string>> ruleL = new Dictionary<string, List<string>>();    //主音在後


    void Start()
    {
        initialize();
        countAllRule();
        //ruleMindex("teste");
        analysis("Barbecue");
        analysis("Elephant");
        analysis("Church");
        analysis("Even");
        analysis("Control");
        //analysis("tteipo");
        //analysisSubStr("arbe");
        //ruleMindex("aieoute");
    }

    private static int countAllRule()
    {
        int sum = 0;

        sum += ruleC.Length;
        sum += ruleM.Length;
        foreach(string key in ruleL.Keys)
        {
            sum += ruleL[key].Count();
        }
        foreach(string key in ruleP.Keys)
        {
            sum += ruleP[key].Count();
        }
        foreach(string key in ruleLongM.Keys)
        {
            sum += ruleLongM[key].Count();
        }

        Debug.Log("Rule count = " + sum);
        return sum;
    }
    private static void initialize()
    {
        if(isInit)
            return;
        isInit = true;
        ruleP.Add("a", new List<string>());
        ruleP["a"].Add("al");
        ruleP["a"].Add("ay");
        //ruleP["a"].Add("au");
        ruleP["a"].Add("aw");
        ruleP.Add("e", new List<string>());
        //ruleP["e"].Add("ee");
        //ruleP["e"].Add("el"); ??
        //ruleP["e"].Add("eu");
        ruleP["e"].Add("ew");
        ruleP["e"].Add("ey");
        ruleP.Add("o", new List<string>());
        //ruleP["o"].Add("oa");
        //ruleP["o"].Add("oi");
        //ruleP["o"].Add("ou");
        //ruleP["o"].Add("oo");
        ruleP["o"].Add("ow");
        ruleP["o"].Add("oy");
        ruleP.Add("u", new List<string>());
        //ruleP["u"].Add("ue");
        ruleP["u"].Add("ul");
        ruleP.Add("s", new List<string>());
        ruleP["s"].Add("sn");
        ruleP["s"].Add("sp");
        ruleP["s"].Add("sw");
        ruleP["s"].Add("sm");
        ruleP.Add("c", new List<string>());        
        ruleP["c"].Add("ce");
        ruleP["c"].Add("ci");
        ruleP["c"].Add("cy");
        ruleP["c"].Add("ck");
        ruleP.Add("g", new List<string>());
        ruleP["g"].Add("ge");
        ruleP["g"].Add("gi");
        ruleP["g"].Add("gy");
        //Debug.Log(ruleP["a"][1]);

        ruleL.Add("l", new List<string>());
        ruleL["l"].Add("ll");      
        ruleL["l"].Add("bl");
        ruleL["l"].Add("fl");
        ruleL["l"].Add("cl");
        ruleL["l"].Add("gl");
        ruleL["l"].Add("pl");
        ruleL["l"].Add("sl");
        ruleL.Add("r", new List<string>());
        ruleL["r"].Add("ar");
        ruleL["r"].Add("er");
        ruleL["r"].Add("ir");
        ruleL["r"].Add("or");
        ruleL["r"].Add("ur");       
        ruleL["r"].Add("br");
        ruleL["r"].Add("cr");
        ruleL["r"].Add("dr");
        ruleL["r"].Add("gr");
        ruleL["r"].Add("fr");
        ruleL["r"].Add("tr");     
        ruleL["r"].Add("pr");
        ruleL.Add("h", new List<string>());        
        ruleL["h"].Add("th");
        ruleL["h"].Add("sh");
        ruleL["h"].Add("ch");
        ruleL["h"].Add("ph");
        ruleL["h"].Add("gh");

        ruleLongM.Add("a", new List<string>());        
        ruleLongM["a"].Add("e");
        ruleLongM["a"].Add("i");
        ruleLongM["a"].Add("u");
        ruleLongM.Add("e", new List<string>());        
        ruleLongM["e"].Add("e");
        ruleLongM["e"].Add("a");
        ruleLongM.Add("i", new List<string>());        
        ruleLongM["i"].Add("e");
        ruleLongM.Add("o", new List<string>());     
        ruleLongM["o"].Add("e");
        ruleLongM["o"].Add("a");
        ruleLongM["o"].Add("u");
        ruleLongM["o"].Add("o");
        ruleLongM["o"].Add("i");
        ruleLongM.Add("u", new List<string>());        
        ruleLongM["u"].Add("e");
    }

    /*
    *Step.1 分析母音前單字(包含前個母音+字尾母音)
    *Step.2 前→後依序比較清單
    *Step.3 母音若中間字≦1個，考慮長母音(e, i, a)，取消後面母音的音
    *loop直到全字串分析完畢
    *Step.4 連續2個list中都有包含母音，選擇較多的組合(捨棄較少的)，若相同則捨棄前
    */

    public void analysis(string eng)
    {
        string tmp = "", resultStr = "";
        int cut = 0;
        List<string> result = new List<string>();
        List<int> vowelIndex = ruleMindex(eng.ToLower());
        eng = eng.ToLower();
        
        cut = vowelIndex[0];    //只有後母音的開頭子字串
        tmp = eng.Substring(0, vowelIndex[0] + 1);
        //分析子字串
        result.AddRange(analysisSubStr(tmp));
        //Debug.Log(tmp);
        for(int i = 1; i < vowelIndex.Count; i++)
        {
            tmp = eng.Substring(cut, (vowelIndex[i] - cut + 1));
            cut = vowelIndex[i];
            //檢查長母音? 若長母音則cut++，並以雙母音做為字節ex: eve → ee v (避開重複判斷，修改短字串讓其不帶字尾母音)
            if(checkLongM(tmp) > -1 && analysisSubStr(tmp).Count == tmp.Length)
            {
                result.AddRange(analysisLongVowel(tmp, checkLongM(tmp)));
                cut++;
            }
            else
            {
                //分析子字串
                result.AddRange(analysisSubStr(tmp));
            }
            //Debug.Log(tmp);
        }
        if(cut < eng.Length)    //只有前母音的結尾子字串
        {
            tmp = eng.Substring(cut);
            cut = eng.Length;
            //分析子字串
            result.AddRange(analysisSubStr(tmp));
            //Debug.Log(tmp);
        }
        //選擇較多的組合(捨棄較少的)
        //Debug.Log(result[2]);
        result = new List<string>(vowelFilter(result));
        foreach(string word in result)
        {
            resultStr += word + " ";
        }
        Debug.Log(resultStr);
            
    }

    private List<int> ruleMindex(string str)
    {
        List<int> index = new List<int>();
        index.Add(str.IndexOfAny(ruleM));   //first
        while(str.IndexOfAny(ruleM, index.Last() + 1) > 0)  //more
        {
            index.Add(str.IndexOfAny(ruleM, index.Last() + 1));
        }
        return index;
    }

    private List<string> analysisSubStr(string str)
    {
        List<string> wordList = new List<string>();
        string tmp = "";
        char[] strIndex = str.ToCharArray();

        for(int i = 0; i < str.Length; i++)
        {
            bool flagP = false, flagL = false;
            if((i + 1 < str.Length) && ruleP.ContainsKey(strIndex[i].ToString()))   //2個字母，往後比較
            {
                for(int j = 0; j < ruleP[strIndex[i].ToString()].Count; j++)
                {
                    tmp = str.Substring(i, 2);
                    if(tmp.Equals(ruleP[strIndex[i].ToString()][j]))
                    {
                        //Debug.Log("P " + tmp);
                        wordList.Add(tmp);
                        i++;
                        flagP = true;
                        break;
                    }
                }
            }
            if(!flagP && (i + 1 < str.Length) && ruleL.ContainsKey(strIndex[i + 1].ToString())) //2個字母，預先往前比較
            {
                for(int j = 0; j < ruleL[strIndex[i + 1].ToString()].Count; j++)
                {
                    tmp = str.Substring(i, 2);
                    if(tmp.Equals(ruleL[strIndex[i + 1].ToString()][j]))
                    {
                        //Debug.Log("L " + tmp);
                        wordList.Add(tmp);
                        i++;
                        flagL = true;
                        break;
                    }
                }
            }
            if(!flagP && !flagL)
            {
                tmp = strIndex[i].ToString();
                foreach(char word in ruleM.Intersect(tmp.ToCharArray()))
                {
                    //Debug.Log("M " + tmp);
                    wordList.Add(word.ToString());
                }
                foreach(char word in ruleC.Intersect(tmp.ToCharArray()))
                {
                    //Debug.Log("C " + tmp);
                    wordList.Add(word.ToString());
                }
            }
        }

        return wordList;
    }

    private int checkLongM(string str)      //-1:無長母音，1:長母音在第1位，2:長母音在第2位(只有e)
    {
        str = str.ToLower();
        string tmp = "";
        string keyTmp = str.Substring(0, 1);      
        if(str.Length <= 3)
        {
            if(ruleLongM.ContainsKey(keyTmp))
            {
                tmp = str.Substring(1, 1);          //檢查[1]
                if(ruleLongM[keyTmp].Contains(tmp))
                {
                    return 1;
                }
                if(str.Length == 3)
                    tmp = str.Substring(2, 1);      //檢查[2]
                else
                    return -1;
                if(tmp.Equals("e"))
                {
                    return 2;
                }
            }
        }
        return -1;
    }


    private List<string> analysisLongVowel(string str, int checkFlag)
    {
        if(checkFlag == 0)
            return null;
        List<string> wordList = new List<string>();
        str = str.ToLower();
        //string longVowel = "";
        if(checkFlag == 1)      //ee = ee       母音連續出現
        {
            wordList.Add(str.Substring(0, 2));            
        }
        else if(checkFlag == 2) //eve = ee v    母音中間間隔1個音
        {
            wordList.Add(str.Substring(0, 1) + "e");
            wordList.Add(str.Substring(1, 1));
        }
        return wordList;
    }

    private List<string> vowelFilter(List<string> result)
    {
        List<string> tmpList = new List<string>();
        //tmpList.DefaultIfEmpty("");
        foreach(string word in result)
        {
            //Debug.Log("filter " + word);
            if(word.IndexOfAny(ruleM) >= 0 && tmpList.LastOrDefault() != null && tmpList.LastOrDefault().IndexOfAny(ruleM) >= 0)
            {
                if(tmpList.LastOrDefault().Length < word.Length && word.Contains(tmpList.LastOrDefault()))
                {
                    tmpList.RemoveAt(tmpList.Count - 1);
                    tmpList.Add(word);
                    continue;
                }else if(word.Equals(tmpList.LastOrDefault()))
                {
                    continue;
                }         
            }
            tmpList.Add(word);
        }
        return tmpList;
    }
}
