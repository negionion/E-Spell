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

	public InputField ledMacIn;
	private BTprofile config;

	private BTprofile profile;
	// Use this for initialization
	void Start () {
		if(!BTsocket.existBTsocket(Constants.bleMicroBit))
		{
			profile = new BTprofile(Constants.bleMicroBit, true);	//defult	
		}
		else
		{
			profile = GameObject.Find(Constants.bleMicroBit).GetComponent<BTsocket>().profile;
		}
		config = profile;
		fullIn.text = profile.full;
		serviceIn.text = profile.serviceID;
		readChIn.text = profile.readChID;
		writeChIn.text = profile.writeChID;

		ledMacIn.text = PlayerPrefs.GetString("LumexLED_BLE_MAC");
	}

	public void btConfig()
	{
		if (BTConfigPanel.activeSelf)
		{
			if (dataCheck())
			{
				BTprofile.fileSave(config);
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
		if (config.full.Length != profile.full.Length || !config.full.Contains("****"))
		{
			fullIn.textComponent.color = Color.red;
			flag = false;
		}
		else
		{
			fullIn.textComponent.color = Color.black;
		}
		if(config.serviceID.Length != profile.serviceID.Length)
		{
			serviceIn.textComponent.color = Color.red;
			flag = false;
		}
		else
		{
			serviceIn.textComponent.color = Color.black;
		}
		if (config.readChID.Length != profile.readChID.Length)
		{
			readChIn.textComponent.color = Color.red;
			flag = false;
		}
		else
		{
			readChIn.textComponent.color = Color.black;
		}
		if (config.writeChID.Length != profile.writeChID.Length)
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
			fullIn.text = profile.full;
			full = profile.full;
		}		
		full.ToLower();
		config.full = full;
	}
	public void setService(string service)
	{
		if (service.Length == 0)
		{
			serviceIn.text = profile.serviceID;
			service = profile.serviceID;
		}
		config.serviceID = service;
		
	}

	public void setReadCh(string readCh)
	{
		if (readCh.Length == 0)
		{
			readChIn.text = profile.readChID;
			readCh = profile.readChID;
		}
		config.readChID = readCh;
		
	}

	public void setWriteCh(string writeCh)
	{
		if (writeCh.Length == 0)
		{
			writeChIn.text = profile.writeChID;
			writeCh = profile.writeChID;
		}
		config.writeChID = writeCh;
	}


	public void setBLE_MAC(InputField field)
    {
        if(field.text.Length == 17)
            PlayerPrefs.SetString("LumexLED_BLE_MAC", field.text);
        else
            field.textComponent.color = Color.red;
    }
}
