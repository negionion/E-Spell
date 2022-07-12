using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BTManager : MonoBehaviour {
	public GameObject connectedPanel;
	public GameObject linkButton;
	private int linkButtonPos;

	public GameObject preConnectBtn;
	public RectTransform conBtnPanel;
	public Transform conBtnArch;
	private bool LEDConnectFlag = false;
	public Text BTlog;
	public Text startBtnTxt;
	private Coroutine ledConnectCoroutine;
	private LumexLED_BTManager ledBTManager;
	private BTsocket btSoc;
	// Use this for initialization
	void Start () {
		linkButtonPos = 530 - 650;
		btSoc = BTsocket.getNewBTsocket(Constants.bleMicroBit, new BTprofile(Constants.bleMicroBit, true));
		Invoke("delayScan", 2f);

		if(BTsocket.BLEisConnected(Constants.bleLumexLED))
		{
			startBtnTxt.fontSize = 60;
			startBtnTxt.text = "開始";
		}
		//StartCoroutine(LumexLED_BTManager.connectLumexLED(() => { connectedPanel.SetActive(true); Debug.Log("success"); }));
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.A))	//除錯用
		{
			addPeripheralButton("123","addr");
			conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + 200);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("home");
		}
		connectedPanel.SetActive(btSoc.bleActive);
		BTlog.text = btSoc.BTLog;
		
		if(btSoc.bleActive && !BTsocket.BLEisConnected(Constants.bleLumexLED))
		{
			BTlog.text += "Connecting to display...";
			startBtnTxt.fontSize = 30;
			startBtnTxt.text = "開始\n(無外接螢幕)";
			if(!LEDConnectFlag)
			{
				ledBTManager = LumexLED_BTManager.getNewLED_BTManager();	
				ledConnectCoroutine = StartCoroutine(ledBTManager.connectLumexLED(() => { 
					startBtnTxt.fontSize = 60;
					startBtnTxt.text = "開始";
				 },
				 (ledBtLog) => 
				 {
					 BTlog.text = "已可開始遊戲！\nLED屏幕連線中...\n";
					 BTlog.text += ledBtLog;
				 }));
				LEDConnectFlag = true;
			}
		}
	}

	public void addPeripheralButton(string addr, string name)
	{
		GameObject newPeripheral = Instantiate(linkButton);
		newPeripheral.transform.SetParent(conBtnArch);
		newPeripheral.transform.localScale = new Vector3(1, 1, 1);
		newPeripheral.transform.localPosition = new Vector2(0, linkButtonPos);
		newPeripheral.GetComponent<LinkButton>().address = addr;
		newPeripheral.GetComponentInChildren<Text>().text = name + "\n" + addr.ToUpper();

		linkButtonPos -= 200;
	}

	private void delayScan()
	{
		btSoc.scan((addr, name) =>
		{
			addPeripheralButton(addr, name);
			conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + 200);
			if(addr.Equals(PlayerPrefs.GetString("preConnectMAC")))
			{
				preConnectBtn.SetActive(true);
			}
		});
	}

	public void disConnected()
	{
		btSoc.disConnect();
		Invoke("delayLoad", 2f);
	}
	private void delayLoad()
	{
		SceneManager.LoadSceneAsync("connect");
	}


	public void preConnect()
	{
		btSoc.connect(PlayerPrefs.GetString("preConnectMAC"), 60f);
	}

	public void gameStart()
	{
		BluetoothLEHardwareInterface.StopScan();
		if(btSoc.bleActive)
		{
			btSoc.subscribe();
			PlayerPrefs.SetString("preConnectMAC", btSoc.bleLinkData.address);			
		}
		if(ledConnectCoroutine != null)
		{			
			StopCoroutine(ledConnectCoroutine);
		}		
		SceneManager.LoadScene("loading");		
	}

	public void sendBLE(string sendStr)
	{
		btSoc.writeCharacteristic(sendStr);
	}

	public void noConnectMode()
	{
		BluetoothLEHardwareInterface.StopScan();
		try{		
		if(BTsocket.existBTsocket(Constants.bleMicroBit))
			btSoc.disConnect();
		if(BTsocket.existBTsocket(Constants.bleLumexLED))
			GameObject.Find(Constants.bleLumexLED).GetComponent<BTsocket>().disConnect();
		} catch(System.Exception e)
		{
			Debug.Log(e.InnerException);
		}
		SceneManager.LoadScene("loading");
	}
}
