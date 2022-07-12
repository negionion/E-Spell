using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour {

	public GameObject backWin;

	public GameObject connectState;
	// Use this for initialization
	void Start () {
		backWin.SetActive(false);
		InvokeRepeating("checkConnectState", 2f, 2f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			backWin.SetActive(true);
		}		
	}

	public void checkConnectState()
	{
		connectState.SetActive(BTsocket.BLEisConnected(Constants.bleMicroBit));
	}

	public void backBtn()
	{
		CancelInvoke("checkConnectState");
		StartCoroutine(delayBack());
	}
	public void cancelBtn()
	{
		backWin.SetActive(false);
	}

	private IEnumerator delayBack()
	{
		if(!BTsocket.existBTsocket(Constants.bleLumexLED))
		{
			SceneManager.LoadScene("home");			
		}
		else
		{
			LumexLED_Controler ctrl = GameObject.Find(Constants.bleLumexLED).GetComponent<LumexLED_Controler>();
			ctrl.ledATcommand(LumexLED_Controler.LED_Command.off);
			//GameObject.Find(Constants.bleLumexLED).GetComponent<BTsocket>().disConnect();
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene("home");	
		}	
	}
}
