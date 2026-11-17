local root=assert(app.params['root']);local pc=app.pixelColor
local cfg=json.decode(assert(io.open(root..'/Source/config.json')):read('*a'))
local function write(p,t)local f=assert(io.open(p,'w'));f:write(json.encode(t));f:close()end
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local function blank()return Image(120,120,ColorMode.RGB)end
local function flat(s,f)local im=Image(s.spec);im:drawSprite(s,f);return im end
local function cel(s,li,f)local im=Image(s.spec);local c=s.layers[li]:cel(f);if c then im:drawImage(c.image,c.position)end;return im end
local function diff(a,b)assert(a.width==b.width and a.height==b.height);local n=0;for y=0,a.height-1 do for x=0,a.width-1 do if a:getPixel(x,y)~=b:getPixel(x,y)then n=n+1 end end end;return n end
local function flip(a)local im=Image(a.spec);for y=0,a.height-1 do for x=0,a.width-1 do im:drawPixel(a.width-1-x,y,a:getPixel(x,y))end end;return im end
local function scale(a,n)local im=Image(a.width*n,a.height*n,ColorMode.RGB);for y=0,im.height-1 do for x=0,im.width-1 do im:drawPixel(x,y,a:getPixel(x//n,y//n))end end;return im end
local function bg(a,h)local im=Image(a.spec);im:clear(rgba(h));im:drawImage(a);return im end
local function stat(im)
 local r={pixels=0,partial_alpha=0,bounds_xywh={0,0,0,0}};local bx,by,ex,ey=im.width,im.height,-1,-1;local cs={}
 for y=0,im.height-1 do for x=0,im.width-1 do local p=im:getPixel(x,y);local a=pc.rgbaA(p);if a>0 then r.pixels=r.pixels+1;cs[p]=true;bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y);if a~=255 then r.partial_alpha=r.partial_alpha+1 end end end end
 if ex>=0 then r.bounds_xywh={bx,by,ex-bx+1,ey-by+1};r.last_y=ey end;return r,cs
end
local rootReport={date='2026-10-30',characters={},total_frames=0,total_clips=0,total_cropped_parts=0,visibility_states=0,png_difference=0,reassembly_difference=0,source_sample_difference=0,clip_difference=0,library_difference=0,left_difference=0,partial_alpha=0}
local lineup=Image(360,120,ColorMode.RGB);lineup:clear(rgba('eee5d6'));local summary=Image(360,600,ColorMode.RGB);summary:clear(rgba('eee5d6'))
local actorFlats,actorDurations={},{}
for ci,ch in ipairs(cfg.characters)do
 local id=ch.id;local out=root..'/'..id;local meta=json.decode(assert(io.open(out..'/pose_metadata.json')):read('*a'))
 for _,d in ipairs({'QA/LayerStates','QA/PreviewSources','Previews'})do app.fs.makeAllDirectories(out..'/'..d)end
 local m=Sprite{fromFile=root..'/'..ch.native};assert(m.width==120 and m.height==120 and #m.frames==28 and #m.layers==3 and #m.tags==5)
 local allowed,pal={},{ };for _,h in ipairs(meta.palette)do allowed[rgba(h)]=true;pal[#pal+1]=h end
 assert(#m.palettes[1]==27);for _,l in ipairs(m.layers)do assert(l.isVisible)end
 local report={character=id,canvas={120,120},frames=28,clips=5,layers=3,palette_slots=27,frame_checks={},parts={},rgb_union=0,partial_alpha=0,png_difference=0,reassembly_difference=0,source_sample_difference=0,clip_difference=0,library_difference=0,left_difference=0,visibility_states=0,layer_checks={},role=ch.role}
 local union,flats,durations={}, {},{};local contact=Image(720,600,ColorMode.RGB);contact:clear(rgba('eee5d6'))
 local picks={Idle=1,Walk=2,Run=3,Attack=4,Dead=6}
 for ai,clip in ipairs(meta.clips)do
  local name=clip.name;app.fs.makeAllDirectories(out..'/Export/Frames/'..name);app.fs.makeAllDirectories(out..'/Export/Left/'..name);app.fs.makeAllDirectories(out..'/Parts/'..name)
  local sheet=Image(120*clip.count,120,ColorMode.RGB);local jsonFrames={};local preview=Sprite(360,360,ColorMode.RGB)
  local c=Sprite{fromFile=out..'/Clips/'..id..'_'..name:lower()..'.aseprite'};assert(#c.frames==clip.count and #c.layers==3)
  local prior
  for n=1,clip.count do
   local f=clip.from_frame+n-1;local r=meta.frames[f];local im=flat(m,f);flats[f]=im;durations[f]=r.duration_ms
   local st,cs=stat(im);assert(st.pixels>(name=='Dead'and 25 or 500),'Empty-looking pose '..id..' '..f);assert(st.partial_alpha==0,'Partial alpha')
   local b=st.bounds_xywh;assert(b[1]>=1 and b[2]>=1 and b[1]+b[3]-1<=118 and st.last_y<=117,'Canvas clip '..id..' '..f..': '..json.encode(st))
   for color in pairs(cs)do assert(allowed[color],'Outside palette '..id);union[color]=true end
   if prior then assert(diff(prior,im)>0,'Duplicate adjacent animation pose '..id..' '..f)end;prior=im
   local source=Image{fromFile=root..'/Source/Sampled/'..id..'/'..name..string.format('/Full_%02d.png',n)};assert(diff(source,im)==0,'Source sample mismatch '..id..' '..f)
   local file=string.format('%s_%s_%02d.png',id,name:lower(),n);im:saveAs(out..'/Export/Frames/'..name..'/'..file);assert(diff(im,Image{fromFile=out..'/Export/Frames/'..name..'/'..file})==0)
   local left=flip(im);left:saveAs(out..'/Export/Left/'..name..'/'..file);assert(diff(im,flip(left))==0)
   local assembled=blank()
   for li,l in ipairs(m.layers)do local part=cel(m,li,f);local ps=stat(part);if ps.pixels>0 then
    local pb=ps.bounds_xywh;local cropped=Image(pb[3],pb[4],ColorMode.RGB);cropped:drawImage(part,Point(-pb[1],-pb[2]));local pf=string.format('%s_%02d.png',l.name,n);cropped:saveAs(out..'/Parts/'..name..'/'..pf);assembled:drawImage(Image{fromFile=out..'/Parts/'..name..'/'..pf},Point(pb[1],pb[2]))
    report.parts[#report.parts+1]={frame=f,action=name,local_frame=n,layer=l.name,png='Parts/'..name..'/'..pf,origin_xy={pb[1],pb[2]},size_px={pb[3],pb[4]},pivot_canvas_xy={52,118},pivot_crop_xy={52-pb[1],118-pb[2]},left_origin_xy={120-pb[1]-pb[3],pb[2]},left_pivot_canvas_xy={68,118}}
   end end
   assert(diff(assembled,im)==0,'Part reassembly mismatch');assert(diff(flat(c,n),im)==0,'Clip/master mismatch');assert(math.abs(c.frames[n].duration-r.duration_ms/1000)<.001,'Clip duration mismatch')
   sheet:drawImage(im,Point((n-1)*120,0));contact:drawImage(im,Point((n-1)*120,(ai-1)*120));if n==picks[name]then summary:drawImage(im,Point((ci-1)*120,(ai-1)*120))end
   if n>1 then preview:newEmptyFrame()end;preview.frames[n].duration=r.duration_ms/1000;preview:newCel(preview.layers[1],n,scale(bg(im,'eee5d6'),3))
   st.frame=f;st.action=name;st.local_frame=n;st.duration_ms=r.duration_ms;report.frame_checks[#report.frame_checks+1]=st
   jsonFrames[#jsonFrames+1]={frame=n,rect_xywh={(n-1)*120,0,120,120},duration_ms=r.duration_ms,root_pivot_normalized={52/120,2/120},target_lowest_y=r.target_lowest_y}
  end
  sheet:saveAs(out..'/Export/Sheets/'..id..'_'..name:lower()..'.png')
  for n=1,clip.count do local im=blank();im:drawImage(Image{fromFile=out..'/Export/Sheets/'..id..'_'..name:lower()..'.png'},Point(-(n-1)*120,0));assert(diff(im,flats[clip.from_frame+n-1])==0)end
  write(out..'/Export/Sheets/'..id..'_'..name:lower()..'.json',{character=id,action=name,canvas={120,120},frames=jsonFrames,loop=clip.loop,hold_last=clip.hold_last,attack_active_frames=name=='Attack'and{3,4}or{},dead_semantics=ch.role=='rescue'and 'slime_disabled_human_alive_rescue_wait'or 'gel_body_dissolves',left_root_pivot_normalized={68/120,2/120},unity_runtime_connected=false})
  scale(bg(sheet,'eee5d6'),2):saveAs(out..'/Previews/'..name..'_strip_2x.png');preview:saveAs(out..'/QA/PreviewSources/'..name..'_3x.aseprite');preview:close();c:close()
 end
 local colorList={};for color in pairs(union)do colorList[#colorList+1]=string.format('%02x%02x%02x',pc.rgbaR(color),pc.rgbaG(color),pc.rgbaB(color))end;table.sort(colorList);assert(#colorList<=26);report.rgb_union=#colorList;report.palette_used_rgb=colorList
 local idle=stat(flats[1]);local ending=stat(flats[28])
 if ch.role=='hunt'then
  -- Cute endings retain a small sleeping face in a broad low jelly puddle.
  -- Require at least50% height collapse and a wide silhouette.
  report.puddle_height_ratio=ending.bounds_xywh[4]/idle.bounds_xywh[4]
  assert(report.puddle_height_ratio<.5 and ending.bounds_xywh[3]>ending.bounds_xywh[4]*1.6,'Defeated gel did not collapse to a broad low puddle');report.ending='dissolved_slime_puddle'
 else
  local human=stat(cel(m,1,28));assert(human.pixels>600 and human.bounds_xywh[4]>idle.bounds_xywh[4]*.45,'Rescue human disappeared');report.ending='human_alive_kneeling_rescue_wait';report.final_human_body=human
  local naked=Sprite{fromFile=out..'/human_without_slime_120.aseprite'};for f=1,28 do assert(diff(flat(naked,f),cel(m,1,f))==0)end;naked:close()
 end
 report.walk_run_first_pose_difference=diff(flats[5],flats[11]);assert(report.walk_run_first_pose_difference>100,'Run is a duplicate of Walk')
 for li,l in ipairs(m.layers)do local s=Sprite{fromFile=out..'/Parts/Native/'..l.name..'.aseprite'};assert(#s.frames==28 and #s.tags==5);for f=1,28 do assert(diff(flat(s,f),cel(m,li,f))==0)end;s:close()end
 for _,clip in ipairs(meta.clips)do local f=clip.from_frame+picks[clip.name]-1;local existing={};for li=1,3 do if stat(cel(m,li,f)).pixels>0 then existing[#existing+1]=li end end
  for _,mode in ipairs({'solo','hidden'})do local sh=Image(360,120,ColorMode.RGB);sh:clear(rgba('eee5d6'))
   for k,li in ipairs(existing)do
    for j,l in ipairs(m.layers)do l.isVisible=(mode=='solo'and j==li)or(mode=='hidden'and j~=li)end
    local path=out..'/QA/LayerStates/'..clip.name..'_'..mode..'_'..li..'.aseprite';m:saveAs(path);local s=Sprite{fromFile=path}
    for j,l in ipairs(s.layers)do assert(l.isVisible==((mode=='solo'and j==li)or(mode=='hidden'and j~=li)))end
    local shown=flat(s,f);shown:saveAs(out..'/QA/LayerStates/'..clip.name..'_'..mode..'_'..li..'.png');sh:drawImage(bg(shown,'eee5d6'),Point((k-1)*120,0));s:close();report.visibility_states=report.visibility_states+1
   end;scale(sh,2):saveAs(out..'/QA/'..clip.name..'_'..mode..'_2x.png')
  end;for _,l in ipairs(m.layers)do l.isVisible=true end;assert(diff(flat(m,f),flats[f])==0)
 end
 m:close();local again=Sprite{fromFile=root..'/'..ch.native};for _,l in ipairs(again.layers)do assert(l.isVisible)end;again:close()
 contact:saveAs(out..'/Previews/all_actions_native.png');scale(contact,2):saveAs(out..'/Previews/all_actions_2x.png');flats[1]:saveAs(out..'/Export/'..id..'_idle.png');flats[28]:saveAs(out..'/Export/'..id..'_dead_end.png');scale(bg(flats[1],'eee5d6'),4):saveAs(out..'/Previews/idle_4x.png')
 local light=Image(240,120,ColorMode.RGB);light:drawImage(bg(flats[1],'eee5d6'));light:drawImage(bg(flats[1],'252637'),Point(120,0));scale(light,3):saveAs(out..'/Previews/light_dark_3x.png')
 lineup:drawImage(flats[1],Point((ci-1)*120,0));actorFlats[ci]=flats;actorDurations[ci]=durations
 write(out..'/Export/parts.json',{character=id,canvas={120,120},palette=pal,parts=report.parts,root_pivot={52,118},left_root_pivot={68,118},pixel_mirror='x_left=119-x_right',root_boundary_mirror='x_left_root=120-x_right_root',unity_runtime_connected=false})
 write(out..'/QA/validation.json',report)
 rootReport.characters[#rootReport.characters+1]={id=id,frames=28,clips=5,rgb_union=report.rgb_union,cropped_parts=#report.parts,visibility_states=report.visibility_states,ending=report.ending,png_difference=0,reassembly_difference=0,source_sample_difference=0,clip_difference=0,library_difference=0,left_difference=0,partial_alpha=0,walk_run_difference=report.walk_run_first_pose_difference}
 rootReport.total_frames=rootReport.total_frames+28;rootReport.total_clips=rootReport.total_clips+5;rootReport.total_cropped_parts=rootReport.total_cropped_parts+#report.parts;rootReport.visibility_states=rootReport.visibility_states+report.visibility_states
 print(string.format('Verified %s:28 poses, %d colors, %d parts, %d native visibility states.',id,report.rgb_union,#report.parts,report.visibility_states))
end
app.fs.makeAllDirectories(root..'/QA/PreviewSources')
lineup:saveAs(root..'/Previews/lineup_native.png');scale(lineup,3):saveAs(root..'/Previews/lineup_3x.png');scale(summary,2):saveAs(root..'/Previews/action_summary_2x.png')
local compare=Sprite(1080,360,ColorMode.RGB)
for f=1,28 do if f>1 then compare:newEmptyFrame()end;compare.frames[f].duration=actorDurations[1][f]/1000;local im=Image(1080,360,ColorMode.RGB);im:clear(rgba('eee5d6'));for ci=1,3 do im:drawImage(scale(actorFlats[ci][f],3),Point((ci-1)*360,0))end;compare:newCel(compare.layers[1],f,im)end
compare:saveAs(root..'/QA/PreviewSources/three_characters_all_actions_3x.aseprite');compare:close()
write(root..'/QA/validation.json',rootReport)
print(string.format('All 3 characters / 15 clips /84 poses verified; %d cropped part slots and %d saved/reopened material states.',rootReport.total_cropped_parts,rootReport.visibility_states))
