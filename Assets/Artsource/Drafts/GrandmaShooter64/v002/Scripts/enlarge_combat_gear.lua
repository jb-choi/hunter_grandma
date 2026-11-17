-- Enlarge role equipment in preserved editable natives, retaining character layers.
local root=assert(app.params['root']);local backup=assert(app.params['backup']);local pc=app.pixelColor
local C={o='281e32',d='47243e',k='6d2949',r='a1455e',p='d66b6e',R='ee403d',g='4a5034',G='7b874f',L='a9b16e',v='153d49',c='2e6c77',u='5b9d9c',a='ae615b',m='d98b77',s='f4bf9f',i='ffe1b4',w='fff5d7',h='d7ccc8',H='aa9ba9',j='7d6c86',J='57475f',n='424a61',N='7c8d9e',t='744632',b='a7663a',B='d3934d',y='efb34a',Y='ffdc78',q='653979',Q='985d9d',z='30334d',Z='5f627a'}
local ink={};for k,h in pairs(C)do ink[k]=pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local function blank()return Image(64,64,ColorMode.RGB)end
local im
local function px(x,y,c)assert(x>=0 and x<64 and y>=0 and y<64,'Clipped drawing coordinate');im:drawPixel(x,y,ink[c])end
local function rect(x1,y1,x2,y2,c)for y=y1,y2 do for x=x1,x2 do px(x,y,c)end end end
local function line(x1,y1,x2,y2,c)
 local dx,dy=math.abs(x2-x1),math.abs(y2-y1);local sx,sy=x1<x2 and 1 or -1,y1<y2 and 1 or -1;local er=dx-dy
 while true do px(x1,y1,c);if x1==x2 and y1==y2 then break end;local e=er*2;if e>-dy then er=er-dy;x1=x1+sx end;if e<dx then er=er+dx;y1=y1+sy end end
end
local function poly(points,c)
 local lo,hi=63,0;for _,p in ipairs(points)do lo=math.min(lo,p[2]);hi=math.max(hi,p[2])end
 for y=lo,hi do local xs={};for i,p in ipairs(points)do local q=points[i%#points+1]
  if(p[2]<=y and q[2]>y)or(q[2]<=y and p[2]>y)then xs[#xs+1]=p[1]+(y-p[2])*(q[1]-p[1])/(q[2]-p[2])end
 end;table.sort(xs);for i=1,#xs-1,2 do for x=math.ceil(xs[i]),math.floor(xs[i+1])do px(x,y,c)end end end
 for i,p in ipairs(points)do local q=points[i%#points+1];line(p[1],p[2],q[1],q[2],c)end
end
local function ellipse(cx,cy,rx,ry,c)for y=cy-ry,cy+ry do for x=cx-rx,cx+rx do if((x-cx)/rx)^2+((y-cy)/ry)^2<=1.05 then px(x,y,c)end end end end
local function grid(x,y,rows)for yy,row in ipairs(rows)do for xx=1,#row do local c=row:sub(xx,xx);if c~='.'then px(x+xx-1,y+yy-1,c)end end end end
local function pixels(img)local n=0;for y=0,img.height-1 do for x=0,img.width-1 do if pc.rgbaA(img:getPixel(x,y))>0 then n=n+1 end end end;return n end
local function flat(s)local img=blank();img:drawSprite(s,1);return img end
local specs=json.decode(assert(io.open(backup..'/role_specs.json','r')):read('*a'))
local metrics={};local beforeAfter=Image(256,128,ColorMode.RGB);beforeAfter:clear(pc.rgba(238,229,214,255))
for index,spec in ipairs(specs)do
 local s=Sprite{fromFile=backup..'/'..spec.name..'.aseprite'};assert(#s.layers==8 and #s.frames==1)
 local before=flat(s);beforeAfter:drawImage(before,Point((index-1)*64,0))
 local oldPixels=pixels(s.layers[5]:cel(1).image)
 local rear,gear,hands,fx=blank(),blank(),blank(),blank()
 if spec.n==5 then
  im=rear
  -- Broad pressurized fuel pack, two straps, brass valve and pressure dial.
  poly({{4,17},{10,17},{13,20},{13,37},{10,40},{3,39},{1,36},{1,21}},'o')
  rect(3,20,11,36,'r');rect(3,21,5,35,'p');rect(10,21,11,36,'k')
  rect(2,23,12,25,'n');line(3,23,11,23,'N');rect(2,33,12,35,'n');line(3,33,11,33,'N')
  rect(5,15,9,18,'o');rect(6,15,8,16,'y');px(7,14,'y')
  ellipse(7,20,2,2,'o');ellipse(7,20,1,1,'i');px(8,20,'R')
  im=gear
  for d=0,2 do line(6+d,35,10+d,41,'o');line(10+d,41,21+d,41,'o');line(21+d,41,29+d,31,'o')end
  line(7,35,11,40,'N');line(11,40,21,40,'n');line(21,40,30,30,'N')
  -- Long combustion chamber feeding a flared, brass-rimmed flame nozzle.
  poly({{22,22},{28,21},{33,22},{35,23},{41,20},{46,20},{48,22},{48,30},{45,32},{40,32},{34,29},{29,29},{28,33},{25,33},{25,28},{22,27}},'o')
  rect(24,23,33,27,'n');line(25,23,32,23,'N');line(24,27,32,27,'z')
  rect(25,28,27,31,'t');line(26,29,26,31,'b')
  poly({{34,24},{40,22},{43,22},{43,30},{40,30},{34,28}},'n')
  line(35,24,40,23,'N');line(35,28,40,29,'Z')
  rect(43,21,45,31,'b');line(43,22,43,30,'y');line(45,22,45,30,'B')
  rect(46,22,47,30,'z');line(47,23,47,29,'o')
  for x=35,41,2 do line(x,24,x,28,'o');px(x,24,'N')end
  rect(28,20,31,21,'o');line(29,20,30,20,'R');px(29,19,'p');px(26,24,'w')
  im=hands
  grid(29,28,{'ammm.','msssi','mssmm','.aaa.'});grid(40,30,{'mmmmi','sssss','mmmaa'})
  im=fx
  -- A widening continuous flame fan instead of a small muzzle star.
  poly({{48,25},{51,23},{52,21},{55,22},{57,18},{59,19},{62,16},{61,23},{62,25},{60,27},{62,31},{60,31},{62,35},{57,33},{55,34},{54,30},{51,30},{50,28},{48,27}},'R')
  poly({{49,25},{53,23},{55,24},{58,21},{60,21},{59,25},{61,26},{59,28},{60,31},{56,30},{55,32},{53,28},{51,28},{49,27}},'y')
  poly({{49,26},{53,24},{55,25},{58,23},{58,26},{60,27},{57,28},{57,30},{54,28},{51,27}},'Y')
  line(49,26,55,26,'w');px(56,25,'w');px(54,27,'i');px(60,19,'Y')
  spec.muzzle={48,26};spec.grips={{31,30},{43,31}}
 elseif spec.n==6 then
  -- Keep her hip ammunition pouch; replace the whole main weapon layer.
  local rc=s.layers[1]:cel(1);rear:drawImage(rc.image,rc.position)
  im=gear
  poly({{23,23},{27,21},{38,21},{42,23},{44,23},{44,31},{40,33},{33,33},{32,36},{29,36},{29,32},{23,30}},'o')
  poly({{24,24},{27,22},{38,22},{41,24},{42,29},{39,31},{31,31},{30,33},{30,30},{24,29}},'n')
  line(27,22,37,22,'N');rect(25,25,29,28,'t');line(26,25,28,25,'b')
  rect(32,24,39,27,'z');line(33,24,38,24,'Z');px(39,25,'R')
  rect(31,19,38,21,'o');line(32,20,37,20,'N')
  -- Three cooled barrels with a heavy rotating front collar.
  rect(41,22,56,31,'o');rect(42,23,54,30,'z')
  for y=23,29,3 do line(42,y,55,y,'N');line(42,y+1,55,y+1,'n')end
  for x=44,50,3 do px(x,24,'o');px(x,27,'o');px(x,30,'o')end
  rect(51,22,53,31,'o');line(52,23,52,30,'Z')
  rect(55,23,57,30,'o');line(55,24,55,29,'N');px(57,24,'z');px(57,27,'z');px(57,29,'z')
  -- Oversized ammunition drum and a visible loop of linked brass cartridges.
  ellipse(37,36,6,6,'o');ellipse(37,36,5,5,'g');ellipse(37,36,3,3,'G');ellipse(37,36,1,1,'y')
  line(34,32,38,32,'L');line(41,34,41,38,'v');px(37,36,'Y')
  for d=0,1 do line(28+d,30,26+d,35,'o');line(26+d,35,27+d,42,'o');line(27+d,42,33+d,42,'o')end
  for y=32,40,2 do rect(26,y,29,y,'y');px(27,y,'Y');px(25,y,'t')end
  rect(28,42,32,42,'y');px(30,42,'Y')
  im=hands
  grid(29,31,{'ammm.','msssi','mssmm','.aaa.'});grid(45,32,{'mmmm.','issss','msssm','.aaa.'})
  im=fx
  grid(58,22,{'..y..','.yYy.','yYwYy','.YwY.','YwwwY','.YwY.','yYwYy','.yYy.','..y..'})
  line(58,19,61,19,'Y');line(59,34,61,34,'y')
  spec.muzzle={58,26};spec.grips={{31,33},{47,33}}
 elseif spec.n==7 then
  local rc=s.layers[1]:cel(1);rear:drawImage(rc.image,rc.position)
  im=gear
  -- Oversized optic with a separate objective bell and cyan lens.
  rect(34,22,37,26,'o');rect(35,23,37,25,'n')
  rect(37,21,48,26,'o');rect(38,22,47,25,'n');line(39,22,46,22,'N');line(39,25,46,25,'z')
  rect(47,20,51,27,'o');rect(48,21,50,26,'n');line(50,22,50,25,'u');px(50,22,'i')
  rect(41,19,44,21,'o');line(42,20,43,20,'Z')
  rect(39,26,40,28,'o');rect(46,27,47,28,'o')
  -- Deep wooden stock, straight receiver and a long slim barrel remain distinct.
  poly({{23,28},{28,27},{33,28},{35,27},{45,27},{47,29},{46,32},{36,32},{32,35},{26,35},{23,33}},'o')
  poly({{24,29},{28,28},{33,29},{35,29},{33,32},{30,34},{25,33}},'t');line(25,29,29,29,'b');line(25,32,29,33,'B')
  rect(35,28,46,31,'n');line(36,28,45,28,'N');rect(37,31,40,32,'z')
  rect(34,32,35,35,'o');line(35,33,35,34,'t')
  rect(45,28,58,30,'o');line(46,28,57,28,'N');line(46,29,57,29,'Z')
  rect(57,27,60,31,'o');line(58,28,59,28,'N');px(58,29,'z');px(59,30,'z')
  -- Extended folding bipod emphasizes an anti-materiel sniper silhouette.
  line(46,32,44,38,'o');line(47,32,45,38,'Z');line(50,32,53,38,'o');line(51,32,54,38,'N')
  line(43,38,46,38,'o');line(52,38,55,38,'o')
  im=hands
  grid(31,33,{'mmm.','miss','mssm','.aa.'});grid(44,32,{'mmmi','ssss','maam'})
  im=fx
  grid(60,26,{'..y','..Y','.Yw','Yww','.Yw','..Y','..y'})
  spec.muzzle={60,29};spec.grips={{33,34},{46,33}}
 else
  im=rear
  -- Larger wheels and a wider wheelbase; cargo remains a transport cue.
  for _,cx in ipairs({13,51})do
   ellipse(cx,52,8,8,'o');ellipse(cx,52,7,7,'z');ellipse(cx,52,5,5,'N');ellipse(cx,52,4,4,'n')
   line(cx-3,49,cx+3,55,'h');line(cx-3,55,cx+3,49,'Z');line(cx,48,cx,56,'h');line(cx-4,52,cx+4,52,'N');ellipse(cx,52,1,1,'y')
  end
  for dy=0,1 do line(13,52+dy,25,40+dy,'o');line(25,40+dy,38,52+dy,'o');line(13,52+dy,38,52+dy,'o')end
  line(15,50,25,42,'N');line(15,54,37,53,'Z')
  rect(3,34,21,37,'o');line(4,35,20,35,'N')
  poly({{3,37},{16,36},{21,39},{20,47},{5,48},{2,44}},'o')
  poly({{4,38},{16,37},{19,40},{18,45},{6,46},{3,43}},'b');rect(6,38,7,45,'t');rect(15,38,16,45,'t');rect(9,40,12,42,'B');px(10,40,'y')
  rect(4,31,16,34,'o');rect(5,32,15,33,'g');rect(7,32,8,33,'G');rect(12,32,13,33,'t')
  im=gear
  poly({{13,36},{27,36},{32,38},{30,40},{14,40},{11,38}},'o');line(14,37,28,37,'r');line(16,36,25,36,'p')
  poly({{27,37},{35,34},{41,35},{45,39},{44,43},{40,45},{30,45},{26,42}},'o')
  poly({{29,38},{35,35},{40,36},{43,39},{42,42},{39,43},{30,43},{28,41}},'r');line(30,38,36,36,'p');line(36,36,39,36,'R');line(31,43,39,43,'k')
  -- Enlarged finned V-twin engine with two visible chrome exhaust runs.
  poly({{24,43},{32,43},{35,45},{39,43},{42,46},{41,51},{37,54},{27,54},{23,51}},'o')
  poly({{25,45},{29,44},{34,49},{30,52},{26,49}},'Z');poly({{35,46},{39,44},{41,47},{38,51},{34,51}},'N')
  line(26,45,30,46,'h');line(26,47,31,48,'n');line(27,49,31,50,'n');line(36,46,39,45,'h');line(35,48,39,47,'n')
  ellipse(33,51,3,2,'n');px(33,51,'h')
  for d=0,2 do line(46+d,36,51+d,50,'o')end
  line(47,36,52,50,'N');line(48,37,53,49,'h')
  poly({{43,31},{51,31},{55,33},{55,37},{51,39},{45,38}},'o');rect(49,32,53,36,'y');rect(50,32,53,34,'Y');line(51,32,52,32,'w')
  line(44,32,43,28,'o');line(43,28,49,28,'o');line(44,29,49,29,'N');line(44,33,39,33,'o')
  poly({{41,44},{45,43},{51,43},{57,46},{59,49},{56,49},{53,46},{46,45}},'o');line(45,44,51,44,'r');line(51,44,56,47,'p')
  for dy=0,1 do line(26,53+dy,44,53+dy,'o');line(28,56+dy,44,56+dy,'o')end
  line(26,53,43,53,'N');line(29,56,43,56,'h');rect(44,53,45,54,'z');rect(44,56,45,57,'z')
  im=hands
  local hc=s.layers[7]:cel(1);hands:drawImage(hc.image,hc.position)
  -- Boot restored in front of the larger engine, resting on its footpeg.
  poly({{32,46},{35,46},{35,48},{40,48},{41,50},{32,50},{31,49}},'o');rect(32,47,34,48,'t');line(33,49,39,49,'b')
  im=fx;grid(19,54,{'Hh..','hHH.','.HhH','..HH'})
  spec.wheel_centers={{13,52},{51,52}};spec.wheel_radius=8
 end
 local function set(index,img)local c=assert(s.layers[index]:cel(1));c.image=img;c.position=Point(0,0)end
 set(1,rear);set(5,gear);set(7,hands);set(8,fx)
 for _,l in ipairs(s.layers)do l.isVisible=true end
 local count=pixels(gear);assert(count>oldPixels,'Equipment did not grow')
 spec.gear_revision='large_role_features_2026-10-28';spec.gear_source_backup=backup;spec.equipment_layer_pixels_before=oldPixels;spec.equipment_layer_pixels_after=count
 s:saveAs(root..'/'..spec.name..'.aseprite');beforeAfter:drawImage(flat(s),Point((index-1)*64,64));s:close()
 metrics[#metrics+1]={name=spec.name,equipment_pixels_before=oldPixels,equipment_pixels_after=count,preserved_layers={2,3,4,6}}
end
local enlarged=Image(1024,512,ColorMode.RGB)
for y=0,511 do for x=0,1023 do enlarged:drawPixel(x,y,beforeAfter:getPixel(x//4,y//4))end end
enlarged:saveAs(root..'/Previews/combat_roles_weapon_size_before_after_4x.png')
local function write(p,t)local f=assert(io.open(p,'w'));f:write(json.encode(t));f:close()end
write(root..'/QA/CombatRoles/role_specs.json',specs);write(root..'/QA/CombatRoles/gear_size_metrics.json',metrics)
print('Enlarged all four role equipment layers; retained head/body/arm layers from preserved natives.')
