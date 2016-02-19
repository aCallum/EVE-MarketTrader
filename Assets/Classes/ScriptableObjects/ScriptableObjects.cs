using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class AccessTokenData : ScriptableObject {

    public string access_token;
    public string token_type;
    public int expires_in;
    public string expire_time;
    public string refresh_token;
}

public class RegionListEntryData {
    public string id_str;
    public string href;
    public int id;
    public string name;
}

public class RegionListData {
    public string totalCount_str;
    public List<RegionListEntryData> items;
    public int pageCount;
    public string pageCount_str;
    public int totalCount;
}