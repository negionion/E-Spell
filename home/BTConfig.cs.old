using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTConfig : MonoBehaviour {
	public GameObject BTConfigPanel;
	public InputField fullIn;
	public InputField serviceIn;
	public InputField readChIn;
	public InputField writeChIn;
	private BTprofile config;
	// Use this for initialization
	void Start () {
		BTsocket.profile.fileReLoad();
		config = BTsocket.profile;
		fullIn.text = BTsocket.profile.full;
		serviceIn.text = BTsocket.profile.serviceID;
		readChIn.text = BTsocket.profile.readChID;
		writeChIn.text = BTsocket.profile.writeChID;

	}

	public void btConfig()
	{
		if (BTConfigPanel.activeSelf)
		{
			if (dataCheck())
			{
				BTsocket.profile.Save(config);
			}
			else
			{
				return;
			}
		}

		BTConfigPanel.SetActive(!BTConfigPanel.activeSelf);
	}

	private bool dataCheck()
	{
		bool flag = true;
		if (config.full.Length != BTsocket.profile.full.Length || !config.full.Contains("****"))
		{
			fullIn.textComponent.color = Color.red;
			flag = false;
		}
		else
		{
			fullIn.textComponent.color = Color.black;
		}
		if(config.serviceID.Length != BTsocket.profile.serviceID.Length)
		{
			serviceIn.textComponent.color = Color.red;
			flag = false;
		}
		else
		{
			serviceIn.textComponent.color = Color.black;
		}
		if (config.readChID.Length != BTsocket.profile.readChID.Length)
		{
			readChIn.textComponent.color = Color.red;
			flag = false;
		}
		else
		{
			readChIn.textComponent.color = Color.black;
		}
		if (config.writeChID.Length != BTsocket.profile.writeChID.Length)
		{
			writeChIn.textComponent.color = Color.red;
			flag = false;
		}
		else
		{
			writeChIn.textComponent.color = Color.black;
		}
		return flag;
	}

	public void setFull(string full)
	{
		if(full.Length == 0)
		{
			fullIn.text = BTsocket.profile.full;
			full = BTsocket.profile.full;
		}		
		full.ToLower();
		config.full = full;
	}
	public void setService(string service)
	{
		if (service.Length == 0)
		{
			serviceIn.text = BTsocket.profile.serviceID;
			service = BTsocket.profile.serviceID;
		}
		config.serviceID = service;
		
	}

	public void setReadCh(string readCh)
	{
		if (readCh.Length == 0)
		{
			readChIn.text = BTsocket.profile.readChID;
			readCh = BTsocket.profile.readChID;
		}
		config.readChID = readCh;
		
	}

	public void setWriteCh(string writeCh)
	{
		if (writeCh.Length == 0)
		{
			writeChIn.text = BTsocket.profile.writeChID;
			writeCh = BTsocket.profile.writeChID;
		}
		config.writeChID = writeCh;
	}
}
