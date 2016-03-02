using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using EVEMarketTrader;

/// <summary>
/// Shows all groups available in the market
/// </summary>
public class GUIGroupSearch : MonoBehaviour {

    //https://public-crest.eveonline.com/market/groups/

    public Transform content;
    public GameObject info;

    private WWW _www;
    private MarketGroups groups = new MarketGroups();

	// Use this for initialization
	void Start () {
	
        _www = new WWW("https://public-crest.eveonline.com/market/groups/");

        //StartCoroutine(DoRequest());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator DoRequest() { 
    
        while (!_www.isDone) {


            yield return null;
        }

        groups = JsonUtility.FromJson<MarketGroups>(_www.text);

        //info.text += groups.items[16].name;

        for (int i = 0; i < groups.totalCount; i++) {

            GameObject _obj = Instantiate(info, Vector3.zero, Quaternion.identity) as GameObject;
            _obj.hideFlags = HideFlags.HideInHierarchy;
            _obj.transform.SetParent(content.transform, false);

            _obj.GetComponentInChildren<Text>().text = groups.items[i].types.href;
        }

        //_www = new WWW(groups.items[16].types.href);

        //while (!_www.isDone)
        //{


        //    yield return null;
        //}

        //MarketGroupList groupList = JsonUtility.FromJson<MarketGroupList>(_www.text);

        //info.text += groupList.totalCount + " " + groupList.items[0].type.name;
    }
}
