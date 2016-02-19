using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace EVEMarketTrader {

    #region AccessToken

    [System.Serializable]
    public class AccessToken {
        public string access_token;
        public string token_type;
        public int expires_in;
        public string refresh_token;
    }

    #endregion

    #region Regions

    [System.Serializable]
    public class RegionListEntry {
        public string id_str;
        public string href;
        public int id;
        public string name;
    }

    [System.Serializable]
    public class RegionList {
        public string totalCount_str;
        public List<RegionListEntry> items;
        public int pageCount;
        public string pageCount_str;
        public int totalCount;
    }

    #endregion

    #region Oders

    [System.Serializable]
    public class OrderList {

        public string totalCount_str;
        public List<OrderEntry> items;
        public int pageCount;
        public string pageCount_str;
        public int totalCount;
    }

    [System.Serializable]
    public class OrderEntry {

        public string volume_str;
        public bool buy;
        public string issued;
        public double price;
        public long volumeEntered;
        public int minVolume;
        public int volume;
        public string range;
        public string href;
        public string duration_str;
        public Location location;
        public int duration;
        public string minVolume_str;
        public string volumeEntered_str;
        public ItemType type;
        public object id;
        public string id_str;
    }

    [System.Serializable]
    public class Location {

        public string id_str;
        public string href;
        public int id;
        public string name;
    }

    [System.Serializable]
    public class ItemType {

        public string id_str;
        public string href;
        public int id;
        public string name;
    }

    #endregion
}