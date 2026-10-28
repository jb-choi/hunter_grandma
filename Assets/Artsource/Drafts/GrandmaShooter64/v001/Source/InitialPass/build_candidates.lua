-- Original 64x64 pixel artwork drawn and saved inside Aseprite.
-- Rebuild only into a fresh version folder. Do not overwrite user edits.
local root = app.params['root']
assert(root and #root > 0, 'Pass --script-param root=<v001 folder>')
local pc = app.pixelColor
local P = {
  O='352c42', I='fff4dc', S='f3bf9e', D='ce8d80', B='ed8f98',
  H='eee6de', h='bdb7cb', j='8b89a4', W='fffaf0',
  R='d87b91', r='a95072', L='f0a7ac',
  G='79926e', g='465e58', K='acc18a',
  V='a693c5', v='706089', l='cfb7db',
  A='eddfbc', a='c6b38e', T='a66d50', t='6d4950',
  M='65788c', m='42485e', N='b4ced0', F='f2b65f', f='df7857'
}
local keys={'O','I','S','D','B','H','h','j','W','R','r','L','G','g','K','V','v','l','A','a','T','t','M','m','N','F','f'}
local colors={}
for k,hx in pairs(P) do
  colors[k]=pc.rgba(tonumber(hx:sub(1,2),16),tonumber(hx:sub(3,4),16),tonumber(hx:sub(5,6),16),255)
end
local im
local function px(x,y,c)
  if x>=0 and x<64 and y>=0 and y<64 then im:drawPixel(x,y,colors[c] or 0) end
end
local function rect(x1,y1,x2,y2,c)
  for y=y1,y2 do for x=x1,x2 do px(x,y,c) end end
end
local function line(x1,y1,x2,y2,c)
  local dx,dy=math.abs(x2-x1),math.abs(y2-y1)
  local sx,sy=x1<x2 and 1 or -1,y1<y2 and 1 or -1
  local err=dx-dy
  while true do
    px(x1,y1,c)
    if x1==x2 and y1==y2 then break end
    local e=err*2
    if e>-dy then err=err-dy;x1=x1+sx end
    if e<dx then err=err+dx;y1=y1+sy end
  end
end
local function inside(x,y,pts)
  local yes=false
  local j=#pts
  for i=1,#pts do
    local a,b=pts[i],pts[j]
    if ((a[2]>y) ~= (b[2]>y)) and (x<(b[1]-a[1])*(y-a[2])/(b[2]-a[2])+a[1]) then yes=not yes end
    j=i
  end
  return yes
end
local function shape(test,c,edge)
  local mask={}
  for y=0,63 do
    for x=0,63 do if test(x,y) then mask[y*64+x]=true end end
  end
  if edge then
    for y=1,62 do for x=1,62 do
      local k=y*64+x
      if not mask[k] and (mask[k-1] or mask[k+1] or mask[k-64] or mask[k+64]) then px(x,y,edge) end
    end end
  end
  for y=0,63 do for x=0,63 do if mask[y*64+x] then px(x,y,c) end end end
end
local function poly(pts,c,edge)
  shape(function(x,y) return inside(x+0.5,y+0.5,pts) end,c,edge)
end
local function oval(x1,y1,x2,y2,c,edge)
  local cx,cy=(x1+x2)/2,(y1+y2)/2
  local rx,ry=(x2-x1+1)/2,(y2-y1+1)/2
  shape(function(x,y) return ((x-cx)/rx)^2+((y-cy)/ry)^2<=1 end,c,edge)
end
local function box(x1,y1,x2,y2,c,edge)
  if edge then rect(x1-1,y1-1,x2+1,y2+1,edge) end
  rect(x1,y1,x2,y2,c)
end
local function grid(x,y,rows)
  for j,row in ipairs(rows) do for i=1,#row do
    local c=row:sub(i,i)
    if c~='.' then px(x+i-1,y+j-1,c) end
  end end
end
local spr
local function layer(name,draw)
  local l=spr:newLayer(); l.name=name
  im=Image(64,64,ColorMode.RGB)
  draw()
  spr:newCel(l,1,im,Point(0,0))
end
local function glasses(x,y,kind)
  if kind=='round' then
    grid(x,y,{'..OO.....OO..','.OIIIO.OIIIO.','OIIIOOOIIIOO','OIIOIOOIIOIO','.OIIIO.OIIIO.','..OO.....OO..'})
    -- Quiet single-pixel highlights and two-pixel pupils.
    rect(x+4,y+2,x+4,y+3,'O'); rect(x+11,y+2,x+11,y+3,'O')
  else
    grid(x,y,{'OOO.....OOO','OIIIO.OIIIO','OIIOOOOIIOO','.OIIO.OIIO.','..OO...OO..'})
    px(x+3,y+2,'O');px(x+9,y+2,'O')
  end
end
local function smile(x,y)
  px(x,y,'D');rect(x+1,y+1,x+3,y+1,'t');px(x+4,y,'D');rect(x+1,y+2,x+2,y+2,'B')
end
local function boot(x,y,base)
  poly({{x,y},{x+6,y},{x+7,y+4},{x+10,y+5},{x+10,y+7},{x-1,y+7},{x-1,y+3}},base,'O')
  rect(x,y+1,x+5,y+2,'T');rect(x,y+6,x+9,y+6,'t');px(x+7,y+4,'T')
end
local function muzzle(x,y,kind)
  if kind==1 then
    grid(x,y,{'....F...','...FF...','F.FIF.F.','.FIIIF..','FFIWIIFF','.FIIIF..','F.FIF.F.','...FF...','....F...'})
  elseif kind==2 then
    grid(x,y,{'.....F..','..F.FF..','...FIF..','FFIIWIFF','...FIF..','..F.FF..','.....F..'})
  else
    grid(x,y,{'...F....','F.FIF...','.FIIIF..','FFIWIIFF','.FIIIF..','F.FIF...','...F....'})
  end
end
local function c1()
  layer('01_Legs_Shoes',function()
    box(21,48,26,55,'S','O');rect(22,51,25,55,'A')
    box(33,48,38,55,'S','O');rect(34,51,37,55,'A')
    poly({{19,55},{26,55},{28,57},{29,59},{17,59},{17,57}},'t','O')
    rect(19,55,24,56,'T');rect(18,59,28,59,'O')
    poly({{33,55},{39,55},{43,57},{43,59},{32,59}},'t','O')
    rect(34,55,38,56,'T');rect(33,59,42,59,'O')
  end)
  layer('02_Cardigan_Apron',function()
    poly({{23,32},{32,32},{36,35},{38,43},{40,51},{17,51},{18,41},{19,36}},'R','O')
    poly({{20,36},{24,34},{27,35},{25,46},{20,46}},'L')
    poly({{34,37},{36,39},{37,47},{39,50},{34,50},{32,43}},'r')
    poly({{25,34},{31,34},{33,38},{31,43},{25,42},{23,37}},'I','r')
    poly({{24,42},{33,42},{35,50},{22,50}},'A','r')
    rect(25,43,31,44,'I');line(24,46,24,49,'a');line(32,46,33,49,'a')
    box(26,45,30,47,'I','a');px(29,45,'B')
    rect(22,51,37,51,'r');rect(24,50,32,50,'I')
    grid(26,32,{'SSSSS','DSSSD','.RRR.','RLRLR','.RRR.'})
    px(21,40,'F');px(21,44,'F')
  end)
  layer('03_Silver_Bun_Face',function()
    oval(14,9,24,19,'h','O');oval(15,10,22,16,'H');rect(17,10,20,11,'W')
    line(15,17,18,18,'j');box(20,15,23,16,'R','r')
    oval(19,12,42,32,'h','O')
    oval(23,17,42,33,'S','O')
    poly({{27,18},{38,18},{40,22},{40,28},{36,32},{29,32},{25,29},{24,23}},'I')
    oval(20,25,25,29,'S','O');px(22,27,'D')
    poly({{20,21},{21,16},{26,12},{34,12},{40,15},{42,19},{39,20},{36,17},{34,20},{30,17},{27,20},{25,19},{24,25},{22,26}},'H','j')
    line(25,14,29,13,'W');line(32,13,36,14,'W');px(23,17,'W');rect(21,22,22,24,'H')
    glasses(26,22,'round')
    rect(25,28,28,29,'B');rect(39,28,41,29,'B')
    px(43,26,'S');px(43,27,'D');smile(32,29)
    px(23,30,'F');px(23,31,'I')
  end)
  layer('04_Sleeves_Arms',function()
    poly({{21,35},{25,35},{29,39},{34,37},{36,41},{29,44},{25,42},{21,39}},'R','O')
    line(22,36,26,39,'L');line(25,41,28,42,'r')
    poly({{32,35},{36,35},{38,37},{43,36},{44,39},{37,42},{33,40}},'R','O')
    line(34,36,36,38,'L');rect(39,37,42,39,'I')
  end)
  layer('05_Wooden_Shotgun',function()
    poly({{29,35},{33,34},{38,35},{39,38},{35,39},{32,41},{30,40}},'T','O')
    line(30,36,34,36,'A');line(31,39,34,37,'t')
    box(37,34,53,36,'m','O');rect(39,34,52,34,'N');rect(41,35,52,35,'M')
    rect(38,37,47,38,'T');line(40,37,47,37,'A');rect(52,33,53,37,'O')
    box(37,37,40,40,'O');rect(38,38,39,39,'S')
    px(51,32,'M');px(52,32,'O')
  end)
  layer('06_Grip_Hands',function()
    grid(35,37,{'OSSO.','OSISO','.SSSO','..OO.'})
    grid(43,36,{'OSSO','SIIS','SDDS','.OO.'})
  end)
  layer('07_Muzzle_FX',function() muzzle(54,30,1) end)
end
local function c2()
  layer('01_Legs_Boots',function()
    poly({{22,46},{29,46},{28,54},{21,54}},'g','O');line(24,47,24,52,'G')
    poly({{32,46},{38,46},{39,54},{33,54}},'G','O');line(35,48,36,53,'K')
    boot(20,52,'t');boot(34,52,'t');rect(21,54,25,54,'F');rect(35,54,39,54,'F')
  end)
  layer('02_Hunter_Coat',function()
    poly({{24,31},{32,31},{37,35},{37,41},{40,48},{32,49},{29,46},{25,49},{19,48},{21,37}},'G','O')
    poly({{23,36},{27,34},{28,43},{23,46},{21,46}},'K')
    poly({{32,35},{35,36},{36,43},{38,47},{32,47}},'g')
    poly({{26,32},{31,32},{32,37},{29,40},{25,36}},'A','g')
    line(29,39,29,46,'g');px(30,40,'F');px(30,44,'F')
    box(20,43,37,44,'t','O');rect(27,43,30,45,'F');rect(28,44,29,44,'t')
    line(22,36,26,41,'T');line(21,37,25,42,'a')
    box(19,40,23,46,'T','t');rect(20,41,22,42,'A')
    grid(25,31,{'SSSSSS','.ffff.','ffFfff','..ff..','..f...'})
  end)
  layer('03_Ranger_Hat_Face',function()
    oval(18,17,39,32,'h','O');oval(22,19,40,33,'S','O')
    oval(24,19,38,30,'I')
    oval(19,25,24,29,'S','O');px(21,27,'D')
    grid(19,20,{'HHHh','HWHh','HHh.','Hh..','H...'})
    grid(35,19,{'HHHh','HWHh','.HHh','..Hh','...h'})
    poly({{17,19},{18,14},{21,11},{24,12},{31,10},{37,13},{39,19}},'G','O')
    poly({{20,14},{23,12},{25,13},{30,11},{34,13},{35,16},{20,16}},'K')
    rect(18,17,38,19,'t');rect(21,17,34,17,'T')
    poly({{17,19},{40,19},{44,21},{43,22},{20,22},{16,21}},'G','O');rect(20,19,39,19,'K')
    grid(15,11,{'..F','FKF','.K.','.K.','.K.','..K'})
    grid(26,24,{'OOO.....OOO','OIOO...OIOO','OIIOOOOOIIO','.OO.....OO.'})
    px(28,25,'O');px(37,25,'O')
    rect(24,28,27,29,'B');rect(38,28,40,29,'B');px(41,26,'S')
    smile(31,29);px(22,30,'F')
  end)
  layer('04_Coat_Arms',function()
    poly({{22,34},{26,35},{30,39},{35,37},{37,40},{31,44},{27,43},{23,40}},'G','O')
    line(23,35,27,39,'K');line(27,42,30,43,'g')
    poly({{32,34},{36,35},{39,38},{44,36},{45,39},{39,42},{35,41}},'G','O')
    line(34,35,37,38,'K');line(36,40,39,41,'g')
  end)
  layer('05_Brass_Rifle',function()
    poly({{28,34},{32,33},{37,34},{40,35},{38,38},{33,40},{29,39}},'T','O')
    line(29,35,33,34,'A');line(30,38,34,37,'t')
    box(37,33,54,35,'m','O');rect(38,33,53,33,'N');rect(42,34,53,34,'M')
    box(39,35,48,37,'T','t');rect(40,35,47,35,'F')
    rect(52,32,53,36,'O');px(51,31,'N');px(52,31,'O')
    box(37,36,40,39,'O');rect(38,37,39,38,'S')
    rect(37,32,40,32,'F');px(39,31,'O')
  end)
  layer('06_Grip_Hands',function()
    grid(35,36,{'OSSO.','OSIIO','.SSSO','..OO.'})
    grid(44,36,{'OSSO','SIIS','SDDS','.OO.'})
  end)
  layer('07_Muzzle_FX',function() muzzle(55,30,2) end)
end
local function c3()
  layer('01_Tights_Slippers',function()
    box(21,49,26,56,'v','O');rect(22,51,25,55,'l')
    box(33,49,38,56,'v','O');rect(34,51,37,55,'l')
    poly({{19,55},{25,55},{28,57},{29,59},{17,59},{17,57}},'R','O')
    rect(19,56,24,57,'L');px(25,58,'F')
    poly({{33,55},{39,55},{43,57},{43,59},{32,59}},'R','O')
    rect(34,56,38,57,'L');px(40,58,'F')
  end)
  layer('02_Lavender_Dress_Apron',function()
    poly({{22,32},{31,32},{36,36},{37,43},{40,51},{17,51},{19,40},{19,36}},'V','O')
    poly({{20,36},{24,33},{27,35},{25,45},{20,47}},'l')
    poly({{33,36},{36,39},{36,45},{39,50},{33,50}},'v')
    poly({{25,33},{29,33},{31,39},{23,39}},'I','v')
    poly({{24,39},{33,39},{35,50},{21,50}},'A','v')
    poly({{25,40},{31,40},{33,48},{24,48}},'I')
    line(23,41,22,47,'a');line(32,41,34,48,'a')
    box(25,44,30,47,'A','a');px(27,45,'R');rect(26,46,28,46,'R')
    grid(18,43,{'RRR','RLR','RRR','.r.'})
    grid(26,32,{'SSSS','.DD.','AIIA','.AA.'})
    line(20,51,37,51,'v');rect(23,50,33,50,'A')
  end)
  layer('03_Curly_Hair_Face',function()
    oval(18,12,41,33,'h','O')
    oval(16,18,22,26,'h','O');oval(17,25,23,31,'h','O')
    oval(20,11,27,18,'H','j');oval(27,10,34,17,'H','j');oval(34,12,41,19,'H','j')
    oval(23,18,42,33,'S','O');oval(25,19,39,31,'I')
    oval(20,25,25,29,'S','O');px(22,27,'D')
    grid(20,14,{'..WWWHHH........','HHWWWWHHH.......','HHHHHHjHHHH.....','.HHHjHHWWWHHH...','..HHHHHWWWWHHH..','..HjHHHHHHjHHHH.','..HHHjHHHHHHHHH.','..HHH..HHH..HHH.'})
    grid(18,21,{'HHHh','HWHh','.HHh','HHH.','HWH.','.Hh.','..h.'})
    glasses(27,23,'cat')
    rect(25,28,28,29,'B');rect(39,28,41,29,'B');px(43,27,'S')
    smile(32,29)
    grid(37,12,{'..R.R..','.RLRLR.','..RRR..','...F...'})
    px(23,30,'F');px(23,31,'I')
  end)
  layer('04_Sweater_Arms',function()
    poly({{22,35},{26,36},{30,40},{37,38},{39,41},{32,44},{27,44},{22,40}},'V','O')
    line(23,36,27,40,'l');line(27,43,31,43,'v')
    poly({{33,36},{37,37},{40,39},{44,37},{46,40},{40,44},{36,42}},'V','O')
    line(34,37,38,40,'l');rect(42,39,44,40,'A')
  end)
  layer('05_Compact_Pistol',function()
    poly({{39,36},{45,36},{44,39},{43,43},{39,43},{40,39},{38,39}},'T','O')
    line(41,39,40,42,'t');rect(40,40,41,40,'A')
    poly({{39,33},{50,33},{50,34},{54,34},{54,36},{46,36},{45,38},{39,38}},'M','O')
    rect(40,33,49,33,'N');rect(46,34,53,34,'N');rect(40,36,44,37,'m')
    rect(51,33,52,33,'O');px(40,32,'O');px(44,39,'F')
    box(44,38,46,40,'O');px(45,39,'S')
  end)
  layer('06_Grip_Hands',function()
    grid(36,39,{'OSSSO.','SIISSO','SDSSSO','.OOO..'})
    grid(42,38,{'OSSO','SIIS','SDSS','.OO.'})
  end)
  layer('07_Muzzle_FX',function() muzzle(55,31,3) end)
end
local names={'grandma_01_rose','grandma_02_ranger','grandma_03_lavender'}
local makers={c1,c2,c3}
app.fs.makeAllDirectories(root..'/Export')
app.fs.makeAllDirectories(root..'/QA')
for i,name in ipairs(names) do
  spr=Sprite(64,64,ColorMode.RGB)
  spr:deleteLayer(spr.layers[1])
  makers[i]()
  spr.frames[1].duration=0.1
  local tag=spr:newTag(1,1);tag.name='shoot_base';tag.color=Color{r=216,g=123,b=145}
  local pal=Palette(#keys+1);pal:setColor(0,Color{r=0,g=0,b=0,a=0})
  for k,key in ipairs(keys) do pal:setColor(k,Color{rgbaPixel=colors[key]}) end
  spr:setPalette(pal)
  spr:saveAs(root..'/'..name..'.aseprite')
  local flat=Image(spr.spec);flat:drawSprite(spr,1)
  flat:saveAs(root..'/Export/'..name..'.png')
  spr:close()
end
print('Saved three original layered 64x64 candidates.')
