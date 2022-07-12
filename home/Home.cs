using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Home : MonoBehaviour {

	public InputField awsDDBid;

	void Awake()
	{
		
	}

	void Start()
	{
		if(BTsocket.BLEisConnected(Constants.bleMicroBit))
			GameObject.Find(Constants.bleMicroBit).GetComponent<BTsocket>().disConnect();
		//StartCoroutine(ledDisconnect());

		awsDDBid.text = PlayerPrefs.GetString(AWS_DynamoDB.prefsIDKey);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
	}

	private IEnumerator ledDisconnect(float disConnectTimer = 10f)
	{
		if(!BTsocket.existBTsocket(Constants.bleLumexLED))
			yield break;
		yield return new WaitForSeconds(disConnectTimer);
		GameObject.Find(Constants.bleLumexLED).GetComponent<BTsocket>().disConnect();
	}

	public void gameStart()
	{
		SceneManager.LoadScene("connect");
	}
	public void arduinoHEXLink(string URL)
	{
		Application.OpenURL(URL);
	}

	public void awsSetID(Text _id)
	{
		PlayerPrefs.SetString(AWS_DynamoDB.prefsIDKey, _id.text);
	}

}
