# EVE-MarketTrader
Entry for The EVE Online API Challenge

EVE-MarketTrader is a standalone application that allows players of EVE Online the ability to search and filter market orders to find highly profitable trades. MarketTrader is prototyped in Unity and makes use of:

  - Unity uGUI
  - SQLite
  - EVE CREST API
  - EVE Static Data Export (SDE)
  - EVE Online Image Server

Standalone Windows application. OSX planned.
Created for [The EVE Online API Challenge].

### Version
  - 0.0.1 - public CREST data 
  - 0.0.3 - public CREST cached by ets. Less list-processing.
  - 0.0.5 - public CREST cached by ets. Less list-processing.

### Release
[v0.0.5].

### Structure
Written as a non-game application.

* GUIMarketView - search for specific items in regions, sort by price
* GUIAPIStatus - shows status about currently authed pilot (and token expiry info)
* GUIRoutePlanner - 

### Documentation
Nope.

### Authenticated CREST Usage

 - **characterLocationRead**
 - **characterNavigationWrite**

### Public CREST 

 - **market**
 - **solarsystems**

### Todos

 - Add inter-regional trade browsing
 - Complete route planner
 - Implement group search
 - OSx build

License
----

MIT

Copyright © 2016 Alastair Callum

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

[The EVE Online API Challenge]:<http://community.eveonline.com/news/dev-blogs/the-eve-online-api-challenge-1/>
