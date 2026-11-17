-- Native 64px role edits. Reuse original character pixels; redraw gear and poses.
local root=assert(app.params['root']);local pc=app.pixelColor
local C={o='281e32',d='47243e',k='6d2949',r='a1455e',p='d66b6e',R='ee403d',g='4a5034',G='7b874f',L='a9b16e',v='153d49',c='2e6c77',u='5b9d9c',a='ae615b',m='d98b77',s='f4bf9f',i='ffe1b4',w='fff5d7',h='d7ccc8',H='aa9ba9',j='7d6c86',J='57475f',n='424a61',N='7c8d9e',t='744632',b='a7663a',B='d3934d',y='efb34a',Y='ffdc78',q='653979',Q='985d9d',z='30334d',Z='5f627a'}
local ink={};for k,h in pairs(C) do ink[k]=pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255) end
local function blank() return Image(64,64,ColorMode.RGB) end
local im
local function px(x,y,c) if x>=0 and x<64 and y>=0 and y<64 then im:drawPixel(x,y,ink[c]) end end
local function rect(x1,y1,x2,y2,c) for y=y1,y2 do for x=x1,x2 do px(x,y,c) end end end
local function line(x1,y1,x2,y2,c)
 local dx,dy=math.abs(x2-x1),math.abs(y2-y1);local sx,sy=x1<x2 and 1 or -1,y1<y2 and 1 or -1;local er=dx-dy
 while true do px(x1,y1,c);if x1==x2 and y1==y2 then break end;local e=er*2;if e>-dy then er=er-dy;x1=x1+sx end;if e<dx then er=er+dx;y1=y1+sy end end
end
local function poly(points,c)
 local lo,hi=63,0;for _,p in ipairs(points) do lo=math.min(lo,p[2]);hi=math.max(hi,p[2]) end
 for y=lo,hi do local xs={};for i,p in ipairs(points) do local q=points[i%#points+1]
  if(p[2]<=y and q[2]>y)or(q[2]<=y and p[2]>y)then xs[#xs+1]=p[1]+(y-p[2])*(q[1]-p[1])/(q[2]-p[2]) end
 end;table.sort(xs);for i=1,#xs-1,2 do for x=math.ceil(xs[i]),math.floor(xs[i+1]) do px(x,y,c) end end end
 for i,p in ipairs(points) do local q=points[i%#points+1];line(p[1],p[2],q[1],q[2],c) end
end
local function ellipse(cx,cy,rx,ry,c)
 for y=cy-ry,cy+ry do for x=cx-rx,cx+rx do if ((x-cx)/rx)^2+((y-cy)/ry)^2<=1.05 then px(x,y,c) end end end
end
local function grid(x,y,rows)
 for yy,row in ipairs(rows) do for xx=1,#row do local c=row:sub(xx,xx);if c~='.' then px(x+xx-1,y+yy-1,c) end end end
end
local function copy(src,dst,fn) for y=0,63 do for x=0,63 do if fn(x,y) then dst:drawPixel(x,y,src:getPixel(x,y)) end end end end
local function clear(dst,x1,y1,x2,y2) for y=y1,y2 do for x=x1,x2 do dst:drawPixel(x,y,0) end end end
local specs={
 {n=5,old='grandma_05_redhood',name='grandma_05_flamethrower',role='flamethrower',title='화방 할매',muzzle={49,26},grips={{32,28},{43,28}}},
 {n=6,old='grandma_06_teatime',name='grandma_06_machinegun',role='machinegun',title='기관총 할매',muzzle={55,27},grips={{32,31},{46,30}}},
 {n=7,old='grandma_07_mosswitch',name='grandma_07_sniper',role='sniper',title='저격수 할매',muzzle={59,29},grips={{34,33},{46,32}}},
 {n=8,old='grandma_08_wildrose',name='grandma_08_motorcycle',role='motorcycle_transport',title='오토바이 운송 할매',grips={{44,32},{46,32}},wheel_centers={{13,53},{49,53}}}
}
local all={}
for _,spec in ipairs(specs) do
 local dest=root..'/'..spec.name..'.aseprite'
 assert(not app.fs.isFile(dest) or app.params['revise']=='true','Existing role native: preserve before revision')
 local src=Sprite{fromFile=root..'/'..spec.old..'.aseprite'};src.layers[4].isVisible=false
 local orig=blank();orig:drawSprite(src,1)
 local palette=Palette(#src.palettes[1]);for i=0,#palette-1 do palette:setColor(i,src.palettes[1]:getColor(i)) end
 local rear,body,head,far,gear,near,hands,fx=blank(),blank(),blank(),blank(),blank(),blank(),blank(),blank()
 if spec.n==5 then
  -- Red hood, silver braid and cloak retained. Entire new tank/hose/nozzle assembly.
  copy(orig,head,function(x,y)return y<=20 and not(x>=36 and y>=17)end)
  copy(orig,body,function(x,y)return y>=21 end)
  clear(body,20,21,44,29);clear(body,36,21,63,23)
  im=body
  poly({{20,22},{28,21},{33,25},{34,31},{29,36},{22,35},{18,29}},'o')
  poly({{21,23},{27,22},{31,25},{32,30},{28,34},{23,33},{20,28}},'v')
  line(25,25,25,33,'t');rect(26,29,29,30,'b');px(27,29,'y')
  -- Fuel cylinders visible behind the left shoulder, rounded with metal straps.
  im=rear
  poly({{4,20},{7,19},{10,20},{11,23},{11,36},{9,39},{4,38},{2,35},{2,23}},'o')
  rect(4,22,9,35,'r');rect(4,22,5,34,'p');rect(8,23,9,35,'k')
  rect(3,25,10,27,'n');line(4,25,9,25,'N');rect(3,34,10,35,'n')
  rect(5,18,8,20,'o');rect(6,18,7,19,'y')
  -- Far sleeve and supporting wrist reach under the nozzle.
  im=far
  poly({{29,25},{33,25},{38,29},{42,27},{45,28},{44,32},{39,34},{35,33},{30,30}},'o')
  poly({{30,26},{33,26},{38,31},{42,29},{43,30},{41,32},{38,32},{34,30}},'r')
  line(32,27,36,30,'p');line(38,32,41,31,'k')
  im=gear
  -- Hose follows a deliberate dark arc rather than a straight floating segment.
  for d=0,2 do line(6+d,35,10+d,40,'o');line(10+d,40,20+d,40,'o');line(20+d,40,30+d,30,'o') end
  line(7,35,11,39,'n');line(11,39,20,39,'n');line(20,39,31,29,'n')
  grid(27,22,{
   '..ooooooo...............',
   '.onnNNnnooooooooooo.....',
   'otnnNNnnnnnzzNNNnnoooo..',
   'otnnnnnnnnnzznnnnnnyyo..',
   'otttnnoooonzznnnnnnyyo..',
   '.ottooo..oooonnnnnoooo..',
   '..otoo.....oooooooo.....',
   '..otto..................',
   '...oo...................'
  })
  rect(45,24,46,26,'b');line(47,24,47,27,'y');px(33,24,'w')
  im=near
  poly({{21,23},{23,25},{24,28},{28,27},{31,26},{34,28},{32,32},{28,33},{23,32},{19,28},{18,25}},'o')
  poly({{21,24},{22,26},{24,30},{28,29},{31,28},{32,29},{30,31},{26,31},{23,30},{20,27}},'r')
  line(20,25,23,29,'p');line(26,31,29,31,'k')
  im=hands
  grid(30,26,{'amm.','mssi','msmm','.aa.'});grid(41,27,{'.mmm','miss','mssm','.aa.'})
  im=fx
  grid(49,19,{
   '..........R..',
   '.........Rr..',
   '......RRRry..',
   '.....RryyR...',
   '...RRryYYyRR.',
   '..RryyYYwYyrR',
   'RryYwYYwYYyr.',
   'yYwwwwwwYyrR.',
   '.ryYYwYYyyyrR',
   '..RryYYyyRr..',
   '....RryyR.R..',
   '.....RrRyR...',
   '.......Rr....',
   '........R....'
  })
 elseif spec.n==6 then
  -- Teal cardigan, topknot and floral apron stay recognizable.
  copy(orig,head,function(x,y)return y<=24 and not(x>=33 and y>=18) and not(x>=31 and y>=20)end)
  copy(orig,body,function(x,y)return y>=25 end);clear(body,22,25,43,32)
  im=body
  poly({{18,25},{25,25},{30,29},{31,35},{27,39},{19,37},{16,31}},'o')
  poly({{19,26},{24,26},{28,29},{29,35},{26,37},{20,35},{17,31}},'c')
  line(19,28,20,33,'u');line(26,30,27,35,'v')
  -- Apron bib behind the arms/weapon.
  poly({{24,28},{27,29},{29,35},{26,38},{23,36}},'i');line(25,30,26,35,'w')
  im=rear
  poly({{11,31},{16,32},{18,39},{13,42},{9,39}},'o');rect(11,33,15,38,'g');line(11,33,14,33,'G')
  rect(11,35,15,36,'t');px(13,35,'y')
  im=far
  poly({{26,27},{30,28},{35,32},{43,30},{47,29},{48,32},{43,35},{35,36},{30,33}},'o')
  poly({{28,28},{30,29},{35,34},{42,32},{46,30},{46,32},{41,34},{35,34},{31,32}},'c')
  line(31,31,35,33,'u')
  im=gear
  grid(25,23,{
   '.........ooo....................',
   '..ooooooonnnooooooo.............',
   '.otttnNNNNnNNNNNNnnooooooooooo..',
   'ottttnnnnnnnnnnnnnnnNNNNNNNNnno.',
   'ottttnnnnnnnnzzzznnnnnnnnnnnnno.',
   '.otttnooonnnnnnnnnnnooooooooooo.',
   '..ooo...oonnnnnnnnoooo..........',
   '........onnoooyyoo..............',
   '........onnoooyyyo..............',
   '........oooo..oyyo..............'
  })
  -- Ammunition drum and a brass feed belt make the role visible at 1x.
  ellipse(38,33,4,4,'o');ellipse(38,33,3,3,'g');ellipse(38,33,2,2,'G');px(38,33,'y')
  line(29,30,27,34,'o');line(27,34,27,39,'o');line(27,39,31,40,'o')
  for y=32,38,2 do rect(27,y,29,y,'y');px(28,y,'Y');px(26,y,'t') end
  rect(28,40,31,40,'y');line(49,25,51,25,'h');px(45,26,'o');px(47,26,'o');px(49,26,'o')
  im=near
  poly({{18,26},{22,27},{24,31},{28,29},{31,29},{34,31},{32,34},{27,36},{22,35},{18,32},{16,28}},'o')
  poly({{19,27},{21,28},{23,33},{27,33},{30,31},{32,31},{30,33},{26,34},{22,34},{18,30}},'c')
  line(19,28,21,32,'u');line(24,34,26,34,'v')
  im=hands
  grid(30,29,{'amm.','mssi','mssm','.aa.'});grid(44,29,{'mmmi','ssss','mmma'})
  im=fx
  grid(55,23,{'...y...','..yYy..','.yYwYy.','yYwwwYy','.yYwYy.','..yYy..','...y...'})
  line(57,20,60,20,'Y');line(59,33,61,33,'y')
 elseif spec.n==7 then
  -- Witch hat, silver braids and moss cloak; new slender scoped rifle.
  copy(orig,head,function(x,y)return y<=26 and not(x>=38 and y>=23)end)
  copy(orig,body,function(x,y)return y>=27 end);clear(body,27,27,61,34);clear(body,34,35,61,37)
  im=body
  poly({{19,27},{28,27},{33,31},{34,39},{28,44},{18,41},{16,34}},'o')
  poly({{20,28},{27,28},{31,31},{32,38},{27,42},{19,39},{17,34}},'g')
  line(20,30,19,37,'G');line(23,29,22,38,'G');line(28,31,30,37,'v')
  rect(25,35,28,38,'k');line(25,35,27,35,'r');line(18,40,27,42,'t')
  im=rear
  poly({{10,32},{15,31},{19,34},{19,40},{13,42},{9,38}},'o')
  poly({{11,33},{15,32},{17,35},{17,39},{13,40},{10,37}},'t');line(12,33,16,35,'b')
  line(12,35,15,38,'G');px(13,34,'y')
  im=far
  poly({{29,31},{32,32},{38,35},{43,31},{46,31},{47,34},{41,38},{36,38},{31,35}},'o')
  poly({{30,32},{32,33},{37,36},{40,35},{44,32},{45,33},{41,36},{37,36},{32,34}},'k')
  line(34,34,37,35,'r')
  im=gear
  -- Scope is above, not merged into, the barrel silhouette.
  rect(37,24,47,27,'o');rect(39,25,45,26,'n');px(38,25,'u');rect(46,25,47,26,'N')
  rect(40,27,41,28,'o');rect(45,27,46,28,'o')
  poly({{29,29},{34,28},{39,29},{49,29},{49,31},{37,31},{33,34},{29,34}},'o')
  poly({{30,30},{34,29},{37,30},{34,32},{30,33}},'t');line(31,30,34,30,'b')
  rect(37,29,58,30,'o');line(38,29,56,29,'N');line(41,30,58,30,'n')
  rect(57,28,59,31,'o');line(57,29,58,29,'Z')
  line(40,32,42,35,'o');line(49,32,48,35,'o');px(41,33,'n')
  im=near
  poly({{20,29},{23,31},{25,35},{30,33},{33,31},{36,33},{34,36},{28,39},{24,39},{20,36},{18,32}},'o')
  poly({{21,30},{22,32},{25,37},{28,37},{32,34},{34,33},{34,34},{29,37},{25,37},{21,34}},'g')
  line(20,31,22,34,'G');line(25,38,28,38,'v')
  im=hands
  grid(32,31,{'mmm.','miss','mssm','.aa.'});grid(44,31,{'mmmi','ssss','maam'})
  im=fx
  grid(59,27,{'..y.','.yYy','yYwY','.yYy','..y.'})
 else
  -- Wildrose rides a transport motorcycle. Reuse her hat/face/braid pixels.
  copy(orig,head,function(x,y)return (y<=24 and not(x>=36 and y>=16) and not(x>=30 and y>=21))or(x<=13 and y>=25 and y<=40)end)
  im=rear
  for _,cx in ipairs({13,49}) do
   ellipse(cx,53,7,7,'o');ellipse(cx,53,6,6,'z');ellipse(cx,53,4,4,'N');ellipse(cx,53,3,3,'n')
   line(cx-3,50,cx+3,56,'h');line(cx-3,56,cx+3,50,'Z');line(cx,49,cx,57,'h');ellipse(cx,53,1,1,'y')
  end
  -- Frame triangles, swingarm, rack and rear luggage pannier.
  for dy=0,1 do line(13,53+dy,25,40+dy,'o');line(25,40+dy,36,52+dy,'o');line(13,53+dy,36,52+dy,'o') end
  line(15,51,25,42,'N');line(15,54,35,53,'Z')
  rect(4,35,20,37,'o');line(5,35,18,35,'N')
  poly({{4,37},{14,36},{20,38},{19,45},{5,46},{3,43}},'o')
  poly({{5,38},{14,37},{18,39},{17,43},{6,44},{4,42}},'b');rect(7,38,8,43,'t');rect(14,38,15,43,'t');rect(9,39,12,40,'B');px(10,39,'y')
  im=body
  -- New coat torso and bent riding legs, fully drawn under the arms and tank.
  poly({{15,22},{24,22},{30,26},{31,32},{28,37},{21,40},{13,36},{11,29}},'o')
  poly({{16,23},{24,23},{28,26},{29,32},{26,36},{21,38},{14,35},{13,29}},'v')
  line(15,25,14,31,'c');line(27,27,28,32,'n');rect(18,28,21,31,'b')
  poly({{19,33},{25,34},{36,37},{40,41},{37,45},{33,48},{31,48},{31,44},{34,41},{26,40},{19,39},{15,36}},'o')
  poly({{21,34},{25,35},{34,38},{37,41},{34,44},{32,46},{32,43},{34,40},{25,39},{20,37}},'n')
  line(25,36,33,38,'Z');line(35,41,33,44,'z')
  poly({{31,45},{34,45},{34,48},{38,48},{39,50},{30,50},{29,48}},'o');rect(31,46,33,48,'t');line(32,49,37,49,'b')
  rect(14,33,26,35,'o');line(15,33,25,34,'b');rect(21,34,23,35,'y')
  -- Neck scarf, a small original red identity accent.
  poly({{19,23},{25,22},{27,24},{24,26},{21,25},{23,30},{20,29}},'R');line(20,23,24,23,'p')
  im=far
  poly({{26,25},{29,25},{34,29},{43,30},{47,31},{46,34},{41,34},{33,32},{28,30}},'o')
  poly({{27,26},{29,26},{34,30},{41,31},{45,31},{45,32},{41,32},{33,31},{28,29}},'v');line(31,28,34,30,'c')
  im=gear
  -- Seat/tank/engine chrome, with a distinct front fork and headlamp.
  poly({{14,36},{26,36},{31,38},{29,40},{15,39},{12,38}},'o');line(15,37,27,37,'d');line(17,36,25,36,'r')
  poly({{28,37},{36,35},{41,37},{43,41},{40,44},{31,44},{27,41}},'o')
  poly({{30,38},{36,36},{40,38},{41,40},{39,42},{31,42},{29,40}},'r');line(31,38,36,37,'p');px(37,37,'R');line(32,42,38,42,'k')
  poly({{24,44},{34,43},{39,47},{36,52},{26,52},{22,49}},'o')
  rect(26,45,34,50,'Z');line(26,45,33,45,'N');line(25,47,35,47,'n');line(26,49,34,49,'n');ellipse(30,48,2,2,'n');px(30,48,'h')
  for d=0,2 do line(43+d,36,48+d,51,'o') end
  line(44,36,49,51,'N');line(45,37,50,50,'h')
  poly({{42,32},{49,32},{52,34},{51,37},{46,38},{43,36}},'o');rect(47,33,50,35,'y');rect(48,33,50,34,'Y');px(49,33,'w')
  line(43,32,42,29,'o');line(42,29,47,29,'o');line(43,30,47,30,'N');line(42,33,38,33,'o')
  poly({{40,46},{43,45},{48,45},{54,47},{55,49},{52,49},{49,47},{43,47}},'o');line(43,46,49,46,'r');line(49,46,53,48,'p')
  line(22,53,40,53,'o');line(23,54,41,54,'o');line(24,53,39,53,'N');px(40,53,'h')
  im=near
  poly({{16,26},{20,26},{23,31},{28,33},{37,32},{43,31},{46,32},{45,35},{37,36},{27,37},{22,35},{18,32},{15,29}},'o')
  poly({{17,27},{19,27},{21,32},{27,35},{36,34},{43,32},{44,33},{42,34},{36,35},{27,35},{23,34},{19,31},{16,29}},'v')
  line(17,28,20,31,'c');line(24,34,28,35,'n')
  im=hands
  grid(42,31,{'mmm..','issmm','msssm','.aaa.'});px(45,32,'i')
  im=fx
  -- Subtle exhaust only; transport role does not acquire a gun.
  grid(17,53,{'Hh..','hHH.','.HhH','..HH'})
 end
 local s=Sprite(64,64,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(palette)
 local layers={
  {'01_Rear_Gear_Vehicle',rear},{'02_Body_Legs',body},{'03_Head_Hair',head},{'04_Far_Arm',far},
  {'05_Weapon_Vehicle',gear},{'06_Near_Arm',near},{'07_Hands_Details',hands},{'08_Role_FX',fx}
 }
 for _,v in ipairs(layers) do local l=s:newLayer();l.name=v[1];l.blendMode=BlendMode.NORMAL;s:newCel(l,1,v[2],Point(0,0)) end
 s.frames[1].duration=0.1;local tag=s:newTag(1,1);tag.name=spec.role..'_base'
 s:saveAs(dest);s:close();src:close();all[#all+1]=spec
end
local f=assert(io.open(root..'/QA/CombatRoles/role_specs.json','w'));f:write(json.encode(all));f:close()
print('Saved four native role revisions with eight editable layers each.')
