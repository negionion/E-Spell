using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour {
	private static string voca;
	public static string vocabulary
	{
		get{
			return voca;
		}
		private set{
			voca = value;
		}
	}

	private static string chi;
	public static string vocabularyChi
	{
		get{
			return chi;
		}
		private set{
			chi = value;
		}
	}

	public static bool isSpell = false;
	private bool success = false;
	private BTsocket btSoc = null;
	public LED_Ctrl ledCtrl;
	public GameCtrl gameCtrl;
	private bool statFlag = false;

	[SerializeField]
	private GameObject panel;
	[SerializeField]
	private AudioSource vocabularyVoice;

	private string btBuf = "";
	public string getBufData
	{
		get
		{
			return btBuf;
		}
	}

	public TTS_AndroidLocal tts;
	public Text spellStatTxt;

	public int hintCnt = 0;

	private int opl1000_tial = 0;

	public AWS_DynamoDB awsDDB;
	private bool putFlag = false;
	void Start()
	{
		isSpell = false;
		if(!BTsocket.existBTsocket(Constants.bleMicroBit))
			return;
		//if(GameObject.Find(Constants.bleLumexLED) != null && GameObject.Find(Constants.bleLumexLED).GetComponent<LumexLED_Controler>() != null)
		btSoc = GameObject.Find(Constants.bleMicroBit).GetComponent<BTsocket>();
		sendVocabulary("");		
	}

	void Update()
	{
		if(btSoc == null)
			return;
		if(!isSpell)
			return;
		btSoc.getReceiveText((data) =>
		{
			data = data.Substring(0, data.Length - 1);	//delete package tail '#'
			btBuf = data.Replace("_", "");				//get spell state		
			
			if(GameCtrl.isGame)
			{
				if(data.Contains("!!!true"))
				{
					spellStatTxt.text = vocabulary;
					btBuf = vocabulary;
					statFlag = true;
					if(!putFlag)
					{
						putFlag = true;
						awsDDB.putItem(vocabulary, hintCnt);
					}
				}
				else if(data.Contains("!!!btnA"))
				{
					hint();
				}
				else if(data.Contains("!!!btnB"))
				{
					voice();
				}
				else
				{
					spellStatTxt.text = btBuf;	
				}
				return;
			}
			
			if(data.Contains("!!!true"))
			{
				spellStatTxt.text = vocabulary;
				btBuf = vocabulary;
				statFlag = true;
				if(!putFlag)
				{
					putFlag = true;
					awsDDB.putItem(vocabulary, hintCnt);
				}
				ledCtrl.setLine(vocabulary, vocabulary);
				ledCtrl.writeLine("Complete");				
				return;
			}
			else if(data.Contains("!!!btnA"))
			{
				hint();
				return;
			}
			else if(data.Contains("!!!btnB"))
			{
				voice();
				return;
			}			
			if(!isSuccess())
			{		
				spellStatTxt.text = btBuf;		
				ledCtrl.setLine(btBuf, vocabulary);				
				if(statFlag)	//成功→失敗時，需要清除Complete畫面
				{
					statFlag = false;
					ledCtrl.clearDisplay();
				}
			}
			else
			{
				spellStatTxt.text = vocabulary;
				ledCtrl.setLine(vocabulary, vocabulary);
			}
		});
	}

	public bool isSuccess()
	{
		success = btBuf.Equals(vocabulary);
		return success;
	}
	private void sendVocabulary(string vocabulary)
	{
		if(btSoc != null && btSoc.bleActive)
		{
			btSoc.writeCharacteristic(vocabulary + "#" + opl1000_tial);
			opl1000_tial = opl1000_tial == 9 ? 0 : (opl1000_tial + 1);
		}
	}

	

	public void spellStart()
	{
		isSpell = true;
		CancelInvoke("spellStart");
		success = false;
		hintCnt = 0;
		putFlag = false;
		sendVocabulary(vocabulary);
		btBuf = "";
	}
	public void spellInitial(string _eng, string _chi)
	{
		isSpell = true;
		success = false;
		vocabulary = _eng;
		vocabularyChi = _chi;
		btBuf = "";
		hintCnt = 0;
		putFlag = false;
		sendVocabulary("");
		panel.SetActive(true);
		ledCtrl.setLine("********".Substring(0, vocabulary.Length), vocabulary);
		spellStatTxt.gameObject.SetActive(true);
		spellStatTxt.text = "********".Substring(0, vocabulary.Length);	
	}

	public void spellClose()
	{
		isSpell = false;
		if(SpellListPlay.isPlay)
		{
			return;
		}
		CancelInvoke("spellStart");
		success = false;
		vocabulary = "";
		sendVocabulary("");
		panel.SetActive(false);
		ledCtrl.clearDisplay();	

		btBuf = "";
	}

	public void spellPlayClose()
	{
		isSpell = false;
		CancelInvoke("spellStart");
		success = false;
		vocabulary = "";
		sendVocabulary("");
		panel.SetActive(false);
		ledCtrl.clearDisplay();	

		btBuf = "";
	}

	public void hint()
	{
		if(btSoc != null && btSoc.bleActive)
			sendVocabulary("???#");
		else
			return;
		hintCnt++;
		if(GameCtrl.isGame)
		{
			ledCtrl.setColor(100);
			ledCtrl.printStr(3, 1, vocabulary, 4f);
		}
		else
			StartCoroutine(ledCtrl.printLine());
	}
	public void voice()
	{
		CancelInvoke("voice");
		//Debug.Log(vocabulary);
		if(VoiceLoad.load(vocabulary) != null)
		{
			vocabularyVoice.clip = VoiceLoad.load(vocabulary);
			vocabularyVoice.Play();			
		}
		else
		{
			tts.ttsPlayAudio(vocabulary);
		}
	}
}
