using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using EVEMarketTrader;

public class GUIRoutePlanner : MonoBehaviour {

    public GUINavigation guiNavigation;
    public GUIMarketBrowser guiMarketBrowser;

    public Transform regionMapScrollView;
    public Transform constellationMapScrollView;

    public Transform regionMapContent;
    public Transform constellationMapContent;
    
    public GameObject imagePrefab;

    private WWW _www;

    private Transform _zoomTarget;

    private List<Transform> _regionButtons = new List<Transform>();
    private List<Transform> _solarSystemButtons = new List<Transform>();

    public AccessTokenData accessTokenData;

    public InputField charLocRegion;
    public InputField charLocSolarSystem;
    public InputField charLocStation;

    public InputField buyLocation;
    public InputField sellLocation;

    public Text errorDetails;

    private Color _systemColor;
    private float _pow = 400000000000000;

    //
    public void OnEnable() {

        if (accessTokenData.IsValid()) {
            StartCoroutine(DoRequestCharacterLocation());
        }
    }

	// Use this for initialization
	void Start () {

        _zoomTarget = regionMapContent;

        List<List<object>> _items = new List<List<object>>();
        _items = DatabaseProvider.GetRegions();
        
        for (int i = 0; i < _items.Count; i++) {

            GameObject _obj = Instantiate(imagePrefab, new Vector3((float)_items[i][2] / _pow, (float)_items[i][4] / _pow, -10), Quaternion.identity) as GameObject;

            _obj.transform.localScale = (Vector3.one / _zoomTarget.localScale.x);
            _obj.transform.SetParent(regionMapContent.transform, false);
            _regionButtons.Add(_obj.transform);

            GUIRegionButton _regionButton = _obj.GetComponentInChildren<GUIRegionButton>();
            _regionButton.regionID = _items[i][0].ToString();
            _regionButton.regionName = _items[i][1].ToString();
            _regionButton.regionLabel.text = _items[i][1].ToString();
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (regionMapScrollView.gameObject.activeSelf) {
            _zoomTarget = regionMapContent;
        }
        else if (constellationMapScrollView.gameObject.activeSelf) {
            _zoomTarget = constellationMapContent;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {

            _zoomTarget.localScale += Vector3.one * 0.2f;
            _zoomTarget.localScale = Vector3.ClampMagnitude(_zoomTarget.localScale, 15);

            UpdateButtonScales();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) {

            _zoomTarget.localScale -= Vector3.one * 0.08f;

            UpdateButtonScales();

            if (_zoomTarget.localScale.x <= 0.5f) {
                _zoomTarget.localScale = Vector3.one * 0.5f;
            }
        }         
	}

    private void UpdateButtonScales() { 
    
        foreach (Transform _transform in _regionButtons) {
            _transform.localScale = (Vector3.one / _zoomTarget.localScale.x) * 1.5f;
        }

        foreach (Transform _transform in _solarSystemButtons) {
            _transform.localScale = (Vector3.one / _zoomTarget.localScale.x);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_show"></param>
    /// <param name="_parent"></param>
    public void RegionButtonPressed(string _id, bool _show, Transform _parent) {     

        // Query all solar systems in _id

        GUIRegionButton _button = _parent.GetComponent<GUIRegionButton>();

        if (_show) {

            if (_button.isPopulated) {

                for (int i = 0; i < _button.solarsystems.Count; i++) {

                    _button.solarsystems[i].gameObject.SetActive(true);
                }
            }
            else {
                StartCoroutine(DoRequestConstellations(_id, _parent));
            }
        }
        else {

            for (int i = 0; i < _button.solarsystems.Count; i++) {

                _button.solarsystems[i].gameObject.SetActive(false);
            }
        }
    }
       
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_regionID"></param>
    /// <param name="_parent"></param>
    /// <returns></returns>
    private IEnumerator DoRequestConstellations(string _regionID, Transform _parent) {

        WWW _www = new WWW("https://public-crest.eveonline.com/regions/" + _regionID + "/");

        while (!_www.isDone) {
            yield return null;
        }

        Region _region = new Region();
        _region = JsonUtility.FromJson<Region>(_www.text);
        
        for (int i = 0; i < _region.constellations.Count; i++) {

            List<List<object>> _items = new List<List<object>>();
            _items = DatabaseProvider.GetSolarSystems(_region.constellations[i].id_str);

            _systemColor = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 1, 1);

            for (int j = 0; j < _items.Count; j++) {

                GameObject _obj = Instantiate(imagePrefab, new Vector3((float)_items[j][4] / _pow, (float)_items[j][6] / _pow, 0), Quaternion.identity) as GameObject;

                _obj.hideFlags = HideFlags.HideInHierarchy;

                _obj.transform.localScale = (Vector3.one / _zoomTarget.localScale.x);
                _obj.transform.SetParent(regionMapContent.transform, false);

                _parent.GetComponent<GUIRegionButton>().solarsystems.Add(_obj.transform);

                _solarSystemButtons.Add(_obj.transform);

                GUIRegionButton _regionButton = _obj.GetComponentInChildren<GUIRegionButton>();
                _obj.GetComponent<Image>().color = _systemColor;
                //_regionButton.regionID = _items[i][0].ToString();
                //_regionButton.regionName = _items[i][1].ToString();
                _regionButton.regionLabel.text = _items[j][1].ToString();

            }
        }

        _parent.GetComponent<GUIRegionButton>().isPopulated = true;
    }

    private IEnumerator DoRequestCharacterLocation() {

        Dictionary<string, string> _headers = new Dictionary<string, string>();
        
        _headers.Add("Authorization", "Bearer " + accessTokenData.access_token);
        _headers.Add("Content-Type", "application/x-www-form-urlencoded");
        _headers.Add("Host", "login.eveonline.com");

        //_data = System.Text.Encoding.UTF8.GetBytes("");

        Debug.Log("CharID: " + PlayerPrefs.GetInt("character_id"));

        WWW _www = new WWW("https://crest-tq.eveonline.com/characters/" + PlayerPrefs.GetInt("character_id") + "/location/", null, _headers);

        Debug.Log(_www.url);

        while (!_www.isDone) {
            yield return null;
        }

        if (_www.error != "") {
            errorDetails.text = _www.error;
        }

        if (_www.text.Contains("error")) {
            errorDetails.text +=  " : " + _www.text;
        }
        
        if (_www.text != "" && _www.text != "{}" && !_www.text.Contains("error")) {

            Debug.Log(_www.text);

            CharacterLocation _location = new CharacterLocation();
            _location = JsonUtility.FromJson<CharacterLocation>(_www.text);

            Debug.Log("Pilot is in: " + _location.solarSystem.name);

            charLocSolarSystem.text = _location.solarSystem.name;

            int _regionID = Int32.Parse(DatabaseProvider.GetRegionIDFromSolarSystem(_location.solarSystem.name)[0][0].ToString());
            //string _regionName = DatabaseProvider.GetRegionFromSolarSystem(_location.solarSystem.name)[0][1].ToString();

            //Debug.Log("Pilot is in: " + _regionID);

            foreach (Transform _button in _solarSystemButtons) {

                if (_button.GetComponent<GUIRegionButton>().regionID == _regionID.ToString()) {

                    _button.GetComponent<Image>().color = Color.cyan;

                    charLocSolarSystem.text = _button.GetComponent<GUIRegionButton>().regionName;
                }
            }
        }
    }

    public void ResetZoom() {

        _zoomTarget.transform.localScale = Vector3.one;

        UpdateButtonScales();

    }

    public void CloseAllRegions() { 
    

    }

    public Image testLine;

    Transform _A;
    Transform _B;

    public void SetDestinationLabels() {

        buyLocation.text = guiMarketBrowser.currentSellOrder.location.name;
        sellLocation.text = guiMarketBrowser.currentBuyOrder.location.name;

        guiNavigation.RoutePlannerButtonPressed();

        foreach (Transform _button in _regionButtons) {

            if (_button.GetComponent<GUIRegionButton>().regionName == guiMarketBrowser.regionDropdownA.options[guiMarketBrowser.regionDropdownA.value].text) {

                _A = _button;
                _button.GetComponent<Image>().color = Color.green;
            }

            if (_button.GetComponent<GUIRegionButton>().regionName == guiMarketBrowser.regionDropdownB.options[guiMarketBrowser.regionDropdownB.value].text) {

                _B = _button;
                _button.GetComponent<Image>().color = Color.red;
            }
        }

        float _distance = Vector3.Distance(_B.position, _A.position);
        Debug.Log(_B.position);
        testLine.rectTransform.sizeDelta = new Vector2(_distance * 2, 10);
        testLine.rectTransform.position = (_A.position + _B.position) / 2;
        testLine.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2((_A.position - _B.position).y, (_A.position - _B.position).x)));

    }

    /// <summary>
    /// 
    /// </summary>
    public void SetClientWaypoints() {

        if (guiMarketBrowser.currentSellOrder.id != 0 && guiMarketBrowser.currentBuyOrder.id != 0) {
            StartCoroutine(DoSetClientWaypoints());
        }
        else {

            errorDetails.text = "Please select a Buy and Sell order from the Market Browser.";
        }

        
    }

    private IEnumerator DoSetClientWaypoints() {

        Waypoint _waypointA = new Waypoint();
        _waypointA.clearOtherWaypoints = true;
        _waypointA.first = false;

        _waypointA.solarSystem = new TargetSystem();

        int _idA = Int32.Parse(DatabaseProvider.GetSolarSystemIDFromStationID(guiMarketBrowser.currentSellOrder.location.id_str)[0][0].ToString());

        _waypointA.solarSystem.id = _idA;
        _waypointA.solarSystem.href = "http://crest.regner.dev/solarsystems/" + _idA + "/";

        Waypoint _waypointB = new Waypoint();
        _waypointB.clearOtherWaypoints = false;
        _waypointB.first = false;

        _waypointB.solarSystem = new TargetSystem();

        int _idB = Int32.Parse(DatabaseProvider.GetSolarSystemIDFromStationID(guiMarketBrowser.currentBuyOrder.location.id_str)[0][0].ToString());

        _waypointB.solarSystem.id = _idB;
        _waypointB.solarSystem.href = "http://crest.regner.dev/solarsystems/" + _idB + "/";

        Dictionary<string, string> _headers = new Dictionary<string, string>();

        _headers.Add("Authorization", "Bearer " + accessTokenData.access_token);
        _headers.Add("Host", "login.eveonline.com");

        byte[] _dataA = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(_waypointA));
        byte[] _dataB = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(_waypointB));

        WWW _www = new WWW("https://login.eveonline.com/oauth/token", _dataA, _headers);

        while (!_www.isDone) {
            yield return null;
        }

        if (_www.error != "") {
            Debug.Log(_www.error);
            errorDetails.text = _www.error + " : " + _www.text;
        }

        _www = new WWW("https://login.eveonline.com/oauth/token", _dataB, _headers);

        while (!_www.isDone) {
            yield return null;
        }

        if (_www.error != "") {
            Debug.Log(_www.error);
            errorDetails.text = _www.error + " : " + _www.text;
        }
    }
}
