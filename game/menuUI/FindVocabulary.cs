using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class FindVocabulary : MonoBehaviour {
	public GameObject panel;
	[SerializeField]
	private InputField engIn;

	[SerializeField]
	private LoadVocabulary loadVocabulary;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void findVocabulary(InputField keyword)
	{		
		loadVocabulary.deleteAllVocabulary();
		if(engIn.text.Length > 8)
		{
			engIn.textComponent.color = Color.red;
			return;
		}
		if(keyword.text.Length == 0)
		{
			loadVocabulary.loadAllVocabulary();
			return;
		}
		loadVocabulary.sqlite.dbConnection();
		loadVocabulary.sqlite.dbOpen();
		loadVocabulary.reader = loadVocabulary.sqlite.dbQuery("SELECT * FROM spell WHERE vocabulary LIKE '" + keyword.text + "%';");
		while(loadVocabulary.reader.Read())
		{
			loadVocabulary.creatVocabularyButton(loadVocabulary.reader.GetString(loadVocabulary.reader.GetOrdinal("vocabulary")), loadVocabulary.reader.GetString(loadVocabulary.reader.GetOrdinal("chinese")));
		}
		loadVocabulary.reader = loadVocabulary.sqlite.dbQuery("SELECT * FROM custom WHERE vocabulary LIKE '" + keyword.text + "%';");
		while(loadVocabulary.reader.Read())
		{
			loadVocabulary.creatVocabularyButton(loadVocabulary.reader.GetString(loadVocabulary.reader.GetOrdinal("vocabulary")), loadVocabulary.reader.GetString(loadVocabulary.reader.GetOrdinal("chinese")));
		}
		loadVocabulary.sqlite.dbClose();
		loadVocabulary.sqlite.dbDisconnection();
		engIn.textComponent.color = Color.black;
	}

}
