using UnityEngine;
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

    public AccessTokenData accessTokenData;

    private string _authURL = "https://login.eveonline.com/oauth/authorize/?response_type=code";
    private string _redirect = "&redirect_uri=http://www.alastaircallum.com/w/eve-markettrader/auth.php";
    private string _clientID = "&client_id=bd720d8fb60841b88e343b0546577431";
    private string _scope = "&scope=characterFittingsRead characterFittingsWrite characterLocationRead characterNavigationWrite";
    private string _state = "&state=state987654";

    private string _url;

    private WWW _www;
    private Dictionary<string, string> _headers = new Dictionary<string,string>();
    private byte[] _data;

    // Use this for initialization
    private void Start() {

        _headers.Add("Authorization", "Basic YmQ3MjBkOGZiNjA4NDFiODhlMzQzYjA1NDY1Nzc0MzE6SG1zRlgyY1E3aGNrNXUzM0lHeGNVdmV6a2JJOGYyUlU4aVcwUE0zcA==");
        _headers.Add("Content-Type", "application/x-www-form-urlencoded");
        _headers.Add("Host", "login.eveonline.com");
    }

    /// <summary>
    /// 
    /// </summary>
    public void SignInWithEVEPressed() {

        _url = _authURL + _redirect + _clientID + _scope + _state;

        Application.OpenURL(_url);

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
