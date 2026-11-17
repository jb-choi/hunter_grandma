-- Sundeok 120px trial: native pixel artwork, quilted coat and hybrid animation.
local root=assert(app.params['root']);local source=assert(app.params['source']);local pc=app.pixelColor
local target=root..'/grandma_01_sundeok_120.aseprite'
assert(not app.fs.isFile(target)or app.params['revise']=='true','Preserve edits before revising')
local hex={'281e32','47243e','6d2949','ae615b','d98b77','f4bf9f','ffe1b4','fff5d7','57475f','7d6c86','aa9ba9','d7ccc8','744632','a7663a','d3934d','efb34a','ffdc78','4a5034','a1455e','d66b6e','ee403d','7b874f','30334d','424a61','7c8d9e','5f627a'}
local keys={'o','d','k','a','m','s','i','w','J','j','H','h','t','b','B','y','Y','g','r','p','R','G','z','n','N','Z'}
local ink,colors={},{};local pal=Palette(27);pal:setColor(0,Color{r=0,g=0,b=0,a=0})
for i,h in ipairs(hex)do local r,g,b=tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16);ink[keys[i]]=pc.rgba(r,g,b,255);colors[i]={r,g,b};pal:setColor(i,Color{r=r,g=g,b=b,a=255})end
local function blank()return Image(120,120,ColorMode.RGB)end
local im
local function px(x,y,c)x=math.floor(x+.5);y=math.floor(y+.5);if x>=0 and x<120 and y>=0 and y<120 then im:drawPixel(x,y,ink[c])end end
local function rect(x1,y1,x2,y2,c)for y=math.floor(y1),math.floor(y2)do for x=math.floor(x1),math.floor(x2)do px(x,y,c)end end end
local function line(x1,y1,x2,y2,c)
 x1,y1,x2,y2=math.floor(x1+.5),math.floor(y1+.5),math.floor(x2+.5),math.floor(y2+.5)
 local dx,dy=math.abs(x2-x1),math.abs(y2-y1);local sx,sy=x1<x2 and 1 or -1,y1<y2 and 1 or -1;local er=dx-dy
 while true do px(x1,y1,c);if x1==x2 and y1==y2 then break end;local e=er*2;if e>-dy then er=er-dy;x1=x1+sx end;if e<dx then er=er+dx;y1=y1+sy end end
end
local function poly(points,c)
 local lo,hi=119,0;for _,p in ipairs(points)do lo=math.min(lo,p[2]);hi=math.max(hi,p[2])end
 for y=math.ceil(lo),math.floor(hi)do local xs={};for i,p in ipairs(points)do local q=points[i%#points+1]
  if(p[2]<=y and q[2]>y)or(q[2]<=y and p[2]>y)then xs[#xs+1]=p[1]+(y-p[2])*(q[1]-p[1])/(q[2]-p[2])end
 end;table.sort(xs);for i=1,#xs-1,2 do for x=math.ceil(xs[i]),math.floor(xs[i+1])do px(x,y,c)end end end
 for i,p in ipairs(points)do local q=points[i%#points+1];line(p[1],p[2],q[1],q[2],c)end
end
local function ellipse(cx,cy,rx,ry,c)for y=math.floor(cy-ry),math.ceil(cy+ry)do for x=math.floor(cx-rx),math.ceil(cx+rx)do if((x-cx)/rx)^2+((y-cy)/ry)^2<=1.05 then px(x,y,c)end end end end
local function shift(img,dx,dy)local out=blank();out:drawImage(img,Point(math.floor(dx+.5),math.floor(dy+.5)));return out end
local function stroke(a,b,r,c)
 local dx,dy=b[1]-a[1],b[2]-a[2];local length=math.max(1,math.sqrt(dx*dx+dy*dy));local nx,ny=-dy/length*r,dx/length*r
 poly({{a[1]+nx,a[2]+ny},{b[1]+nx,b[2]+ny},{b[1]-nx,b[2]-ny},{a[1]-nx,a[2]-ny}},c);ellipse(a[1],a[2],r,r,c);ellipse(b[1],b[2],r,r,c)
end
local function map(p)
 if pc.rgbaA(p)==0 then return 0 end
 local r,g,b=pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p);local best,idx=1e9,1
 for i,v in ipairs(colors)do local distance=(r-v[1])^2+(g-v[2])^2+(b-v[3])^2;if distance<best then best=distance;idx=i end end
 return ink[keys[idx]]
end
-- Preserve Sundeok's recognizable head/face, then refine at one logical pixel.
local old=Sprite{fromFile=source..'/grandma_04_sundeok_tea.aseprite'}
local small=Image(64,64,ColorMode.RGB);local headLayers=0
for _,l in ipairs(old.layers)do if l.name:find('Head_Hair')or l.name:find('Face_Glasses')then local c=l:cel(1);small:drawImage(c.image,Point(c.position.x,c.position.y-2));headLayers=headLayers+1 end end
assert(headLayers==2,'Expected seated tea head and face source layers')
old:close();local headBase=blank()
for y=0,119 do for x=0,119 do headBase:drawPixel(x,y,map(small:getPixel(x//2+2,y//2+2)))end end
im=headBase
line(28,11,31,10,'h');px(32,11,'H');line(39,8,41,9,'h');line(47,13,49,14,'h')
line(20,20,22,18,'H');line(23,21,25,19,'h');line(31,16,33,17,'j')
px(44,25,'i');px(45,26,'s');line(35,33,38,33,'p');px(54,32,'m');line(43,38,47,38,'w')
headBase=shift(headBase,0,2)
local function quilt(img,step)
 local original=Image(img);im=img
 for y=0,119 do for x=0,119 do local p=original:getPixel(x,y)
  if p==ink.B or p==ink.y or p==ink.b then
   if(x+2*y)%step<=1 or(x-2*y)%step<=1 then img:drawPixel(x,y,ink.b)end
  end
 end end
end
local coat=blank();im=coat
poly({{25,38},{48,38},{59,42},{65,53},{68,66},{65,77},{57,82},{25,82},{15,75},{12,62},{16,48}},'o')
poly({{25,40},{48,40},{57,44},{62,54},{65,66},{62,75},{55,79},{26,79},{18,73},{15,62},{19,49}},'t')
poly({{26,42},{47,41},{55,45},{59,54},{62,65},{59,75},{52,77},{27,76},{20,70},{18,60},{22,49}},'B')
poly({{40,44},{47,43},{53,47},{56,57},{57,69},{53,75},{44,75},{43,60}},'y')
quilt(coat,20);im=coat
poly({{25,40},{31,39},{38,44},{43,40},{49,40},{48,46},{42,49},{33,46},{26,45}},'g')
line(28,41,33,44,'b');line(42,44,46,42,'B')
line(42,48,44,76,'t');line(43,49,45,75,'Y')
for _,y in ipairs({53,62,71})do rect(44,y,45,y+1,'w');px(46,y+1,'t')end
line(21,74,28,78,'b');line(28,79,54,79,'t');line(55,78,61,74,'b')
poly({{23,65},{33,66},{32,73},{24,72}},'b');line(24,65,32,66,'Y')
local function flower(x,y,size)
 ellipse(x,y,size,size,'p');ellipse(x-2,y,1,2,'R');ellipse(x+2,y,1,2,'r');px(x,y,'Y');line(x+1,y+3,x+4,y+5,'g');px(x+3,y+3,'G')
end
local function leg(hip,knee,ankle,far)
 stroke(hip,knee,10,'o');stroke(knee,ankle,6,'o')
 stroke(hip,knee,8,far and'd'or'k');stroke(knee,ankle,5,far and'd'or'k')
 line(hip[1]+3,hip[2]+3,knee[1]+3,knee[2]-2,far and'k'or'r')
 line(knee[1]+2,knee[2]+2,ankle[1]+2,ankle[2]-3,'d')
 flower(math.floor((hip[1]+knee[1])/2),math.floor((hip[2]+knee[2])/2),2)
 flower(math.floor(knee[1]-2),math.floor(knee[2]+4),1)
 rect(ankle[1]-4,ankle[2]-1,ankle[1]+4,ankle[2]+2,'H');line(ankle[1]-3,ankle[2]-1,ankle[1]+3,ankle[2]-1,'h')
 poly({{ankle[1]-5,ankle[2]+1},{ankle[1]+5,ankle[2]+1},{ankle[1]+12,ankle[2]+4},{ankle[1]+13,ankle[2]+6},{ankle[1]-6,ankle[2]+6},{ankle[1]-7,ankle[2]+4}},'o')
 rect(ankle[1]-4,ankle[2]+2,ankle[1]+5,ankle[2]+4,'d');line(ankle[1]-3,ankle[2]+2,ankle[1]+4,ankle[2]+2,'r');line(ankle[1]+5,ankle[2]+4,ankle[1]+10,ankle[2]+4,'p')
end
local function makeBody(pose,step,bob,lean,depth,blink)
 local result=blank();im=result
 if pose=='Walk'then
  local phase=(step-1)*math.pi/3;local sn,cs=math.sin(phase),math.cos(phase)
  leg({47,77},{47-10*sn,95},{44-18*sn,111-math.max(0,-cs)*7},true)
  leg({39,77},{43+10*sn,94},{44+18*sn,111-math.max(0,cs)*7},false)
 else
  leg({49+lean*.3,77+depth},{63+lean*.2,94},{75,111},true)
  leg({36+lean*.3,77+depth},{28,94+depth*.2},{21,111},false)
 end
 result:drawImage(shift(coat,lean,bob+depth))
 local head=Image(headBase);if blink then im=head;rect(50,28,53,31,'s');line(50,30,53,30,'o')end
 result:drawImage(shift(head,lean,bob+depth));return result
end
local gunModel=blank();im=gunModel
-- Native short pump shotgun, wood furniture and two clearly separated metal rails.
poly({{0,8},{4,6},{16,9},{21,6},{34,6},{38,8},{59,5},{63,6},{63,11},{37,12},{32,15},{24,15},{21,21},{17,21},{16,15},{6,15},{1,17}},'o')
poly({{2,9},{5,8},{16,11},{21,9},{22,13},{17,14},{15,13},{6,13},{3,15}},'t');line(4,9,12,11,'b');line(4,12,10,12,'B')
rect(23,8,35,12,'n');line(24,8,34,8,'N');line(26,12,33,12,'z');rect(26,9,30,10,'Z');px(31,10,'y')
rect(36,7,61,10,'z');line(37,7,60,7,'N');line(38,9,61,9,'n');rect(60,6,63,11,'o');line(61,7,62,7,'N');px(62,10,'z')
rect(39,11,51,15,'o');rect(40,11,50,13,'b');line(41,11,49,11,'B');for x=42,48,3 do line(x,12,x,13,'t')end
rect(18,15,20,19,'t');line(19,16,19,18,'b');line(22,17,25,17,'n');line(24,14,25,16,'n')
local function transform(img,x,y,angle)
 local out=blank();local a=angle*math.pi/180;local c,sn=math.cos(a),math.sin(a)
 -- Inverse sampling fills every destination pixel and keeps the palette exact.
 for yy=0,119 do for xx=0,119 do local dx,dy=xx-x,yy-y;local sx=math.floor(dx*c+dy*sn+18+.5);local sy=math.floor(-dx*sn+dy*c+15+.5)
  if sx>=0 and sx<120 and sy>=0 and sy<120 then out:drawPixel(xx,yy,img:getPixel(sx,sy))end
 end end
 local function point(sx,sy)local dx,dy=sx-18,sy-15;return {math.floor(x+dx*c-dy*sn+.5),math.floor(y+dx*sn+dy*c+.5)}end
 return out,point
end
local function sleeve(shoulder,elbow,wrist,far)
 local out=blank();im=out
 stroke(shoulder,elbow,8,'o');stroke(elbow,wrist,6,'o')
 stroke(shoulder,elbow,6,far and'b'or'B');stroke(elbow,wrist,4,far and'B'or'y')
 quilt(out,20);im=out
 line(shoulder[1]-2,shoulder[2],elbow[1]-2,elbow[2]-2,'Y')
 ellipse(wrist[1],wrist[2],4,4,'t');ellipse(wrist[1]+1,wrist[2]-1,3,3,'s');line(wrist[1]-1,wrist[2]-2,wrist[1]+2,wrist[2]-2,'i');px(wrist[1]+3,wrist[2],'m')
 return out
end
local clips={{name='Idle',count=4,ms=220},{name='Walk',count=6,ms=110},{name='Reload',count=6,ms=150},{name='Aim',count=3,ms=140},{name='Fire',count=4,ms=90},{name='Collect',count=4,ms=160}}
local records={}
local function makePose(action,f)
 local bob,lean,depth=0,0,0;local angle=0;local wx,wy=60,52;local back,front,ammo,pickup,fx=blank(),blank(),blank(),blank(),blank()
 if action=='Idle'then bob=({0,-1,-2,-1})[f];angle=8
 elseif action=='Walk'then bob=({0,-1,-2,0,-1,-2})[f];angle=12
 elseif action=='Reload'then bob=1;angle=({18,18,18,18,8,0})[f];wy=57
 elseif action=='Aim'then angle=({10,3,-3})[f]
 elseif action=='Fire'then wx=60+({-3,-5,-2,0})[f];angle=({-8,-5,-1,0})[f];bob=({-1,0,0,0})[f]
 elseif action=='Collect'then lean=({2,7,7,2})[f];depth=({2,8,8,2})[f];angle=28;wx=60+lean*.5;wy=63+depth end
 local body=makeBody(action=='Walk'and'Walk'or'Stand',f,bob,lean,depth,action=='Idle'and f==3)
 local gun,point=transform(gunModel,wx,wy+bob,angle)
 local trigger=point(18,15);local support=point(45,16);local muzzle=point(63,8)
 local nearShoulder={24+lean,50+bob+depth};local farShoulder={53+lean,48+bob+depth}
 local nearWrist={trigger[1],trigger[2]+1};local farWrist={support[1],support[2]+1}
 local nearElbow={36+lean,65+bob+depth};local farElbow={68+lean,64+bob+depth}
 if action=='Reload'then
  farWrist=({support,{48,77},{62,67},{70,64},{78,59},support})[f];farElbow={60,68}
  if f==2 or f==3 or f==4 then im=ammo;rect(farWrist[1]-1,farWrist[2]-6,farWrist[1]+2,farWrist[2]-1,'o');rect(farWrist[1],farWrist[2]-5,farWrist[1]+1,farWrist[2]-2,'R');line(farWrist[1],farWrist[2]-1,farWrist[1]+2,farWrist[2]-1,'y')end
  if f==5 then gun,point=transform(gunModel,wx-2,wy+bob,angle);farWrist=point(40,16);muzzle=point(63,8)end
 elseif action=='Collect'then
  farWrist=trigger;farElbow={64+lean,68+depth};nearWrist=({{65,82},{84,104},{80,97},{64,76}})[f];nearElbow={51+lean,79+depth}
  im=pickup
  local bagx,bagy=83,109;if f==3 then bagx,bagy=80,101 elseif f==4 then bagx,bagy=64,80 end
  if f>=2 then poly({{bagx-4,bagy-2},{bagx-3,bagy-6},{bagx+2,bagy-6},{bagx+5,bagy-1},{bagx+3,bagy+3},{bagx-3,bagy+3}},'o');rect(bagx-2,bagy-4,bagx+2,bagy+1,'b');line(bagx-2,bagy-5,bagx+2,bagy-5,'i');px(bagx,bagy-2,'B')end
 end
 back=sleeve(farShoulder,farElbow,farWrist,true);front=sleeve(nearShoulder,nearElbow,nearWrist,false)
 if action=='Fire'and f<=2 then
  im=fx;local mx,my=muzzle[1],muzzle[2]
  if f==1 then poly({{mx+1,my},{mx+5,my-2},{mx+7,my-8},{mx+9,my-3},{mx+14,my-3},{mx+11,my+1},{mx+15,my+5},{mx+9,my+4},{mx+7,my+9},{mx+5,my+4},{mx+1,my+2}},'y');line(mx+2,my+1,mx+10,my+1,'w');line(mx+7,my-4,mx+7,my+5,'Y')
  else ellipse(mx+3,my,3,2,'Y');line(mx+1,my,mx+4,my,'w');rect(76,34,79,35,'y');px(78,33,'Y')end
 end
 local result={Idle_Full=blank(),Body=body,BackArm=back,Weapon=gun,FrontArm=front,Ammo=ammo,Pickup=pickup,FX=fx}
 if action=='Idle'then local whole=blank();for _,name in ipairs({'Body','BackArm','Weapon','FrontArm'})do whole:drawImage(result[name]);result[name]=blank()end;result.Idle_Full=whole end
 return result,{action=action,local_frame=f,shoulder_back=farShoulder,shoulder_front=nearShoulder,weapon_pivot=trigger,hand_back=farWrist,hand_front=nearWrist,muzzle=muzzle,upper_offset_xy={lean,bob+depth},floor_boundary_y=118}
end
local layerNames={'Idle_Full','Body','BackArm','Weapon','FrontArm','Ammo','Pickup','FX'}
local function newSprite()
 local s=Sprite(120,120,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pal)
 for _,name in ipairs(layerNames)do local l=s:newLayer();l.name=name;l.blendMode=BlendMode.NORMAL end;return s
end
local s=newSprite();local fi=0
for _,clip in ipairs(clips)do local first=fi+1
 for f=1,clip.count do fi=fi+1;if fi>1 then s:newEmptyFrame()end;s.frames[fi].duration=clip.ms/1000
  local parts,meta=makePose(clip.name,f);meta.frame=fi;meta.duration_ms=clip.ms
  if clip.name=='Fire'then meta.duration_ms=({60,90,110,150})[f];s.frames[fi].duration=meta.duration_ms/1000 end
  for li,name in ipairs(layerNames)do s:newCel(s.layers[li],fi,parts[name],Point(0,0))end
  records[#records+1]=meta
 end
 local tag=s:newTag(first,fi);tag.name=clip.name
end
s:saveAs(target)
-- Separate clips preserve the same 120px coordinate space and editable layers.
for _,clip in ipairs(clips)do
 local out=newSprite();local ci=0
 for f,r in ipairs(records)do if r.action==clip.name then ci=ci+1;if ci>1 then out:newEmptyFrame()end;out.frames[ci].duration=r.duration_ms/1000
  for li=1,#layerNames do local c=s.layers[li]:cel(f);out:newCel(out.layers[li],ci,Image(c.image),c.position)end
 end end
 local tag=out:newTag(1,ci);tag.name=clip.name;app.fs.makeAllDirectories(root..'/Clips');out:saveAs(root..'/Clips/sundeok_'..clip.name:lower()..'.aseprite');out:close()
end
-- Demonstration: retain the walking body while independent Aim/Fire parts change.
local demo=newSprite();local demoRecords={}
for f=1,12 do if f>1 then demo:newEmptyFrame()end;demo.frames[f].duration=.11
 local wf=(f-1)%6+1;local walk=makePose('Walk',wf);local action=f>=5 and f<=8 and'Fire'or'Aim';local localFrame=action=='Fire'and(f-4)or 3
 local upper,meta=makePose(action,localFrame);local bob=({0,-1,-2,0,-1,-2})[wf]
 for li,name in ipairs(layerNames)do local img=name=='Body'and walk.Body or(name=='Idle_Full'and blank()or shift(upper[name],0,bob-meta.upper_offset_xy[2]));demo:newCel(demo.layers[li],f,img,Point(0,0))end
 demoRecords[#demoRecords+1]={frame=f,walk_frame=wf,upper_action=action,upper_frame=localFrame,upper_translation_y=bob-meta.upper_offset_xy[2]}
end
local dtag=demo:newTag(1,12);dtag.name='Walk_Aim_Fire';demo:saveAs(root..'/sundeok_walk_aim_fire_demo.aseprite');demo:close();s:close()
local function write(path,obj)local f=assert(io.open(path,'w'));f:write(json.encode(obj));f:close()end
write(root..'/Source/pose_metadata.json',{canvas={120,120},palette=hex,frames=records,clips=clips,layer_order=layerNames,logical_floor_boundary_y=118,coordinate_system='top left origin, x right, y down; full-canvas part coordinates',reference='Existing v002 head pixels magnified 2x, palette remapped, then native 1px face/hair refinements; coat, legs, sleeves, hands and shotgun newly authored'})
write(root..'/Source/demo_metadata.json',demoRecords)
local f=assert(io.open(root..'/Export/sundeok_26.gpl','w'));f:write('GIMP Palette\nName: Sundeok120 shared 26 opaque colors\nColumns: 13\n');for i,v in ipairs(colors)do f:write(string.format('%d %d %d Color_%02d\n',v[1],v[2],v[3],i))end;f:close()
print('Built Sundeok120: 27 poses across 6 clips; quilted coat; 26-color shared palette; hybrid parts and walking-fire demo.')
