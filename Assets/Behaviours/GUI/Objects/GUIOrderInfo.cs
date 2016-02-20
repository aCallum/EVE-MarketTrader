using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

using EVEMarketTrader;

public class GUIOrderInfo : MonoBehaviour {

    public InputField desiredVolume;
    public InputField salesTax;

    public Text fromInfo;
    public Text toInfo;

    public Text grossMargin;
    public Text tax;
    public Text netMargin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateMargins(ref OrderEntry _buy, ref OrderEntry _sell) {

        if (_buy == null || _sell == null) {

            grossMargin.text = "0.00";
            tax.text = "0.00";
            netMargin.text = "0.00";

            return;
        }

#region BUY COST

        int _X = int.Parse(desiredVolume.text);
        double _U = _sell.price;

        double _buyValue = _X * _U;
        

        double _buyCost = (((_buyValue)) + CalculateSalesTax(_buyValue));

#endregion

#region SELL COST

       // int _Y = int.Parse(desiredVolume.text);
        double _V = _buy.price;

        double _sellValue = _X * _V;
        
        double _totalSellCost = (((_sellValue) - 100) - CalculateSalesTax(_sellValue));

        double _netMargin = _totalSellCost - _buyCost;

#endregion

        double _B = CalculateSalesTax(_buyValue) + CalculateSalesTax(_sellValue);

        double _grossMargin = _buyCost;

        double _tax = _B;
        //double _netMargin = _totalSellCost;

        grossMargin.text = _grossMargin.ToString("F2");
        tax.text = "Total Tax: " + _tax.ToString("F2");
        netMargin.text = _netMargin.ToString("F2");
    }
    
    private double CalculateSalesTax(double _value) {
        return 0.015f * _value;
    }
}
