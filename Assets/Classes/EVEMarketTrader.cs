using UnityEngine;
using UnityEditor;
using System.Collections;

namespace EVEMarketTrader {

    [System.Serializable]
    public class AccessToken {
        public string access_token;
        public string token_type;
        public int expires_in;
        public string refresh_token;
    }
}