local root=assert(app.params['root']);local pc=app.pixelColor;local out=root..'/TacticalV2'
local original=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'};assert(#original.frames==33,'Expected33-frame master; inspect changes before rebuilding v2')
local names={'Body','BackArm','Weapon','FrontArm','Ammo','Pickup','FX'}
local pal=Palette(original.palettes[1]);local colors,labs={},{}
local function blank()return Image(120,120,ColorMode.RGB)end
local function lin(v)v=v/255;return v<=.04045 and v/12.92 or((v+.055)/1.055)^2.4 end
local function lab(r,g,b)r,g,b=lin(r),lin(g),lin(b);local l=(.4122214708*r+.5363325363*g+.0514459929*b)^(1/3);local m=(.2119034982*r+.6806995451*g+.1073969566*b)^(1/3);local s=(.0883024619*r+.2817188376*g+.6299787005*b)^(1/3);return{.2104542553*l+.793617785*m-.0040720468*s,1.9779984951*l-2.428592205*m+.4505937099*s,.0259040371*l+.7827717662*m-.808675766*s}end
for i=1,26 do local c=pal:getColor(i);colors[i]=pc.rgba(c.red,c.green,c.blue,255);labs[i]=lab(c.red,c.green,c.blue)end
-- Light gray curl colors register the head without mistaking dark jacket
-- creases for the deepest purple-gray hair shade.
local hair,coat,pants={},{},{};for i=10,12 do hair[colors[i]]=true end;for i=13,18 do coat[colors[i]]=true end;for _,i in ipairs({2,3,19,20,21,22})do pants[colors[i]]=true end
local cache={};local function map(p)if pc.rgbaA(p)<128 then return 0 end;if cache[p]then return cache[p]end;local c=lab(pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p));local best,idx=1e30,1;for i,v in ipairs(labs)do local d=1.2*(v[1]-c[1])^2+(v[2]-c[2])^2+(v[3]-c[3])^2;if d<best then best,idx=d,i end end;cache[p]=colors[idx];return colors[idx]end
local function cel(s,li,f)local im=blank();local c=s.layers[li]:cel(f);if c then im:drawImage(c.image,c.position)end;return im end
local function bounds(im)local bx,by,ex,ey=im.width,im.height,-1,-1;for y=0,im.height-1 do for x=0,im.width-1 do if pc.rgbaA(im:getPixel(x,y))>=128 then bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y)end end end;assert(ex>=bx);return{bx=bx,by=by,ex=ex,ey=ey,w=ex-bx+1,h=ey-by+1}end
local function anatomy(body)
 local bx,by,ex,ey=120,120,-1,-1;for y=0,70 do for x=0,119 do if hair[body:getPixel(x,y)]then bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y)end end end;assert(ex>=bx,'Missing gray perm')
 local sy=ey+10;local l,r=120,-1;for y=sy-4,math.min(100,sy+6)do for x=0,119 do if coat[body:getPixel(x,y)]then l=math.min(l,x);r=math.max(r,x)end end end
 return{head={(bx+ex)/2,(by+ey)/2},shoulder={(l+r)/2,sy},hair_bounds={bx,by,ex-bx+1,ey-by+1}}
end
local function shift(im,dx,dy)local a=blank();a:drawImage(im,Point(dx,dy));return a end
local function flat(parts)local a=blank();for _,v in ipairs(parts)do a:drawImage(v)end;return a end
local function scale(im,n)local a=Image(im.width*n,im.height*n,ColorMode.RGB);for y=0,a.height-1 do for x=0,a.width-1 do a:drawPixel(x,y,im:getPixel(x//n,y//n))end end;return a end
local function write(path,value)local f=assert(io.open(path,'w'));f:write(json.encode(value));f:close()end
local function sprite(layerNames)local s=Sprite(120,120,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pal);for _,n in ipairs(layerNames or names)do local l=s:newLayer();l.name=n end;return s end

local specs={
 {tag='walk_v2_tactical',kind='Walk',file='walk_v2_tiptoe_body.png',count=6,cols=3,rows=2,start=28,ms={140,140,140,140,140,140}},
 {tag='reload_v2_tactical',kind='Reload',file='reload_v2_body.png',count=6,cols=3,rows=2,start=34,ms={150,150,150,150,150,150}},
 {tag='fire_v2_tactical',kind='Fire',file='fire_v2_body.png',count=4,cols=2,rows=2,start=40,ms={100,70,100,150}}}
local aim={};for li=1,7 do aim[li]=cel(original,li,19)end;local baseAn=anatomy(aim[1]);local bw=bounds(aim[3])
local oldFx=cel(original,7,21);local fb=bounds(oldFx);local baseFlash=blank();baseFlash:drawImage(oldFx,Point(107-fb.bx,38-math.floor((fb.by+fb.ey)/2)));local bf=bounds(baseFlash)
local assemblyBounds=bounds(flat({aim[2],aim[3],aim[4]}))
local maxReadyDx=math.min(118-assemblyBounds.ex,118-bf.ex+3)
local poses,records={},{};local set=Sprite(360,360,ColorMode.RGB);local allContact=Image(720,360,ColorMode.RGB);allContact:clear(pc.rgba(238,229,214,255));local seq=0
for _,d in ipairs({'Source/Sampled','Export/Frames','Export/Left','Export/Sheets','Export/Gifs','QA/PreviewSources','QA/Candidates','Parts/Frames'})do app.fs.makeAllDirectories(out..'/'..d)end
local function footSpan(body)local l,r=120,-1;for y=100,117 do for x=0,119 do if pc.rgbaA(body:getPixel(x,y))>0 then l=math.min(l,x);r=math.max(r,x)end end end;return r>=l and r-l+1 or 0 end
for ai,spec in ipairs(specs)do
 local source=Image{fromFile=out..'/Source/'..spec.file};local cw,ch=source.width//spec.cols,source.height//spec.rows
 local cells,bs,hxs={}, {},{};local maxH=0
 for f=1,spec.count do
  local cell=Image(cw,ch,ColorMode.RGB);cell:drawImage(source,Point(-((f-1)%spec.cols)*cw,-((f-1)//spec.cols)*ch));local b=bounds(cell);cells[f]=cell;bs[f]=b;maxH=math.max(maxH,b.h)
  local hx1,hx2=cw,-1;for y=b.by,math.floor(b.by+b.h*.5)do for x=b.bx,b.ex do if hair[map(cell:getPixel(x,y))]then hx1=math.min(hx1,x);hx2=math.max(hx2,x)end end end;assert(hx2>=hx1,'Missing gray curls');hxs[f]=(hx1+hx2)/2
 end
 local sc=106/maxH;local targetX=62;local minx=120;for f=1,spec.count do minx=math.min(minx,targetX+(bs[f].bx-hxs[f])*sc)end;if minx<2 then targetX=targetX+2-minx end
 local clip=sprite();local preview=Sprite(360,360,ColorMode.RGB);local pairPreview=Sprite(720,360,ColorMode.RGB);local sheet=Image(120*spec.count,120,ColorMode.RGB)
 app.fs.makeAllDirectories(out..'/Source/Sampled/'..spec.tag);app.fs.makeAllDirectories(out..'/Export/Frames/'..spec.tag);app.fs.makeAllDirectories(out..'/Export/Left/'..spec.tag);app.fs.makeAllDirectories(out..'/Parts/Frames/'..spec.tag)
 for f=1,spec.count do
  seq=seq+1;local body=blank();local b,cell=bs[f],cells[f];local bodyDx=spec.kind=='Fire'and({0,-2,-1,0})[f]or 0
  for y=0,119 do for x=0,119 do local sx=math.floor(hxs[f]+(x-targetX-bodyDx)/sc+.5);local sy=math.floor(b.ey+(y-117)/sc+.5);if sx>=0 and sx<cw and sy>=0 and sy<ch then body:drawPixel(x,y,map(cell:getPixel(sx,sy)))end end end
  -- One common newly authored narrow forefoot stance keeps the three actions
  -- on model. Reload/Fire generation studies are retained as studies only.
  -- Native 0-2px upper-body compression is anchored at the waist; feet do not slide.
  if spec.kind~='Walk'then
   local anchor=poses[28][1];local dip=spec.kind=='Reload'and({0,1,2,2,1,0})[f]or({0,1,1,0})[f]
   local recoil=spec.kind=='Fire'and({0,-2,-1,0})[f]or 0;body=blank()
   for y=0,119 do for x=0,119 do local weight=math.max(0,math.min(1,(82-y)/40));local sx=math.floor(x-recoil*weight+.5);local sy=math.floor(y-dip*weight+.5);if sx>=0 and sx<120 and sy>=0 and sy<120 then body:drawPixel(x,y,anchor:getPixel(sx,sy))end end end
  end
  local an=anatomy(body);local readyDx=math.max(1-bw.bx,math.min(maxReadyDx,math.floor(an.head[1]-baseAn.head[1]+.5)));local readyDy=math.floor(an.head[2]-baseAn.head[2]+.5)
  local parts={body,blank(),blank(),blank(),blank(),blank(),blank()};local sourceFrame=19;local extraX,extraY=0,0;local mode
  if spec.kind=='Reload'then
   sourceFrame=10+f;local sourceWeapon=cel(original,3,sourceFrame);local sw=bounds(sourceWeapon);extraX=bw.bx-sw.bx;extraY=bw.by-sw.by
   for li=2,7 do parts[li]=shift(cel(original,li,sourceFrame),readyDx+extraX,readyDy+extraY)end
   mode='Authored reload hands and shell, normalized to steady shouldered shotgun position'
  else
   extraX=spec.kind=='Fire'and({0,-3,-1,0})[f]or 0;extraY=spec.kind=='Fire'and({0,-1,0,0})[f]or 0
   for li=2,4 do parts[li]=shift(aim[li],readyDx+extraX,readyDy+extraY)end
   if spec.kind=='Fire'and f==2 then parts[7]=shift(baseFlash,readyDx+extraX,readyDy+extraY)end
   mode=spec.kind=='Fire'and'Compact authored Aim assembly recoil plus source Fire2 flash'or'Steady authored Aim assembly on new short forefoot gait'
  end
  local complete=flat(parts);local cb=bounds(complete);assert(cb.bx>=1 and cb.ex<=118 and cb.by>=1 and cb.ey<=117,'Clipped '..spec.tag..' '..f..' bounds '..json.encode(cb)..' ready '..readyDx..','..readyDy)
  local frame=spec.start+f-1;poses[frame]=parts
  if frame>#original.frames then original:newEmptyFrame()end
  original.frames[frame].duration=spec.ms[f]/1000
  for li=1,7 do local c=original.layers[li]:cel(frame);if c then c.image=Image(parts[li]);c.position=Point(0,0)else original:newCel(original.layers[li],frame,parts[li])end end
  local bodyPath='TacticalV2/Source/Sampled/'..spec.tag..'/Body_'..string.format('%02d',f)..'.png';body:saveAs(root..'/'..bodyPath)
  complete:saveAs(out..'/Export/Frames/'..spec.tag..'/'..string.format('%02d.png',f));local left=blank();for y=0,119 do for x=0,119 do left:drawPixel(119-x,y,complete:getPixel(x,y))end end;left:saveAs(out..'/Export/Left/'..spec.tag..'/'..string.format('%02d.png',f))
  records[#records+1]={frame=frame,action=spec.tag,aseprite_tag=spec.tag,local_frame=f,duration_ms=spec.ms[f],floor_boundary_y=118,root_pivot={60,118},head_center=an.head,source=bodyPath,source_scale=sc,upper_source_frame=sourceFrame,upper_offset_xy={readyDx+extraX,readyDy+extraY},upper_mode=mode,muzzle={106+readyDx+(spec.kind=='Reload'and 0 or extraX),38+readyDy+(spec.kind=='Reload'and 0 or extraY)},weapon_pivot={61+readyDx+(spec.kind=='Reload'and 0 or extraX),46+readyDy+(spec.kind=='Reload'and 0 or extraY)},weapon_angle=0,foot_span_px=footSpan(body),toe_weighted_walk=spec.kind=='Walk',compact_recoil=spec.kind=='Fire',loop=spec.kind=='Walk',flash=f==2 and spec.kind=='Fire'}
  if f>1 then clip:newEmptyFrame();preview:newEmptyFrame();pairPreview:newEmptyFrame()end
  if seq>1 then set:newEmptyFrame()end
  clip.frames[f].duration=spec.ms[f]/1000;preview.frames[f].duration=spec.ms[f]/1000;pairPreview.frames[f].duration=spec.ms[f]/1000;set.frames[seq].duration=spec.ms[f]/1000
  for li=1,7 do clip:newCel(clip.layers[li],f,parts[li])end
  local bg=blank();bg:clear(pc.rgba(238,229,214,255));bg:drawImage(complete);local large=scale(bg,3);preview:newCel(preview.layers[1],f,large);set:newCel(set.layers[1],seq,large)
  local oldFrame=spec.kind=='Walk'and 27+f or spec.kind=='Reload'and 10+f or 19+f
  local oldSprite=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'};local old=blank();old:clear(pc.rgba(238,229,214,255));old:drawSprite(oldSprite,oldFrame);oldSprite:close()
  local pair=Image(240,120,ColorMode.RGB);pair:drawImage(old);pair:drawImage(bg,Point(120,0));pairPreview:newCel(pairPreview.layers[1],f,scale(pair,3))
  sheet:drawImage(complete,Point((f-1)*120,0));allContact:drawImage(bg,Point((f-1)*120,(ai-1)*120))
 end
 local tag=clip:newTag(1,spec.count);tag.name=spec.tag;clip:saveAs(out..'/QA/Candidates/'..spec.tag..'.aseprite');clip:close()
 preview:saveAs(out..'/QA/PreviewSources/'..spec.tag..'_3x.aseprite');preview:close();pairPreview:saveAs(out..'/QA/PreviewSources/'..spec.tag..'_comparison_3x.aseprite');pairPreview:close()
 sheet:saveAs(out..'/Export/Sheets/'..spec.tag..'.png')
end
set:saveAs(out..'/QA/PreviewSources/tactical_v2_set_3x.aseprite');set:close();allContact:saveAs(out..'/Previews/v2_set_native.png');scale(allContact,2):saveAs(out..'/Previews/v2_set_2x.png')
while #original.tags>0 do original:deleteTag(original.tags[1])end
local tags={{'Idle',1,4},{'walk_v1_normal',5,10},{'reload_v1_normal',11,16},{'Aim',17,19},{'fire_v1_normal',20,23},{'Collect',24,27},{'walk_v2_tactical',28,33},{'reload_v2_tactical',34,39},{'fire_v2_tactical',40,43}}
for _,v in ipairs(tags)do local tag=original:newTag(v[2],v[3]);tag.name=v[1]end
original:saveAs(out..'/QA/Candidates/grandma_01_sundeok_120.aseprite');original:close()
write(out..'/pose_metadata.json',{date='2026-11-02',canvas={120,120},root_pivot={60,118},frames=records,clips=specs,tags=tags,ready_assembly_from=19,flash_source_frame=21,runtime_changed=false,max_ready_dx=maxReadyDx})
print('Built43-frame candidate: refined forefoot Walk6, high-ready Reload6, compact Fire4. First27 frames retained.')
