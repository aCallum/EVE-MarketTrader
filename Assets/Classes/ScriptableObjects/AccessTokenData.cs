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

    public bool IsValid() {

        if (expire_time != "") {
            return (DateTime.Now < DateTime.Parse(expire_time)) ? true : false;            
        }
        else {
            return false;
        }
        
    }
}