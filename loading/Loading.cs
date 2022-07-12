using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
	[SerializeField]
	private Text loadText;

	//public Text btLog;
	private float timing = 0f;

	//private bool load = true;
	// Use this for initialization
	void Start()
	{
		Time.timeScale = 1;
		//.................................
		SceneManager.LoadSceneAsync("game");
		loadText.text = "讀取中";
		//.................................
	}

	// Update is called once per frame
	void Update()
	{
		//btLog.text = BlueTooth.BTLog;
		timing += Time.deltaTime;
		if (timing > 0.3f)
		{
			timing = 0;
			loadText.text += ".";
			loadText.text = loadText.text == "讀取中...." ? "讀取中" : loadText.text;
		}
		/*if(BlueTooth.BTStatus && load)
		{
			load = false;
			BlueTooth.BTread();
			SceneManager.LoadSceneAsync("game");
		}*/
	}

	public void toHome()
	{
		SceneManager.LoadScene("home");
	}
}