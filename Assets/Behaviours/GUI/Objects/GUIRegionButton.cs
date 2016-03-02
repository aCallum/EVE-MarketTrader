using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;

public class GUIRegionButton : MonoBehaviour, IPointerUpHandler {

    public string regionID;
    public string regionName;

    public Text regionLabel;

    public bool isVisible = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerUp(PointerEventData eventData) {

        isVisible = !isVisible;

        GetComponentInParent<GUIRoutePlanner>().RegionButtonPressed(regionID, isVisible);        
    }
}
