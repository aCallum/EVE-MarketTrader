using UnityEngine;

using System;
using System.Collections;

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