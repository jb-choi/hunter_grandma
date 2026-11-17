local root=assert(app.params['root']);local out=root..'/TacticalV2';local pc=app.pixelColor
local before=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'}
local s=Sprite{fromFile=out..'/QA/Candidates/grandma_01_sundeok_120.aseprite'}
local function blank()return Image(120,120,ColorMode.RGB)end
local function cel(sp,li,f)local im=blank();local c=sp.layers[li]:cel(f);if c then im:drawImage(c.image,c.position)end;return im end
local function full(sp,f)local im=blank();im:drawSprite(sp,f);return im end
local function diff(a,b)local n=0;for y=0,119 do for x=0,119 do if a:getPixel(x,y)~=b:getPixel(x,y)then n=n+1 end end end;return n end
local function stat(im)local b={x=120,y=120,ex=-1,ey=-1,n=0};for y=0,119 do for x=0,119 do if pc.rgbaA(im:getPixel(x,y))>0 then b.x=math.min(b.x,x);b.y=math.min(b.y,y);b.ex=math.max(b.ex,x);b.ey=math.max(b.ey,y);b.n=b.n+1 end end end;return b end
local function foot(im)local l,r=120,-1;for y=100,117 do for x=0,119 do if pc.rgbaA(im:getPixel(x,y))>0 then l=math.min(l,x);r=math.max(r,x)end end end;return r-l+1 end
local function write(path,t)local f=assert(io.open(path,'w'));f:write(json.encode(t));f:close()end
local function scale(im,n)local a=Image(im.width*n,im.height*n,ColorMode.RGB);for y=0,a.height-1 do for x=0,a.width-1 do a:drawPixel(x,y,im:getPixel(x//n,y//n))end end;return a end
local function sprite(names)local sp=Sprite(120,120,ColorMode.RGB);sp:deleteLayer(sp.layers[1]);sp:setPalette(s.palettes[1]);for _,name in ipairs(names)do local l=sp:newLayer();l.name=name end;return sp end
local specs={{'Idle',1,4},{'walk_v1_normal',5,10},{'reload_v1_normal',11,16},{'Aim',17,19},{'fire_v1_normal',20,23},{'Collect',24,27},{'walk_v2_tactical',28,33},{'reload_v2_tactical',34,39},{'fire_v2_tactical',40,43}}
local names={'Body','BackArm','Weapon','FrontArm','Ammo','Pickup','FX'}
assert(#before.frames==33,'Canonical changed; inspect before install')
assert(s.width==120 and s.height==120 and #s.frames==43 and #s.layers==7 and #s.tags==9)
local expected={};for _,v in ipairs(specs)do expected[v[1]]=v end
for _,tag in ipairs(s.tags)do local v=assert(expected[tag.name]);assert(tag.fromFrame.frameNumber==v[2]and tag.toFrame.frameNumber==v[3],'Wrong tag range')end
for li,l in ipairs(s.layers)do assert(l.name==names[li]and l.opacity==255 and l.isVisible and l.blendMode==BlendMode.NORMAL)end
local allowed,union={},{};for i=1,26 do local c=s.palettes[1]:getColor(i);allowed[pc.rgba(c.red,c.green,c.blue,255)]=true end
local report={date='2026-11-02',canvas={120,120},frames=43,tags=9,layers=7,original_frames_preserved=27,original_layer_slot_difference=0,partial_alpha=0,palette_outside=0,visibility_states=0,new_cropped_parts={},checks={},footprint_old={},footprint_new={}}
for f=1,27 do assert(math.abs(s.frames[f].duration-before.frames[f].duration)<.0001);for li=1,7 do assert(diff(cel(s,li,f),cel(before,li,f))==0,'Original cel modified')end end
local bw=stat(cel(before,3,19));local weapon=cel(before,3,19)
local oldSum,newSum=0,0
for n=1,6 do local old=foot(cel(before,1,27+n));local new=foot(cel(s,1,27+n));oldSum=oldSum+old;newSum=newSum+new;report.footprint_old[n]=old;report.footprint_new[n]=new end
report.footprint_ratio=newSum/oldSum;assert(report.footprint_ratio<.8,'Walking footprint not sufficiently narrower')
for f=1,43 do
 local im=full(s,f)
 for li=1,7 do local p=cel(s,li,f);for y=0,119 do for x=0,119 do local px=p:getPixel(x,y);local a=pc.rgbaA(px);if a>0 then assert(a==255 and allowed[px],'Palette/alpha error');union[px]=true end end end end
 if f>=28 then
  local spec=f<=33 and specs[7]or f<=39 and specs[8]or specs[9];local n=f-spec[2]+1;local tag=spec[1];local b=stat(im)
  assert(b.x>=1 and b.ex<=118 and b.y>=1 and b.ey<=117 and b.n>1800,'Clipped/empty frame')
  local png=Image{fromFile=out..'/Export/Frames/'..tag..'/'..string.format('%02d.png',n)};assert(png.width==120 and png.height==120 and diff(png,im)==0,'PNG mismatch')
  local body=Image{fromFile=out..'/Source/Sampled/'..tag..'/Body_'..string.format('%02d.png',n)};assert(diff(body,cel(s,1,f))==0,'Body mismatch')
  local left=Image{fromFile=out..'/Export/Left/'..tag..'/'..string.format('%02d.png',n)}
  for y=0,119 do for x=0,119 do assert(left:getPixel(119-x,y)==im:getPixel(x,y),'Left flip mismatch')end end
  local sheet=Image{fromFile=out..'/Export/Sheets/'..tag..'.png'};local cell=blank();cell:drawImage(sheet,Point(-(n-1)*120,0));assert(diff(cell,im)==0,'Sheet mismatch')
  if n>1 then assert(diff(im,full(s,f-1))>0,'Adjacent duplicate frame')end
  local w=cel(s,3,f);local wb=stat(w);local translated=blank();translated:drawImage(weapon,Point(wb.x-bw.x,wb.y-bw.y));assert(diff(translated,w)==0,'Shotgun changed or distorted')
  assert((stat(cel(s,7,f)).n>0)==(f==41),'Unexpected FX');assert(stat(cel(s,6,f)).n==0,'Unexpected Pickup')
  if f>=34 and f<=39 then assert((stat(cel(s,5,f)).n>0)==(stat(cel(before,5,10+n)).n>0),'Reload shell missing')end
  if f>=34 then
   local anchor=cel(s,1,28);local current=cel(s,1,f)
   for y=83,119 do for x=0,119 do assert(anchor:getPixel(x,y)==current:getPixel(x,y),'Planted legs/feet slid')end end
  end
  local assembled=blank()
  for li,l in ipairs(s.layers)do local p=cel(s,li,f);local pb=stat(p);if pb.n>0 then
   local crop=Image(pb.ex-pb.x+1,pb.ey-pb.y+1,ColorMode.RGB);crop:drawImage(p,Point(-pb.x,-pb.y));local path='Parts/Frames/'..tag..'/'..l.name..'_'..string.format('%02d',n)..'.png';crop:saveAs(out..'/'..path);assembled:drawImage(Image{fromFile=out..'/'..path},Point(pb.x,pb.y))
   report.new_cropped_parts[#report.new_cropped_parts+1]={frame=f,local_frame=n,action=tag,layer=l.name,png=path,origin_xy={pb.x,pb.y},size={crop.width,crop.height},pivot_canvas_xy={60,118},pivot_crop_xy={60-pb.x,118-pb.y}}
  end end;assert(diff(assembled,im)==0,'Part reassembly mismatch')
  report.checks[#report.checks+1]={frame=f,action=tag,pixels=b.n,bounds={b.x,b.y,b.ex-b.x+1,b.ey-b.y+1},duration_ms=math.floor(s.frames[f].duration*1000+.5),foot_span=foot(cel(s,1,f))}
 end
end
local used=0;for _ in pairs(union)do used=used+1 end;assert(used<=26);report.rgb_union=used
app.fs.makeAllDirectories(out..'/QA/LayerStates')
for _,f in ipairs({28,35,41})do
 local contact=Image(840,240,ColorMode.RGB);contact:clear(pc.rgba(238,229,214,255))
 for mi,mode in ipairs({'solo','hidden'})do for li=1,7 do
  local predicted=blank()
  for j,l in ipairs(s.layers)do
   l.isVisible=(mode=='solo'and j==li)or(mode=='hidden'and j~=li)
   if l.isVisible then predicted:drawImage(cel(s,j,f))end
  end
  local path=out..'/QA/LayerStates/f'..f..'_'..mode..'_'..li..'.aseprite';s:saveAs(path)
  local state=Sprite{fromFile=path};for j,l in ipairs(state.layers)do assert(l.isVisible==((mode=='solo'and j==li)or(mode=='hidden'and j~=li)))end
  local visible=full(state,f);assert(diff(visible,predicted)==0,'Saved visible rendering mismatch');visible:saveAs(path..'.png');state:close()
  contact:drawImage(visible,Point((li-1)*120,(mi-1)*120));report.visibility_states=report.visibility_states+1
 end end
 contact:saveAs(out..'/QA/f'..f..'_solo_hidden_native.png');scale(contact,2):saveAs(out..'/QA/f'..f..'_solo_hidden_2x.png')
end
for _,l in ipairs(s.layers)do l.isVisible=true end
local contact=Image(720,1080,ColorMode.RGB);contact:clear(pc.rgba(238,229,214,255))
for row,v in ipairs(specs)do for f=v[2],v[3]do contact:drawImage(full(s,f),Point((f-v[2])*120,(row-1)*120))end end
contact:saveAs(out..'/Previews/all43_poses_native.png');scale(contact,2):saveAs(out..'/Previews/all43_poses_2x.png')
local contrast=Image(720,720,ColorMode.RGB)
for ai=1,3 do local v=specs[6+ai];for mi,c in ipairs({pc.rgba(238,229,214,255),pc.rgba(31,28,39,255)})do for f=v[2],v[3]do local bg=blank();bg:clear(c);bg:drawImage(full(s,f));contrast:drawImage(bg,Point((f-v[2])*120,((ai-1)*2+mi-1)*120))end end end
contrast:saveAs(out..'/QA/light_dark_native.png');scale(contrast,2):saveAs(out..'/QA/light_dark_2x.png')
-- Install only after all candidate checks above pass.
s:saveAs(root..'/grandma_01_sundeok_120.aseprite')
contact:saveAs(root..'/Previews/all_poses_native.png');scale(contact,2):saveAs(root..'/Previews/all_poses_2x.png')
for _,v in ipairs({specs[2],specs[3],specs[5],specs[7],specs[8],specs[9]})do
 local clip=sprite(names)
 for f=v[2],v[3]do local n=f-v[2]+1;if n>1 then clip:newEmptyFrame()end;clip.frames[n].duration=s.frames[f].duration;for li=1,7 do clip:newCel(clip.layers[li],n,cel(s,li,f))end end
 local tag=clip:newTag(1,v[3]-v[2]+1);tag.name=v[1];local path=root..'/Clips/sundeok_'..v[1]..'.aseprite';clip:saveAs(path)
 local legacy=({walk_v1_normal='walk',reload_v1_normal='reload',fire_v1_normal='fire'})[v[1]];if legacy then clip:saveAs(root..'/Clips/sundeok_'..legacy..'.aseprite')end
 local reopened=Sprite{fromFile=path};for f=v[2],v[3]do local n=f-v[2]+1;for li=1,7 do assert(diff(cel(reopened,li,n),cel(s,li,f))==0,'Clip mismatch')end end;reopened:close();clip:close()
end
for li,name in ipairs(names)do
 local lib=sprite({name});for f=1,43 do if f>1 then lib:newEmptyFrame()end;lib.frames[f].duration=s.frames[f].duration;lib:newCel(lib.layers[1],f,cel(s,li,f))end;for _,v in ipairs(specs)do local tag=lib:newTag(v[2],v[3]);tag.name=v[1]end
 lib:saveAs(root..'/Parts/Native/sundeok_'..name:lower()..'.aseprite');lib:close()
 local new=sprite({name});for f=28,43 do local n=f-27;if n>1 then new:newEmptyFrame()end;new.frames[n].duration=s.frames[f].duration;new:newCel(new.layers[1],n,cel(s,li,f))end
 for ai=7,9 do local v=specs[ai];local tag=new:newTag(v[2]-27,v[3]-27);tag.name=v[1]end
 local path=out..'/Parts/Native/'..name..'.aseprite';new:saveAs(path);new:close();local saved=Sprite{fromFile=path}
 for f=28,43 do assert(diff(full(saved,f-27),cel(s,li,f))==0,'V2 library mismatch')end;saved:close()
end
s:close();local saved=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'};assert(#saved.frames==43 and #saved.tags==9);for _,l in ipairs(saved.layers)do assert(l.isVisible)end
for li,name in ipairs(names)do local lib=Sprite{fromFile=root..'/Parts/Native/sundeok_'..name:lower()..'.aseprite'};assert(#lib.frames==43 and #lib.tags==9);for f=1,43 do assert(diff(full(lib,f),cel(saved,li,f))==0,'Library mismatch')end;lib:close()end
for f=1,27 do for li=1,7 do assert(diff(cel(saved,li,f),cel(before,li,f))==0)end end;saved:close();before:close()
report.cropped_part_slots=#report.new_cropped_parts;report.part_library_slots=301;report.v2_part_library_slots=112;report.passed=true
write(out..'/Export/parts.json',{canvas={120,120},root_pivot={60,118},parts=report.new_cropped_parts});write(out..'/QA/validation.json',report)
print('PASS:43frames/9tags; first27 unchanged;16 revised/new poses;26color binary alpha; independent shotgun;42 saved/reopened visibility states;301+112 library slots; cropped parts '..report.cropped_part_slots..'; footprint ratio '..report.footprint_ratio)

