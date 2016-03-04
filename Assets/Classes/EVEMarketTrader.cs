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

    #region CharacterBasic

    [System.Serializable]
    public class CharBasic {

        public int CharacterID;
        public string CharacterName;
        public string ExpiresOn;
        public string Scopes;
        public string TokenType;
        public string CharacterOwnerHash;
        public string IntellectualProperty;        
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

    [System.Serializable]
    public class MarketBuyOrders {

        public string href;
    }

    [System.Serializable]
    public class ConstellationRef {

        public string href;
        public int id;
        public string id_str;
    }

    [System.Serializable]
    public class MarketSellOrders {

        public string href;
    }

    [System.Serializable]
    public class Region {

        public string description;
        public MarketBuyOrders marketBuyOrders;
        public List<ConstellationRef> constellations;
        public string name;
        public string id_str;
        public int id;
        public MarketSellOrders marketSellOrders;
    }

    [System.Serializable]
    public class Position {

        public double y;
        public double x;
        public double z;
    }

    [System.Serializable]
    public class RegionRef {

        public string href;
    }

    [System.Serializable]
    public class SolarSystemRef {

        public string href;
        public int id;
        public string id_str;
    }

    [System.Serializable]
    public class Constellation {

        public Position position;
        public RegionRef region;
        public List<SolarSystemRef> systems;
        public string name;
    }

    [System.Serializable]
    public class Stats {

        public string href;
    }

    [System.Serializable]
    public class Planet {

        public string href;
    }

    [System.Serializable]
    public class SolarSystem {

        public Stats stats;
        public string name;
        public double securityStatus;
        public string securityClass;
        public string href;
        public string id_str;
        public List<Planet> planets;
        public Position position;
        public ConstellationRef constellation;
        public int id;
    }

    [System.Serializable]
    public class SolarSystemLocation  {

        public string id_str;
        public string href;
        public int id;
        public string name;
    }

    [System.Serializable]
    public class CharacterLocation {

        public SolarSystemLocation solarSystem;
    }

    public class TargetSystem {
        public string href;
        public int id;
    }

    public class Waypoint {
        public TargetSystem solarSystem;
        public bool first;
        public bool clearOtherWaypoints;
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

    #region Type

    [System.Serializable]
    public class Dogma {

    }

    [System.Serializable]
    public class ItemTypeD {

        public double capacity;
        public string description;
        public string portionSize_str;
        public int iconID;
        public int portionSize;
        public string iconID_str;
        public double volume;
        public Dogma dogma;
        public double radius;
        public string id_str;
        public bool published;
        public double mass;
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