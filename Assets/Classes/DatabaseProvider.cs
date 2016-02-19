using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using System;
using System.Data;
using Mono.Data.Sqlite;

public static class DatabaseProvider {

    private static IDbConnection _dbConnection;
    private static IDbCommand _dbCommand;
    private static IDataReader _dbReader;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_query"></param>
    /// <returns></returns>
    private static List<object> DBQuery(string _query) {

        //string sqlQuery = "SELECT TYPEID FROM Types WHERE TYPENAME = '" + _value + "'";

        // If the DB is not connected 
        if (_dbConnection == null || _dbConnection.State != ConnectionState.Open) {
            DBConnect();
        }

        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.CommandText = _query;

        _dbReader = _dbCommand.ExecuteReader();

        List<object> _values = new List<object>();

        //int _fieldCount = 

        while (_dbReader.Read()) {
            _values.Add(_dbReader.GetValue(0));
        }

        DBDisconnect();

        return _values;
    }

    /// <summary>
    /// 
    /// </summary>
    private static void DBConnect() {

        string _connection = "URI=file:" + Application.dataPath + "/StreamingAssets/EveTrader.db";

        _dbConnection = new SqliteConnection(_connection);
        _dbConnection.Open();
    }

    /// <summary>
    /// 
    /// </summary>
    private static void DBDisconnect() {

        if (_dbConnection != null && _dbConnection.State != ConnectionState.Closed) {

            _dbReader.Close();
            _dbReader = null;
            _dbCommand.Dispose();
            _dbCommand = null;
            _dbConnection.Close();
            _dbConnection = null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static List<UnityEngine.UI.Dropdown.OptionData> GetRegionNames() {

        List<object> _values = DBQuery("SELECT REGIONNAME FROM Regions");

        List<UnityEngine.UI.Dropdown.OptionData> _regionNames = new List<UnityEngine.UI.Dropdown.OptionData>();

        for (int i = 0; i < _values.Count; i++) {
            UnityEngine.UI.Dropdown.OptionData _data = new UnityEngine.UI.Dropdown.OptionData();
            _data.text = _values[i].ToString();
            _regionNames.Add(_data);
		}

        return _regionNames;
    }

    public static int GetItemID(string _itemName) {

        List<object> _values = DBQuery("SELECT TYPEID FROM Types WHERE TYPENAME = '" + _itemName + "'");

        if (_values.Count <= 0) {
            return -1;
        }

        return Int32.Parse(_values[0].ToString());
    }

    public static int GetRegionID(string _regionName) {

        List<object> _values = DBQuery("SELECT REGIONID FROM Regions WHERE REGIONNAME = '" + _regionName + "'");

        if (_values.Count <= 0) {
            return -1;
        }

        return Int32.Parse(_values[0].ToString());
    }

}
