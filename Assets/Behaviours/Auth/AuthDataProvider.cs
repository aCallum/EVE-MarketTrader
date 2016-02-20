﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using EVEMarketTrader;

public class AuthDataProvider : MonoBehaviour {
    
    //public string authKey = "FPBNsFSugwzuVarmIGzY2ph12QDLl_8KTE9fJz4b5_hN6fWddkw5emleEk4KcJv40";
    public Text authKey;
    public Button requestButton;

    public ApplicationData applicationData;
    public AccessTokenData accessTokenData;

    private string _authURL;
    private string _redirect;
    private string _clientID;
    private string _scope;
    private string _state;

    private string _urlSOO;

    private WWW _www;
    private Dictionary<string, string> _headers = new Dictionary<string,string>();
    private byte[] _data;

    // Use this for initialization
    private void Start() {

        _authURL = applicationData.authorisationURL;
        _redirect = "&redirect_uri=" + applicationData.redirectURI;
        _clientID = "&client_id=" + applicationData.clientID;
        _scope = "&scope=" + applicationData.scope;
        _state = "&state=" + applicationData.state;

        _headers.Add("Authorization", applicationData.authorisation);
        _headers.Add("Content-Type", "application/x-www-form-urlencoded");
        _headers.Add("Host", "login.eveonline.com");
    }

    /// <summary>
    /// 
    /// </summary>
    public void SignInWithEVEPressed() {

        _urlSOO = _authURL + _redirect + _clientID + _scope + _state;
        Application.OpenURL(_urlSOO);
    }

    /// <summary>
    /// 
    /// </summary>
	public void RequestAccessTokenPressed () {

        if (authKey.text == "") {
            return;
        }

        _data = System.Text.Encoding.UTF8.GetBytes("grant_type=authorization_code&code=" + authKey.text);
        
        StartCoroutine(DoRequest());
	}
	
	// Update is called once per frame
	void Update () {

        //requestButton.interactable = authKey.text == "" ? false : true;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoRequest() {

        requestButton.interactable = false;

        _www = new WWW("https://login.eveonline.com/oauth/token", _data, _headers);

        while (!_www.isDone) {

            yield return null;
        }

        Debug.Log(_www.text);

        // Deserialise the access token
        AccessToken _accessToken = new AccessToken();
        _accessToken = JsonUtility.FromJson<AccessToken>(_www.text);

        accessTokenData.access_token = _accessToken.access_token;
        accessTokenData.token_type = _accessToken.token_type;
        accessTokenData.expires_in = _accessToken.expires_in;
        accessTokenData.expire_time = (DateTime.Now + TimeSpan.FromSeconds(_accessToken.expires_in)).ToString();
        accessTokenData.refresh_token = _accessToken.refresh_token;

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Main");
    }
}