using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FileLoad : MonoBehaviour {

	public Text debugTxt;
	//private string path = "D:\\Unity_project\\LAB_spell\\spell\\AssetBundles\\spell_data.assetbundle.data";
	private string path;
	public AssetBundle ab;
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void test()
	{
		debugTxt.text = Application.persistentDataPath + "\n";
		path = Application.persistentDataPath+ "/spell_data.assetbundle.data";
		debugTxt.text += path + "\n";
		try
		{
			ab = AssetBundle.LoadFromFile(path);
			debugTxt.text += ab.name;
			TextAsset txtAB = (TextAsset)ab.LoadAsset("vocabulary.txt");
			debugTxt.text = txtAB.text;
		}catch(System.Exception ee)
		{
			debugTxt.text += ee.ToString();
		}
	}
}
