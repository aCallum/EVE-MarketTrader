using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

using EVEMarketTrader;

public class GUIOrderInfo : MonoBehaviour {

    public GUIMarketBrowser guiMarketBrowser;

    public InputField desiredVolume;
    public InputField cargoCapacity;
    public InputField salesTax;

    public Text fromInfo;
    public Text toInfo;

    public Text grossMargin;
    public Text tax;
    public Text netMargin;

    public bool autoUpdateQuantity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateMargins(ref OrderEntry _buy, ref OrderEntry _sell, int _volume) {

        if (_buy == null || _sell == null) {

            grossMargin.text = "0.00";
            tax.text = "0.00";
            netMargin.text = "0.00";

            return;
        }

#region BUY COST

        int _X = _volume;
        double _U = _sell.price;
        double _buyValue = _X * _U;        

        double _buyCost = _buyValue;

#endregion

#region SELL COST

       // int _Y = int.Parse(desiredVolume.text);
        double _V = _buy.price;

        double _sellValue = _X * _V;
        
        double _totalSellCost = (((_sellValue) - 100) - CalculateSalesTax(_sellValue));

        double _netMargin = _totalSellCost - _buyCost;

#endregion

        double _B = CalculateSalesTax(_sellValue);

        double _grossMargin = _buyCost;

        double _tax = _B;
        //double _netMargin = _totalSellCost;

        grossMargin.text = "<color=red>-" + _grossMargin.ToString("N") + "</color> ISK";
        tax.text = "Tax: " + _tax.ToString("N") + " ISK";
        netMargin.text = (_netMargin > 0) ? "<color=green>" + _netMargin.ToString("N") + "</color> ISK" : "<color=red>" + _netMargin.ToString("F2") + "</color> ISK";

        if (autoUpdateQuantity) {
            CalculateMaxQuantity();
        }
    }
    
    private double CalculateSalesTax(double _value) {
        return (float.Parse(salesTax.text) / 100) * _value;
    }

    public void UpdateAutoUpdate(bool _val) {

        autoUpdateQuantity = _val;
    }

    public void CalculateMaxQuantity() {     
        
        StartCoroutine(DoRequestItemType());
    }

    private IEnumerator DoRequestItemType() { 
    
        WWW _www = new WWW(guiMarketBrowser.currentSellOrder.type.href);

        while (!_www.isDone) {
            yield return null;  
        }

        ItemTypeD _item = new ItemTypeD();
        _item = JsonUtility.FromJson<ItemTypeD>(_www.text);

        float _currentItemVolume = (float)_item.volume;
        float _desiredVolume = Mathf.Floor(float.Parse(cargoCapacity.text) / _currentItemVolume);

        _desiredVolume = (_desiredVolume > guiMarketBrowser.currentSellOrder.volume) ? guiMarketBrowser.currentSellOrder.volume : _desiredVolume;

        desiredVolume.text = _desiredVolume.ToString();
        guiMarketBrowser.UpdateVolume(_desiredVolume.ToString());
    }
}
