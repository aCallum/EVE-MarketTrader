using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class ApplicationData : ScriptableObject {

    public string authorisationURL;
    public string redirectURI;
    public string clientID;
    public string scope;
    public string state;

    public string authorisation;

}