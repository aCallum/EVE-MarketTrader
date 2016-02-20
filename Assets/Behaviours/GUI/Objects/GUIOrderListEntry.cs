using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using EVEMarketTrader;

/// <summary>
/// 
/// </summary>
public class GUIOrderListEntry : MonoBehaviour {

    public OrderEntry orderData;

    public Text text2;
    public Text text3;
    public Text text4;
    public Text text5;
    public Text text6;

    public void GUIOrderListEntrySelected() {

        GetComponentInParent<GUIMarketBrowser>().SetInfo(ref orderData, orderData.buy);
    }
}