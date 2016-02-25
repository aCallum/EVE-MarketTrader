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

        public double id;
        public string id_str;

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

    #region MarketGroups

    [System.Serializable]
    public class ItemTypes {

        public string href;
    }

    [System.Serializable]
    public class ParentGroup {

        public string href;
    }

    [System.Serializable]
    public class Group {

        public string name;
        public string href;
        public string id_str;
        public int id;
        public ItemTypes types;
        public string description;
        public ParentGroup parentGroup;
    }

    [System.Serializable]
    public class MarketGroups {

        public string totalCount_str;
        public List<Group> items;
        public int pageCount;
        public string pageCount_str;
        public int totalCount;
    }

    #endregion

    #region Market

    [System.Serializable]
    public class MarketGroup {

        public string href;
        public int id;
        public string id_str;
    }

    [System.Serializable]
    public class Icon {

        public string href;
    }

    [System.Serializable]
    public class Type {

        public string id_str;
        public string href;
        public int id;
        public string name;
        public Icon icon;
    }

    [System.Serializable]
    public class Item {

        public MarketGroup marketGroup;
        public Type type;
        public int id;
        public string id_str;
    }

    [System.Serializable]
    public class Next {

        public string href;
    }

    [System.Serializable]
    public class MarketGroupList {

        public string totalCount_str;
        public int pageCount;
        public List<Item> items;
        public Next next;
        public int totalCount;
        public string pageCount_str;
    }

    #endregion
}