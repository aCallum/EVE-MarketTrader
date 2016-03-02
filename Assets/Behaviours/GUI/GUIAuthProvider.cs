using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using EVEMarketTrader;

public class GUIAuthProvider : MonoBehaviour {
    
    public InputField authKey;
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

        authKey.text = PlayerPrefs.GetString("refresh_token");
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
                
        StartCoroutine(DoRequestAccessToken());
	}

    /// <summary>
    /// 
    /// </summary>
    public void SkipAuthPressed() {

        SceneManager.LoadScene("Main");
    }
	
	// Update is called once per frame
	void Update () {

        //requestButton.interactable = authKey.text == "" ? false : true;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoRequestAccessToken() {

        requestButton.interactable = false;

        // Clear any headers
        _headers.Clear();

        _headers.Add("Authorization", applicationData.authorisation);
        _headers.Add("Content-Type", "application/x-www-form-urlencoded");
        _headers.Add("Host", "login.eveonline.com");

        // Check to see if we have a refresh token we can use
        string _refresh_token = PlayerPrefs.GetString("refresh_token");

        // If we have a refresh token available, use it to authenticate
        if (_refresh_token != null && _refresh_token != "") {
            _data = System.Text.Encoding.UTF8.GetBytes("grant_type=refresh_token&refresh_token=" + _refresh_token);
        }
        // Otherwise request auth..
        else {
            _data = System.Text.Encoding.UTF8.GetBytes("grant_type=authorization_code&code=" + authKey.text);
        }

        _www = new WWW("https://login.eveonline.com/oauth/token", _data, _headers);

        while (!_www.isDone) {

            yield return null;
        }

        Debug.Log(_www.text);

        // Deserialise the access token
        AccessToken _accessToken = new AccessToken();
        _accessToken = JsonUtility.FromJson<AccessToken>(_www.text);

        Debug.Log(_accessToken.access_token); 

        accessTokenData.access_token = _accessToken.access_token;
        accessTokenData.token_type = _accessToken.token_type;
        accessTokenData.expires_in = _accessToken.expires_in;
        accessTokenData.expire_time = (DateTime.Now + TimeSpan.FromSeconds(_accessToken.expires_in)).ToString();
        accessTokenData.refresh_token = _accessToken.refresh_token;

        // If we have not assigned a refresh token yet, do so now
        // if (PlayerPrefs.GetString("refresh_token") == null || PlayerPrefs.GetString("refresh_token") == "") {
        PlayerPrefs.SetString("refresh_token", _accessToken.refresh_token);
        //}        
        
        SceneManager.LoadScene("Main");
    }
}
