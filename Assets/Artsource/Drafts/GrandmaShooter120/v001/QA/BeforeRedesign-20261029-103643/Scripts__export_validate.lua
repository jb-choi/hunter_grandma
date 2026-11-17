-- Reopen native files; export full poses, cropped parts, left views and sheets.
local root=assert(app.params['root']);local pc=app.pixelColor
local meta=json.decode(assert(io.open(root..'/Source/pose_metadata.json','r')):read('*a'))
local function write(p,o)local f=assert(io.open(p,'w'));f:write(json.encode(o));f:close()end
local function blank()return Image(120,120,ColorMode.RGB)end
local function flat(s,f)local im=Image(s.spec);im:drawSprite(s,f);return im end
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local function bg(img,h)local im=Image(img.width,img.height,ColorMode.RGB);im:clear(rgba(h));im:drawImage(img);return im end
local function scale(img,n)local im=Image(img.width*n,img.height*n,ColorMode.RGB);for y=0,im.height-1 do for x=0,im.width-1 do im:drawPixel(x,y,img:getPixel(x//n,y//n))end end;return im end
local function diff(a,b)assert(a.width==b.width and a.height==b.height);local n=0;for y=0,a.height-1 do for x=0,a.width-1 do if a:getPixel(x,y)~=b:getPixel(x,y)then n=n+1 end end end;return n end
local function flip(a)local out=Image(a.spec);for y=0,a.height-1 do for x=0,a.width-1 do out:drawPixel(a.width-1-x,y,a:getPixel(x,y))end end;return out end
local function inspect(im)
 local r={pixels=0,partial_alpha=0,colors=0,bounds_xywh={0,0,0,0}};local bx,by,ex,ey=im.width,im.height,-1,-1;local colors={}
 for y=0,im.height-1 do for x=0,im.width-1 do local p=im:getPixel(x,y);local a=pc.rgbaA(p);if a>0 then r.pixels=r.pixels+1;colors[p]=true;bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y);if a~=255 then r.partial_alpha=r.partial_alpha+1 end end end end
 for _ in pairs(colors)do r.colors=r.colors+1 end;if ex>=0 then r.bounds_xywh={bx,by,ex-bx+1,ey-by+1};r.last_opaque_y=ey end;return r,colors
end
local function celImage(s,li,f)local out=Image(s.spec);local cel=s.layers[li]:cel(f);if cel then out:drawImage(cel.image,cel.position)end;return out end
local qa=root..'/QA';app.fs.makeAllDirectories(root..'/Export/Frames');app.fs.makeAllDirectories(root..'/Export/Left');app.fs.makeAllDirectories(root..'/Export/Sheets');app.fs.makeAllDirectories(qa..'/LayerStates');app.fs.makeAllDirectories(qa..'/PreviewSources')
local s=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'}
assert(s.width==120 and s.height==120 and #s.frames==27 and #s.layers==8 and #s.tags==6)
local allowed={};for _,h in ipairs(meta.palette)do allowed[rgba(h)]=true end
local report={date='2026-10-29',canvas={120,120},frames=#s.frames,tags={},palette_slots=#s.palettes[1],frame_checks={},parts={},global_rgb_union={},palette_outside_pixels=0,partial_alpha_pixels=0,composite_png_difference=0,part_reassembly_difference=0,left_mirror_roundtrip_difference=0,visibility_states=0,preview_clips={},ground_boundary_y=118}
local global,flats={},{},{}
local contact=Image(720,720,ColorMode.RGB);contact:clear(rgba('eee5d6'))
local clipRow=0;local frameByName={};local previewPick={Idle=1,Walk=2,Reload=4,Aim=3,Fire=1,Collect=2}
local overview=Image(360,240,ColorMode.RGB);overview:clear(rgba('eee5d6'))
for ci,clip in ipairs(meta.clips)do
 local action=clip.name;app.fs.makeAllDirectories(root..'/Export/Frames/'..action);app.fs.makeAllDirectories(root..'/Export/Left/'..action);app.fs.makeAllDirectories(root..'/Parts/'..action)
 local sheet=Image(clip.count*120,120,ColorMode.RGB);local cells={};local localCount=0;local prior
 local preview=Sprite(360,360,ColorMode.RGB);preview:setPalette(s.palettes[1]);preview.layers[1].name='Preview_Flat_Background'
 for fi,pose in ipairs(meta.frames)do if pose.action==action then
  localCount=localCount+1;local full=flat(s,fi);flats[fi]=full;frameByName[action]=frameByName[action]or{};frameByName[action][localCount]=fi
  local r,colors=inspect(full);assert(r.pixels>2500 and r.partial_alpha==0 and r.last_opaque_y==117,json.encode({frame=fi,stats=r}))
  for color in pairs(colors)do global[color]=true;if not allowed[color]then report.palette_outside_pixels=report.palette_outside_pixels+1 end end
  r.frame=fi;r.action=action;r.local_frame=localCount;r.duration_ms=pose.duration_ms;r.reassembly_difference=0;r.png_difference=0;r.previous_frame_difference=prior and diff(full,prior)or 0
  if prior then assert(r.previous_frame_difference>0,'Identical animation frames')end;prior=full
  local filename='sundeok_'..action:lower()..'_'..string.format('%02d',localCount)..'.png'
  local fullPath=root..'/Export/Frames/'..action..'/'..filename;full:saveAs(fullPath);r.png_difference=diff(full,Image{fromFile=fullPath});assert(r.png_difference==0)
  report.composite_png_difference=report.composite_png_difference+r.png_difference
  local left=flip(full);left:saveAs(root..'/Export/Left/'..action..'/'..filename);local roundtrip=diff(full,flip(left));assert(roundtrip==0);report.left_mirror_roundtrip_difference=report.left_mirror_roundtrip_difference+roundtrip
  local assembled=blank();local fullIdlePixels=inspect(celImage(s,1,fi)).pixels
  if action=='Idle'then assert(fullIdlePixels>2500);for li=2,8 do assert(inspect(celImage(s,li,fi)).pixels==0,'Idle must be whole drawing')end
  else assert(fullIdlePixels==0);for _,li in ipairs({2,3,4,5})do assert(inspect(celImage(s,li,fi)).pixels>100,'Required hybrid part absent')end end
  for li,l in ipairs(s.layers)do
   local part=celImage(s,li,fi);local stats=inspect(part)
   if stats.pixels>0 then
    local b=stats.bounds_xywh;local cropped=Image(b[3],b[4],ColorMode.RGB);cropped:drawImage(part,Point(-b[1],-b[2]))
    local partFile=string.format('%s_%02d.png',l.name,localCount);local partPath=root..'/Parts/'..action..'/'..partFile;cropped:saveAs(partPath)
    local loaded=Image{fromFile=partPath};assembled:drawImage(loaded,Point(b[1],b[2]))
    local pivot=pose.weapon_pivot
    if l.name=='BackArm'then pivot=pose.shoulder_back elseif l.name=='FrontArm'then pivot=pose.shoulder_front elseif l.name=='Body'or l.name=='Idle_Full'then pivot={60,118}elseif l.name=='FX'then pivot=pose.muzzle end
    report.parts[#report.parts+1]={frame=fi,action=action,local_frame=localCount,layer=l.name,png='Parts/'..action..'/'..partFile,origin_xy={b[1],b[2]},size_px={b[3],b[4]},pivot_canvas_xy=pivot,pivot_crop_xy={pivot[1]-b[1],pivot[2]-b[2]},muzzle_canvas_xy=pose.muzzle,hand_back_canvas_xy=pose.hand_back,hand_front_canvas_xy=pose.hand_front,shoulder_back_canvas_xy=pose.shoulder_back,shoulder_front_canvas_xy=pose.shoulder_front,left_origin_xy={120-b[1]-b[3],b[2]},left_pivot_canvas_xy={119-pivot[1],pivot[2]}}
   end
  end
  r.reassembly_difference=diff(full,assembled);assert(r.reassembly_difference==0);report.part_reassembly_difference=report.part_reassembly_difference+r.reassembly_difference
  cells[#cells+1]={frame=localCount,rect_xywh={(localCount-1)*120,0,120,120},duration_ms=pose.duration_ms,pivot_normalized={.5,2/120},source_png='Frames/'..action..'/'..filename}
  sheet:drawImage(full,Point((localCount-1)*120,0));contact:drawImage(full,Point((localCount-1)*120,clipRow*120));report.frame_checks[#report.frame_checks+1]=r
  if localCount==previewPick[action]then overview:drawImage(full,Point(((ci-1)%3)*120,((ci-1)//3)*120))end
  if localCount>1 then preview:newEmptyFrame()end;preview.frames[localCount].duration=pose.duration_ms/1000;preview:newCel(preview.layers[1],localCount,scale(bg(full,'eee5d6'),3),Point(0,0))
 end end
 assert(localCount==clip.count);sheet:saveAs(root..'/Export/Sheets/sundeok_'..action:lower()..'.png');scale(bg(sheet,'eee5d6'),2):saveAs(root..'/Previews/'..action..'_strip_2x.png')
 write(root..'/Export/Sheets/sundeok_'..action:lower()..'.json',{canvas={120,120},frames=cells,pivot_normalized={.5,2/120},hybrid=action~='Idle'})
 for i=1,clip.count do local loaded=Image{fromFile=root..'/Export/Sheets/sundeok_'..action:lower()..'.png'};local extracted=blank();extracted:drawImage(loaded,Point(-(i-1)*120,0));assert(diff(extracted,flats[frameByName[action][i]])==0)end
 preview:saveAs(qa..'/PreviewSources/'..action..'_3x.aseprite');preview:close();report.preview_clips[#report.preview_clips+1]=action;clipRow=clipRow+1
 report.tags[#report.tags+1]={name=action,frames=clip.count,hybrid=action~='Idle'}
end
local colorList={};for p in pairs(global)do colorList[#colorList+1]=string.format('%02x%02x%02x',pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p))end;table.sort(colorList);report.global_rgb_union=colorList;report.global_rgb_count=#colorList
assert(report.global_rgb_count<=26 and report.palette_outside_pixels==0 and report.palette_slots==27)
-- Actual native visibility states are saved/reopened for one representative of each clip.
for ci,clip in ipairs(meta.clips)do
 local fi=frameByName[clip.name][previewPick[clip.name]];local existing={}
 for li=1,8 do if inspect(celImage(s,li,fi)).pixels>0 then existing[#existing+1]=li end end
 for _,mode in ipairs({'solo','hidden'})do
  local sheet=Image(480,240,ColorMode.RGB);sheet:clear(rgba('eee5d6'))
  for k,li in ipairs(existing)do
   for j,l in ipairs(s.layers)do l.isVisible=(mode=='solo'and j==li)or(mode=='hidden'and j~=li)end
   local path=qa..'/LayerStates/'..clip.name..'_'..mode..'_'..li..'.aseprite';s:saveAs(path)
   local state=Sprite{fromFile=path};for j,l in ipairs(state.layers)do assert(l.isVisible==((mode=='solo'and j==li)or(mode=='hidden'and j~=li)))end
   local shown=flat(state,fi);shown:saveAs(qa..'/LayerStates/'..clip.name..'_'..mode..'_'..li..'.png');sheet:drawImage(bg(shown,'eee5d6'),Point(((k-1)%4)*120,((k-1)//4)*120));state:close();report.visibility_states=report.visibility_states+1
  end
  scale(sheet,2):saveAs(qa..'/'..clip.name..'_'..mode..'_2x.png')
 end
 for _,l in ipairs(s.layers)do l.isVisible=true end;assert(diff(flat(s,fi),flats[fi])==0)
end
s:close();local again=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'};for _,l in ipairs(again.layers)do assert(l.isVisible)end;again:close()
-- Walking-fire compositing demo: all moving bodies match exported Walk parts exactly.
local dmeta=json.decode(assert(io.open(root..'/Source/demo_metadata.json','r')):read('*a'))
local demo=Sprite{fromFile=root..'/sundeok_walk_aim_fire_demo.aseprite'};assert(#demo.frames==12)
local dpreview=Sprite(360,360,ColorMode.RGB);dpreview:setPalette(pal or demo.palettes[1]);local demoSheet=Image(720,240,ColorMode.RGB);demoSheet:clear(rgba('eee5d6'))
for f,r in ipairs(dmeta)do
 assert(diff(celImage(demo,2,f),celImage(Sprite{fromFile=root..'/Clips/sundeok_walk.aseprite'},2,r.walk_frame))==0,'Walk body changed in aim/fire demo')
 local frame=flat(demo,f);local _,cs=inspect(frame);for color in pairs(cs)do assert(allowed[color],'Demo palette mismatch')end
 if f>1 then dpreview:newEmptyFrame()end;dpreview.frames[f].duration=.11;dpreview:newCel(dpreview.layers[1],f,scale(bg(frame,'eee5d6'),3),Point(0,0));demoSheet:drawImage(frame,Point(((f-1)%6)*120,((f-1)//6)*120))
end
dpreview:saveAs(qa..'/PreviewSources/Walk_Aim_Fire_3x.aseprite');dpreview:close();demo:close();report.walk_aim_fire_demo_frames=12;scale(demoSheet,2):saveAs(root..'/Previews/Walk_Aim_Fire_strip_2x.png')
contact:saveAs(root..'/Previews/all_poses_native.png');scale(contact,2):saveAs(root..'/Previews/all_poses_2x.png');scale(overview,3):saveAs(root..'/Previews/six_actions_3x.png')
local hero=flats[1];hero:saveAs(root..'/Export/sundeok_idle.png');flats[20]:saveAs(root..'/Export/sundeok_fire.png');scale(bg(hero,'eee5d6'),4):saveAs(root..'/Previews/sundeok_idle_4x.png')
local pair=Image(240,120,ColorMode.RGB);pair:drawImage(bg(hero,'eee5d6'));pair:drawImage(bg(hero,'252637'),Point(120,0));scale(pair,3):saveAs(root..'/Previews/sundeok_light_dark_3x.png')
local silhouette=blank();for y=0,119 do for x=0,119 do if pc.rgbaA(hero:getPixel(x,y))>0 then silhouette:drawPixel(x,y,pc.rgba(0,0,0,255))end end end;silhouette:saveAs(root..'/Export/sundeok_locked.png')
local portrait=blank();portrait:drawImage(hero,Point(15,15));for y=98,119 do for x=0,119 do portrait:drawPixel(x,y,0)end end;portrait:saveAs(root..'/Export/sundeok_portrait.png')
write(root..'/Export/sundeok_parts.json',{canvas={120,120},character_palette_rgb=meta.palette,parts=report.parts,frame_pose_metadata='../Source/pose_metadata.json',default_root_pivot_normalized={.5,2/120},left_pixel_transform='x_left=119-x_right; y unchanged; root floor pivot uses boundary coordinates (60,118)',unity_runtime_connected=false})
write(qa..'/validation.json',report)
print(string.format('Validated 27 frames, %d RGB union, %d cropped parts, %d native visibility states; PNG/parts/left/sheet roundtrips exact.',report.global_rgb_count,#report.parts,report.visibility_states))
