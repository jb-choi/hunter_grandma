local root=assert(app.params['root']);local pc=app.pixelColor
local sample=json.decode(assert(io.open(root..'/Source/sampling.json')):read('*a'))
local hex={};for _,h in ipairs(sample.palette)do hex[#hex+1]=h end
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local pal=Palette(27);pal:setColor(0,Color{r=0,g=0,b=0,a=0});for i,h in ipairs(hex)do local p=rgba(h);pal:setColor(i,Color{r=pc.rgbaR(p),g=pc.rgbaG(p),b=pc.rgbaB(p),a=255})end
local function blank()return Image(120,120,ColorMode.RGB)end
local function shift(im,dx,dy)local out=blank();out:drawImage(im,Point(dx,dy));return out end
local function set(hs)local t={};for _,h in ipairs(hs)do t[rgba(h)]=true end;return t end
local skin=set({'ad674e','dc9671','ffd0a0'});local hair=set({'66667c','a2a3b4','d9d5d3'})
local function dst(a,b)return math.sqrt((a[1]-b[1])^2+(a[2]-b[2])^2)end
local function groups(im,pred)
 local seen,gs={},{};for y=0,119 do for x=0,119 do local k=y*120+x;if not seen[k]and pred(x,y,im:getPixel(x,y))then
 local q,i={{x,y}},1;local g={pts={},sx=0,sy=0};seen[k]=true
 while i<=#q do local p=q[i];i=i+1;g.pts[#g.pts+1]=p;g.sx=g.sx+p[1];g.sy=g.sy+p[2]
 for dy=-1,1 do for dx=-1,1 do local xx,yy=p[1]+dx,p[2]+dy;if xx>=0 and xx<120 and yy>=0 and yy<120 then local kk=yy*120+xx;if not seen[kk]and pred(xx,yy,im:getPixel(xx,yy))then seen[kk]=true;q[#q+1]={xx,yy}end end end end end
 g.n=#g.pts;g.c={g.sx/g.n,g.sy/g.n};gs[#gs+1]=g end end end;table.sort(gs,function(a,b)return a.n>b.n end);return gs
end
local function nearestHands(im,target,skip)
 local gs=groups(im,function(x,y,p)return y>60 and skin[p]end);local best,pt=1e9,target
 for _,g in ipairs(gs)do if g.n>=3 and(not skip or dst(g.c,skip)>5)then local d=dst(g.c,target);if d<best then best,pt=d,g.c end end end;return{math.floor(pt[1]+.5),math.floor(pt[2]+.5)}
end
local function seg(x,y,a,b)local dx,dy=b[1]-a[1],b[2]-a[2];local t=math.max(0,math.min(1,((x-a[1])*dx+(y-a[2])*dy)/math.max(1,dx*dx+dy*dy)));return math.sqrt((x-a[1]-t*dx)^2+(y-a[2]-t*dy)^2)end
local function arm(im,sh,el,ha)
 local out=blank()
 for y=math.max(0,sh[2]-5),119 do for x=0,119 do local p=im:getPixel(x,y);local dh=dst({x,y},ha)
  if pc.rgbaA(p)>0 and not hair[p]and p~=rgba('a82932')and p~=rgba('e0493b')and(seg(x,y,sh,el)<=6 or seg(x,y,el,ha)<=5 or dh<=6)and(not skin[p]or dh<8)then out:drawPixel(x,y,p)end
 end end
 local gs=groups(out,function(x,y,p)return pc.rgbaA(p)>0 end);for i,g in ipairs(gs)do if i>1 then for _,p in ipairs(g.pts)do out:drawPixel(p[1],p[2],0)end end end;return out
end
local function retarget(im,sh,old,target)
 local out=blank();local dy,dx=target[2]-old[2],target[1]-old[1];local span=old[2]-sh[2]
 if math.abs(span)<8 then return im end
 local k=1+dy/span;if k<=.3 then return im end
 for y=0,119 do for x=0,119 do local yy=sh[2]+(y-sh[2])/k;local w=math.max(0,math.min(1,(yy-sh[2])/span));local xx=x-dx*w;xx,yy=math.floor(xx+.5),math.floor(yy+.5)
 if xx>=0 and xx<120 and yy>=0 and yy<120 then out:drawPixel(x,y,im:getPixel(xx,yy))end end end;return out
end
local function rect(im,x1,y1,x2,y2,h)for y=y1,y2 do for x=x1,x2 do if x>=0 and x<120 and y>=0 and y<120 then im:drawPixel(x,y,rgba(h))end end end end
local function heldArm(sh,palm)
 local im=blank();local ha={78,72};local el={math.floor((sh[1]+ha[1])/2),math.max(sh[2],ha[2])+3}
 for y=0,119 do for x=0,119 do local d=math.min(seg(x,y,sh,el)/4.5,seg(x,y,el,ha)/3.5)
  if d<=1 then local h=d>.82 and '191e2b'or d>.6 and '123b48'or y<(sh[2]+ha[2])/2 and '397b85'or '215967';if dst({x,y},ha)<6 then h='ffedc3'end;im:drawPixel(x,y,rgba(h))end
 end end
 for y=68,79 do for x=66,77 do local p=palm:getPixel(x,y);if pc.rgbaA(p)>0 then im:drawPixel(x+6,y-2,p)end end end
 return im
end
local function bag(cx,cy)
 local im=blank();local rows={'...oo...','..oaa.o.','...bb...','..obbo..','.obccbo.','obccccbo','obccc cbo','obcccbbo','.oooooo.'}
 for y,row in ipairs(rows)do for x=1,#row do local c=row:sub(x,x);local h=({o='352c3c',a='aaa06c',b='603d2c',c='995c33'})[c];if h then local xx,yy=cx+x-5,cy+y-5;if xx>=0 and xx<120 and yy>=0 and yy<120 then im:drawPixel(xx,yy,rgba(h))end end end end;return im
end
local rear={math.floor(sample.vehicle.native_wheel_centers[1][1]+.5),104}
local front={math.floor(sample.vehicle.native_wheel_centers[2][1]+.5),104}
local raw=Image{fromFile=root..'/Vehicle/motorcycle_raw.png'};local frame=Image(raw)
for y=0,119 do for x=0,119 do
 local inner=dst({x,y},rear)<10 or dst({x,y},front)<10
 local fork=seg(x,y,{front[1]-10,79},front)<1.8
 local pipes=x<86 and y>=99 and y<=106
 if inner and not fork and not pipes then frame:drawPixel(x,y,0)end
end end
local function wheel(center,phase)
 local out=blank();local a=phase*math.pi/180
 for y=center[2]-13,center[2]+13 do for x=center[1]-13,center[1]+13 do if x>=0 and x<120 and y>=0 and y<120 then
  local r=dst({x,y},center);if r<=13 then local h=r>12 and '191e2b'or r>10 and '352c3c'or r>9 and '66667c'or '485675'
  if r<8 then h='273a56';local theta=math.atan(y-center[2],x-center[1])-a;local d=math.abs(math.sin(theta*3));if d<.18 then h='b5c6cb'elseif d<.32 then h='6f8192'end end
  if r<=2 then h=r<=1 and 'b5c6cb'or '66667c'end;out:drawPixel(x,y,rgba(h))end
 end end end;return out
end
local function fx(n)
 local out=blank();for i=1,4 do local x=((i*29-n*9)%109)+1;local y=({110,115,90,108})[i];rect(out,x,y,math.min(118,x+5+i),y,'6f8192')end;return out
end
local names={'MotionFX','RearWheel','FrontWheel','Vehicle','BackArm','Body','FrontArm','Pickup'}
local function sprite(ns)local s=Sprite(120,120,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pal);for _,n in ipairs(ns or names)do local l=s:newLayer();l.name=n end;return s end
local clips={{name='Idle',count=4,ms=180},{name='Ride',count=6,ms=80},{name='Collect',count=8,ms=85}}
local m=sprite();local poses,records={},{};local fi=0;local hold
local targetHands={{72,73},{77,101},{78,108},{74,109},{68,106},{64,83},{43,76},{74,73}}
for _,clip in ipairs(clips)do local start=fi+1;for f=1,clip.count do fi=fi+1;if fi>1 then m:newEmptyFrame()end
 local full=Image{fromFile=root..'/Source/Sampled/'..clip.name..string.format('/Rider_%02d.png',f)};local body=Image{fromFile=root..'/Source/Sampled/'..clip.name..string.format('/Body_%02d.png',f)}
 local ft=sample.frames[fi].helmet_native;local ns={math.floor(ft[1]-11+.5),math.floor(ft[2]+ft[4]+9+.5)}
 for y=math.max(0,math.floor(ft[2])-1),math.min(119,math.floor(ft[2]+ft[4])-1)do for x=math.max(0,math.floor(ft[1]-ft[3]/2)-1),math.min(119,math.ceil(ft[1]+ft[3]/2)+1)do
  local p=body:getPixel(x,y);local h=p==rgba('603d2c')and '414333'or p==rgba('995c33')and '72704f'or p==rgba('c88b4f')and 'aaa06c'or nil;if h then body:drawPixel(x,y,rgba(h))end
 end end
 local fs={math.floor(ft[1]-2+.5),ns[2]-2}
 local target=clip.name=='Collect'and targetHands[f]or{73,72};local nh=nearestHands(full,target)
 local fh=nearestHands(full,{80,70},nh)
 local ne={math.floor((ns[1]+nh[1])/2+.5),nh[2]>ns[2]+15 and nh[2]-12 or math.max(ns[2]+7,nh[2]+2)}
 local fe={math.floor((fs[1]+fh[1])/2+.5),math.max(fs[2]+6,fh[2]+3)}
 local near=arm(full,ns,ne,nh);local far=arm(full,fs,fe,fh);local oldHand={nh[1],nh[2]}
 if clip.name=='Collect'and f>=2 and f<=5 then near=retarget(near,ns,nh,target);nh={target[1],target[2]}end
 if clip.name~='Collect'or f==1 or f==8 then local dx,dy=72-nh[1],74-nh[2];near=shift(near,dx,dy);ns={ns[1]+dx,ns[2]+dy};nh={72,74}end
 if clip.name=='Collect'and f==7 then near=shift(near,39-nh[1],82-nh[2]);ns={ns[1]+39-nh[1],ns[2]+82-nh[2]};nh={39,82}end
 if fi==1 then hold=Image(near)end
 far=heldArm(fs,hold);fh={78,72}
 local moving=clip.name~='Idle';local phase=moving and(fi-5)*10 or 0
 local parts={MotionFX=moving and fx(fi)or blank(),RearWheel=wheel(rear,phase),FrontWheel=wheel(front,phase),Vehicle=Image(frame),BackArm=far,Body=body,FrontArm=near,Pickup=blank()}
 local pickupState='none'
 if clip.name=='Collect'then
  if f<=3 then local px=({110,98,86})[f];parts.Pickup=bag(px,113);pickupState='ground_approaching'
  elseif f==4 then parts.Pickup=bag(nh[1],113);pickupState='grasp'
  elseif f<=7 then parts.Pickup=bag(nh[1],nh[2]+5);pickupState=f==7 and 'stow'or 'carried' end
 end
 local rec={frame=fi,action=clip.name,local_frame=f,duration_ms=clip.ms,moving=moving,wheel_phase_deg=phase,rear_wheel=rear,front_wheel=front,wheel_radius=13,shoulder_front=ns,shoulder_back=fs,hand_front=nh,hand_front_original=oldHand,hand_back=fh,boot_anchor={57,101},pickup_state=pickupState,ground_boundary_y=118,source='Source/Sampled/'..clip.name..string.format('/Rider_%02d.png',f)}
 m.frames[fi].duration=clip.ms/1000;for li,n in ipairs(names)do m:newCel(m.layers[li],fi,parts[n],Point(0,0))end;poses[fi]=parts;records[fi]=rec
 end;local t=m:newTag(start,fi);t.name=clip.name end
m:saveAs(root..'/../grandma_05_motorcycle_120.aseprite')
for _,clip in ipairs(clips)do local out=sprite();local n=0;for f,r in ipairs(records)do if r.action==clip.name then n=n+1;if n>1 then out:newEmptyFrame()end;out.frames[n].duration=r.duration_ms/1000;for li,k in ipairs(names)do out:newCel(out.layers[li],n,poses[f][k],Point(0,0))end end end;local t=out:newTag(1,n);t.name=clip.name;out:saveAs(root..'/Clips/motorcycle_'..clip.name:lower()..'.aseprite');out:close()end
for _,name in ipairs(names)do local out=sprite({name});for f,r in ipairs(records)do if f>1 then out:newEmptyFrame()end;out.frames[f].duration=r.duration_ms/1000;out:newCel(out.layers[1],f,poses[f][name],Point(0,0))end;for _,t in ipairs(m.tags)do local nt=out:newTag(t.fromFrame.frameNumber,t.toFrame.frameNumber);nt.name=t.name end;out:saveAs(root..'/Parts/Native/motorcycle_'..name:lower()..'.aseprite');out:close()end
local v=sprite({'RearWheel','FrontWheel','Vehicle'});v:newCel(v.layers[1],1,wheel(rear,0));v:newCel(v.layers[2],1,wheel(front,0));v:newCel(v.layers[3],1,frame);v:saveAs(root..'/Vehicle/motorcycle.aseprite');local vi=blank();vi:drawSprite(v,1);vi:saveAs(root..'/Vehicle/motorcycle.png');v:close()
local rider=sprite({'BackArm','Body','FrontArm'});for f,r in ipairs(records)do if f>1 then rider:newEmptyFrame()end;rider.frames[f].duration=r.duration_ms/1000;for li,n in ipairs({'BackArm','Body','FrontArm'})do rider:newCel(rider.layers[li],f,poses[f][n])end end;for _,t in ipairs(m.tags)do local nt=rider:newTag(t.fromFrame.frameNumber,t.toFrame.frameNumber);nt.name=t.name end;rider:saveAs(root..'/motorcycle_rider_120.aseprite');rider:close()
local demo=sprite();local dmeta={}
for f=1,24 do if f>1 then demo:newEmptyFrame()end;demo.frames[f].duration=.08;local sf=f<=8 and(5+(f-1)%6)or f<=16 and(11+f-9)or(5+(f-17+4)%6)
 for li,n in ipairs(names)do local im=poses[sf][n];if n=='RearWheel'then im=wheel(rear,(f-1)*10)elseif n=='FrontWheel'then im=wheel(front,(f-1)*10)elseif n=='MotionFX'then im=fx(f)end;demo:newCel(demo.layers[li],f,im)end
 dmeta[#dmeta+1]={frame=f,source_frame=sf,moving=true,wheel_phase_deg=(f-1)*10,world_distance_px=(f-1)*12,pickup_event=f==12 and 'grasp'or f==15 and 'stow'or 'none'}
end
demo:saveAs(root..'/motorcycle_drive_by_pickup.aseprite');demo:close();m:close()
local function write(p,t)local f=assert(io.open(p,'w'));f:write(json.encode(t));f:close()end
write(root..'/Source/pose_metadata.json',{date='2026-10-30',canvas={120,120},palette=hex,clips=clips,layer_order=names,frames=records,motorcycle_walk_slot='Ride',latest_user_overrides={'olive rounded level-1-style helmet','drive-by pickup without stopping or dismounting'},method='Base-referenced ImageGen drawings and restored layers; native palette mapping, limb registration, complete wheels/spoke phases and separate pickup/FX'})
write(root..'/Source/demo_metadata.json',dmeta)
print('Built 18 frames / Idle,Ride,Collect; 8 layers, separate vehicle and rider; 24-frame continuous moving pickup demo.')
