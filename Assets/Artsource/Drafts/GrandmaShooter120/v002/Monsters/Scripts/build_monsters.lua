-- Native 120px conversion and editable, palette-limited monster animation package.
local root=assert(app.params['root']);local pc=app.pixelColor
local cfg=json.decode(assert(io.open(root..'/Source/config.json')):read('*a'))
local function write(path,value)local f=assert(io.open(path,'w'));f:write(json.encode(value));f:close()end
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local function blank()return Image(120,120,ColorMode.RGB)end
local function lin(v)v=v/255;return v<=.04045 and v/12.92 or((v+.055)/1.055)^2.4 end
local function lab(r,g,b)r,g,b=lin(r),lin(g),lin(b);local l=(.4122214708*r+.5363325363*g+.0514459929*b)^(1/3);local m=(.2119034982*r+.6806995451*g+.1073969566*b)^(1/3);local s=(.0883024619*r+.2817188376*g+.6299787005*b)^(1/3);return{.2104542553*l+.793617785*m-.0040720468*s,1.9779984951*l-2.428592205*m+.4505937099*s,.0259040371*l+.7827717662*m-.808675766*s}end
local function bounds(im,label)
 local bx,by,ex,ey=im.width,im.height,-1,-1
 for y=0,im.height-1 do for x=0,im.width-1 do if pc.rgbaA(im:getPixel(x,y))>=128 then bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y)end end end
 assert(ex>=bx,'Empty generated cell '..(label or ''));return{bx=bx,by=by,ex=ex,ey=ey,w=ex-bx+1,h=ey-by+1}
end
local function crop(im,x,y,w,h)local out=Image(w,h,ColorMode.RGB);out:drawImage(im,Point(-x,-y));return out end
-- Generated boards may use unequal row heights. Split through actual alpha
-- gutters so a tall upper-row pose cannot leak into a collapsed lower-row cell.
local function cuts(im,count,horizontal,y0,y1)
 local length=horizontal and im.width or im.height;local occupied={}
 for k=0,length-1 do local found=false
  if horizontal then for y=y0,y1 do if pc.rgbaA(im:getPixel(k,y))>=128 then found=true;break end end
  else for x=0,im.width-1 do if pc.rgbaA(im:getPixel(x,k))>=128 then found=true;break end end end
  occupied[k]=found
 end
 local gaps={};local k=0;while k<length do if not occupied[k]then local a=k;while k<length and not occupied[k]do k=k+1 end;if k-a>=3 then gaps[#gaps+1]={a,k-1}end else k=k+1 end end
 local out={0};for i=1,count-1 do local target=length*i/count;local best,distance=nil,1e9
  for _,g in ipairs(gaps)do local mid=(g[1]+g[2]+1)//2;if g[1]>0 and g[2]<length-1 and mid>out[#out]+12 and mid<length-(count-i)*12 and math.abs(mid-target)<length/count*.45 then local d=math.abs(mid-target);if d<distance then best,distance=mid,d end end end
  out[#out+1]=best or math.floor(target)
 end;out[#out+1]=length;return out
end
local function rectangles(im,cols,rows)
 local ys=cuts(im,rows,false,0,im.height-1);local out={}
 for row=1,rows do local xs=cuts(im,cols,true,ys[row],ys[row+1]-1)
  for col=1,cols do out[#out+1]={x=xs[col],y=ys[row],w=xs[col+1]-xs[col],h=ys[row+1]-ys[row]}end
 end;return out
end
local function foot(im,b)
 local bx,ex=im.width,-1
 for y=math.max(b.by,math.floor(b.ey-b.h*.09)),b.ey do for x=b.bx,b.ex do if pc.rgbaA(im:getPixel(x,y))>=128 then bx=math.min(bx,x);ex=math.max(ex,x)end end end
 return ex>=bx and(bx+ex)/2 or(b.bx+b.ex)/2
end
local function primaryBounds(im,label)
 local w,h=im.width,im.height;local seen={};local best
 for y=0,h-1 do for x=0,w-1 do local k=y*w+x;if not seen[k]and pc.rgbaA(im:getPixel(x,y))>=128 then
  local q,head={k},1;seen[k]=true;local g={bx=x,by=y,ex=x,ey=y}
  while head<=#q do local p=q[head];head=head+1;local xx,yy=p%w,p//w;g.bx=math.min(g.bx,xx);g.by=math.min(g.by,yy);g.ex=math.max(g.ex,xx);g.ey=math.max(g.ey,yy)
   for dy=-1,1 do for dx=-1,1 do local nx,ny=xx+dx,yy+dy;if nx>=0 and nx<w and ny>=0 and ny<h then local nk=ny*w+nx;if not seen[nk]and pc.rgbaA(im:getPixel(nx,ny))>=128 then seen[nk]=true;q[#q+1]=nk end end end end
  end
  if not best or #q>best.n then
   g.n=#q;g.w=g.ex-g.bx+1;g.h=g.ey-g.by+1;local low=g.ey-g.h*.09;local lx,rx=w,-1
   for _,p in ipairs(q)do local xx,yy=p%w,p//w;if yy>=low then lx=math.min(lx,xx);rx=math.max(rx,xx)end end
   g.foot=(lx+rx)/2;best=g
  end
 end end end
 assert(best,'Empty body '..(label or ''));return best
end
local function components(im)
 local seen,out={},{}
 for y=0,119 do for x=0,119 do local k=y*120+x
 if not seen[k]and pc.rgbaA(im:getPixel(x,y))>0 then
  local q,i={{x,y}},1;local g={pts={},bx=x,by=y,ex=x,ey=y};seen[k]=true
  while i<=#q do local p=q[i];i=i+1;g.pts[#g.pts+1]=p;g.bx=math.min(g.bx,p[1]);g.by=math.min(g.by,p[2]);g.ex=math.max(g.ex,p[1]);g.ey=math.max(g.ey,p[2])
   for dy=-1,1 do for dx=-1,1 do local xx,yy=p[1]+dx,p[2]+dy;if xx>=0 and xx<120 and yy>=0 and yy<120 then local kk=yy*120+xx;if not seen[kk]and pc.rgbaA(im:getPixel(xx,yy))>0 then seen[kk]=true;q[#q+1]={xx,yy}end end end end
  end;g.n=#g.pts;out[#out+1]=g
 end end end;table.sort(out,function(a,b)return a.n>b.n end);return out
end
local function shift(im,dx,dy)local out=blank();out:drawImage(im,Point(dx,dy));return out end
local allMeta={date='2026-10-30',canvas={120,120},root_pivot={52,118},characters={},frames={},loop_policy={Idle=true,Walk=true,Run=true,Attack=false,Dead=false},dead_hold_last=true,atack_alias='Attack'}
for _,ch in ipairs(cfg.characters)do
 local id=ch.id;local outdir=root..'/'..id
 for _,d in ipairs({'Clips','Parts/Native','Export/Frames','Export/Sheets','Export/Left','Export/Gifs','Previews','QA'})do app.fs.makeAllDirectories(outdir..'/'..d)end
 local pal=Palette(27);pal:setColor(0,Color{r=0,g=0,b=0,a=0});local hex,pix,labs={},{},{}
 for i,h in ipairs(ch.palette)do hex[#hex+1]=h;local p=rgba(h);pix[i]=p;labs[i]=lab(pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p));pal:setColor(i,Color{r=pc.rgbaR(p),g=pc.rgbaG(p),b=pc.rgbaB(p),a=255})end
 local cache,cleanCache={},{};local cleanForbidden={ [3]=true,[4]=true,[5]=true,[6]=true,[7]=true,[8]=true,[9]=true,[25]=true}
 local function mapped(p,clean)
  if pc.rgbaA(p)<128 then return 0 end;local cch=clean and cleanCache or cache;if cch[p]then return cch[p]end
  local c=lab(pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p));local best,idx=1e9,1
  for i,v in ipairs(labs)do if not clean or not cleanForbidden[i]then local d=1.2*(v[1]-c[1])^2+(v[2]-c[2])^2+(v[3]-c[3])^2;if d<best then best,idx=d,i end end end;cch[p]=pix[idx];return pix[idx]
 end
 local names=ch.role=='rescue'and{'HumanBody','SlimeCoat','DripsFX'}or{'GelBody','SurfaceHighlights','DripsFX'}
 local function sprite(ns)local s=Sprite(120,120,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pal);for _,n in ipairs(ns or names)do local l=s:newLayer();l.name=n end;return s end
 local master=sprite();local poses,records,clips={}, {},{};local fi=0
 for _,a in ipairs(cfg.actions)do
  local count=a.count;local cols,rows=a.grid[1],a.grid[2]
  local source=Image{fromFile=root..'/Source/Generated/'..id..'/'..a.name..'.png'}
  local restored=ch.role=='rescue'and Image{fromFile=root..'/Source/Generated/'..id..'/'..a.name..'_Body.png'}or nil
  local rects=rectangles(source,cols,rows)
  write(root..'/QA/current_rects.json',{character=id,action=a.name,rectangles=rects})
  local cells,bs,fxs={}, {},{};local maxH,maxL,maxR=0,0,0
  for f=1,count do local r=rects[f];local cell=crop(source,r.x,r.y,r.w,r.h);local b=primaryBounds(cell,id..' '..a.name..' '..f..' rect '..json.encode(r));local fx=a.name=='Run'and(b.bx+b.ex)/2 or b.foot;cells[f]=cell;bs[f]=b;fxs[f]=fx;maxH=math.max(maxH,b.h);maxL=math.max(maxL,fx-b.bx);maxR=math.max(maxR,b.ex-fx)end
  local sc=math.min(ch.height/maxH,50/math.max(1,maxL),66/math.max(1,maxR))
  local start=fi+1;app.fs.makeAllDirectories(root..'/Source/Sampled/'..id..'/'..a.name)
  for f=1,count do
   fi=fi+1;if fi>1 then master:newEmptyFrame()end;local b,cell=bs[f],cells[f];local cw,hh=cell.width,cell.height;local rw,rh=0,0
   local bob=a.name=='Run'and(({0,-1,-3,0,-1,-3})[f])or a.name=='Walk'and(({0,0,-1,0,0,-1})[f])or 0
   local floor=117+bob;local full,clean,seed=blank(),blank(),{}
   local rc
   if restored then local r=rects[f];local rx,ry=restored.width/source.width,restored.height/source.height;rw,rh=math.floor(r.w*rx),math.floor(r.h*ry);rc=crop(restored,math.floor(r.x*rx),math.floor(r.y*ry),rw,rh)end
   for y=0,119 do for x=0,119 do
    local sx,sy=fxs[f]+(x-52)/sc,b.ey+(y-floor)/sc;local ix,iy=math.floor(sx+.5),math.floor(sy+.5)
    if ix>=0 and ix<cw and iy>=0 and iy<hh then
     local p=cell:getPixel(ix,iy);full:drawPixel(x,y,mapped(p,false))
     if ch.role=='rescue'and pc.rgbaA(p)>=128 then local r,g,bb=pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p);if g>r+8 and bb>r+8 and g>=bb*.90 then seed[y*120+x]=true end end
    end
    if rc then local xx,yy=math.floor(sx*rw/cw+.5),math.floor(sy*rh/hh+.5);if xx>=0 and xx<rw and yy>=0 and yy<rh then clean:drawPixel(x,y,mapped(rc:getPixel(xx,yy),true))end end
   end end
   local groups=components(full);assert(groups[1],'Empty native frame');local mainMask={};local fxImage=blank()
   for gi,g in ipairs(groups)do
    local structural=gi==1 or g.n>16
    if ch.role=='rescue'and gi>1 then
     local skin=0;for _,p in ipairs(g.pts)do local v=full:getPixel(p[1],p[2]);if v==pix[10]or v==pix[11]or v==pix[12]then skin=skin+1 end end
     structural=skin>g.n*.35
    end
    if gi>1 and(g.bx<=1 or g.ex>=118 or g.by<=1 or g.ey>=118)then structural=false end
    if structural then for _,p in ipairs(g.pts)do mainMask[p[2]*120+p[1]]=true end
    else
     local dx=g.bx<1 and(1-g.bx)or g.ex>118 and(118-g.ex)or 0;local dy=g.by<1 and(1-g.by)or g.ey>117 and(117-g.ey)or 0
     for _,p in ipairs(g.pts)do local v=full:getPixel(p[1],p[2]);if g.n>1 or pc.rgbaG(v)>70 then fxImage:drawPixel(p[1]+dx,p[2]+dy,v)end end
    end
   end
   local body,overlay=blank(),blank()
   if ch.role=='rescue'then
    for y=0,119 do for x=0,119 do local k=y*120+x;if mainMask[k]then
     local coat=false;for dy=-1,1 do for dx=-1,1 do local xx,yy=x+dx,y+dy;if xx>=0 and xx<120 and yy>=0 and yy<120 and seed[yy*120+xx]then coat=true end end end
     local p=full:getPixel(x,y)
     if coat then overlay:drawPixel(x,y,p);body:drawPixel(x,y,clean:getPixel(x,y))else body:drawPixel(x,y,p)end
    end end end
    -- A restored ordinary-person source is retained separately even where the
    -- posed outer coating creates a wider silhouette than the underlying body.
    -- Detached dark coating outlines belong to the coating, not the restored
    -- person. Keep real large/skin components and transfer tiny remnants.
    local bodyGroups=components(body)
    for gi,g in ipairs(bodyGroups)do if gi>1 and g.n<=24 then
     for _,p in ipairs(g.pts)do overlay:drawPixel(p[1],p[2],full:getPixel(p[1],p[2]));body:drawPixel(p[1],p[2],0)end
    end end
   else
    local high={};for i,p in ipairs(pix)do local r,g,bb=pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p);if(r+g+bb)/3>183 and g>150 then high[p]=true end end
    for y=0,119 do for x=0,119 do if mainMask[y*120+x]then local p=full:getPixel(x,y);if high[p]then
     overlay:drawPixel(x,y,p);local under=pix[5]
     for dy=-1,1 do for dx=-1,1 do local xx,yy=x+dx,y+dy;if xx>=0 and xx<120 and yy>=0 and yy<120 then local np=full:getPixel(xx,yy);if pc.rgbaA(np)>0 and not high[np]then under=np end end end end
     body:drawPixel(x,y,under)
    else body:drawPixel(x,y,p)end end end end
   end
   local parts={body,overlay,fxImage};full=blank();for _,im in ipairs(parts)do full:drawImage(im)end
   -- Nearest sampling can extend the last source row by one pixel. Translate
   -- complete parts together instead of clipping soles or puddles.
   local fb=bounds(full);local dx=fb.bx<1 and(1-fb.bx)or fb.ex>118 and(118-fb.ex)or 0;local dy=fb.by<1 and(1-fb.by)or fb.ey>117 and(117-fb.ey)or 0
   if dx~=0 or dy~=0 then for li=1,3 do parts[li]=shift(parts[li],dx,dy)end;clean=shift(clean,dx,dy);full=blank();for _,im in ipairs(parts)do full:drawImage(im)end end
   if ch.role=='rescue'then clean:saveAs(root..'/Source/Sampled/'..id..'/'..a.name..string.format('/Restored_%02d.png',f))end
   local ms=a.durations[f];if id=='z02_large'then if a.name=='Walk'then ms=150 elseif a.name=='Run'then ms=90 elseif a.name=='Idle'then ms=230 end end
   master.frames[fi].duration=ms/1000;for li=1,3 do master:newCel(master.layers[li],fi,parts[li],Point(0,0))end
   full:saveAs(root..'/Source/Sampled/'..id..'/'..a.name..string.format('/Full_%02d.png',f))
   local sr=rects[f];local rec={frame=fi,character=id,action=a.name,local_frame=f,duration_ms=ms,loop=a.loop,hold_last=a.name=='Dead',source_png='Source/Generated/'..id..'/'..a.name..'.png',source_cell_xy={(f-1)%cols,(f-1)//cols},source_cell_size={cw,hh},source_rect_xywh={sr.x,sr.y,sr.w,sr.h},scale=sc,source_anchor_x=fxs[f],horizontal_registration=a.name=='Run'and 'body_bounds_center'or 'lowest_foot_span_center',source_floor_y=b.ey,root_pivot={52,118},target_lowest_y=floor+dy,airborne_offset_y=bob,finish_offset_xy={dx,dy},role=ch.role,dead_semantics=ch.role=='rescue'and 'slime_disabled_human_alive_rescue_wait'or 'gel_body_dissolves'}
   poses[fi]=parts;records[fi]=rec;allMeta.frames[#allMeta.frames+1]=rec
  end
  local tag=master:newTag(start,fi);tag.name=a.name;clips[#clips+1]={name=a.name,count=count,from_frame=start,to_frame=fi,loop=a.loop,hold_last=a.name=='Dead'}
 end
 master:saveAs(root..'/'..ch.native)
 for _,clip in ipairs(clips)do local s=sprite();for n=1,clip.count do local f=clip.from_frame+n-1;if n>1 then s:newEmptyFrame()end;s.frames[n].duration=records[f].duration_ms/1000;for li=1,3 do s:newCel(s.layers[li],n,Image(poses[f][li]),Point(0,0))end end;local t=s:newTag(1,clip.count);t.name=clip.name;s:saveAs(outdir..'/Clips/'..id..'_'..clip.name:lower()..'.aseprite');s:close()end
 for li,name in ipairs(names)do local s=sprite({name});for f=1,28 do if f>1 then s:newEmptyFrame()end;s.frames[f].duration=records[f].duration_ms/1000;s:newCel(s.layers[1],f,Image(poses[f][li]),Point(0,0))end;for _,clip in ipairs(clips)do local t=s:newTag(clip.from_frame,clip.to_frame);t.name=clip.name end;s:saveAs(outdir..'/Parts/Native/'..name..'.aseprite');s:close()end
 if ch.role=='rescue'then local s=sprite({'HumanBody'});for f=1,28 do if f>1 then s:newEmptyFrame()end;s.frames[f].duration=records[f].duration_ms/1000;s:newCel(s.layers[1],f,poses[f][1],Point(0,0))end;for _,clip in ipairs(clips)do local t=s:newTag(clip.from_frame,clip.to_frame);t.name=clip.name end;s:saveAs(outdir..'/human_without_slime_120.aseprite');s:close()end
 local gp=assert(io.open(outdir..'/Export/'..id..'_26.gpl','w'));gp:write('GIMP Palette\nName: '..id..' 26\nColumns:13\n#\n');for _,h in ipairs(hex)do gp:write(string.format('%3d %3d %3d #%s\n',tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),h))end;gp:close()
 write(outdir..'/pose_metadata.json',{character=id,name=ch.name,role=ch.role,canvas={120,120},palette=hex,layer_order=names,frames=records,clips=clips,root_pivot={52,118},left_root_pivot={68,118},source_generation='Built-in ImageGen, native nearest/palette/registration/material separation and restored clothed under-layer'})
 allMeta.characters[#allMeta.characters+1]={id=id,name=ch.name,role=ch.role,native=ch.native,frame_count=28,clips=5,palette_limit=26,root_pivot={52,118},layers=names}
 master:close();print('Built '..id..': 28 poses, 5 clips, 3 material/FX layers.')
end
write(root..'/Source/pose_metadata.json',allMeta)
print('Built all 3 human-form slime characters: 15 clips / 84 frames.')
