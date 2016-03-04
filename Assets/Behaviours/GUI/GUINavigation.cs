using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class GUINavigation : MonoBehaviour {

    //
    public GUIMarketBrowser guiMarketBrowser;
    public GUIGroupSearch guiGroupSearch;
    public GUIAPIStatus guiAPIStatus;
    public GUIRoutePlanner guiRoutePlanner;
    public GUIFavouritesView guiFavouritesView;

    public Image spiStatusImage;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    public void Update() {

        spiStatusImage.color = guiAPIStatus.GetAPIStatus();
    }

    /// <summary>
    /// 
    /// </summary>
    public void MarketBrowserButtonPressed() {

        DisableAllViews();

        guiMarketBrowser.gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void GroupSearchButtonPressed() {

        DisableAllViews();

        guiGroupSearch.gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void RoutePlannerButtonPressed() {

        DisableAllViews();

        guiRoutePlanner.gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void APIStatusButtonPressed() {

        DisableAllViews();

        guiAPIStatus.gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void FavouritesButtonPressed() {

        DisableAllViews();

        guiFavouritesView.gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    private void DisableAllViews() {

        guiMarketBrowser.gameObject.SetActive(false);
        guiGroupSearch.gameObject.SetActive(false);
        guiAPIStatus.gameObject.SetActive(false);
        guiRoutePlanner.gameObject.SetActive(false);
        guiFavouritesView.gameObject.SetActive(false);
    }
}
