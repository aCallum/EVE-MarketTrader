using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Globalization;
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

    public GUIOrderInfo guiOrderInfo;

    public GameObject guiPrefab;

    public Dropdown regionDropdownA;
    public Dropdown regionDropdownB;

    public InputField itemInputField;

    private int _itemID;
    private int _regionIDA;
    private int _regionIDB;


    public int RegionIDA { get { return _regionIDA; } }
    public int RegionIDB { get { return _regionIDB; } }

    private PurchaseMode _purchaseMode;

	private string _buyURL = "";
    private string _sellURL = "";
    
    //private WWW _wwwStream;

    public RectTransform sellOrderContent;
    public RectTransform buyOrderContent;
    
    //public Text info;

    private OrderList _sellOrderList;
    private OrderList _buyOrderList;

    private List<GUIOrderListEntry> _sellEntries = new List<GUIOrderListEntry>();
    private List<GUIOrderListEntry> _buyEntries = new List<GUIOrderListEntry>();

    public OrderEntry currentSellOrder;
    public OrderEntry currentBuyOrder;

    private int _purchaseVolume = 1;

    // Use this for early intialisation
    private void Awake() {
        
        regionDropdownA.options = DatabaseProvider.GetRegionNames();
        regionDropdownB.options = regionDropdownA.options;

        UpdateBuyRegionName(1);
        UpdateSellRegionName(1);
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
    private IEnumerator DoRequestSellOrders() {

        // SELL ORDERS

        for (int i = 0; i < _sellEntries.Count; i++) {
            Destroy(_sellEntries[i].gameObject);
        }

        _sellEntries.Clear();

        _sellURL = "https://public-crest.eveonline.com/market/" + _regionIDA + "/orders/" + (PurchaseMode.Sell).ToString().ToLower() + "/?type=https://public-crest.eveonline.com/types/" + _itemID + "/";
        WWW _wwwStream = new WWW(_sellURL);

        while (!_wwwStream.isDone) {
            yield return null;
        }
        
        _sellOrderList = new OrderList();
        _sellOrderList = JsonUtility.FromJson<OrderList>(_wwwStream.text);

        for (int i = 0; i < _sellOrderList.totalCount; i++) {

            GameObject _obj = Instantiate(guiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            _obj.transform.SetParent(sellOrderContent.transform, false);
            _obj.hideFlags = HideFlags.HideInHierarchy;

            GUIOrderListEntry _entry = _obj.GetComponent<GUIOrderListEntry>();
            _sellEntries.Add(_entry);
        }
        
        SellFilterBy();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoRequestBuyOrders() {

        // BUY ORDERS

        for (int i = 0; i < _buyEntries.Count; i++) {
            Destroy(_buyEntries[i].gameObject);
        }

        _buyEntries.Clear();

        _buyURL = "https://public-crest.eveonline.com/market/" + _regionIDB + "/orders/" + (PurchaseMode.Buy).ToString().ToLower() + "/?type=https://public-crest.eveonline.com/types/" + _itemID + "/";        
        WWW _wwwStream = new WWW(_buyURL);

        while (!_wwwStream.isDone) {
            yield return null;
        }

        _buyOrderList = new OrderList();
        _buyOrderList = JsonUtility.FromJson<OrderList>(_wwwStream.text);
                
        for (int i = 0; i < _buyOrderList.totalCount; i++) {

            GameObject _obj = Instantiate(guiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            _obj.transform.SetParent(buyOrderContent.transform, false);
            _obj.hideFlags = HideFlags.HideInHierarchy;

            GUIOrderListEntry _entry = _obj.GetComponent<GUIOrderListEntry>();
            _buyEntries.Add(_entry);
        }

        BuyFilterBy();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SellFilterBy() {

        if (_sellOrderList.items.Count > 0) {

            _sellOrderList.items.Sort((a, b) => b.price.CompareTo(a.price));

            for (int i = 0; i < _sellOrderList.totalCount; i++) {

                _sellEntries[i].orderData = _sellOrderList.items[i];

                _sellEntries[i].text2.text = _sellEntries[i].orderData.location.name;
                _sellEntries[i].text3.text = _sellEntries[i].orderData.price.ToString("F2");
                _sellEntries[i].text4.text = _sellEntries[i].orderData.minVolume.ToString("F0");
                _sellEntries[i].text5.text = _sellEntries[i].orderData.volume.ToString("F0");
                _sellEntries[i].text6.text = CalculateExpirationDuration(_sellEntries[i].orderData.issued, _sellEntries[i].orderData.duration);
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

                _buyEntries[i].orderData = _buyOrderList.items[i];

                _buyEntries[i].text2.text = _buyEntries[i].orderData.location.name;
                _buyEntries[i].text3.text = _buyEntries[i].orderData.price.ToString("F2");
                _buyEntries[i].text4.text = _buyEntries[i].orderData.minVolume.ToString("F0");
                _buyEntries[i].text5.text = _buyEntries[i].orderData.volume.ToString("F0");
                _buyEntries[i].text6.text = CalculateExpirationDuration(_buyEntries[i].orderData.issued, _buyEntries[i].orderData.duration);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_name"></param>
    public void UpdateItemName(string _name) {
                
        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
        string _itemNameStr = myTI.ToTitleCase(_name);
        itemInputField.text = _itemNameStr;

        //_itemID = // cast from DB..

        _itemID = DatabaseProvider.GetItemID(_itemNameStr);

        if (_itemID == -1) {
            itemInputField.text = "<color=red>" + itemInputField.text + "</color>";
            return;
        }

        StartCoroutine(DoRequestSellOrders());
        StartCoroutine(DoRequestBuyOrders());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_name"></param>
    public void UpdateBuyRegionName(int _name) {
        
        string _regionStr = regionDropdownB.options[_name].text;
        
        _regionIDB = DatabaseProvider.GetRegionID(_regionStr);

        if (_itemID == -1) {
            return;
        }

        StartCoroutine(DoRequestBuyOrders());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_name"></param>
    public void UpdateSellRegionName(int _name) {
        
        string _regionStr = regionDropdownA.options[_name].text;
        
        _regionIDA = DatabaseProvider.GetRegionID(_regionStr);

        if (_itemID == -1) {
            return;
        }

        StartCoroutine(DoRequestSellOrders());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_order"></param>
    /// <param name="_buy"></param>
    public void SetInfo(ref OrderEntry _order, bool _buy) {

        switch (_buy) {

            case true: {

                currentBuyOrder = _order;

                // Set the buy order info
                guiOrderInfo.toInfo.text = _order.location.name;
                break;
            }

            case false: {

                currentSellOrder = _order;

                // Set the sell order info
                guiOrderInfo.fromInfo.text = _order.location.name;
                break;
            }

            default: {
                break;
            }
        }

        guiOrderInfo.UpdateMargins(ref currentBuyOrder, ref currentSellOrder, _purchaseVolume);
    }

    public void UpdateVolume(string _newVol) {

        _purchaseVolume = Int32.Parse(_newVol);

        //guiOrderInfo.UpdateMargins(ref currentBuyOrder, ref currentSellOrder, _purchaseVolume);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_issuedDate"></param>
    /// <param name="_duration"></param>
    /// <returns></returns>
    private string CalculateExpirationDuration(string _issuedDate, int _duration) {

        DateTime _iDate = DateTime.Parse(_issuedDate);
        _iDate += TimeSpan.FromDays(_duration);

        TimeSpan _result = _iDate - DateTime.Now;


        return FormatTimeSpan(_result, false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="span"></param>
    /// <param name="showSign"></param>
    /// <returns></returns>
    private static string FormatTimeSpan(TimeSpan span, bool showSign) {
        string sign = String.Empty;
        if (showSign && (span > TimeSpan.Zero))
            sign = "+";

        return sign + span.Days.ToString("00") + "d " +
               span.Hours.ToString("00") + "h" +
               span.Minutes.ToString("00") + "m";
    }

    public void SwapRegionButtonPressed() {

        int _currentSellRegion = regionDropdownA.value;
        int _currentBuyRegion = regionDropdownB.value;

        regionDropdownB.value = _currentSellRegion;
        regionDropdownA.value = _currentBuyRegion;

        //UpdateSellRegionName(_currentBuyRegion);
        //UpdateBuyRegionName(_currentSellRegion);

        //regionDropdownB.value = _currentSellRegion;
        //regionDropdownA.value = _currentBuyRegion;
    }

    public void AddToFavs() {

        if (itemInputField.text != "") {

            Debug.Log(itemInputField.text);
        }
    }
}
