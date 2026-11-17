local root=assert(app.params['root']);local out=root..'/TacticalWalk';local pc=app.pixelColor
local before=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'};local s=Sprite{fromFile=out..'/Source/grandma_01_sundeok_120_candidate.aseprite'}
local function blank()return Image(120,120,ColorMode.RGB)end
local function cel(sp,li,f)local im=blank();local c=sp.layers[li]:cel(f);if c then im:drawImage(c.image,c.position)end;return im end
local function full(sp,f)local im=blank();im:drawSprite(sp,f);return im end
local function diff(a,b)local n=0;for y=0,119 do for x=0,119 do if a:getPixel(x,y)~=b:getPixel(x,y)then n=n+1 end end end;return n end
local function stat(im)local b={x=120,y=120,ex=-1,ey=-1,n=0};for y=0,119 do for x=0,119 do if pc.rgbaA(im:getPixel(x,y))>0 then b.x=math.min(b.x,x);b.y=math.min(b.y,y);b.ex=math.max(b.ex,x);b.ey=math.max(b.ey,y);b.n=b.n+1 end end end;return b end
local function write(path,t)local f=assert(io.open(path,'w'));f:write(json.encode(t));f:close()end
local function scale(im,n)local a=Image(im.width*n,im.height*n,ColorMode.RGB);for y=0,a.height-1 do for x=0,a.width-1 do a:drawPixel(x,y,im:getPixel(x//n,y//n))end end;return a end
local function sprite(names)local sp=Sprite(120,120,ColorMode.RGB);sp:deleteLayer(sp.layers[1]);sp:setPalette(s.palettes[1]);for _,name in ipairs(names)do local l=sp:newLayer();l.name=name end;return sp end
assert(s.width==120 and s.height==120 and #s.frames==33 and #s.layers==7 and #s.tags==7)
local specs={{'Idle',1,4},{'walk_v1_normal',5,10},{'Reload',11,16},{'Aim',17,19},{'Fire',20,23},{'Collect',24,27},{'walk_v2_tactical',28,33}}
local expected={};for _,v in ipairs(specs)do expected[v[1]]=v end
for _,tag in ipairs(s.tags)do local v=assert(expected[tag.name]);assert(tag.fromFrame.frameNumber==v[2]and tag.toFrame.frameNumber==v[3],'Wrong tag range')end
local allowed,union={},{};for i=1,26 do local c=s.palettes[1]:getColor(i);allowed[pc.rgba(c.red,c.green,c.blue,255)]=true end
local report={date='2026-11-02',frames=33,tags=7,layers=7,original_frames_preserved=27,original_layer_slot_difference=0,normal_walk_difference=0,new_frames=6,new_frame_ms=140,partial_alpha=0,palette_outside=0,visibility_states=0,new_cropped_parts={},checks={}}
for f=1,27 do assert(math.abs(s.frames[f].duration-before.frames[f].duration)<.0001);for li=1,7 do assert(diff(cel(s,li,f),cel(before,li,f))==0,'Original cel modified')end end
local baseWeapon=cel(before,3,19);local bw=stat(baseWeapon);local oldNormal={}
for f=1,6 do oldNormal[f]=full(before,4+f)end
local sheet=Image(720,120,ColorMode.RGB);local contact=Image(720,840,ColorMode.RGB);contact:clear(pc.rgba(238,229,214,255))
for f=1,33 do
 local im=full(s,f);for y=0,119 do for x=0,119 do local p=im:getPixel(x,y);local a=pc.rgbaA(p);if a>0 then assert(a==255 and allowed[p],'Palette/alpha error');union[p]=true end end end
 if f>=28 then
  local n=f-27;local b=stat(im);assert(b.x>=1 and b.ex<=118 and b.y>=1 and b.ey<=117 and b.n>1800,'New frame clipped/empty')
  local png=Image{fromFile=out..'/Export/Frames/sundeok_walk_v2_tactical_'..string.format('%02d',n)..'.png'};assert(diff(png,im)==0,'PNG mismatch')
  assert(math.abs(s.frames[f].duration-.14)<.0001);assert(diff(oldNormal[n],im)>500,'New walk not distinct')
  if n>1 then assert(diff(im,full(s,f-1))>0,'Duplicate gait frame')end
  local w=cel(s,3,f);local wb=stat(w);local shift=blank();shift:drawImage(baseWeapon,Point(wb.x-bw.x,wb.y-bw.y));assert(diff(w,shift)==0,'Independent shotgun was distorted')
  local assembled=blank()
  for li,l in ipairs(s.layers)do local p=cel(s,li,f);local pb=stat(p);if pb.n>0 then
   local crop=Image(pb.ex-pb.x+1,pb.ey-pb.y+1,ColorMode.RGB);crop:drawImage(p,Point(-pb.x,-pb.y));local name=l.name..'_'..string.format('%02d',n)..'.png';crop:saveAs(out..'/Parts/'..name);assembled:drawImage(Image{fromFile=out..'/Parts/'..name},Point(pb.x,pb.y))
   report.new_cropped_parts[#report.new_cropped_parts+1]={frame=f,local_frame=n,layer=l.name,png='Parts/'..name,origin_xy={pb.x,pb.y},size={crop.width,crop.height},pivot_canvas_xy={60,118},pivot_crop_xy={60-pb.x,118-pb.y}}
  end end;assert(diff(assembled,im)==0,'Part reassembly mismatch')
  sheet:drawImage(im,Point((n-1)*120,0));report.checks[#report.checks+1]={frame=f,pixels=b.n,bounds={b.x,b.y,b.ex-b.x+1,b.ey-b.y+1},duration_ms=140,normal_difference=diff(oldNormal[n],im)}
 end
end
for row,v in ipairs(specs)do for f=v[2],v[3]do contact:drawImage(full(s,f),Point((f-v[2])*120,(row-1)*120))end end
sheet:saveAs(out..'/Export/Sheets/sundeok_walk_v2_tactical.png');contact:saveAs(out..'/Previews/all33_poses_native.png');scale(contact,2):saveAs(out..'/Previews/all33_poses_2x.png')
local used=0;for _ in pairs(union)do used=used+1 end;assert(used<=26);report.rgb_union=used
app.fs.makeAllDirectories(out..'/QA/LayerStates')
for _,f in ipairs({28,30,32})do
 for _,mode in ipairs({'solo','hidden'})do for li=1,4 do
  for j,l in ipairs(s.layers)do l.isVisible=mode=='solo'and j==li or mode=='hidden'and j~=li end
  local path=out..'/QA/LayerStates/f'..f..'_'..mode..'_'..li..'.aseprite';s:saveAs(path);local state=Sprite{fromFile=path};for j,l in ipairs(state.layers)do assert(l.isVisible==(mode=='solo'and j==li or mode=='hidden'and j~=li))end
  full(state,f):saveAs(path..'.png');state:close();report.visibility_states=report.visibility_states+1
 end end
end
for _,l in ipairs(s.layers)do l.isVisible=true end
-- All candidate checks passed. Write the requested canonical native and clips.
s:saveAs(root..'/grandma_01_sundeok_120.aseprite')
local names={};for _,l in ipairs(s.layers)do names[#names+1]=l.name end
for _,version in ipairs({{name='walk_v1_normal',start=5},{name='walk_v2_tactical',start=28}})do
 local clip=sprite(names);for n=1,6 do if n>1 then clip:newEmptyFrame()end;local f=version.start+n-1;clip.frames[n].duration=s.frames[f].duration;for li=1,7 do clip:newCel(clip.layers[li],n,cel(s,li,f))end end
 local tag=clip:newTag(1,6);tag.name=version.name;clip:saveAs(root..'/Clips/sundeok_'..version.name..'.aseprite')
 if version.start==5 then clip:saveAs(root..'/Clips/sundeok_walk.aseprite')end
 clip:close()
end
for li,name in ipairs(names)do
 local lib=sprite({name});for f=1,33 do if f>1 then lib:newEmptyFrame()end;lib.frames[f].duration=s.frames[f].duration;lib:newCel(lib.layers[1],f,cel(s,li,f))end;for _,v in ipairs(specs)do local tag=lib:newTag(v[2],v[3]);tag.name=v[1]end
 lib:saveAs(root..'/Parts/Native/sundeok_'..name:lower()..'.aseprite');lib:close()
 local new=sprite({name});for f=1,6 do if f>1 then new:newEmptyFrame()end;new.frames[f].duration=.14;new:newCel(new.layers[1],f,cel(s,li,27+f))end;local tag=new:newTag(1,6);tag.name='walk_v2_tactical';new:saveAs(out..'/Parts/Native/'..name..'.aseprite');new:close()
end
s:close();local saved=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'};assert(#saved.frames==33 and #saved.tags==7);for _,l in ipairs(saved.layers)do assert(l.isVisible)end
for li,name in ipairs(names)do local lib=Sprite{fromFile=root..'/Parts/Native/sundeok_'..name:lower()..'.aseprite'};assert(#lib.frames==33 and #lib.tags==7);for f=1,33 do assert(diff(full(lib,f),cel(saved,li,f))==0,'Library mismatch')end;lib:close()end
for f=1,27 do for li=1,7 do assert(diff(cel(saved,li,f),cel(before,li,f))==0)end end;saved:close();before:close()
report.cropped_part_slots=#report.new_cropped_parts;report.part_library_slots=231;report.passed=true
write(out..'/Export/parts.json',{canvas={120,120},root_pivot={60,118},parts=report.new_cropped_parts});write(out..'/QA/validation.json',report)
print('PASS: installed33 frames/7 exact tags; original27 frames unchanged; new6 poses26-color/binary alpha, independent shotgun and24 saved/reopened layer states; 7 libraries231 slots.')
