local root=assert(app.params['root']);local pc=app.pixelColor;local out=root..'/TacticalWalk'
local original=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'};assert(#original.frames==27,'Expected original27 frames; inspect changes before append')
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
local source=Image{fromFile=out..'/Source/walk_v2_tactical_body_generated.png'};assert(source.width%3==0 and source.height%2==0)
local cw,ch=source.width//3,source.height//2;local cells,bb,hairX={}, {},{};local maxH=0
for f=1,6 do local cell=Image(cw,ch,ColorMode.RGB);cell:drawImage(source,Point(-((f-1)%3)*cw,-((f-1)//3)*ch));local b=bounds(cell);cells[f]=cell;bb[f]=b;maxH=math.max(maxH,b.h)
 local hx1,hx2=cw,-1;for y=b.by,math.floor(b.by+b.h*.5)do for x=b.bx,b.ex do if hair[map(cell:getPixel(x,y))]then hx1=math.min(hx1,x);hx2=math.max(hx2,x)end end end;assert(hx2>=hx1);hairX[f]=(hx1+hx2)/2
end
local sc=106/maxH;local baseParts={};for li=1,7 do baseParts[li]=cel(original,li,19)end;local baseAn=anatomy(baseParts[1]);local targetX=baseAn.head[1]
local minx=120;for f=1,6 do minx=math.min(minx,targetX+(bb[f].bx-hairX[f])*sc)end;if minx<2 then targetX=targetX+2-minx end
local poses,records={},{};local contact=Image(720,240,ColorMode.RGB);contact:clear(pc.rgba(238,229,214,255))
local referenceClip=sprite();local preview=Sprite(360,360,ColorMode.RGB);local compare=Sprite(720,360,ColorMode.RGB)
for _,d in ipairs({'Source/Sampled','Export/Frames','Export/Left','Export/Sheets','Parts','QA/PreviewSources'})do app.fs.makeAllDirectories(out..'/'..d)end
for f=1,6 do
 local body=blank();local cell,b=cells[f],bb[f]
 for y=0,119 do for x=0,119 do local sx=math.floor(hairX[f]+(x-targetX)/sc+.5);local sy=math.floor(b.ey+(y-117)/sc+.5);if sx>=0 and sx<cw and sy>=0 and sy<ch then body:drawPixel(x,y,map(cell:getPixel(sx,sy)))end end end
 -- A ready firearm tracks the steady head/cheek, not the changing sleeve
 -- outline. This avoids weapon pumping as the lower-body gait changes.
 local an=anatomy(body);local dx=math.floor(an.head[1]-baseAn.head[1]+.5);local dy=math.floor(an.head[2]-baseAn.head[2]+.5);local wb=bounds(baseParts[3]);dx=math.max(1-wb.bx,math.min(118-wb.ex,dx))
 local parts={body,shift(baseParts[2],dx,dy),shift(baseParts[3],dx,dy),shift(baseParts[4],dx,dy),blank(),blank(),blank()};local full=flat(parts);poses[f]=parts
 local fb=bounds(full);assert(fb.bx>=1 and fb.ex<=118 and fb.by>=1 and fb.ey<=117,'Tactical pose clipped '..f)
 local hipSum,hipN=0,0;for y=79,94 do for x=0,119 do if pants[body:getPixel(x,y)]then hipSum=hipSum+x;hipN=hipN+1 end end end
 local rec={frame=27+f,action='walk_v2_tactical',aseprite_tag='walk_v2_tactical',local_frame=f,duration_ms=140,source='TacticalWalk/Source/Sampled/Body_'..string.format('%02d',f)..'.png',floor_boundary_y=118,root_pivot={60,118},head_center=an.head,hip_center_x=hipSum/math.max(1,hipN),head_ahead_of_hip_px=an.head[1]-hipSum/math.max(1,hipN),upper_source_frame=19,upper_source_action='Aim',upper_offset_xy={dx,dy},muzzle={106+dx,38+dy},weapon_pivot={61+dx,46+dy},weapon_angle=0,hand_front={56+dx,47+dy},hand_back={84+dx,49+dy},support_grip={85+dx,46+dy},registration='New generated inclined tactical body; independent authored Aim arms and immutable shotgun, head and cheek registration',source_scale=sc}
 records[f]=rec
 body:saveAs(out..'/Source/Sampled/Body_'..string.format('%02d',f)..'.png');full:saveAs(out..'/Export/Frames/sundeok_walk_v2_tactical_'..string.format('%02d',f)..'.png')
 local left=blank();for y=0,119 do for x=0,119 do left:drawPixel(119-x,y,full:getPixel(x,y))end end;left:saveAs(out..'/Export/Left/sundeok_walk_v2_tactical_'..string.format('%02d',f)..'.png')
 if f>1 then referenceClip:newEmptyFrame();preview:newEmptyFrame();compare:newEmptyFrame()end
 referenceClip.frames[f].duration=.14;preview.frames[f].duration=.14;compare.frames[f].duration=.14
 for li=1,7 do referenceClip:newCel(referenceClip.layers[li],f,parts[li])end
 local bg=blank();bg:clear(pc.rgba(238,229,214,255));bg:drawImage(full);preview:newCel(preview.layers[1],f,scale(bg,3))
 local old=blank();old:clear(pc.rgba(238,229,214,255));old:drawSprite(original,4+f);local pair=Image(240,120,ColorMode.RGB);pair:drawImage(old);pair:drawImage(bg,Point(120,0));compare:newCel(compare.layers[1],f,scale(pair,3))
 contact:drawImage(old,Point((f-1)*120,0));contact:drawImage(bg,Point((f-1)*120,120))
end
local tag=referenceClip:newTag(1,6);tag.name='walk_v2_tactical';referenceClip:saveAs(out..'/Source/walk_v2_tactical_candidate.aseprite');referenceClip:close()
preview:saveAs(out..'/QA/PreviewSources/walk_v2_tactical_3x.aseprite');compare:saveAs(out..'/QA/PreviewSources/walk_comparison_3x.aseprite');preview:close();compare:close()
contact:saveAs(out..'/Previews/normal_vs_tactical_native.png');scale(contact,2):saveAs(out..'/Previews/normal_vs_tactical_2x.png')
-- Append to the existing native: preserve every original cel and duration.
for f=1,6 do local frame=original:newEmptyFrame();frame.duration=.14;for li=1,7 do original:newCel(original.layers[li],27+f,poses[f][li])end end
while #original.tags>0 do original:deleteTag(original.tags[1])end
local specs={{'Idle',1,4},{'walk_v1_normal',5,10},{'Reload',11,16},{'Aim',17,19},{'Fire',20,23},{'Collect',24,27},{'walk_v2_tactical',28,33}}
for _,v in ipairs(specs)do local t=original:newTag(v[2],v[3]);t.name=v[1]end
original:saveAs(out..'/Source/grandma_01_sundeok_120_candidate.aseprite');original:close()
write(out..'/pose_metadata.json',{canvas={120,120},root_pivot={60,118},tag='walk_v2_tactical',normal_tag='walk_v1_normal',frames=records,source_grid={3,2},palette_opaque=26,upper_parts_reused_from='Aim frame19',gun_separate=true,game_runtime_changed=false})
print('Built6 new tactical poses and33-frame candidate; original27 cels retained. Native tag ranges explicitly bounded.')
