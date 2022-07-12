using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellPanel : MonoBehaviour {

	[SerializeField]
	private Text eng;
	[SerializeField]
	private Text chi;

	[SerializeField]
	private GameObject successSign;

	[SerializeField]
	private Spell spellManager;
	// Use this for initialization
	void OnEnable () {
		eng.text = Spell.vocabulary;
		chi.text = Spell.vocabularyChi;
		spellManager.Invoke("spellStart", 2f);
		spellManager.Invoke("voice", 4f);
	}
	
	// Update is called once per frame
	void Update () {		
		successSign.SetActive(spellManager.isSuccess());
	}
}
