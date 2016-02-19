using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using EVEMarketTrader;

public enum PurchaseMode {

    Buy,
    Sell,
}

/// <summary>
/// 
/// </summary>
public class GUIMarketBrowser : MonoBehaviour {

    public GameObject guiPrefab;

    public Dropdown regionDropdown;

    private int _itemID;
    private int _regionID;
    private PurchaseMode _purchaseMode;

	private string _buyURL = "https://public-crest.eveonline.com/market/10000002/orders/sell/?type=https://public-crest.eveonline.com/types/34/";
    private string _sellURL = "https://public-crest.eveonline.com/market/10000002/orders/sell/?type=https://public-crest.eveonline.com/types/34/";
    
    private WWW _wwwStream;

    public RectTransform sellOrderContent;
    public RectTransform buyOrderContent;
    
    //public Text info;

    private OrderList _sellOrderList;
    private OrderList _buyOrderList;

    private List<GUIOrderListEntry> _sellEntries = new List<GUIOrderListEntry>();
    private List<GUIOrderListEntry> _buyEntries = new List<GUIOrderListEntry>();

    // Use this for early intialisation
    private void Awake() {
        
        regionDropdown.options = new List<Dropdown.OptionData>(DatabaseProvider.GetRegionNames());
        UpdateRegionName(0);
    }

	// Use this for initialization
	private void Start () {

	}
	
	// Update is called once per frame
	private void Update () {
	
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformRequest() {


        _buyURL = "https://public-crest.eveonline.com/market/" + _regionID + "/orders/" + (PurchaseMode.Buy).ToString().ToLower() + "/?type=https://public-crest.eveonline.com/types/" + _itemID + "/";
        _sellURL = "https://public-crest.eveonline.com/market/" + _regionID + "/orders/" + (PurchaseMode.Sell).ToString().ToLower() + "/?type=https://public-crest.eveonline.com/types/" + _itemID + "/";
        
        for (int i = 0; i < _sellEntries.Count; i++) {
            Destroy(_sellEntries[i].gameObject);
        }

        _sellEntries.Clear();

        for (int i = 0; i < _buyEntries.Count; i++) {
            Destroy(_buyEntries[i].gameObject);
        }

        _sellEntries.Clear();
        _buyEntries.Clear();

        _wwwStream = new WWW(_sellURL);

        while (!_wwwStream.isDone) {

            yield return null;
        }

        _sellOrderList = new OrderList();
        _sellOrderList = JsonUtility.FromJson<OrderList>(_wwwStream.text);
        
        //Debug.Log(_sellOrderList.totalCount + " orders found!");
        //info.text = _sellOrderList.totalCount + " orders found.";
        
        for (int i = 0; i < _sellOrderList.totalCount; i++) {

            GameObject _obj = Instantiate(guiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            _obj.transform.SetParent(sellOrderContent.transform, false);
            _obj.hideFlags = HideFlags.HideInHierarchy;

            GUIOrderListEntry _entry = _obj.GetComponent<GUIOrderListEntry>();
            _sellEntries.Add(_entry);

            _entry.text2.text = _sellOrderList.items[i].location.name;
            _entry.text3.text = _sellOrderList.items[i].price.ToString("F2");
            _entry.text4.text = _sellOrderList.items[i].minVolume.ToString("F0");
            _entry.text5.text = _sellOrderList.items[i].volume.ToString("F0");
            _entry.text6.text = CalculateExpirationDuration(_sellOrderList.items[i].issued, _sellOrderList.items[i].duration);
        }


        /////////////


        _wwwStream = new WWW(_buyURL);

        while (!_wwwStream.isDone) {

            yield return null;
        }

        _buyOrderList = new OrderList();
        _buyOrderList = JsonUtility.FromJson<OrderList>(_wwwStream.text);
        
        //Debug.Log(_sellOrderList.totalCount + " orders found!");
        //info.text = _sellOrderList.totalCount + " orders found.";
        
        for (int i = 0; i < _buyOrderList.totalCount; i++) {

            GameObject _obj = Instantiate(guiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            _obj.transform.SetParent(buyOrderContent.transform, false);
            _obj.hideFlags = HideFlags.HideInHierarchy;

            GUIOrderListEntry _entry = _obj.GetComponent<GUIOrderListEntry>();
            _buyEntries.Add(_entry);

            _entry.text2.text = _buyOrderList.items[i].location.name;
            _entry.text3.text = _buyOrderList.items[i].price.ToString("F2");
            _entry.text4.text = _buyOrderList.items[i].minVolume.ToString("F0");
            _entry.text5.text = _buyOrderList.items[i].volume.ToString("F0");
            _entry.text6.text = CalculateExpirationDuration(_buyOrderList.items[i].issued, _buyOrderList.items[i].duration);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SellFilterBy() {

        if (_sellOrderList.items.Count > 0) {

            _sellOrderList.items.Sort((a, b) => b.price.CompareTo(a.price));
            
            for (int i = 0; i < _sellOrderList.totalCount; i++) {

                _sellEntries[i].text2.text = _sellOrderList.items[i].location.name;
                _sellEntries[i].text3.text = _sellOrderList.items[i].price.ToString("F2");
                _sellEntries[i].text4.text = _sellOrderList.items[i].minVolume.ToString("F0");
                _sellEntries[i].text5.text = _sellOrderList.items[i].volume.ToString("F0");
                _sellEntries[i].text6.text = CalculateExpirationDuration(_sellOrderList.items[i].issued, _sellOrderList.items[i].duration);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void BuyFilterBy() {

        if (_buyOrderList.items.Count > 0) {

            _buyOrderList.items.Sort((a, b) => a.price.CompareTo(b.price));
            
            for (int i = 0; i < _buyOrderList.totalCount; i++) {

                _buyEntries[i].text2.text = _buyOrderList.items[i].location.name;
                _buyEntries[i].text3.text = _buyOrderList.items[i].price.ToString("F2");
                _buyEntries[i].text4.text = _buyOrderList.items[i].minVolume.ToString("F0");
                _buyEntries[i].text5.text = _buyOrderList.items[i].volume.ToString("F0");
                _buyEntries[i].text6.text = CalculateExpirationDuration(_buyOrderList.items[i].issued, _buyOrderList.items[i].duration);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_name"></param>
    public void UpdateItemName(string _name) {

        Debug.Log("UpdateItemName()" + _name);
        
        string _itemNameStr = _name;

        //_itemID = // cast from DB..

        _itemID = DatabaseProvider.GetItemID(_itemNameStr);

        if (_itemID == -1) {
            return;
        }

        StartCoroutine(PerformRequest());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_name"></param>
    public void UpdateRegionName(int _name) {
        
        string _regionStr = regionDropdown.options[_name].text;
        
        _regionID = DatabaseProvider.GetRegionID(_regionStr);

        if (_itemID == -1) {
            return;
        }

        StartCoroutine(PerformRequest());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_mode"></param>
    public void UpdatePurchaseMode(int _mode) {

        _purchaseMode = (PurchaseMode)_mode;

        StartCoroutine(PerformRequest());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_issuedDate"></param>
    /// <param name="_duration"></param>
    /// <returns></returns>
    private string CalculateExpirationDuration(string _issuedDate, int _duration)
    {

        DateTime _iDate = DateTime.Parse(_issuedDate);
        _iDate += TimeSpan.FromDays(_duration);

        TimeSpan _result = _iDate - DateTime.Now;


        return FormatTimeSpan(_result, false);
    }

    private static string FormatTimeSpan(TimeSpan span, bool showSign)
    {
        string sign = String.Empty;
        if (showSign && (span > TimeSpan.Zero))
            sign = "+";

        return sign + span.Days.ToString("00") + "d " +
               span.Hours.ToString("00") + "h" +
               span.Minutes.ToString("00") + "m";
    }
}
