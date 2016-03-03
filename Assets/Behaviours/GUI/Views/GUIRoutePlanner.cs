using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using EVEMarketTrader;

public class GUIRoutePlanner : MonoBehaviour {

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

    Color _testColor;
    float _pow = 400000000000000;

    public void OnEnable() {

        if (accessTokenData.IsValid()) {

            Debug.Log("Token valid");

            StartCoroutine(DoRequestCharacterLocation());

        }
    }

	// Use this for initialization
	void Start () {

        Debug.Log(_pow);

        _zoomTarget = regionMapContent;

        List<List<object>> _items = new List<List<object>>();
        _items = DatabaseProvider.GetRegions();
        
        for (int i = 0; i < _items.Count; i++) {

            GameObject _obj = Instantiate(imagePrefab, new Vector3((float)_items[i][2] / _pow, (float)_items[i][4] / _pow, (float)_items[i][3] / _pow), Quaternion.identity) as GameObject;

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

            foreach (Transform _transform in _regionButtons) {
                _transform.localScale = (Vector3.one / _zoomTarget.localScale.x);
            }

            foreach (Transform _transform in _solarSystemButtons) {
                _transform.localScale = (Vector3.one / _zoomTarget.localScale.x);
            }

            UpdatePivot();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) {

            _zoomTarget.localScale -= Vector3.one * 0.08f;

            foreach (Transform _transform in _regionButtons) {
                _transform.localScale = (Vector3.one / _zoomTarget.localScale.x);
            }

            foreach (Transform _transform in _solarSystemButtons) {
                _transform.localScale = (Vector3.one / _zoomTarget.localScale.x);
            }

            if (_zoomTarget.localScale.x <= 0.5f) {
                _zoomTarget.localScale = Vector3.one * 0.5f;
            }

            UpdatePivot();
        } 

        
	}

    private void UpdatePivot() {

        Debug.Log(((RectTransform)_zoomTarget).anchoredPosition);

        //((RectTransform)_zoomTarget).pivot = new Vector2(Mathf.InverseLerp(0, 2500, _zoomTarget.position.x), Mathf.InverseLerp(0, 2500, _zoomTarget.position.y));

    }

    //public void UpdatePivot() { 
    
    //}

    public void RegionButtonPressed(string _id, bool _show) {     

        // Query all solar systems in _id

        if (_show) {
            StartCoroutine(DoRequestConstellations(_id));
        }
        else { 
            
        }
        

    }
        
    private IEnumerator DoRequestConstellations(string _regionID) {

        

        WWW _www = new WWW("https://public-crest.eveonline.com/regions/" + _regionID + "/");

        while (!_www.isDone) {
            yield return null;
        }

        Region _region = new Region();
        _region = JsonUtility.FromJson<Region>(_www.text);
        
        for (int i = 0; i < _region.constellations.Count; i++) {

            List<List<object>> _items = new List<List<object>>();
            _items = DatabaseProvider.GetSolarSystems(_region.constellations[i].id_str);

            _testColor = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 1, 1);

            for (int j = 0; j < _items.Count; j++) {

                GameObject _obj = Instantiate(imagePrefab, new Vector3((float)_items[j][4] / _pow, (float)_items[j][6] / _pow, (float)_items[j][5] / _pow), Quaternion.identity) as GameObject;

                _obj.hideFlags = HideFlags.HideInHierarchy;

                _obj.transform.localScale = (Vector3.one / _zoomTarget.localScale.x);
                _obj.transform.SetParent(regionMapContent.transform, false);
                _regionButtons.Add(_obj.transform);

                GUIRegionButton _regionButton = _obj.GetComponentInChildren<GUIRegionButton>();
                _obj.GetComponent<Image>().color = _testColor;
                //_regionButton.regionID = _items[i][0].ToString();
                //_regionButton.regionName = _items[i][1].ToString();
                _regionButton.regionLabel.text = _items[j][1].ToString();

            }

            //_www = new WWW(_region.constellations[i].href);

            //while (!_www.isDone) {
            //    yield return null;
            //}

            //Constellation _constellation = new Constellation();
            //_constellation = JsonUtility.FromJson<Constellation>(_www.text);
            
            //for (int j = 0; j < _constellation.systems.Count; j++) {

                //Debug.Log(_constellation.systems[j].id);

                //_www = new WWW(_constellation.systems[j].href);

                //while (!_www.isDone) {
                //    yield return null;
                //}

                //SolarSystem _solarSystem = new SolarSystem();
                //_solarSystem = JsonUtility.FromJson<SolarSystem>(_www.text);

                //GameObject _obj = Instantiate(imagePrefab, new Vector3((float)_solarSystem.position.x / 200000000000000, (float)_solarSystem.position.z / 200000000000000, (float)_solarSystem.position.y / 200000000000000), Quaternion.identity) as GameObject;
                //_obj.transform.localScale = (Vector3.one / _zoomTarget.localScale.x);

                //_solarSystemButtons.Add(_obj.transform);

                //_obj.transform.SetParent(constellationMapContent.transform, false);
                //_obj.GetComponent<Image>().color = _testColor;
                //GUIRegionButton _regionButton = _obj.GetComponentInChildren<GUIRegionButton>();
                //_regionButton.regionLabel.text = _solarSystem.name;



           // }
        }
    }

    private IEnumerator DoRequestCharacterLocation() {

        Dictionary<string, string> _headers = new Dictionary<string, string>();
        
        _headers.Add("Authorization", "Bearer " + accessTokenData.access_token);
        _headers.Add("Content-Type", "application/x-www-form-urlencoded");
        _headers.Add("Host", "login.eveonline.com");

        //_data = System.Text.Encoding.UTF8.GetBytes("");

        Debug.Log(PlayerPrefs.GetInt("character_id"));

        WWW _www = new WWW("https://crest-tq.eveonline.com/characters/" + PlayerPrefs.GetInt("character_id") + "/location/", null, _headers);

        while (!_www.isDone) {
            yield return null;
        }
        
        if (_www.text != "" && _www.text != "{}") {

            CharacterLocation _location = new CharacterLocation();
            _location = JsonUtility.FromJson<CharacterLocation>(_www.text);

            Debug.Log("Pilot is in: " + _location.solarSystem.name);

            int _regionID = Int32.Parse(DatabaseProvider.GetRegionIDFromSolarSystem(_location.solarSystem.name)[0][0].ToString());
            //string _regionName = DatabaseProvider.GetRegionFromSolarSystem(_location.solarSystem.name)[0][1].ToString();

            //Debug.Log("Pilot is in: " + _regionID);

            foreach (Transform _button in _regionButtons) {

                if (_button.GetComponent<GUIRegionButton>().regionID == _regionID.ToString()) {

                    _button.GetComponent<Image>().color = Color.cyan;
                }
            }
        }
    } 
}
