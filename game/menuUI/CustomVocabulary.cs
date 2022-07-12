using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
public class CustomVocabulary : MonoBehaviour {

	public GameObject panel;
	[SerializeField]
	private InputField engIn;

	[SerializeField]
	private InputField chiIn;

	

	[SerializeField]
	private LoadVocabulary loadVocabulary;

	public void customAdd(string tableName)
	{		
		if(engIn.text.Length > 8 || engIn.text.Length == 0)
		{
			engIn.textComponent.color = Color.red;
			return;
		}
		if(chiIn.text.Length > 8 || chiIn.text.Length == 0)
		{
			chiIn.textComponent.color = Color.red;
			return;
		}
		loadVocabulary.sqlite.dbConnection();
		loadVocabulary.sqlite.dbOpen();
		loadVocabulary.sqlite.dbQuery("INSERT INTO " + tableName + " VALUES (NULL, '" + engIn.text + "', '" + chiIn.text + "', '自訂');");
		loadVocabulary.sqlite.dbClose();
		loadVocabulary.sqlite.dbDisconnection();

		loadVocabulary.creatVocabularyButton(engIn.text, chiIn.text);
		engIn.text = "";
		engIn.textComponent.color = Color.black;
		chiIn.text = "";
		chiIn.textComponent.color = Color.black;	
	}
}
