/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data.MySqlClient;
public class MySQLManager : MonoBehaviour {

	public Text debugTxt;

	[SerializeField]
	private string host;
	[SerializeField]
	private string dbName;
	[SerializeField]
	private string user;
	[SerializeField]
	private string pwd;

	[SerializeField]
	private string tableName;

	public static string sqlCmd = "";

	private MySqlConnection sqlConnect;
	private MySqlCommand sqlCommand;
	private DataSet ds;
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void sqlTest()
	{
		sqlConnect = new MySqlConnection(string.Format("Server = {0}; Database = {1}; User Id = {2}; Password = {3};Connect Timeout = 60;CharSet=utf8;", host, dbName, user, pwd));
		debugTxt.text = string.Format("Server={0}; Database = {1}; User Id = {2}; password = {3};", host, dbName, user, pwd);
		Debug.Log(debugTxt.text);
		try
		{
			sqlConnect.Open();
			debugTxt.text = sqlConnect.Database;
		}catch(System.Exception ee)
		{
			debugTxt.text = ee.ToString();
		}
		/*sqlCmd = "select * from " + tableName;
		debugTxt.text += sqlCmd;
		
		sqlCommand = new MySqlCommand(sqlCmd, sqlConnect);
		ds = new DataSet();
		try
        {
            MySqlDataAdapter da = new MySqlDataAdapter(sqlCmd, sqlConnect);
            da.Fill(ds);
            Debug.Log("Query success!");
			debugTxt.text += ds.Tables[0].Rows[0][1];
            Debug.Log(ds.Tables[0].Rows[0][1]);
        }
        catch (System.Exception ee)
        {
            throw new System.Exception("SQL: " + sqlCmd + "\n" + ee.Message.ToString());
        }
		sqlConnect.Close();
	}
}*/
