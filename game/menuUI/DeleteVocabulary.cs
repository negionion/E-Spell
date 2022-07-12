using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
public class DeleteVocabulary : MonoBehaviour
{
    [SerializeField]
	private LoadVocabulary loadVocabulary;

    [SerializeField]
    private Spell spell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        loadVocabulary.sqlite.dbConnection();
		loadVocabulary.sqlite.dbOpen();

        loadVocabulary.reader = loadVocabulary.sqlite.dbQuery("SELECT vocabulary FROM custom WHERE vocabulary == '" + Spell.vocabulary + "';");
        gameObject.transform.localScale = loadVocabulary.reader.Read() ? Vector3.one : Vector3.zero;
        
        loadVocabulary.sqlite.dbClose();
		loadVocabulary.sqlite.dbDisconnection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void deleteVocabulary()
    {
        if(SpellListPlay.isPlay)    //play模式下禁止刪除
            return;
        loadVocabulary.sqlite.dbConnection();
		loadVocabulary.sqlite.dbOpen();

        loadVocabulary.sqlite.dbQuery("DELETE FROM custom WHERE vocabulary = '" + Spell.vocabulary + "' AND chinese = '" + Spell.vocabularyChi + "';");

        loadVocabulary.sqlite.dbClose();
		loadVocabulary.sqlite.dbDisconnection();

        spell.spellClose();
        loadVocabulary.deleteAllVocabulary();
        loadVocabulary.loadAllVocabulary();       
    }
}
