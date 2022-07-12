using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.Text;

public class SQLiteManager{

	private SqliteConnection dbSqlConnection = null;//資料庫連線
    private SqliteCommand dbSqlCommand = null;//資料庫的命令功能
    private SqliteDataReader dbSqlReader = null;//讀取資料庫的資料

	private string dbPath, dbName;
	
	
	public SQLiteManager(string _dbPath = "", string _dbName = "")
	{
		dbPath = @"Data Source=" + _dbPath;
		dbName = _dbName;
	}

	public void dbConnection()
	{
		dbSqlConnection = new SqliteConnection(dbPath + dbName);
	}
	public void dbDisconnection()
	{
		dbSqlConnection.Close();
		if (dbSqlReader != null)
        {
            dbSqlReader.Dispose();
            dbSqlReader = null;
        }
        if (dbSqlCommand != null)
        {
            dbSqlCommand.Dispose();
            dbSqlCommand = null;
        }
        if (dbSqlConnection != null)
        {
            dbSqlConnection.Dispose();
            dbSqlConnection = null;
        }
	}

	public void dbOpen()
	{
		dbSqlConnection.Open();
	}
	public void dbClose()
	{
		dbSqlConnection.Close();
	}

	public bool isTableExists(string tableName)
    {
        dbSqlCommand = dbSqlConnection.CreateCommand();
        dbSqlCommand.CommandText = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + tableName + "';";

        if (Convert.ToInt32(dbSqlCommand.ExecuteScalar()) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public SqliteDataReader dbQuery(string query)
	{
		dbSqlCommand = dbSqlConnection.CreateCommand();
		dbSqlCommand.CommandText = query;
		dbSqlReader = dbSqlCommand.ExecuteReader();
		dbSqlCommand.Dispose();
		dbSqlCommand = null;
		return dbSqlReader;
	}

	/*public List<string[]> dbReadRow()
	{
		List<string[]> list = new List<string[]>();
		while(dbSqlReader.Read())
		{
			string[] row = new string[4];
			for(int i = 0; i < 4; i++)
			{
				row[i] = dbSqlReader.GetValue(i).ToString();
			}
			list.Add(row);
		}
		return list;
	}*/
	/*public List<List<string>> dbReadTableAll()
	{
		List<List<string>> list = new List<List<string>>();
		while(dbSqlReader.Read())
		{
			List<string> row = new List<string>();
			for(int i = 0; i < 4; i++)
			{
				row.Add(dbSqlReader.GetValue(i).ToString());
			}
			list.Add(row);
		}
		return list;
	}*/
}
