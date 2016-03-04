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
    private static List<List<object>> DBQuery(string _query) {

        //string sqlQuery = "SELECT TYPEID FROM Types WHERE TYPENAME = '" + _value + "'";

        // If the DB is not connected 
        if (_dbConnection == null || _dbConnection.State != ConnectionState.Open) {
            DBConnect();
        }

        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.CommandText = _query;

        _dbReader = _dbCommand.ExecuteReader();

        List<List<object>> _values = new List<List<object>>();

        int _fieldCount = _dbReader.FieldCount;

        while (_dbReader.Read()) {

            List<object> _fields = new List<object>();

            for (int i = 0; i < _fieldCount; i++) {
                _fields.Add(_dbReader.GetValue(i));
            }

            _values.Add(_fields);
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

        List<List<object>> _values = DBQuery("SELECT REGIONNAME FROM Regions");

        List<UnityEngine.UI.Dropdown.OptionData> _regionNames = new List<UnityEngine.UI.Dropdown.OptionData>();

        for (int i = 0; i < _values.Count; i++) {
            UnityEngine.UI.Dropdown.OptionData _data = new UnityEngine.UI.Dropdown.OptionData();
            _data.text = _values[i][0].ToString();
            _regionNames.Add(_data);
		}

        return _regionNames;
    }

    public static int GetItemID(string _itemName) {

        List<List<object>> _values = DBQuery("SELECT TYPEID FROM Types WHERE TYPENAME = '" + _itemName + "'");
        
        if (_values.Count <= 0) {
            Debug.Log("NONE FOUND");
            return -1;
        }

        return Int32.Parse(_values[0][0].ToString());
    }

    public static int GetRegionID(string _regionName) {

        List<List<object>> _values = DBQuery("SELECT REGIONID FROM Regions WHERE REGIONNAME = '" + _regionName + "'");

        if (_values.Count <= 0) {
            return -1;
        }

        return Int32.Parse(_values[0][0].ToString());
    }

    public static List<List<object>> GetRegions() {

        List<List<object>> _values = DBQuery("SELECT * FROM Regions");

        return _values;
    }
    public static List<List<object>> GetRegionNameFromID(string _id) {

        List<List<object>> _values = DBQuery("SELECT REGIONNAME FROM Regions WHERE REGIONID ='" + _id + "'");

        return _values;
    }


    public static List<List<object>> GetSolarSystems(string _constID) {

        List<List<object>> _values = DBQuery("SELECT * FROM SolarSystems WHERE CONSTELLATIONID ='" + _constID + "'");

        return _values;
    }

    public static List<List<object>> GetRegionIDFromSolarSystem(string _solarSystemName) {

        List<List<object>> _values = DBQuery("SELECT REGIONID FROM SolarSystems WHERE SOLARSYSTEMNAME ='" + _solarSystemName + "'");

        return _values;
    }

    public static List<List<object>> GetSolarSystemIDFromStationID(string _stationID) {

        List<List<object>> _values = DBQuery("SELECT SOLARSYSTEMID FROM Stations WHERE STATIONID ='" + _stationID + "'");

        return _values;
    } 
}
