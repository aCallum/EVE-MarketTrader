# EVE-MarketTrader
Entry for The EVE Online API Challenge

EVE-MarketTrader is a standalone application that helps players of EVE Online make use of inter region trade routes. The desired waypoint route can then be sent directly to the client for directions to the buy/sell locations. Currently supported feartures:

  - Bi-region market scanning
  - Buy/Sell location Waypoint plotting (CREST API currently only supports solar systems, MarketTrader will work best when stations can be set using this endpoint)
  - Item name search
  - Favourite item list search
  

MarketTrader is prototyped in Unity (why? because that's what I know) and makes use of:

  - Unity uGUI
  - SQLite
  - EVE CREST API
  - EVE Static Data Export (SDE)
  - EVE Online Image Server

Standalone Windows application. OSX planned.
Created for [The EVE Online API Challenge].

### Authenticated CREST Usage

 - **characterLocationRead**
 - **characterNavigationWrite** - Currently only supports setting the Solar System. MarketTrader will work best when stations can be set with this endpoint.

### Public CREST 

 - **market**
 - **solarsystems**
 - **regions**

### Version
  - 0.0.1 - public CREST data 
  - 0.0.3 - market filtering
  - 0.0.5 - authed CREST data
  - 0.1.0 - regions/solar systems

### Release

### Structure
Written as a non-game application.

* GUIMarketView - search for specific items in regions, sort by price
* GUIAPIStatus - shows status about currently authed pilot (and token expiry info)
* GUIRoutePlanner - 

### Documentation
Nope.

### Todos

 - Add inter-regional trade browsing
 - Add route to route planner
 - Implement favourite item search
 - OSx build
 - Market history graphs (limited by MIT license for desired software for this)
 - Market prediction graphs (limited by MIT license for desired software for this)

License
----

MIT

Copyright © 2016 Alastair Callum

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

[The EVE Online API Challenge]:<http://community.eveonline.com/news/dev-blogs/the-eve-online-api-challenge-1/>
