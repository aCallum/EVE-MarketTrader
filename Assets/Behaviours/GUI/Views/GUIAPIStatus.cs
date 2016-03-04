using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using EVEMarketTrader;

public class GUIAPIStatus : MonoBehaviour {

    public AccessTokenData accessTokenData;
    public ApplicationData applicationData;

    public Image statusImage;
    public RawImage characterPortrait;

    public Text pilotName;
    public Text tokenExpiry;

    public Button refreshTokenButton;

    private Texture _protrait = new Texture();

    private WWW _www;
    private Dictionary<string, string> _headers = new Dictionary<string, string>();
    private byte[] _data;

    private string _refreshTokenURI = "https://login.eveonline.com/oauth/token";
    private string _charURI = "https://image.eveonline.com/Character/"; //_256.jpg";
    

	// Use this for initialization
	void Start () {
                
        StartCoroutine(DoRequestCharacterPortrait());
	}
	
	// Update is called once per frame
	void Update () {

        statusImage.color = GetAPIStatus();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoRequestCharacterPortrait() {
        
        if (accessTokenData.access_token != null || 
            accessTokenData.access_token != "")
        {
            if (DateTime.Parse(accessTokenData.expire_time) > DateTime.Now) {
                tokenExpiry.text = "Valid until: " + accessTokenData.expire_time;
            }
            else {

                tokenExpiry.text = "Access token expired";
                pilotName.text = "Unknown";
            }
        }

        _headers.Clear();

        _headers.Add("Authorization", "Bearer " + accessTokenData.access_token);
        _headers.Add("Content-Type", "application/x-www-form-urlencoded");
        _headers.Add("Host", "login.eveonline.com");

        //_data = System.Text.Encoding.UTF8.GetBytes("");

        _www = new WWW("https://login.eveonline.com/oauth/verify", null, _headers);

        while (!_www.isDone) {
            yield return null;            
        }

        Debug.LogWarning(_www.text);

        CharBasic _charBasic = new CharBasic();
        _charBasic = JsonUtility.FromJson<CharBasic>(_www.text);

        PlayerPrefs.SetInt("character_id", _charBasic.CharacterID);

        if (_charBasic.CharacterName != null) {
            pilotName.text = _charBasic.CharacterName;
        }

        _www = new WWW(_charURI + _charBasic.CharacterID + "_256.jpg");

        while (!_www.isDone) {
            yield return null;            
        }

        string path = string.Format("{0}/"+ _charBasic.CharacterID +"_256.jpg", Application.persistentDataPath);
        Debug.Log(path);
        File.WriteAllBytes(path, _www.texture.EncodeToPNG());

        _protrait = _www.texture;
        characterPortrait.texture = _protrait;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Color GetAPIStatus() {

        if (accessTokenData.access_token == "" || accessTokenData.access_token == null) {
            return Color.red;
        }

        if (DateTime.Now < DateTime.Parse(accessTokenData.expire_time)) {
            return Color.green;
        }

        if (DateTime.Now > DateTime.Parse(accessTokenData.expire_time)) {
            return Color.red;
        }

        return Color.white;
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshAccessToken() {

        StartCoroutine(DoRefreshAccessToken());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoRefreshAccessToken() {

        _headers.Clear();

        _headers.Add("Authorization", applicationData.authorisation);
        _headers.Add("Content-Type", "application/x-www-form-urlencoded");
        _headers.Add("Host", "login.eveonline.com");

        _data = System.Text.Encoding.UTF8.GetBytes("grant_type=refresh_token&refresh_token=" + PlayerPrefs.GetString("refresh_token"));

        _www = new WWW(_refreshTokenURI, _data, _headers);

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

        StartCoroutine(DoRequestCharacterPortrait());
    }
}
