local root=assert(app.params['root']);local pc=app.pixelColor
local meta=json.decode(assert(io.open(root..'/Source/pose_metadata.json')):read('*a'))
local function write(p,t)local f=assert(io.open(p,'w'));f:write(json.encode(t));f:close()end
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local function blank()return Image(120,120,ColorMode.RGB)end
local function flat(s,f)local im=Image(s.spec);im:drawSprite(s,f);return im end
local function cel(s,l,f)local im=Image(s.spec);local c=s.layers[l]:cel(f);if c then im:drawImage(c.image,c.position)end;return im end
local function diff(a,b)assert(a.width==b.width and a.height==b.height);local n=0;for y=0,a.height-1 do for x=0,a.width-1 do if a:getPixel(x,y)~=b:getPixel(x,y)then n=n+1 end end end;return n end
local function flip(a)local im=Image(a.spec);for y=0,a.height-1 do for x=0,a.width-1 do im:drawPixel(a.width-1-x,y,a:getPixel(x,y))end end;return im end
local function scale(a,n)local im=Image(a.width*n,a.height*n,ColorMode.RGB);for y=0,im.height-1 do for x=0,im.width-1 do im:drawPixel(x,y,a:getPixel(x//n,y//n))end end;return im end
local function bg(a,h)local im=Image(a.spec);im:clear(rgba(h));im:drawImage(a);return im end
local function stat(im)
 local r={pixels=0,partial_alpha=0,bounds={120,120,0,0}};local bx,by,ex,ey=120,120,-1,-1;local cs={}
 for y=0,119 do for x=0,119 do local p=im:getPixel(x,y);local a=pc.rgbaA(p);if a>0 then r.pixels=r.pixels+1;cs[p]=true;bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y);if a~=255 then r.partial_alpha=r.partial_alpha+1 end end end end
 if ex>=0 then r.bounds={bx,by,ex-bx+1,ey-by+1};r.last_y=ey end;return r,cs
end
local qa=root..'/QA';for _,d in ipairs({'Export/Left','QA/LayerStates','QA/PreviewSources','UI'})do app.fs.makeAllDirectories(root..'/'..d)end
local m=Sprite{fromFile=root..'/../grandma_05_motorcycle_120.aseprite'};assert(m.width==120 and m.height==120 and #m.frames==18 and #m.tags==3 and #m.layers==8)
local allowed,ps={},{};for _,h in ipairs(meta.palette)do allowed[rgba(h)]=true;ps[#ps+1]=h end
local report={date='2026-10-30',canvas={120,120},frames=18,layers=8,palette_slots=#m.palettes[1],frame_checks={},parts={},partial_alpha=0,outside_palette_pixels=0,png_difference=0,reassembly_difference=0,sheet_difference=0,left_roundtrip_difference=0,visibility_states=0,clip_difference=0,part_library_difference=0,rider_difference=0,vehicle_difference=0,ui_difference=0}
local union,flats,first={}, {},{};local contact=Image(960,360,ColorMode.RGB);contact:clear(rgba('eee5d6'));local overview=Image(360,120,ColorMode.RGB);overview:clear(rgba('eee5d6'));local picks={Idle=1,Ride=3,Collect=4}
for ci,clip in ipairs(meta.clips)do
 local name=clip.name;first[name]=first[name]or 1;app.fs.makeAllDirectories(root..'/Export/Frames/'..name);app.fs.makeAllDirectories(root..'/Export/Left/'..name);app.fs.makeAllDirectories(root..'/Parts/'..name)
 local sh=Image(clip.count*120,120,ColorMode.RGB);local frames={};local prev;local n=0
 local preview=Sprite(360,360,ColorMode.RGB)
 local clipSprite=Sprite{fromFile=root..'/Clips/motorcycle_'..name:lower()..'.aseprite'};assert(#clipSprite.frames==clip.count)
 for f,r in ipairs(meta.frames)do if r.action==name then
  n=n+1;if n==1 then first[name]=f end;local full=flat(m,f);flats[f]=full
  local st,cs=stat(full);assert(st.pixels>2500 and st.partial_alpha==0 and st.last_y==117,'Canvas/alpha/ground frame '..f..': '..json.encode(st))
  assert(st.bounds[1]>=1 and st.bounds[1]+st.bounds[3]-1<=118,'Clipped canvas frame '..f)
  for p in pairs(cs)do union[p]=true;assert(allowed[p],'Outside character palette')end
  if prev then assert(diff(prev,full)>0,'Identical adjacent animation frame')end;prev=full
  local filename=string.format('motorcycle_%s_%02d.png',name:lower(),n);full:saveAs(root..'/Export/Frames/'..name..'/'..filename);assert(diff(full,Image{fromFile=root..'/Export/Frames/'..name..'/'..filename})==0)
  local left=flip(full);left:saveAs(root..'/Export/Left/'..name..'/'..filename);assert(diff(full,flip(left))==0)
  local assembly=blank()
  for li,l in ipairs(m.layers)do
   local p=cel(m,li,f);local s=stat(p);if s.pixels>0 then local b=s.bounds;local crop=Image(b[3],b[4],ColorMode.RGB);crop:drawImage(p,Point(-b[1],-b[2]));local file=string.format('%s_%02d.png',l.name,n);crop:saveAs(root..'/Parts/'..name..'/'..file);assembly:drawImage(Image{fromFile=root..'/Parts/'..name..'/'..file},Point(b[1],b[2]))
    local pivot={60,118};if l.name=='RearWheel'then pivot={r.rear_wheel[1],r.rear_wheel[2]}elseif l.name=='FrontWheel'then pivot={r.front_wheel[1],r.front_wheel[2]}elseif l.name=='FrontArm'then pivot={r.shoulder_front[1],r.shoulder_front[2]}elseif l.name=='BackArm'then pivot={r.shoulder_back[1],r.shoulder_back[2]}elseif l.name=='Pickup'then pivot={b[1]+b[3]//2,b[2]}end
    report.parts[#report.parts+1]={frame=f,action=name,local_frame=n,layer=l.name,png='Parts/'..name..'/'..file,origin_xy={b[1],b[2]},size_px={b[3],b[4]},pivot_canvas_xy=pivot,pivot_crop_xy={pivot[1]-b[1],pivot[2]-b[2]},hand_front={r.hand_front[1],r.hand_front[2]},hand_back={r.hand_back[1],r.hand_back[2]}}
   end
  end
  assert(diff(assembly,full)==0,'Part reassembly mismatch');assert(diff(full,flat(clipSprite,n))==0,'Clip differs from master')
  st.frame=f;st.action=name;st.local_frame=n;st.duration_ms=r.duration_ms;report.frame_checks[#report.frame_checks+1]=st
  sh:drawImage(full,Point((n-1)*120,0));contact:drawImage(full,Point((n-1)*120,(ci-1)*120));if n==picks[name]then overview:drawImage(full,Point((ci-1)*120,0))end
  frames[#frames+1]={frame=n,rect_xywh={(n-1)*120,0,120,120},duration_ms=r.duration_ms,pivot_normalized={.5,2/120}}
  if n>1 then preview:newEmptyFrame()end;preview.frames[n].duration=r.duration_ms/1000;preview:newCel(preview.layers[1],n,scale(bg(full,'eee5d6'),3))
 end end
 sh:saveAs(root..'/Export/Sheets/motorcycle_'..name:lower()..'.png');write(root..'/Export/Sheets/motorcycle_'..name:lower()..'.json',{canvas={120,120},frames=frames,layered=true,walk_slot=name=='Ride'})
 for i=1,clip.count do local test=blank();test:drawImage(Image{fromFile=root..'/Export/Sheets/motorcycle_'..name:lower()..'.png'},Point(-(i-1)*120,0));assert(diff(test,flats[first[name]+i-1])==0)end
 scale(bg(sh,'eee5d6'),2):saveAs(root..'/Previews/'..name..'_strip_2x.png');preview:saveAs(qa..'/PreviewSources/'..name..'_3x.aseprite');preview:close();clipSprite:close()
end
for ci,clip in ipairs(meta.clips)do local f=first[clip.name]+picks[clip.name]-1;local existing={};for li=1,8 do if stat(cel(m,li,f)).pixels>0 then existing[#existing+1]=li end end
 for _,mode in ipairs({'solo','hidden'})do local sh=Image(480,240,ColorMode.RGB);sh:clear(rgba('eee5d6'))
  for k,li in ipairs(existing)do
   for j,l in ipairs(m.layers)do l.isVisible=(mode=='solo'and j==li)or(mode=='hidden'and j~=li)end
   local p=qa..'/LayerStates/'..clip.name..'_'..mode..'_'..li..'.aseprite';m:saveAs(p);local s=Sprite{fromFile=p}
   for j,l in ipairs(s.layers)do assert(l.isVisible==((mode=='solo'and j==li)or(mode=='hidden'and j~=li)))end
   local shown=flat(s,f);shown:saveAs(qa..'/LayerStates/'..clip.name..'_'..mode..'_'..li..'.png');sh:drawImage(bg(shown,'eee5d6'),Point(((k-1)%4)*120,((k-1)//4)*120));s:close();report.visibility_states=report.visibility_states+1
  end;scale(sh,2):saveAs(qa..'/'..clip.name..'_'..mode..'_2x.png')
 end;for _,l in ipairs(m.layers)do l.isVisible=true end;assert(diff(flat(m,f),flats[f])==0)
end
for li,l in ipairs(m.layers)do local s=Sprite{fromFile=root..'/Parts/Native/motorcycle_'..l.name:lower()..'.aseprite'};assert(#s.frames==18 and #s.tags==3);for f=1,18 do assert(diff(flat(s,f),cel(m,li,f))==0)end;s:close()end
local rider=Sprite{fromFile=root..'/motorcycle_rider_120.aseprite'};for f=1,18 do local im=blank();for _,l in ipairs({5,6,7})do im:drawImage(cel(m,l,f))end;assert(diff(im,flat(rider,f))==0)end;rider:close()
local vehicle=Sprite{fromFile=root..'/Vehicle/motorcycle.aseprite'};assert(diff(flat(vehicle,1),Image{fromFile=root..'/Vehicle/motorcycle.png'})==0);vehicle:close()
local count=0;for _ in pairs(union)do count=count+1 end;assert(count<=26 and report.palette_slots==27);report.global_rgb_union=count
contact:saveAs(root..'/Previews/all_poses_native.png');scale(contact,2):saveAs(root..'/Previews/all_poses_2x.png');scale(overview,3):saveAs(root..'/Previews/three_actions_3x.png')
flats[1]:saveAs(root..'/Export/motorcycle_idle.png');scale(bg(flats[1],'eee5d6'),4):saveAs(root..'/Previews/motorcycle_idle_4x.png')
local pair=Image(240,120,ColorMode.RGB);pair:drawImage(bg(flats[1],'eee5d6'));pair:drawImage(bg(flats[1],'252637'),Point(120,0));scale(pair,3):saveAs(root..'/Previews/light_dark_3x.png')
local portrait=blank();local b=cel(m,6,1);local cropped=Image(43,50,ColorMode.RGB);cropped:drawImage(b,Point(-36,-31));portrait:drawImage(scale(cropped,2),Point(17,10))
local lock=blank();for y=0,119 do for x=0,119 do if pc.rgbaA(portrait:getPixel(x,y))>0 then lock:drawPixel(x,y,rgba('191e2b'))end end end
for _,item in ipairs({{'motorcycle_portrait',portrait},{'motorcycle_portrait_locked',lock}})do local s=Sprite(120,120,ColorMode.RGB);s:setPalette(m.palettes[1]);s:newCel(s.layers[1],1,item[2]);s:saveAs(root..'/UI/'..item[1]..'.aseprite');item[2]:saveAs(root..'/UI/'..item[1]..'.png');s:close();local again=Sprite{fromFile=root..'/UI/'..item[1]..'.aseprite'};assert(diff(flat(again,1),Image{fromFile=root..'/UI/'..item[1]..'.png'})==0);again:close()end
m:close();local check=Sprite{fromFile=root..'/../grandma_05_motorcycle_120.aseprite'};for _,l in ipairs(check.layers)do assert(l.isVisible)end;check:close()
local d=Sprite{fromFile=root..'/motorcycle_drive_by_pickup.aseprite'};local dm=json.decode(assert(io.open(root..'/Source/demo_metadata.json')):read('*a'));assert(#d.frames==24)
local dp=Sprite(360,360,ColorMode.RGB);local world=Sprite(720,320,ColorMode.RGB);local strip=Image(960,360,ColorMode.RGB);strip:clear(rgba('eee5d6'))
for f,r in ipairs(dm)do
 assert(r.moving and r.world_distance_px==(f-1)*12 and r.wheel_phase_deg==(f-1)*10)
 local im=flat(d,f);local st,cs=stat(im);assert(st.partial_alpha==0);for color in pairs(cs)do assert(allowed[color])end
 if f>1 then dp:newEmptyFrame();world:newEmptyFrame()end;dp.frames[f].duration=.08;world.frames[f].duration=.08;dp:newCel(dp.layers[1],f,scale(bg(im,'eee5d6'),3))
 local scene=Image(720,320,ColorMode.RGB);scene:clear(rgba('eee5d6'))
 for y=276,319 do for x=0,719 do scene:drawPixel(x,y,rgba('42465b'))end end
 for i=0,11 do local x=(i*72-f*24)%720;for xx=x,math.min(719,x+24)do for y=299,302 do scene:drawPixel(xx,y,rgba('aaa06c'))end end end
 scene:drawImage(scale(im,2),Point(240,40));world:newCel(world.layers[1],f,scene)
 strip:drawImage(im,Point(((f-1)%8)*120,((f-1)//8)*120))
end
dp:saveAs(qa..'/PreviewSources/DriveByPickup_3x.aseprite');world:saveAs(qa..'/PreviewSources/DriveByPickup_World_2x.aseprite');dp:close();world:close();d:close();scale(strip,2):saveAs(root..'/Previews/drive_by_pickup_strip_2x.png');report.continuous_drive_by_frames=24;report.world_forward_step_px=12;report.pickup_grasp_frame=12;report.stow_frame=15
write(root..'/Export/motorcycle_parts.json',{canvas={120,120},palette=ps,parts=report.parts,root_pivot_canvas={60,118},unity_pivot_normalized={.5,2/120},coordinate_system='top-left; x right, y down',bike_rider_separate=true,source_pose_metadata='../Source/pose_metadata.json',unity_runtime_connected=false})
write(qa..'/validation.json',report)
print(string.format('Validated 18 poses / %d colors / %d cropped part slots / %d saved visibility states; native-PNG, clip, part, sheet, left, rider, vehicle and UI equality exact. Continuous moving demo:24.',count,#report.parts,report.visibility_states))
