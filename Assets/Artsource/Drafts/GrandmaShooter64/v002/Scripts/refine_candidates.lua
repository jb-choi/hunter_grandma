-- Native Aseprite pixel refinement: material ramps, outlines, faces, and FX.
local root=assert(app.params['root']);local pc=app.pixelColor
local specs=json.decode(assert(io.open(root..'/Source/generation_prompts.json','r')):read('*a'))
local function rgba(h) return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255) end
local C={o='281e32',d='47243e',s='f4bf9f',m='d98b77',a='ae615b',i='ffe1b4',w='fff5d7',b='d66b6e',r='a1455e',h='aa9ba9',H='d7ccc8',j='7d6c86',g='efb34a',f='ffdc78',t='744632',T='a7663a',z='7c8d9e',n='424a61'}
local pixels={};for k,v in pairs(C) do pixels[k]=rgba(v) end
local im
local function px(x,y,c) if x>=0 and x<64 and y>=0 and y<64 then im:drawPixel(x,y,pixels[c]) end end
local function rect(x1,y1,x2,y2,c) for y=y1,y2 do for x=x1,x2 do px(x,y,c) end end end
local function line(x1,y1,x2,y2,c)
 local dx,dy=math.abs(x2-x1),math.abs(y2-y1);local sx,sy=x1<x2 and 1 or -1,y1<y2 and 1 or -1;local er=dx-dy
 while true do px(x1,y1,c);if x1==x2 and y1==y2 then break end;local e=2*er;if e>-dy then er=er-dy;x1=x1+sx end;if e<dx then er=er+dx;y1=y1+sy end end
end
local function grid(x,y,rows)
 for yy,row in ipairs(rows) do for xx=1,#row do local c=row:sub(xx,xx);if c~='.' then px(x+xx-1,y+yy-1,c) end end end
end
local function lowglasses(x,y)
 grid(x,y,{'.tt..tt.','tssttsst','.tt..tt.'})
end
local fns={
 function()
  rect(20,15,24,18,'s');rect(26,14,28,18,'s')
  line(21,15,23,15,'j');px(24,16,'j');px(26,14,'j');line(27,13,28,13,'j')
  line(21,17,23,17,'o');px(24,18,'a');px(27,15,'w');rect(28,15,28,16,'o')
  lowglasses(21,18);rect(19,19,20,19,'b');px(28,18,'m');px(29,19,'m')
  line(23,21,26,21,'d');rect(24,21,25,21,'w');px(25,22,'r')
 end,
 function()
  rect(28,13,30,16,'s');rect(32,12,34,16,'s')
  line(28,13,30,12,'j');line(32,12,33,12,'j');px(30,14,'w');rect(31,14,31,15,'o')
  px(33,14,'w');rect(34,14,34,15,'o');px(35,16,'m');rect(28,16,29,16,'b')
  px(30,18,'a');line(31,18,33,18,'d');px(32,19,'s');px(33,17,'a')
 end,
 function()
  rect(22,17,25,19,'s');rect(27,16,30,20,'s')
  line(22,17,24,16,'j');line(28,16,30,15,'j');line(22,18,24,18,'o')
  px(29,17,'w');rect(30,17,30,18,'o');lowglasses(23,19)
  rect(20,20,21,20,'b');px(31,20,'m');line(25,22,28,22,'d');rect(26,22,27,22,'w');px(27,23,'b')
 end,
 function()
  rect(28,22,31,25,'s');rect(33,21,35,25,'s')
  line(28,22,30,21,'j');line(33,21,35,22,'j');line(28,24,30,23,'o')
  px(34,23,'w');rect(35,23,35,24,'o');rect(27,25,28,25,'b');px(36,25,'m')
  grid(30,26,{'oddd','owwd','.rb.'})
 end,
 function()
  rect(23,17,26,19,'s');rect(28,16,31,19,'s')
  line(23,17,25,16,'j');line(28,16,30,15,'j');line(24,18,26,18,'o')
  px(29,17,'w');rect(30,17,30,18,'o');px(31,19,'m');px(23,19,'b')
  line(26,21,29,21,'d');rect(27,21,28,21,'w');px(30,20,'a')
 end
}
local muzzle={{51,20},{57,19},{48,20},{54,28},{55,18}}
local hairRects={{{8,4,30,17}},{{22,10,35,15},{5,17,25,28}},{{8,4,33,18}},{{15,18,35,24},{10,24,26,35}},{{12,13,33,19},{4,20,20,42}}}
local faceRects={{18,13,29,22},{27,12,35,19},{20,15,31,23},{26,21,36,28},{22,16,32,22}}
local skinMap={[rgba('ffc68b')]=pixels.s,[rgba('df9166')]=pixels.m,[rgba('ae624f')]=pixels.a,[rgba('ffe8b6')]=pixels.i}
local report={}
for idx,spec in ipairs(specs.candidates) do
 local src=Sprite{fromFile=root..'/Source/'..spec.name..'_curated.aseprite'}
 local base=Image(src.spec);base:drawSprite(src,1)
 local pal=src.palettes[1];local pcopy=Palette(#pal);for i=0,#pal-1 do
  local c=pal:getColor(i);local old=pc.rgba(c.red,c.green,c.blue,c.alpha);local value=skinMap[old]
  if value then pcopy:setColor(i,Color{r=pc.rgbaR(value),g=pc.rgbaG(value),b=pc.rgbaB(value),a=255}) else pcopy:setColor(i,c) end
 end
 src:close()
 for y=0,63 do for x=0,63 do local p=base:getPixel(x,y);if skinMap[p] then base:drawPixel(x,y,skinMap[p]) end end end
 local mx,my=table.unpack(muzzle[idx]);local removed=0
 -- Separate the entire authored muzzle zone, not just bright colors.
 for y=my-6,my+6 do for x=mx,63 do if pc.rgbaA(base:getPixel(x,y))>0 then base:drawPixel(x,y,0);removed=removed+1 end end end
 -- Hair uses a silver ramp rather than the steel/navy highlights sampled from the concept.
 for _,r in ipairs(hairRects[idx]) do for y=r[2],r[4] do for x=r[1],r[3] do
  local p=base:getPixel(x,y)
  if p==rgba('7c8d9e') then base:drawPixel(x,y,pixels.h) elseif p==rgba('5f627a') then base:drawPixel(x,y,pixels.j) end
 end end end
 local fr=faceRects[idx]
 for y=fr[2],fr[4] do for x=fr[1],fr[3] do
  local p=base:getPixel(x,y);if p==rgba('efb34a') or p==rgba('d3934d') then base:drawPixel(x,y,pixels.s) end
 end end
 -- Remove isolated sample specks only, while preserving connected clusters and anatomy.
 local remove={}
 for y=1,62 do for x=1,62 do
  if pc.rgbaA(base:getPixel(x,y))>0 then
   local n=0;for yy=y-1,y+1 do for xx=x-1,x+1 do if not(xx==x and yy==y) and pc.rgbaA(base:getPixel(xx,yy))>0 then n=n+1 end end end
   if n==0 then table.insert(remove,{x,y}) end
  end
 end end
 for _,p in ipairs(remove) do base:drawPixel(p[1],p[2],0) end
 local contour=Image(64,64,ColorMode.RGB)
 for y=1,61 do for x=1,62 do
  if pc.rgbaA(base:getPixel(x,y))==0 and (pc.rgbaA(base:getPixel(x-1,y))>0 or pc.rgbaA(base:getPixel(x+1,y))>0 or pc.rgbaA(base:getPixel(x,y-1))>0 or pc.rgbaA(base:getPixel(x,y+1))>0) then
   contour:drawPixel(x,y,pixels.o)
  end
 end end
 local face=Image(64,64,ColorMode.RGB);im=face;fns[idx]()
 local fx=Image(64,64,ColorMode.RGB);im=fx
 grid(mx,my-2,{'..g...','.gig..','gfwifg','.gig..','..g...'})
 local s=Sprite(64,64,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pcopy)
 local function layer(name,image)
  local l=s:newLayer();l.name=name;s:newCel(l,1,image,Point(0,0))
 end
 layer('01_Contour',contour);layer('02_Character_Combined',base);layer('03_Face_Expression',face);layer('04_Muzzle_FX',fx)
 s.frames[1].duration=0.1;local tag=s:newTag(1,1);tag.name='shoot_base'
 local out=root..'/'..spec.name..'.aseprite'
 assert(not app.fs.isFile(out) or app.params['overwrite']=='true','Existing native: use a new folder or preserve edits first.')
 s:saveAs(out)
 local full=Image(s.spec);full:drawSprite(s,1);full:saveAs(root..'/Export/'..spec.name..'.png')
 s.layers[4].isVisible=false;local plain=Image(s.spec);plain:drawSprite(s,1);plain:saveAs(root..'/Export/'..spec.name..'_no_fx.png')
 s:close()
 table.insert(report,{name=spec.name,muzzle={mx,my},removed_generated_fx_pixels=removed,removed_isolated_sample_pixels=#remove,pixel_method='native Aseprite material-ramp mapping + silhouette contour + authored facial pixels + redrawn muzzle FX',layers=4})
end
local f=assert(io.open(root..'/QA/refinement.json','w'));f:write(json.encode(report));f:close()
print('Saved 5 refined native candidates, 4 layers each, full and no-FX PNGs.')
