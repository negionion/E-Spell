using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KindVocabulary : MonoBehaviour
{
    [SerializeField]
	private LoadVocabulary loadVocabulary;

    public void kindSelect(string kind)
    {
        loadVocabulary.deleteAllVocabulary();
		loadVocabulary.sqlite.dbConnection();
		loadVocabulary.sqlite.dbOpen();
		if(kind.Equals("自訂"))
		{
			loadVocabulary.reader = loadVocabulary.sqlite.dbQuery("SELECT * FROM custom WHERE other = '自訂';");
			while(loadVocabulary.reader.Read())
			{
				loadVocabulary.creatVocabularyButton(loadVocabulary.reader.GetString(loadVocabulary.reader.GetOrdinal("vocabulary")), loadVocabulary.reader.GetString(loadVocabulary.reader.GetOrdinal("chinese")));
			}
		}
		else
		{
			loadVocabulary.reader = loadVocabulary.sqlite.dbQuery("SELECT * FROM spell WHERE other = '" + kind + "';");
			while(loadVocabulary.reader.Read())
			{
				loadVocabulary.creatVocabularyButton(loadVocabulary.reader.GetString(loadVocabulary.reader.GetOrdinal("vocabulary")), loadVocabulary.reader.GetString(loadVocabulary.reader.GetOrdinal("chinese")));
			}
		}
		loadVocabulary.sqlite.dbClose();
		loadVocabulary.sqlite.dbDisconnection();
    }
}
