<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.4" tiledversion="1.4.3" name="water" tilewidth="16" tileheight="16" tilecount="2805" columns="51">
 <image source="water.png" width="816" height="880"/>
 <terraintypes>
  <terrain name="River" tile="460"/>
 </terraintypes>
 <tile id="206">
  <animation>
   <frame tileid="206" duration="300"/>
   <frame tileid="209" duration="300"/>
   <frame tileid="212" duration="300"/>
   <frame tileid="209" duration="300"/>
  </animation>
 </tile>
 <tile id="359" terrain=",0,0,0">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="4" height="6"/>
  </objectgroup>
  <animation>
   <frame tileid="359" duration="300"/>
   <frame tileid="362" duration="300"/>
   <frame tileid="365" duration="300"/>
   <frame tileid="362" duration="300"/>
  </animation>
 </tile>
 <tile id="360" terrain="0,,0,0">
  <objectgroup draworder="index" id="2">
   <object id="1" x="12" y="0" width="4" height="6"/>
  </objectgroup>
  <animation>
   <frame tileid="360" duration="300"/>
   <frame tileid="363" duration="300"/>
   <frame tileid="366" duration="300"/>
   <frame tileid="363" duration="300"/>
  </animation>
 </tile>
 <tile id="362">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="4" height="6"/>
  </objectgroup>
 </tile>
 <tile id="363">
  <objectgroup draworder="index" id="2">
   <object id="1" x="12" y="0" width="4" height="6"/>
  </objectgroup>
 </tile>
 <tile id="365">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="4" height="6"/>
  </objectgroup>
 </tile>
 <tile id="366">
  <objectgroup draworder="index" id="2">
   <object id="1" x="12" y="0" width="4" height="6"/>
  </objectgroup>
 </tile>
 <tile id="410" terrain="0,0,,0">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="12" width="4" height="4"/>
  </objectgroup>
  <animation>
   <frame tileid="410" duration="300"/>
   <frame tileid="413" duration="300"/>
   <frame tileid="416" duration="300"/>
   <frame tileid="413" duration="300"/>
  </animation>
 </tile>
 <tile id="411" terrain="0,0,0,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="12" y="12" width="4" height="4"/>
  </objectgroup>
  <animation>
   <frame tileid="411" duration="300"/>
   <frame tileid="414" duration="300"/>
   <frame tileid="417" duration="300"/>
   <frame tileid="414" duration="300"/>
  </animation>
 </tile>
 <tile id="413">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="12" width="4" height="4"/>
  </objectgroup>
 </tile>
 <tile id="414">
  <objectgroup draworder="index" id="2">
   <object id="1" x="12" y="12" width="4" height="4"/>
  </objectgroup>
 </tile>
 <tile id="416">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="12" width="4" height="4"/>
  </objectgroup>
 </tile>
 <tile id="417">
  <objectgroup draworder="index" id="2">
   <object id="1" x="12" y="12" width="4" height="4"/>
  </objectgroup>
 </tile>
 <tile id="460" terrain=",,,0">
  <objectgroup draworder="index" id="2">
   <object id="1" x="4" y="16">
    <polygon points="0,0 -2,0 -2,-2 -1,-2 -1,-4 0,-4 0,-9 1,-9 1,-10 3,-10 3,-14 12,-14 12,0"/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="460" duration="300"/>
   <frame tileid="463" duration="300"/>
   <frame tileid="466" duration="300"/>
   <frame tileid="463" duration="300"/>
  </animation>
 </tile>
 <tile id="461" terrain=",,0,0">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="2" width="16" height="14"/>
  </objectgroup>
  <animation>
   <frame tileid="461" duration="300"/>
   <frame tileid="464" duration="300"/>
   <frame tileid="467" duration="300"/>
   <frame tileid="464" duration="300"/>
  </animation>
 </tile>
 <tile id="462" terrain=",,0,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="2">
    <polygon points="0,0 5,0 5,1 6,1 6,2 9,2 9,4 11,4 11,5 12,5 12,6 13,6 13,12 14,12 14,14 0,14"/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="462" duration="300"/>
   <frame tileid="465" duration="300"/>
   <frame tileid="468" duration="300"/>
   <frame tileid="465" duration="300"/>
  </animation>
 </tile>
 <tile id="463">
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="16">
    <polygon points="0,0 0,-2 1,-2 1,-4 2,-4 2,-9 3,-9 3,-10 5,-10 5,-14 14,-14 14,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="464">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="2" width="16" height="14"/>
  </objectgroup>
 </tile>
 <tile id="465">
  <objectgroup draworder="index" id="2">
   <object id="2" x="0" y="2">
    <polygon points="0,0 5,0 5,1 6,1 6,2 9,2 9,4 11,4 11,5 12,5 12,6 13,6 13,12 14,12 14,14 0,14"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="466">
  <objectgroup draworder="index" id="2">
   <object id="2" x="2" y="16">
    <polygon points="0,0 0,-2 1,-2 1,-4 2,-4 2,-9 3,-9 3,-10 5,-10 5,-14 14,-14 14,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="467">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="2" width="16" height="14"/>
  </objectgroup>
 </tile>
 <tile id="468">
  <objectgroup draworder="index" id="2">
   <object id="2" x="0" y="2">
    <polygon points="0,0 5,0 5,1 6,1 6,2 9,2 9,4 11,4 11,5 12,5 12,6 13,6 13,12 14,12 14,14 0,14"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="511" terrain=",0,,0">
  <objectgroup draworder="index" id="2">
   <object id="2" x="2" y="0" width="14" height="16"/>
  </objectgroup>
  <animation>
   <frame tileid="511" duration="300"/>
   <frame tileid="514" duration="300"/>
   <frame tileid="517" duration="300"/>
   <frame tileid="514" duration="300"/>
  </animation>
 </tile>
 <tile id="512" terrain="0,0,0,0">
  <animation>
   <frame tileid="512" duration="300"/>
   <frame tileid="515" duration="300"/>
   <frame tileid="518" duration="300"/>
   <frame tileid="515" duration="300"/>
  </animation>
 </tile>
 <tile id="513" terrain="0,,0,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="14" height="16"/>
  </objectgroup>
  <animation>
   <frame tileid="513" duration="300"/>
   <frame tileid="516" duration="300"/>
   <frame tileid="519" duration="300"/>
   <frame tileid="516" duration="300"/>
  </animation>
 </tile>
 <tile id="514">
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="0" width="14" height="16"/>
  </objectgroup>
 </tile>
 <tile id="516">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="14" height="16"/>
  </objectgroup>
 </tile>
 <tile id="517">
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="0" width="14" height="16"/>
  </objectgroup>
 </tile>
 <tile id="519">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="14" height="16"/>
  </objectgroup>
 </tile>
 <tile id="562" terrain=",0,,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="0">
    <polygon points="0,0 0,3 1,3 1,10 4,10 4,13 14,13 14,0"/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="562" duration="300"/>
   <frame tileid="565" duration="300"/>
   <frame tileid="568" duration="300"/>
   <frame tileid="565" duration="300"/>
  </animation>
 </tile>
 <tile id="563" terrain="0,0,,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="14">
    <polygon points="0,0 16,0 16,-14 0,-14"/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="563" duration="300"/>
   <frame tileid="566" duration="300"/>
   <frame tileid="569" duration="300"/>
   <frame tileid="566" duration="300"/>
  </animation>
 </tile>
 <tile id="564" terrain="0,,,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="14">
    <polygon points="0,0 10,0 10,-4 13,-4 13,-6 14,-6 14,-14 0,-14"/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="564" duration="300"/>
   <frame tileid="567" duration="300"/>
   <frame tileid="570" duration="300"/>
   <frame tileid="567" duration="300"/>
  </animation>
 </tile>
 <tile id="565">
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="0">
    <polygon points="0,0 0,3 1,3 1,10 4,10 4,13 14,13 14,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="566">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="14"/>
  </objectgroup>
 </tile>
 <tile id="567">
  <objectgroup draworder="index" id="2">
   <object id="2" x="0" y="14">
    <polygon points="0,0 10,0 10,-4 13,-4 13,-6 14,-6 14,-14 0,-14"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="568">
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="0">
    <polygon points="0,0 0,3 1,3 1,10 4,10 4,13 14,13 14,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="569">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="14"/>
  </objectgroup>
 </tile>
 <tile id="570">
  <objectgroup draworder="index" id="2">
   <object id="2" x="0" y="14">
    <polygon points="0,0 10,0 10,-4 13,-4 13,-6 14,-6 14,-14 0,-14"/>
   </object>
  </objectgroup>
 </tile>
</tileset>
