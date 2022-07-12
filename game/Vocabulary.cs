using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vocabulary : MonoBehaviour {

	private Button btn;
	private Text engTxt;
	private string eng, chi;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnClick()
	{
		if(SpellList.listMode)
		{
			SpellList.addVocabulary(eng, chi);
			/*Text num = gameObject.AddComponent<Text>();
			num.text = SpellList.listNum(eng, chi).ToString();
			num.rectTransform.position = new Vector2(150, 50);
			num.fontSize = 50;*/
		}

		else if(Analysis_hint_vocabulary.analysisFlag)
		{
			GameObject.Find("awsAnalysis").GetComponent<Analysis_hint_vocabulary>().analysisGet(eng, chi);
		}
		else
		{
			GameObject.Find("spell").GetComponent<Spell>().spellInitial(eng, chi);
		}
	}

	public void initial(string _eng, string _chi)
	{
		btn = GetComponent<Button>();
		engTxt = GetComponentInChildren<Text>();
		eng = _eng;
		chi = _chi;
		btn.onClick.AddListener(OnClick);
		engTxt.text = eng;
	}
	
}
