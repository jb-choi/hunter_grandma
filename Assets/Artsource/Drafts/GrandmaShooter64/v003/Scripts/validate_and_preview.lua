-- Reopen final natives; inspect visible states and actual pixel exports.
local root=assert(app.params['root']);local pc=app.pixelColor
local spec=json.decode(assert(io.open(root..'/Source/generation_prompts.json','r')):read('*a'))
local function rgba(h) return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255) end
local function flat(s) local im=Image(s.spec);im:drawSprite(s,1);return im end
local function fill(im,x1,y1,x2,y2,c) for y=y1,y2 do for x=x1,x2 do im:drawPixel(x,y,c) end end end
local function scale(im,n,path)
 local r=Image(im.width*n,im.height*n,ColorMode.RGB)
 for y=0,r.height-1 do for x=0,r.width-1 do r:drawPixel(x,y,im:getPixel(x//n,y//n)) end end
 r:saveAs(path);return r
end
local function bg(im,color)
 local r=Image(im.width,im.height,ColorMode.RGB);r:clear(rgba(color));r:drawImage(im);return r
end
local F={
 ['0']={'111','101','101','101','111'},['1']={'010','110','010','010','111'},['2']={'111','001','111','100','111'},['3']={'111','001','111','001','111'},
 ['4']={'101','101','111','001','001'},['5']={'111','100','111','001','111'},['6']={'111','100','111','101','111'},['7']={'111','001','010','010','010'},
 ['8']={'111','101','111','101','111'},['9']={'111','101','111','001','111'},
 A={'010','101','111','101','101'},B={'110','101','110','101','110'},C={'111','100','100','100','111'},D={'110','101','101','101','110'},
 E={'111','100','110','100','111'},F={'111','100','110','100','100'},G={'111','100','101','101','111'},H={'101','101','111','101','101'},
 I={'111','010','010','010','111'},J={'111','001','001','101','111'},K={'101','101','110','101','101'},L={'100','100','100','100','111'},
 M={'10001','11011','10101','10101','10101'},N={'1001','1101','1011','1001','1001'},O={'111','101','101','101','111'},P={'110','101','110','100','100'},
 Q={'111','101','101','111','001'},R={'110','101','110','101','101'},S={'111','100','111','001','111'},T={'111','010','010','010','010'},
 U={'101','101','101','101','111'},V={'101','101','101','101','010'},W={'10101','10101','10101','11011','10001'},X={'101','101','010','101','101'},
 Y={'101','101','010','010','010'},Z={'111','001','010','100','111'},['/']={'001','001','010','100','100'},['-']={'000','000','111','000','000'}
}
local function text(im,x,y,str,color,mul)
 mul=mul or 1
 for i=1,#str do local g=F[str:sub(i,i)]
  if g then
   for yy,row in ipairs(g) do for xx=1,#row do if row:sub(xx,xx)=='1' then fill(im,x+(xx-1)*mul,y+(yy-1)*mul,x+xx*mul-1,y+yy*mul-1,color) end end end
   x=x+(#g[1]+1)*mul
  else x=x+3*mul end
 end
end
local function inspect(im)
 local count,partial=0,0;local colors={};local bx,by,ex,ey=64,64,-1,-1
 for y=0,63 do for x=0,63 do local p=im:getPixel(x,y);local a=pc.rgbaA(p)
  if a>0 then count=count+1;colors[p]=true;bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y);if a~=255 then partial=partial+1 end end
 end end
 local n=0;for _ in pairs(colors) do n=n+1 end
 return {opaque_pixels=count,partial_alpha_pixels=partial,used_opaque_colors=n,bounds_xywh={bx,by,ex-bx+1,ey-by+1},last_opaque_y=ey}
end
local function diff(a,b)
 assert(a.width==b.width and a.height==b.height);local n=0
 for y=0,a.height-1 do for x=0,a.width-1 do if a:getPixel(x,y)~=b:getPixel(x,y) then n=n+1 end end end
 return n
end
local function write(path,obj) local f=assert(io.open(path,'w'));f:write(json.encode(obj));f:close() end
for _,dir in ipairs({'QA/LayerReview/States','QA/LayerReview/Solo','QA/LayerReview/Hidden','Previews'}) do app.fs.makeAllDirectories(root..'/'..dir) end
local flats,clean,reports={},{},{}
for i,c in ipairs(spec.candidates) do
 local s=Sprite{fromFile=root..'/'..c.name..'.aseprite'}
 assert(s.width==64 and s.height==64 and #s.frames==1 and #s.layers==4)
 local image=flat(s);flats[i]=image
 local r=inspect(image);r.name=c.name;r.candidate=c.n;r.canvas={64,64};r.frames=1;r.tag=s.tags[1].name;r.frame_duration_ms=s.frames[1].duration*1000;r.layers={};r.visibility_states={};r.palette={}
 r.native_png_difference=diff(image,Image{fromFile=root..'/Export/'..c.name..'.png'})
 assert(r.native_png_difference==0 and r.partial_alpha_pixels==0 and r.used_opaque_colors<=32)
 local colorSet={}
 for p=0,#s.palettes[1]-1 do
  local v=s.palettes[1]:getColor(p);table.insert(r.palette,{index=p,r=v.red,g=v.green,b=v.blue,a=v.alpha});colorSet[pc.rgba(v.red,v.green,v.blue,v.alpha)]=true
 end
 assert(#r.palette==33)
 for y=0,63 do for x=0,63 do local p=image:getPixel(x,y);assert(pc.rgbaA(p)==0 or colorSet[p],c.name..': palette mismatch') end end
 for j,l in ipairs(s.layers) do assert(l.isVisible and l.opacity==255);table.insert(r.layers,{index=j,name=l.name,isVisible=l.isVisible,opacity=l.opacity,blend=tostring(l.blendMode)}) end
 scale(bg(image,'eee5d6'),8,root..'/Previews/'..c.name..'_8x.png')
 scale(bg(image,'252637'),8,root..'/Previews/'..c.name..'_dark_8x.png')
 s.layers[4].isVisible=false;clean[i]=flat(s)
 r.no_fx_native_png_difference=diff(clean[i],Image{fromFile=root..'/Export/'..c.name..'_no_fx.png'})
 assert(r.no_fx_native_png_difference==0);s.layers[4].isVisible=true
 for _,mode in ipairs({'Solo','Hidden'}) do
  local sheet=Image(128,128,ColorMode.RGB);sheet:clear(rgba('eee5d6'))
  for j,l in ipairs(s.layers) do
   for k,ll in ipairs(s.layers) do ll.isVisible=(mode=='Solo' and k==j) or (mode=='Hidden' and k~=j) end
   local statePath='QA/LayerReview/States/'..c.name..'_'..mode:lower()..'_'..j..'.aseprite'
   s:saveAs(root..'/'..statePath)
   local state=Sprite{fromFile=root..'/'..statePath};local visibility={}
   for k,ll in ipairs(state.layers) do visibility[k]=ll.isVisible;assert(ll.isVisible==((mode=='Solo' and k==j) or (mode=='Hidden' and k~=j))) end
   local rendered=flat(state)
   local pngPath='QA/LayerReview/'..mode..'/'..c.name..'_'..j..'.png'
   rendered:saveAs(root..'/'..pngPath)
   local x,y=((j-1)%2)*64,((j-1)//2)*64
   sheet:drawImage(bg(rendered,(j%2==0) and 'ded5c9' or 'f5ecdf'),Point(x,y));text(sheet,x+2,y+2,string.format('%02d',j),rgba('7d6c86'))
   table.insert(r.visibility_states,{mode=mode,layer=j,layer_name=l.name,isVisible=visibility,state_file=statePath,png=pngPath,pixels=inspect(rendered)})
   state:close()
  end
  scale(sheet,4,root..'/QA/LayerReview/'..c.name..'_'..mode:lower()..'_4x.png')
 end
 for _,l in ipairs(s.layers) do l.isVisible=true end
 r.restored_composite_difference=diff(flat(s),image);assert(r.restored_composite_difference==0)
 s:close();table.insert(reports,r)
end
-- Appended to the shared native-validation prefix by the build command.
local before={}
for i,c in ipairs(spec.candidates) do before[i]=Image{fromFile=root..'/Source/Before/'..c.name..'.png'} end
local captions={'05 REDHOOD','07 MOSSWITCH'}
local current=Image(160,100,ColorMode.RGB);current:clear(rgba('f7f0e4'))
text(current,8,6,'GRANDMA / AGE REVISION',rgba('281e32'))
for i=1,2 do
 local x=8+(i-1)*80
 fill(current,x-1,18,x+64,83,rgba('d9cfc4'));current:drawImage(bg(flats[i],'eee5d6'),Point(x,19));text(current,x,89,captions[i],rgba('281e32'))
end
scale(current,6,root..'/Previews/aged_candidates_05_07.png')
local comparison=Image(160,202,ColorMode.RGB);comparison:clear(rgba('f7f0e4'))
text(comparison,8,6,'BEFORE',rgba('7d6c86'));text(comparison,88,6,'AFTER / V003',rgba('281e32'))
for i=1,2 do
 local y=22+(i-1)*88
 comparison:drawImage(bg(before[i],'eee5d6'),Point(8,y));comparison:drawImage(bg(flats[i],'eee5d6'),Point(88,y))
 text(comparison,8,y+69,captions[i],rgba('281e32'));text(comparison,88,y+69,captions[i],rgba('281e32'))
end
text(comparison,8,191,'64 X 64 / NEAREST 6X',rgba('7d6c86'))
scale(comparison,6,root..'/Previews/age_before_after_64px_6x.png')
local native=Image(128,64,ColorMode.RGB);native:drawImage(flats[1]);native:drawImage(flats[2],Point(64,0));native:saveAs(root..'/Previews/aged_candidates_native.png')
-- Keep 04/06/08 from v002; only substitute revised 05 and 07 in the current set.
local board=Image(232,198,ColorMode.RGB);board:clear(rgba('f7f0e4'))
text(board,8,6,'GRANDMA / CURRENT BASES',rgba('281e32'))
local images={Image{fromFile=root..'/../v002/Export/grandma_04_sundeok.png'},flats[1],Image{fromFile=root..'/../v002/Export/grandma_06_teatime.png'},flats[2],Image{fromFile=root..'/../v002/Export/grandma_08_wildrose.png'}}
local titles={'04 SUNDEOK','05 REDHOOD','06 TEATIME','07 MOSSWITCH','08 WILDROSE'}
local slots={{8,19},{84,19},{160,19},{46,105},{122,105}}
for i,p in ipairs(slots) do
 fill(board,p[1]-1,p[2]-1,p[1]+64,p[2]+64,rgba('d9cfc4'));board:drawImage(bg(images[i],'eee5d6'),Point(p[1],p[2]));text(board,p[1]+1,p[2]+69,titles[i],rgba('281e32'))
end
text(board,8,187,'05 / 07 AGE REVISION V003',rgba('7d6c86'))
scale(board,5,root..'/Previews/current_candidates_04-08_64px.png')
local concepts=Image(560,650,ColorMode.RGB);concepts:clear(rgba('f7f0e4'))
text(concepts,16,12,'CONCEPT / BEFORE AND AFTER',rgba('281e32'),3)
for i,c in ipairs(spec.candidates) do
 for j,part in ipairs({'Before','Generated'}) do
  local path=root..'/Source/'..part..'/'..c.name..'_generated.png'
  local source=Image{fromFile=path};local thumb=Image(240,240,ColorMode.RGB)
  for y=0,239 do for x=0,239 do thumb:drawPixel(x,y,source:getPixel(math.min(source.width-1,math.floor((x+0.5)*source.width/240)),math.min(source.height-1,math.floor((y+0.5)*source.height/240)))) end end
  local x,y=20+(j-1)*280,48+(i-1)*300
  concepts:drawImage(thumb,Point(x,y));text(concepts,x,y+248,string.format('%02d',c.n)..((j==1) and ' BEFORE' or ' AFTER'),rgba('281e32'),3)
 end
end
concepts:saveAs(root..'/Previews/age_before_after_concepts.png')
write(root..'/QA/native_validation.json',{schema_version=1,date='2026-10-21',tool='Aseprite Lua independent reload of 2 revised natives and 16 saved visible states',candidates=reports})
print('PASS: 2 age revisions, 4 native/PNG comparisons, 16 reopened layer states, palette membership and binary alpha.')

