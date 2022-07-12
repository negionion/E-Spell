using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour {

	[SerializeField]
	private GameObject[] menuPanel = new GameObject[3];
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void openPanel(int id)
	{
		if(menuPanel[id].activeSelf)
		{
			menuPanel[id].SetActive(false);
			return;
		}
		for(int i = 0; i < menuPanel.Length; i++)
		{
			menuPanel[i].SetActive(id == i);
		}
	}
}
