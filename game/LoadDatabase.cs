using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using UnityEngine.UI;
public class LoadDatabase : MonoBehaviour {

	private SqliteDataReader reader;
	private SQLiteManager sqlite;
	public Text debugTxt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void sqliteTest()
	{
		debugTxt.text = "";
	#if UNITY_EDITOR
		sqlite = new SQLiteManager("D:/SQLite/database/","mhcilab.db");
	#elif UNITY_ANDROID
		sqlite = new SQLiteManager(Application.persistentDataPath + "/","mhcilab.db");
	#endif
		debugTxt.text = Application.persistentDataPath + "/" + "mhcilab.db" + "\n";
		sqlite.dbConnection();
		sqlite.dbOpen();
		reader = sqlite.dbQuery("select * from spell;");
		while(reader.Read())
		{
			debugTxt.text += reader.GetString(reader.GetOrdinal("vocabulary"));
		}
		reader.Dispose();
	}
}
