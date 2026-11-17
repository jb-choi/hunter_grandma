-- ImageGen-authored redesign -> native layered 120px prototype, independent shotgun.
local root=assert(app.params['root']);local pc=app.pixelColor
local paletteHex={'281e32','47243e','6d2949','ae615b','d98b77','f4bf9f','ffe1b4','fff5d7','57475f','7d6c86','aa9ba9','d7ccc8','744632','a7663a','d3934d','efb34a','ffdc78','4a5034','a1455e','d66b6e','ee403d','7b874f','30334d','424a61','7c8d9e','5f627a'}
local pal=Palette(27);pal:setColor(0,Color{r=0,g=0,b=0,a=0})
local ink={};local function color(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
for i,h in ipairs(paletteHex)do local p=color(h);ink[h]=p;pal:setColor(i,Color{r=pc.rgbaR(p),g=pc.rgbaG(p),b=pc.rgbaB(p),a=255})end
local function blank()return Image(120,120,ColorMode.RGB)end
local function shift(img,x,y)local out=blank();out:drawImage(img,Point(math.floor(x+.5),math.floor(y+.5)));return out end
local function rect(im,x1,y1,x2,y2,c)for y=y1,y2 do for x=x1,x2 do if x>=0 and x<im.width and y>=0 and y<im.height then im:drawPixel(x,y,color(c))end end end end
local function inside(x,y,poly)local c=false;local j=#poly;for i,p in ipairs(poly)do local q=poly[j];if((p[2]>y)~=(q[2]>y))and(x<(q[1]-p[1])*(y-p[2])/(q[2]-p[2])+p[1])then c=not c end;j=i end;return c end
local full=Image{fromFile=root..'/Source/redesign_full_120.png'};local bodyBase=Image{fromFile=root..'/Source/redesign_body_120.png'};local gunBase=Image{fromFile=root..'/Source/redesign_shotgun_sampled.png'}
local function extract(poly,skinOnly)
 local out=blank();for y=0,119 do for x=0,119 do if inside(x+.5,y+.5,poly)then local p=full:getPixel(x,y);local metal=p==color('30334d')or p==color('424a61')or p==color('7c8d9e')or p==color('5f627a');if not metal and(not skinOnly or p==color('ae615b')or p==color('d98b77')or p==color('f4bf9f')or p==color('ffe1b4')or p==color('fff5d7')or p==color('281e32')or p==color('47243e'))then out:drawPixel(x,y,p)end end end end;return out
end
local nearUpper=extract({{15,43},{24,42},{32,47},{36,55},{35,63},{28,66},{18,63},{13,55}})
local nearLower=extract({{32,57},{44,53},{49,49},{56,48},{63,50},{64,55},{59,59},{51,60},{46,64},{36,66},{30,63}})
local farUpper=extract({{61,54},{69,54},{76,56},{80,61},{77,67},{68,66},{62,61}})
local farLower=extract({{75,58},{80,54},{81,50},{85,48},{92,49},{94,54},{90,59},{85,61},{81,65},{76,64}})
local function warp(image,origin,dest,angle,stretch)
 local out=blank();local a=angle*math.pi/180;local c,s=math.cos(a),math.sin(a);stretch=stretch or 1
 for y=0,119 do for x=0,119 do local dx,dy=x-dest[1],y-dest[2];local sx=math.floor(origin[1]+(dx*c+dy*s)/stretch+.5);local sy=math.floor(origin[2]+(-dx*s+dy*c)/stretch+.5)
  if sx>=0 and sx<image.width and sy>=0 and sy<image.height then out:drawPixel(x,y,image:getPixel(sx,sy))end
 end end;return out
end
local function ang(a,b)return math.atan(b[2]-a[2],b[1]-a[1])*180/math.pi end
local function dist(a,b)return math.sqrt((b[1]-a[1])^2+(b[2]-a[2])^2)end
local function segment(image,a,b,ta,tb)return warp(image,a,ta,ang(ta,tb)-ang(a,b),dist(ta,tb)/dist(a,b))end
local function arm(far,shoulder,elbow,hand)
 local a,b,c=far and{64,52}or{23,47},far and{76,63}or{34,62},far and{85,54}or{60,54}
 local upper=segment(far and farUpper or nearUpper,a,b,shoulder,elbow);upper:drawImage(segment(far and farLower or nearLower,b,c,elbow,hand));return upper
end
local function weapon(x,y,angle)
 local out=warp(gunBase,{32,12},{x,y},angle,1);local a=angle*math.pi/180;local c,s=math.cos(a),math.sin(a)
 local function point(px,py)local dx,dy=px-32,py-12;return{math.floor(x+dx*c-dy*s+.5),math.floor(y+dx*s+dy*c+.5)}end
 return out,point
end
local function body(action,f,bob,lean,depth)
 local out=blank()
 if action~='Walk'then for y=80,119 do for x=0,119 do out:drawPixel(x,y,bodyBase:getPixel(x,y))end end
 else
  local phase=(f-1)*math.pi/3
  for _,side in ipairs({1,-1})do
   local stride=side*math.sin(phase)*9;local lift=math.max(0,side*math.cos(phase))*5
   for y=80,117 do for x=0,119 do local sourceSpan=side==1 and 35 or 37;local sy=80+(y-80)*sourceSpan/(37-lift);local factor=(sy-80)/sourceSpan;local sx=x-stride*factor;local ix,iy=math.floor(sx+.5),math.floor(sy+.5)
    if ix>=0 and ix<120 and iy>=80 and iy<120 and((side==1 and ix<50)or(side==-1 and ix>=50))then local p=bodyBase:getPixel(ix,iy);if pc.rgbaA(p)>0 then out:drawPixel(x,y,p)end end
   end end
  end
 end
 -- Draw the overlapping coat hem last to keep the waist closed while walking.
 for y=0,84 do for x=0,119 do local p=bodyBase:getPixel(x,y);if pc.rgbaA(p)>0 then local xx,yy=x+lean,y+bob+depth;if xx>=0 and xx<120 and yy>=0 and yy<120 then out:drawPixel(xx,yy,p)end end end end
 return out
end
local clips={{name='Idle',count=4,ms=220},{name='Walk',count=6,ms=110},{name='Reload',count=6,ms=150},{name='Aim',count=3,ms=140},{name='Fire',count=4,ms=90},{name='Collect',count=4,ms=160}}
local names={'Body','BackArm','Weapon','FrontArm','Ammo','Pickup','FX'}
local function pose(action,f)
 local bob,lean,depth=0,0,0;local wx,wy,angle=61,55,0
 if action=='Idle'then bob=({0,-1,-2,-1})[f];angle=2
 elseif action=='Walk'then bob=({0,-1,-2,0,-1,-2})[f];angle=5
 elseif action=='Aim'then angle=({5,1,-3})[f]
 elseif action=='Fire'then wx=61+({-3,-5,-2,0})[f];angle=({-8,-5,-1,0})[f];bob=({-1,0,0,0})[f]
 elseif action=='Reload'then wy=60;angle=({15,15,15,15,5,0})[f]
 elseif action=='Collect'then lean=({2,8,8,2})[f];depth=({2,12,12,2})[f];wx=64;wy=67;angle=20 end
 local gun,point=weapon(wx,wy+bob,angle);local trigger=point(32,12);local support=point(56,12);local muzzle=point(77,4)
 local ns,fs={23+lean,47+bob+depth},{64+lean,52+bob+depth};local ne,fe={34+lean,62+bob+depth},{76+lean,63+bob+depth}
 local nh,fh={trigger[1]-1,trigger[2]-1},support
 local ammo,pickup,fx=blank(),blank(),blank()
 if action=='Reload'then fh=({support,{51,76},{62,67},{69,64},{78,60},support})[f];fe={68,66}
  if f>=2 and f<=4 then rect(ammo,fh[1]-1,fh[2]-6,fh[1]+2,fh[2]-1,'281e32');rect(ammo,fh[1],fh[2]-5,fh[1]+1,fh[2]-2,'ee403d');rect(ammo,fh[1],fh[2]-1,fh[1]+2,fh[2]-1,'efb34a')end
 elseif action=='Collect'then
  fh=support;fe={77,80};nh=({{65,82},{70,100},{70,96},{64,76}})[f];ne=({{38,68},{43,79},{43,77},{38,66}})[f]
  if f>=2 then local bx,by=nh[1],nh[2]+5;rect(pickup,bx-4,by-3,bx+4,by+3,'281e32');rect(pickup,bx-3,by-2,bx+3,by+2,'a7663a');rect(pickup,bx-2,by-4,bx+2,by-4,'ffe1b4')end
 end
 if action=='Fire'and f<=2 then local x,y=muzzle[1],muzzle[2];rect(fx,x+1,y-1,x+6,y+1,'efb34a');rect(fx,x+4,y-4,x+6,y+4,'ffdc78');rect(fx,x+2,y,x+9,y,'fff5d7');if f==2 then rect(fx,76,36,79,37,'efb34a')end end
 local parts={Body=body(action,f,bob,lean,depth),BackArm=arm(true,fs,fe,fh),Weapon=gun,FrontArm=arm(false,ns,ne,nh),Ammo=ammo,Pickup=pickup,FX=fx}
 return parts,{action=action,local_frame=f,shoulder_back=fs,shoulder_front=ns,weapon_pivot=trigger,hand_back=fh,hand_front=nh,muzzle=muzzle,upper_offset_xy={lean,bob+depth},floor_boundary_y=118}
end
local function sprite(w,h,layerNames)local s=Sprite(w or 120,h or 120,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pal);for _,n in ipairs(layerNames or names)do local l=s:newLayer();l.name=n;l.blendMode=BlendMode.NORMAL end;return s end
local function composite(parts)local out=blank();for _,n in ipairs(names)do out:drawImage(parts[n])end;return out end
local s=sprite();local records={};local fi=0
for _,clip in ipairs(clips)do local start=fi+1;for f=1,clip.count do fi=fi+1;if fi>1 then s:newEmptyFrame()end;local parts,m=pose(clip.name,f);m.frame=fi;m.duration_ms=clip.ms
 if clip.name=='Fire'then m.duration_ms=({60,90,110,150})[f]end;s.frames[fi].duration=m.duration_ms/1000
 for li,n in ipairs(names)do s:newCel(s.layers[li],fi,parts[n],Point(0,0))end;records[#records+1]=m
end;local tag=s:newTag(start,fi);tag.name=clip.name end
s:saveAs(root..'/grandma_01_sundeok_120.aseprite')
for _,clip in ipairs(clips)do local out=sprite();local ci=0;for f,r in ipairs(records)do if r.action==clip.name then ci=ci+1;if ci>1 then out:newEmptyFrame()end;out.frames[ci].duration=r.duration_ms/1000
 for li=1,#names do local c=s.layers[li]:cel(f);if c then out:newCel(out.layers[li],ci,Image(c.image),c.position)end end
end end;local tag=out:newTag(1,ci);tag.name=clip.name;out:saveAs(root..'/Clips/sundeok_'..clip.name:lower()..'.aseprite');out:close()end;s:close()
-- Independent weapon source: no body/arms, complete stock and trigger guard.
local weapons=root..'/Weapons/Shotgun';app.fs.makeAllDirectories(weapons)
local standalone=sprite(gunBase.width,gunBase.height,{'Shotgun_Complete'});standalone:newCel(standalone.layers[1],1,gunBase,Point(0,0));standalone:saveAs(weapons..'/sundeok_shotgun.aseprite');standalone:close();gunBase:saveAs(weapons..'/sundeok_shotgun.png')
local function standalonePart(path,name,img)
 app.fs.makeAllDirectories(root..'/Parts/'..path);local out=sprite(120,120,{name});out:newCel(out.layers[1],1,img,Point(0,0));out:saveAs(root..'/Parts/'..path..'/sundeok_'..name:lower()..'_base.aseprite');out:close();img:saveAs(root..'/Parts/'..path..'/sundeok_'..name:lower()..'_base.png')
end
standalonePart('Body','Body',bodyBase)
local near=Image(nearUpper);near:drawImage(nearLower);standalonePart('Arms','FrontArm',near)
local far=Image(farUpper);far:drawImage(farLower);standalonePart('Arms','BackArm',far)
-- Walking-body + independent aim/fire weapon/arms demonstration.
local demo=sprite();local demoMeta={}
for f=1,12 do if f>1 then demo:newEmptyFrame()end;demo.frames[f].duration=.11;local wf=(f-1)%6+1;local walk=pose('Walk',wf);local action=f>=5 and f<=8 and'Fire'or'Aim';local localFrame=action=='Fire'and(f-4)or 3;local upper,m=pose(action,localFrame);local bob=({0,-1,-2,0,-1,-2})[wf]
 for li,n in ipairs(names)do local img=n=='Body'and walk.Body or shift(upper[n],0,bob-m.upper_offset_xy[2]);demo:newCel(demo.layers[li],f,img,Point(0,0))end
 demoMeta[#demoMeta+1]={frame=f,walk_frame=wf,upper_action=action,upper_frame=localFrame,upper_translation_y=bob-m.upper_offset_xy[2]}
end;local tag=demo:newTag(1,12);tag.name='Walk_Aim_Fire';demo:saveAs(root..'/sundeok_walk_aim_fire_demo.aseprite');demo:close()
local function write(p,t)local f=assert(io.open(p,'w'));f:write(json.encode(t));f:close()end
write(root..'/Source/pose_metadata.json',{canvas={120,120},palette=paletteHex,frames=records,clips=clips,layer_order=names,logical_floor_boundary_y=118,idle_policy='Body and arms remain editable; weapon is separate even in Idle; flattened Idle PNGs are convenience outputs',reference='Three built-in ImageGen assets (full concept, restored armless body, independent complete shotgun), native Aseprite sampling and palette mapping, masked/articulated arms; animation prototypes'})
write(root..'/Source/demo_metadata.json',demoMeta)
write(weapons..'/sundeok_shotgun.json',{canvas={gunBase.width,gunBase.height},pivot_xy={32,12},trigger_grip_xy={32,12},support_grip_xy={56,12},muzzle_xy={77,4},native='sundeok_shotgun.aseprite',png='sundeok_shotgun.png',source='../../Source/Generated/sundeok_shotgun_generated.png',palette_scope='Shared Sundeok 26 opaque colors; no character/hand pixels'})
print('Redesigned master and six clips saved, with independent 78x17 shotgun native/PNG and separate body/arms.')
