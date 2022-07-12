using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class LoadVocabulary : MonoBehaviour {

	[SerializeField]
	private GameObject vocabularyButton;
	public SqliteDataReader reader;
	public SQLiteManager sqlite;
	// Use this for initialization

	private Vector2 btnPos;
	private int count = 0;

	[SerializeField]
	private string fileName, bundleName;

	public RectTransform panel;

	void Start () {
		iniPanel();
		loadSpellDatabase();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void iniPanel()
	{
		btnPos.Set(-170, -100);
		count = 0;
	}

	public void creatVocabularyButton(string _eng, string _chi)
	{
		GameObject vocabularyBtn = Instantiate(vocabularyButton);
		vocabularyBtn.transform.SetParent(transform);
		vocabularyBtn.transform.localScale = new Vector3(1, 1, 1);
		vocabularyBtn.transform.localPosition = btnPos;
		vocabularyBtn.GetComponent<Vocabulary>().initial(_eng, _chi);

		count++;
		btnPos.x = -btnPos.x;
		btnPos.y = -((count / 2) * 150) - 100;
		panel.sizeDelta = new Vector2(0, (((count + 1) / 2) + 1) * 150 + 150);
	}

	public void deleteAllVocabulary()
	{
		iniPanel();
		for(int i = 0; i < this.transform.childCount; i++)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}
	}

	/*private void loadVocabularyTable()
	{
		AssetBundle bundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + bundleName);
		//AssetBundle bundle = AssetBundle.LoadFromFile("D:\\Unity_project\\LAB_spell\\spell\\AssetBundles\\spell_data.assetbundle.data");
		TextAsset txtData = (TextAsset)bundle.LoadAsset("vocabulary.txt");
		string[] vocabulary = txtData.text.Split('\n');
		for(int i = 0; i < vocabulary.Length; i++)
		{
			creatVocabularyButton(vocabulary[i]);
		}
		bundle.Unload(false);
	}*/

	private void loadSpellDatabase()
	{
	#if UNITY_EDITOR
		sqlite = new SQLiteManager(Application.persistentDataPath + "/","mhcilab.db");
	#elif UNITY_ANDROID
		sqlite = new SQLiteManager(Application.persistentDataPath + "/","mhcilab.db");
	#endif

	loadAllVocabulary();		
	}

	public void loadAllVocabulary()
	{
		sqlite.dbConnection();
		sqlite.dbOpen();
		if(!sqlite.isTableExists("custom"))	//creat custom table
		{
			sqlite.dbQuery("CREATE TABLE custom(num INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, vocabulary VARCHAR(10) NOT NULL, chinese VARCHAR(20) NOT NULL, other VARCHAR(20));");
		}
		reader = sqlite.dbQuery("SELECT * FROM spell;");
		while(reader.Read())
		{
			creatVocabularyButton(reader.GetString(reader.GetOrdinal("vocabulary")), reader.GetString(reader.GetOrdinal("chinese")));
		}
		reader = sqlite.dbQuery("SELECT * FROM custom;");
		while(reader.Read())
		{
			creatVocabularyButton(reader.GetString(reader.GetOrdinal("vocabulary")), reader.GetString(reader.GetOrdinal("chinese")));
		}
		sqlite.dbClose();
		sqlite.dbDisconnection();
	}
}
