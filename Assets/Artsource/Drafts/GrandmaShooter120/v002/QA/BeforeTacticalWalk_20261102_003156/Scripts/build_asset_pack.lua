-- Assemble the approved design's generated action drawings into independent parts.
local root=assert(app.params['root']);local pc=app.pixelColor
local hex={'281e32','47243e','6d2949','ae615b','d98b77','f4bf9f','ffe1b4','fff5d7','57475f','7d6c86','aa9ba9','d7ccc8','744632','a7663a','d3934d','efb34a','ffdc78','4a5034','a1455e','d66b6e','ee403d','7b874f','30334d','424a61','7c8d9e','5f627a'}
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local pal=Palette(27);pal:setColor(0,Color{r=0,g=0,b=0,a=0});for i,h in ipairs(hex)do local p=rgba(h);pal:setColor(i,Color{r=pc.rgbaR(p),g=pc.rgbaG(p),b=pc.rgbaB(p),a=255})end
local function set(hs)local t={};for _,h in ipairs(hs)do t[rgba(h)]=true end;return t end
local skin=set({'ae615b','d98b77','f4bf9f','ffe1b4','fff5d7'});local metal=set({'30334d','424a61','7c8d9e','5f627a'});local hair=set({'57475f','7d6c86','aa9ba9','d7ccc8'});local coat=set({'744632','a7663a','d3934d','efb34a','ffdc78','4a5034'});local brown=set({'281e32','47243e','744632','a7663a','d3934d','efb34a','ffdc78'})
local function blank()return Image(120,120,ColorMode.RGB)end
local function shift(img,dx,dy)local out=blank();out:drawImage(img,Point(math.floor(dx+.5),math.floor(dy+.5)));return out end
local function dist(a,b)return math.sqrt((a[1]-b[1])^2+(a[2]-b[2])^2)end
local function components(img,predicate)
 local seen,groups={},{}
 for y=0,119 do for x=0,119 do local k=y*120+x;if not seen[k]and predicate(x,y,img:getPixel(x,y))then
  local q,head={{x,y}},1;local group={pts={},x=0,y=0,bx=x,by=y,ex=x,ey=y};seen[k]=true
  while head<=#q do local p=q[head];head=head+1;group.pts[#group.pts+1]=p;group.x=group.x+p[1];group.y=group.y+p[2];group.bx=math.min(group.bx,p[1]);group.by=math.min(group.by,p[2]);group.ex=math.max(group.ex,p[1]);group.ey=math.max(group.ey,p[2])
   for dy=-1,1 do for dx=-1,1 do local xx,yy=p[1]+dx,p[2]+dy;if xx>=0 and xx<120 and yy>=0 and yy<120 then local kk=yy*120+xx;if not seen[kk]and predicate(xx,yy,img:getPixel(xx,yy))then seen[kk]=true;q[#q+1]={xx,yy}end end end end
  end
  group.count=#group.pts;group.center={group.x/group.count,group.y/group.count};groups[#groups+1]=group
 end end end;table.sort(groups,function(a,b)return a.count>b.count end);return groups
end
local function anatomy(body)
 local bx,by,ex,ey=120,120,0,0
 for y=0,70 do for x=0,119 do if hair[body:getPixel(x,y)]then bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y)end end end
 local hx=(bx+ex)/2;local sy=ey+10;local l,r=120,0
 for y=math.max(0,sy-4),math.min(112,sy+6)do for x=0,119 do if coat[body:getPixel(x,y)]then l=math.min(l,x);r=math.max(r,x)end end end
 if l>=r then l,r=hx-20,hx+22 end
 return{head_center={hx,(by+ey)/2},hair_bounds={bx,by,ex-bx+1,ey-by+1},front_shoulder={l+3,sy},back_shoulder={r-3,sy}}
end
local gun=Image{fromFile=root..'/Weapons/Shotgun/sundeok_shotgun.png'}
local function fitGun(full,body,an)
 local pts={};local maxx=-1
 for y=30,94 do for x=math.max(65,math.floor(an.head_center[1]+14)),117 do if metal[full:getPixel(x,y)]then pts[#pts+1]={x,y};maxx=math.max(maxx,x)end end end
 if #pts<15 then return{61,55},0,{106,47},'fallback'end
 local edgeY,edgeN=0,0;for _,p in ipairs(pts)do if p[1]>=maxx-2 then edgeY=edgeY+p[2];edgeN=edgeN+1 end end;edgeY=edgeY/math.max(1,edgeN)
 local n,sx,sy,sxx,sxy=0,0,0,0,0
 for _,p in ipairs(pts)do if p[1]>maxx-27 and math.abs(p[2]-edgeY)<16 then n=n+1;sx=sx+p[1];sy=sy+p[2];sxx=sxx+p[1]^2;sxy=sxy+p[1]*p[2]end end
 local slope=(n*sxy-sx*sy)/math.max(1,n*sxx-sx*sx);local angle=math.max(-20,math.min(32,math.atan(slope)*180/math.pi));local my,count=0,0
 for _,p in ipairs(pts)do if p[1]>=maxx-2 then my=my+p[2];count=count+1 end end
 local muzzle={math.min(117,maxx+1),my/math.max(1,count)};local a=angle*math.pi/180;local c,s=math.cos(a),math.sin(a)
 return{muzzle[1]-45*c-8*s,muzzle[2]-45*s+8*c},angle,muzzle,'metal_axis'
end
local function gunImage(pivot,angle)
 local out=blank();local a=angle*math.pi/180;local c,s=math.cos(a),math.sin(a)
 for y=0,119 do for x=0,119 do local dx,dy=x-pivot[1],y-pivot[2];local sx,sy=math.floor(32+dx*c+dy*s+.5),math.floor(12-dx*s+dy*c+.5);if sx>=0 and sx<gun.width and sy>=0 and sy<gun.height then out:drawPixel(x,y,gun:getPixel(sx,sy))end end end
 local function point(x,y)local dx,dy=x-32,y-12;return{math.floor(pivot[1]+dx*c-dy*s+.5),math.floor(pivot[2]+dx*s+dy*c+.5)}end;return out,point
end
local function segmentDistance(x,y,a,b)
 local dx,dy=b[1]-a[1],b[2]-a[2];local t=math.max(0,math.min(1,((x-a[1])*dx+(y-a[2])*dy)/math.max(1,dx*dx+dy*dy)));return math.sqrt((x-a[1]-t*dx)^2+(y-a[2]-t*dy)^2)
end
local function extractArm(full,body,shoulder,elbow,hand,near,gunPivot,gunAngle)
 local out=blank()
 for y=0,119 do for x=0,119 do local p=full:getPixel(x,y)
  if pc.rgbaA(p)>0 and not metal[p]and not hair[p]and y>=shoulder[2]-7 then
   local dh=dist({x,y},hand);local upper=segmentDistance(x,y,shoulder,elbow);local lower=segmentDistance(x,y,elbow,hand)
   local inArm=upper<=(near and 10 or 8)or lower<=(near and 7 or 6)or dh<=7
   local validSkin=not skin[p]or(not skin[body:getPixel(x,y)]and dh<=9)
   local axis=gunPivot[2]-8+(x-gunPivot[1])*math.tan(gunAngle*math.pi/180)
   local insideOldGun=x>=gunPivot[1]-32 and y>=axis-6 and y<=axis+7
   local skinEdge=false
   if dh<=9 and(p==rgba('281e32')or p==rgba('47243e')or p==rgba('6d2949'))then
    for yy=math.max(0,y-1),math.min(119,y+1)do for xx=math.max(0,x-1),math.min(119,x+1)do if skin[full:getPixel(xx,yy)]then skinEdge=true end end end
   end
   local cleanWeapon=not insideOldGun or(skin[p]and dh<=9)or skinEdge
   if inArm and validSkin and cleanWeapon and(dh>7 or skin[p]or skinEdge)then out:drawPixel(x,y,p)end
  end
 end end
 -- Repaint the sleeve that the original held gun occluded. This keeps arm files
 -- continuous when the independent weapon is hidden or moved by the animator.
 for y=math.max(0,shoulder[2]-2),119 do for x=0,119 do
  local axis=gunPivot[2]-8+(x-gunPivot[1])*math.tan(gunAngle*math.pi/180)
  if x>=gunPivot[1]-32 and y>=axis-6 and y<=axis+7 and dist({x,y},hand)>6 then
   local u=segmentDistance(x,y,shoulder,elbow)/(near and 9 or 7)
   local v=segmentDistance(x,y,elbow,hand)/(near and 6 or 5)
   local d=math.min(u,v)
   if d<=1 then
    local h=d>.86 and '47243e'or d>.64 and 'a7663a'or 'd3934d'
    if d<.8 then local a,b=(x+y)%12,(x-y)%12;if a<=1 or b<=1 then h='a7663a'elseif a==3 or b==3 then h='efb34a'end end
    out:drawPixel(x,y,rgba(h))
   end
  end
 end end
 local groups=components(out,function(x,y,p)return pc.rgbaA(p)>0 end)
 for i,g in ipairs(groups)do if i>1 and g.count<20 then for _,p in ipairs(g.pts)do out:drawPixel(p[1],p[2],0)end end end
 return out
end
local function handCandidates(full,body,an)
 local gs=components(full,function(x,y,p)return y>an.hair_bounds[2]+an.hair_bounds[4]-2 and skin[p]and not skin[body:getPixel(x,y)]end)
 local out={};for _,g in ipairs(gs)do if g.count>=3 and g.count<300 then out[#out+1]=g end end;return out
end
local function nearestHand(candidates,target,skip)
 local best,hand=1e9,nil;for _,g in ipairs(candidates)do if g~=skip then local d=dist(g.center,target);if d<best then best,hand=d,g end end end
 return hand,best
end
local function round(p)return{math.floor(p[1]+.5),math.floor(p[2]+.5)}end
local function nearestSkin(img,p)
 local best,point=1e9,p;for y=0,119 do for x=0,119 do if skin[img:getPixel(x,y)]then local d=dist({x,y},p);if d<best then best,point=d,{x,y}end end end end;return best,point
end
local function restoreOccludedHand(img,p)
 for y=math.max(0,p[2]-4),math.min(119,p[2]+4)do for x=math.max(0,p[1]-5),math.min(119,p[1]+5)do
  local d=((x-p[1])/5)^2+((y-p[2])/4)^2
  if d<=1 then local h=d>.74 and '47243e'or y>p[2]+1 and 'd98b77'or 'f4bf9f';if d<.55 and y<p[2]then h='ffe1b4'end;img:drawPixel(x,y,rgba(h))end
 end end
end
local names={'Body','BackArm','Weapon','FrontArm','Ammo','Pickup','FX'}
local function newSprite(layerNames)
 local s=Sprite(120,120,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pal);for _,n in ipairs(layerNames or names)do local l=s:newLayer();l.name=n;l.blendMode=BlendMode.NORMAL end;return s
end
local function rectangle(img,x1,y1,x2,y2,h)for y=y1,y2 do for x=x1,x2 do if x>=0 and x<120 and y>=0 and y<120 then img:drawPixel(x,y,rgba(h))end end end end
local clips={{name='Idle',count=4,ms=220},{name='Walk',count=6,ms=110},{name='Reload',count=6,ms=150},{name='Aim',count=3,ms=140},{name='Fire',count=4,ms=100},{name='Collect',count=4,ms=170}}
local poseAngles={Idle={0,0,0,0},Reload={0,0,0,0,0,0},Aim={25,-5,0},Fire={0,-8,16,-3},Collect={25,30,28,20}}
local handOverrides={Reload={
 [2]={front={31,62},back={54,56},gun={59,55}},
 [3]={front={51,56},back={63,54},gun={56,55}},
 [4]={front={55,56},back={63,54},gun={60,55}},
 [5]={front={51,55},back={91,56},gun={56,54}}},
 Aim={[2]={front={54,54},back={93,50}},[3]={front={56,47},back={84,49}}},
 Fire={[2]={front={44,60},back={78,53},gun={49,59},front_shoulder={19,52},back_shoulder={61,52}}}}
local master=newSprite();local poses,records={},{};local fi=0;local idleParts,idleAn
for _,clip in ipairs(clips)do local start=fi+1
 for f=1,clip.count do fi=fi+1;if fi>1 then master:newEmptyFrame()end
  local full=Image{fromFile=root..'/Source/Sampled/'..clip.name..string.format('/Full_%02d.png',f)}
  local body=Image{fromFile=root..'/Source/Sampled/'..clip.name..string.format('/Body_%02d.png',f)};local an=anatomy(body)
  local parts={Body=body,BackArm=blank(),Weapon=blank(),FrontArm=blank(),Ammo=blank(),Pickup=blank(),FX=blank()}
  local pivot,angle,muzzle,method,trigger,support,nh,fh
  local ns,fs=an.front_shoulder,an.back_shoulder
  if clip.name=='Walk'then
   -- Keep the skull/torso on a stable root. Shoe-centroid registration alone
   -- drifts during passing poses and would clip the long independent shotgun.
   local rootShift=math.floor(idleAn.head_center[1]-an.head_center[1]+.5)
   local minBodyX=120;for y=0,119 do for x=0,119 do if pc.rgbaA(body:getPixel(x,y))>0 then minBodyX=math.min(minBodyX,x)end end end
   rootShift=math.max(rootShift,2-minBodyX)
   body=shift(body,rootShift,0);parts.Body=body;an=anatomy(body);ns,fs=an.front_shoulder,an.back_shoulder
   local dx=math.floor(an.head_center[1]-idleAn.head_center[1]+.5);local dy=an.hair_bounds[2]-idleAn.hair_bounds[2]
   for _,n in ipairs({'BackArm','Weapon','FrontArm'})do parts[n]=shift(idleParts[n],dx,dy)end
   local base=records[1];pivot={base.weapon_pivot[1]+dx,base.weapon_pivot[2]+dy};angle=base.weapon_angle;muzzle={base.muzzle[1]+dx,base.muzzle[2]+dy};trigger={pivot[1],pivot[2]};support={base.support_grip[1]+dx,base.support_grip[2]+dy};nh={base.hand_front[1]+dx,base.hand_front[2]+dy};fh={base.hand_back[1]+dx,base.hand_back[2]+dy};method='Independent Idle carried arms on head-registered generated Walk body'
  else
   pivot,angle,muzzle,method=fitGun(full,body,an);angle=poseAngles[clip.name][f];local gun0,point=gunImage(pivot,angle);trigger,support=point(32,12),point(56,12)
   local candidates=handCandidates(full,body,an);local h1=nearestHand(candidates,trigger);local h2=nearestHand(candidates,support,h1)
   nh=h1 and round(h1.center)or trigger;fh=h2 and round(h2.center)or support
   -- The immutable gun is anchored to the drawn gripping hand; barrel regression
   -- only locates the correct hand. This avoids disconnected receivers on small poses.
   if h1 then pivot={h1.center[1]+5,h1.center[2]-1}end
   if clip.name=='Reload'and f==2 then
    local free=nearestHand(candidates,{trigger[1]-18,trigger[2]+15});if free then nh=round(free.center);local hold=nearestHand(candidates,trigger,free);fh=hold and round(hold.center)or trigger end
   elseif clip.name=='Reload'and(f==3 or f==4)then fh={trigger[1]+4,trigger[2]+1}
   elseif clip.name=='Collect'then
    local free=nearestHand(candidates,{105,f==1 and 80 or f==2 and 103 or f==3 and 90 or 60},h1);fh=free and round(free.center)or{100,90}
   end
   local override=handOverrides[clip.name]and handOverrides[clip.name][f]
   if override then nh=override.front or nh;fh=override.back or fh;pivot=override.gun or pivot;ns=override.front_shoulder or ns;fs=override.back_shoulder or fs;method='Manually reviewed drawn hand registration'end
   local ne={ns[1]+5,math.max(ns[2]+11,nh[2]+6)};local fe={(fs[1]+fh[1])/2,math.max(fs[2]+12,fh[2]+8)}
   if fh[2]>fs[2]+25 then fe={(fs[1]+fh[1])/2,fh[2]-12}end
   parts.FrontArm=extractArm(full,body,ns,ne,nh,true,pivot,angle);parts.BackArm=extractArm(full,body,fs,fe,fh,false,pivot,angle)
   if clip.name=='Reload'and(f==3 or f==4)then restoreOccludedHand(parts.BackArm,fh)end
   -- Register the gun against actual drawn skin rather than an unrelated pose origin.
   parts.Weapon,point=gunImage(pivot,angle);trigger,support,muzzle=point(32,12),point(56,12),point(77,4)
   if clip.name=='Reload'and f>=2 and f<=4 then local hp=f==2 and nh or{trigger[1]+2,trigger[2]};rectangle(parts.Ammo,hp[1]+2,hp[2]-4,hp[1]+4,hp[2]+1,'281e32');rectangle(parts.Ammo,hp[1]+3,hp[2]-3,hp[1]+3,hp[2],'ee403d');rectangle(parts.Ammo,hp[1]+2,hp[2]+1,hp[1]+4,hp[2]+1,'efb34a')end
   if clip.name=='Collect'then
    local bagCenter
    if f==1 then bagCenter={106,110}else bagCenter={fh[1],fh[2]+8}end
    for y=math.max(0,bagCenter[2]-9),math.min(119,bagCenter[2]+10)do for x=math.max(0,bagCenter[1]-10),math.min(119,bagCenter[1]+10)do local p=full:getPixel(x,y);if pc.rgbaA(p)>0 and brown[p]and pc.rgbaA(body:getPixel(x,y))==0 and y>=fh[2]+3 then parts.Pickup:drawPixel(x,y,p)end end end
   end
   if clip.name=='Fire'and f==2 then local x,y=muzzle[1],muzzle[2];rectangle(parts.FX,x+1,y-1,x+6,y+1,'efb34a');rectangle(parts.FX,x+4,y-4,x+6,y+4,'ffdc78');rectangle(parts.FX,x+2,y,x+10,y,'fff5d7')end
  end
  local nearDist=nearestSkin(parts.FrontArm,trigger);local farDist=nearestSkin(parts.BackArm,support)
  local record={frame=fi,action=clip.name,local_frame=f,duration_ms=clip.ms,weapon_pivot=round(pivot),weapon_angle=angle,muzzle=round(muzzle),support_grip=round(support),hand_front=round(nh),hand_back=round(fh),shoulder_front=round(ns),shoulder_back=round(fs),upper_offset_xy={0,0},floor_boundary_y=118,trigger_skin_distance=nearDist,support_skin_distance=farDist,registration=method,source='Source/Sampled/'..clip.name..string.format('/Full_%02d.png',f)}
  if clip.name=='Fire'then record.duration_ms=({100,70,110,150})[f]end
  master.frames[fi].duration=record.duration_ms/1000;for li,n in ipairs(names)do master:newCel(master.layers[li],fi,parts[n],Point(0,0))end
  poses[#poses+1]=parts;records[#records+1]=record;if fi==1 then idleParts=parts;idleAn=an end
 end
 local tag=master:newTag(start,fi);tag.name=clip.name
end
master:saveAs(root..'/grandma_01_sundeok_120.aseprite')
for _,clip in ipairs(clips)do
 local out=newSprite();local count=0;for f,r in ipairs(records)do if r.action==clip.name then count=count+1;if count>1 then out:newEmptyFrame()end;out.frames[count].duration=r.duration_ms/1000;for li,n in ipairs(names)do out:newCel(out.layers[li],count,Image(poses[f][n]),Point(0,0))end end end
 local tag=out:newTag(1,count);tag.name=clip.name;out:saveAs(root..'/Clips/sundeok_'..clip.name:lower()..'.aseprite');out:close()
end
app.fs.makeAllDirectories(root..'/Parts/Native')
for li,name in ipairs(names)do
 local out=newSprite({name});for f,r in ipairs(records)do if f>1 then out:newEmptyFrame()end;out.frames[f].duration=r.duration_ms/1000;out:newCel(out.layers[1],f,Image(poses[f][name]),Point(0,0))end
 for _,tag in ipairs(master.tags)do local nt=out:newTag(tag.fromFrame.frameNumber,tag.toFrame.frameNumber);nt.name=tag.name end
 out:saveAs(root..'/Parts/Native/sundeok_'..name:lower()..'.aseprite');out:close()
end
-- Composition demo: generated walking bodies continue while upper weapon action changes.
local demo=newSprite();local demoMeta={}
for f=1,12 do if f>1 then demo:newEmptyFrame()end;demo.frames[f].duration=.11;local wf=5+(f-1)%6;local uf=f>=5 and f<=8 and(20+f-5)or 19
 local wa,ua=anatomy(poses[wf].Body),anatomy(poses[uf].Body);local dx=math.floor(wa.head_center[1]-ua.head_center[1]+.5);local dy=wa.front_shoulder[2]-ua.front_shoulder[2]
 for li,n in ipairs(names)do local img=n=='Body'and poses[wf].Body or shift(poses[uf][n],dx,dy);demo:newCel(demo.layers[li],f,img,Point(0,0))end
 demoMeta[#demoMeta+1]={frame=f,walk_frame=(f-1)%6+1,upper_action=records[uf].action,upper_frame=records[uf].local_frame,upper_translation_xy={dx,dy}}
end
local dt=demo:newTag(1,12);dt.name='Walk_Aim_Fire';demo:saveAs(root..'/sundeok_walk_aim_fire_demo.aseprite');demo:close();master:close()
local function write(path,data)local f=assert(io.open(path,'w'));f:write(json.encode(data));f:close()end
write(root..'/Source/pose_metadata.json',{canvas={120,120},palette=hex,frames=records,clips=clips,layer_order=names,logical_floor_boundary_y=118,weapon_separate_in_idle=true,method='Base-locked built-in ImageGen action sheets + restored bodies; native Aseprite palette mapping, drawn arm masks, immutable independent shotgun'})
write(root..'/Source/demo_metadata.json',demoMeta)
print('Created 6 action clips / 27 layered poses, 7 independent part libraries and 12-frame walk/aim/fire demo.')
