using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text;
using System;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public struct BluetoothData
{
	public string name;
	public string address;
	public string service;
	public string rCharacteristic;
	public string wCharacteristic;
	private BTprofile profile;
	//private static string full = "6e40****-b5a3-f393-e0a9-e50e24dcca9e";
	public BluetoothData(string _name = "", string _address = "", BTprofile _profile = new BTprofile())
	{
		name = _name;
		address = _address;
		profile = _profile;
		service = _profile.serviceID; //"0001";
		rCharacteristic = _profile.readChID; //"0002";	//RX
		wCharacteristic = _profile.writeChID; //"0003";	//TX
	}
	public string fullUUID(string uuid)
	{
		return profile.full.Replace("****", uuid);
	}
	public bool isEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
		{
			uuid1 = fullUUID(uuid1);
		}
		if (uuid2.Length == 4)
		{
			uuid2 = fullUUID(uuid2);
		}
		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
	}
}

public struct BTprofile
{
	private string name;
	public string full;
	public string serviceID;
	public string readChID;
	public string writeChID;


	public BTprofile(string _profileName, string _full = "6e40****-b5a3-f393-e0a9-e50e24dcca9e",
		string _service = "0001", string _readCh = "0002", string _writeCh = "0003")
	{
		name = _profileName;
		full = _full;
		serviceID = _service;
		readChID = _readCh;
		writeChID = _writeCh;
		fileSave(this);
	}

	/*
	*	用於優先讀取已儲存檔案的建構子，不覆蓋已儲存的資料
	*	useFile:
	*		true 	=> 使用已儲存的預設檔案
	*		false 	=> 以輸入參數覆蓋預設檔案
	*	其餘部分與預設建構子相同
	 */
	public BTprofile(string _profileName, bool useFile, string _full = "6e40****-b5a3-f393-e0a9-e50e24dcca9e",
		string _service = "0001", string _readCh = "0002", string _writeCh = "0003")
	{
		if(useFile)
		{
			if(isEmpty(_profileName))
			{
				name = _profileName;
				full = _full;
				serviceID = _service;
				readChID = _readCh;
				writeChID = _writeCh;
				fileSave(this);
			}
			else
			{
				name = _profileName;
				full = PlayerPrefs.GetString(name + "BTfull");
				serviceID = PlayerPrefs.GetString(name + "BTservice");
				readChID = PlayerPrefs.GetString(name + "BTreadCh");
				writeChID = PlayerPrefs.GetString(name + "BTwriteCh");
			}
		}
		else
		{
			name = _profileName;
			full = _full;
			serviceID = _service;
			readChID = _readCh;
			writeChID = _writeCh;
			fileSave(this);
		}
		
	}
	public static bool isEmpty(string name)
	{
		string full = PlayerPrefs.GetString(name + "BTfull");
		string serviceID = PlayerPrefs.GetString(name + "BTservice");
		string readChID = PlayerPrefs.GetString(name + "BTreadCh");
		string writeChID = PlayerPrefs.GetString(name + "BTwriteCh");
		if (full == "" || serviceID == "" || readChID == "" || writeChID == "")
		{
			return true;
		}
		return false;
	}

	public void fileReLoad()
	{
		full = PlayerPrefs.GetString(name + "BTfull");
		serviceID = PlayerPrefs.GetString(name + "BTservice");
		readChID = PlayerPrefs.GetString(name + "BTreadCh");
		writeChID = PlayerPrefs.GetString(name + "BTwriteCh");
	}
	/*public void initialize()
	{
		PlayerPrefs.SetString(name + "BTfull", full);
		PlayerPrefs.SetString(name + "BTservice", serviceID);
		PlayerPrefs.SetString(name + "BTreadCh", readChID);
		PlayerPrefs.SetString(name + "BTwriteCh", writeChID);
		fileReLoad();
	}*/

	public static void fileSave(BTprofile _profile)
	{
		PlayerPrefs.SetString(_profile.name + "BTfull", _profile.full);
		PlayerPrefs.SetString(_profile.name + "BTservice", _profile.serviceID);
		PlayerPrefs.SetString(_profile.name + "BTreadCh", _profile.readChID);
		PlayerPrefs.SetString(_profile.name + "BTwriteCh", _profile.writeChID);
	}
}


public class BTsocket : MonoBehaviour {	
	public BTprofile profile;
	private static bool isInit = false;
	private static bool scanState = false;
	private bool isConnected = false;
	private bool BTStatus = false;
	private bool disConnectFlag = false;
	private Coroutine reConnectCoro = null;
	public bool bleActive
	{
		get{
			return BTStatus;
		}
	}

	private Dictionary<string, BluetoothData> peripheralList;
	private BluetoothData linkData;
	public BluetoothData bleLinkData
	{
		get{
			return linkData;
		}
	}

	private bool readFound = false;
	private bool writeFound = false;
	private string receiveText = "";
	public string BTLog = "";


	public static BTsocket getNewBTsocket(string btName, BTprofile _profile)
	{		
		BTsocket btSoc;
		if(GameObject.Find(btName) != null)
		{
			btSoc = GameObject.Find(btName).GetComponent<BTsocket>();
			btSoc.profile = new BTprofile(btName, _profile.full, _profile.serviceID, _profile.readChID, _profile.writeChID);
			return btSoc;
		}
		GameObject btObj = new GameObject(btName);
		btSoc = btObj.AddComponent<BTsocket>();	
		btSoc.profile = new BTprofile(btName, _profile.full, _profile.serviceID, _profile.readChID, _profile.writeChID);
		btSoc.initialize();
		Debug.Log(btSoc.profile.full);
		Instantiate(btObj);
		DontDestroyOnLoad(btObj);
		return btSoc;
	}

	public static bool existBTsocket(string btName)
	{
		try
		{
			GameObject.Find(btName).GetComponent<BTsocket>();
		}
		catch(NullReferenceException e)
		{
			return false;
		}
		return true;
	} 

	public static bool BLEisConnected(string btName)
	{
		if(!existBTsocket(btName))
			return false;
		return GameObject.Find(btName).GetComponent<BTsocket>().bleActive;
	}
	private void initialize()
	{
		requestAndroidPermission();
		if (isInit)
			return;
		BTLog = "Initialising bluetooth\n";
		BluetoothLEHardwareInterface.Initialize(true, false, () => {; }, (error) => {; });
		isInit = true;
	}

	public void scan()
	{
		BTLog = "Starting scan\n";
		scanState = true;
		peripheralList = new Dictionary<string, BluetoothData>();
		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null,
			(address, name) => 
			{ 
				addPeripheral(address, name);
			},
			(address, name, rssi, advertisingInfo) => {; });
	}

	public void scan(System.Action<string, string> newPeripheralAction)
	{
		BTLog = "Starting scan\n";
		scanState = true;
		peripheralList = new Dictionary<string, BluetoothData>();
		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null,
			(address, name) => 
			{ 
				addPeripheral(address, name);
				newPeripheralAction(address, name);
			},
			(address, name, rssi, advertisingInfo) => {; });
	}

	private void addPeripheral(string address, string name)
	{
		BTLog += ("Found " + address + "\n");
		if (!peripheralList.ContainsKey(address))
		{			
			peripheralList[address] = new BluetoothData(name, address, profile);
		}
	}

	public void connect(string addr, float reConnectTTL = 0f)
	{
		if (isConnected)
			return;
		if (!peripheralList.ContainsKey(addr))
		{
			if(!scanState)
				scan();
			return;
		}
		BTStatus = false;
		linkData = new BluetoothData("", "", profile);
		isConnected = false;
		readFound = false;
		writeFound = false;
		disConnectFlag = false;
		BTLog = "Connecting to \n" + addr + "\n";
		//BTLog += linkData.address + "\n" + linkData.service + "\n" + linkData.rCharacteristic + "\n" + linkData.wCharacteristic + "\n";
		BluetoothLEHardwareInterface.ConnectToPeripheral(addr, 
			(address) => { 
				isConnected = true; 				
			},
			(address, serviceUUID) => {; },
		   	(address, serviceUUID, characteristicUUID) =>
		   	{
				// discovered characteristic
				if (linkData.isEqual(serviceUUID, peripheralList[address].service))
				{				   
					linkData.name = peripheralList[address].name;
					linkData.address = address;
					linkData.service = peripheralList[address].service;
					if (linkData.isEqual(characteristicUUID, peripheralList[address].rCharacteristic))
					{
						readFound = true;
						linkData.rCharacteristic = peripheralList[address].rCharacteristic;
						BTLog += "readTrue \n";
					}
					if (linkData.isEqual(characteristicUUID, peripheralList[address].wCharacteristic))
					{
						writeFound = true;
						linkData.wCharacteristic = peripheralList[address].wCharacteristic;
						BTLog += "writeTrue \n";
					}

					if (readFound && writeFound)
					{
						Invoke("delayConnect", 1f);
						BTLog = address + "\nConnected!\n";
						receiveText = "";
						BluetoothLEHardwareInterface.StopScan();
						scanState = false;
					}
					
				}
			},
			(address) =>
			{
				BluetoothLEHardwareInterface.StopScan();
                scanState = false;
                BTStatus = false;
                isConnected = false;
				if(reConnectTTL > 0f && !disConnectFlag)
				{					                	
					reConnectCoro = StartCoroutine(reConnect(address, reConnectTTL));
					BTLog = "re connecting...";
				}
			});
	}

	private IEnumerator reConnect(string addr, float TTL)
	{
		float timer = 0f, freq = 2f;
		yield return new WaitForSecondsRealtime(freq);
		while(timer < TTL && !BTStatus)
		{			
			connect(addr, TTL);
			timer += freq;
			yield return new WaitForSecondsRealtime(freq);						
		}
		yield return new WaitForSecondsRealtime(1f);
		while(!BTStatus)
		{
			yield return 0;			
		}
		isConnected = true;
		subscribe();
		reConnectCoro = null;
		yield break;
	}
	

	private void delayConnect()
	{
		BTStatus = true;
	}

	public void subscribe()
	{
		if (!BTStatus)
			return;
		BTLog = "SubscribeStart!\n";
		BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(
			linkData.address, linkData.fullUUID(linkData.service), linkData.fullUUID(linkData.rCharacteristic),
			(deviceAddress, notification) => {; },
			(deviceAddress2, characteristic, data) =>
			{				
				if (deviceAddress2.CompareTo(linkData.address) == 0) //讀資料
				{
					if (data.Length > 0)
					{
						string s = Encoding.UTF8.GetString(data); ///Byte to string
						receiveText = s;
					}
				}
			});
	}

	public void readCharacteristic()
	{
		if (!BTStatus)
			return;
		BTLog = "ReadStart!\n";
		/*BTLog += linkData.address;
		BTLog += BluetoothData.fullUUID(linkData.service);
		BTLog += BluetoothData.fullUUID(linkData.rCharacteristic);*/
		BluetoothLEHardwareInterface.ReadCharacteristic(
			linkData.address, linkData.fullUUID(linkData.service), linkData.fullUUID(linkData.rCharacteristic),
			(deviceAddress, data) => 
			{
				if (data.Length > 0)
				{
					string s = Encoding.UTF8.GetString(data); ///Byte to string
					receiveText = s;
				}
			});
	}

	public void writeCharacteristic(string sendStr)
	{
		if (!BTStatus)
			return;
		BTLog = "WriteStart!\n";

		byte[] sendData = Encoding.UTF8.GetBytes(sendStr);
		/*BTLog += linkData.address;
		BTLog += BluetoothData.fullUUID(linkData.service);
		BTLog += BluetoothData.fullUUID(linkData.rCharacteristic);*/
		BluetoothLEHardwareInterface.WriteCharacteristic(
			linkData.address, linkData.fullUUID(linkData.service), linkData.fullUUID(linkData.wCharacteristic),
			sendData, sendData.Length,
			false, (deviceAddress) => {BTLog += "send:" + sendStr + "\n"; });
	}

	public void getReceiveText(ref string buffer)   //讀取資料後的操作
	{
		if(!(receiveText.Length > 0))
			return;
		buffer = receiveText;
		BTLog += "read:" + receiveText + "\n";
		receiveText = "";
	}

	public void getReceiveText(System.Action<string> receiveAction)   //讀取資料後的操作
	{
		if(!(receiveText.Length > 0))
			return;
		receiveAction(receiveText);
		BTLog += "read:" + receiveText + "\n";
		receiveText = "";
	}

	public void disConnect()
	{
		BluetoothLEHardwareInterface.StopScan();
		scanState = false;
		if (!isConnected)
			return;
		BTLog = "";
		disConnectFlag = true;
		if(reConnectCoro != null)
		{
			StopCoroutine(reConnectCoro);
			reConnectCoro = null;
		}
		BluetoothLEHardwareInterface.UnSubscribeCharacteristic(
			linkData.address, linkData.fullUUID(linkData.service), linkData.fullUUID(linkData.rCharacteristic),
			(deviceAddress) => {BTLog = "UnSubscribe " + deviceAddress + "\n";});

		BluetoothLEHardwareInterface.DisconnectPeripheral(linkData.address,
			(deviceAddress) =>
			{
				BTLog += "Disconnect " + deviceAddress + "\n";
				isConnected = false;
				BTStatus = false;
			});
	}

	void OnDestroy()
    {
        disConnect();
    }

	public static void requestAndroidPermission()
	{
		#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        #endif
	}

}
