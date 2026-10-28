-- Aseprite-native age-readability pass for candidates 05 and 07 only.
local root=assert(app.params['root']);local pc=app.pixelColor
local specs=json.decode(assert(io.open(root..'/Source/generation_prompts.json','r')):read('*a'))
local function rgba(h) return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255) end
local C={o='281e32',d='47243e',S='f4bf9f',m='d98b77',a='ae615b',I='ffe1b4',W='fff5d7',b='d66b6e',r='a1455e',h='d7ccc8',j='aa9ba9',k='7d6c86',g='efb34a',f='ffdc78',t='744632'}
local color={};for k,h in pairs(C) do color[k]=rgba(h) end
local im
local function px(x,y,c) im:drawPixel(x,y,color[c]) end
local function rect(x1,y1,x2,y2,c) for y=y1,y2 do for x=x1,x2 do px(x,y,c) end end end
local function grid(x,y,rows)
 for yy,row in ipairs(rows) do for xx=1,#row do local c=row:sub(xx,xx);if c~='.' then px(x+xx-1,y+yy-1,c) end end end
end
local age={
 [5]=function()
  -- Forehead, bushy white brows and half-closed eyes above low spectacles.
  rect(31,16,33,18,'S');rect(35,16,37,18,'S')
  rect(33,14,35,14,'m');px(36,15,'m')
  rect(31,16,33,16,'h');rect(35,16,37,16,'h')
  rect(32,17,33,17,'d');rect(36,17,37,17,'d')
  grid(30,18,{'.tt..tt.','tSSttSSt','.tt..tt.'})
  px(38,19,'m');px(38,20,'I')
  px(30,20,'a');px(31,21,'m');px(37,21,'a')
  rect(33,22,35,22,'t');px(36,21,'t');rect(33,23,35,23,'m')
 end,
 [7]=function()
  rect(30,21,32,24,'S');rect(34,21,36,24,'S')
  rect(31,20,34,20,'m');px(35,21,'m')
  rect(30,22,32,22,'h');rect(34,22,36,22,'h')
  rect(31,23,32,23,'d');rect(35,23,36,23,'d')
  grid(29,24,{'.tt..tt.','tSSttSSt','.tt..tt.'})
  px(37,25,'m');px(38,25,'m');px(38,26,'S')
  px(28,26,'a');px(29,27,'m');px(36,27,'a')
  grid(31,27,{'ddd','dWd','.b.'});px(30,29,'m');px(34,29,'m')
 end
}
local muzzle={[5]={57,21},[7]={56,29}}
local headRect={[5]={26,12,39,23},[7]={25,18,39,29}}
local report={}
for _,c in ipairs(specs.candidates) do
 local src=Sprite{fromFile=root..'/Source/'..c.name..'_curated.aseprite'}
 local base=Image(src.spec);base:drawSprite(src,1)
 local pal=Palette(#src.palettes[1]);for i=0,#pal-1 do pal:setColor(i,src.palettes[1]:getColor(i)) end
 src:close()
 local mx,my=table.unpack(muzzle[c.n]);local removed=0
 for y=my-6,my+6 do for x=mx,63 do if pc.rgbaA(base:getPixel(x,y))>0 then base:drawPixel(x,y,0);removed=removed+1 end end end
 -- Silver hair must not inherit steel-blue highlights. Face gold highlights are skin.
 local r=headRect[c.n]
 for y=0,39 do for x=0,40 do
  local p=base:getPixel(x,y)
  if p==rgba('7c8d9e') then base:drawPixel(x,y,color.j) elseif p==rgba('5f627a') then base:drawPixel(x,y,color.k) end
 end end
 for y=r[2],r[4] do for x=r[1],r[3] do
  local p=base:getPixel(x,y)
  if p==rgba('efb34a') then base:drawPixel(x,y,color.S) elseif p==rgba('d3934d') then base:drawPixel(x,y,color.m) end
 end end
 local specks={}
 for y=1,62 do for x=1,62 do
  if pc.rgbaA(base:getPixel(x,y))>0 then
   local n=0;for yy=y-1,y+1 do for xx=x-1,x+1 do if (xx~=x or yy~=y) and pc.rgbaA(base:getPixel(xx,yy))>0 then n=n+1 end end end
   if n==0 then table.insert(specks,{x,y}) end
  end
 end end
 for _,p in ipairs(specks) do base:drawPixel(p[1],p[2],0) end
 local contour=Image(64,64,ColorMode.RGB)
 for y=1,61 do for x=1,62 do
  if pc.rgbaA(base:getPixel(x,y))==0 and (pc.rgbaA(base:getPixel(x-1,y))>0 or pc.rgbaA(base:getPixel(x+1,y))>0 or pc.rgbaA(base:getPixel(x,y-1))>0 or pc.rgbaA(base:getPixel(x,y+1))>0) then contour:drawPixel(x,y,color.o) end
 end end
 local face=Image(64,64,ColorMode.RGB);im=face;age[c.n]()
 local fx=Image(64,64,ColorMode.RGB);im=fx;grid(mx,my-2,{'..g...','.gIg..','gfWIfg','.gIg..','..g...'})
 local s=Sprite(64,64,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pal)
 local function layer(name,pixels) local l=s:newLayer();l.name=name;s:newCel(l,1,pixels,Point(0,0)) end
 layer('01_Contour',contour);layer('02_Character_Combined',base);layer('03_Aged_Face_Details',face);layer('04_Muzzle_FX',fx)
 s.frames[1].duration=0.1;local tag=s:newTag(1,1);tag.name='shoot_base'
 local native=root..'/'..c.name..'.aseprite'
 assert(not app.fs.isFile(native) or app.params['overwrite']=='true','Existing revised native; preserve edits first.')
 s:saveAs(native)
 local full=Image(s.spec);full:drawSprite(s,1);full:saveAs(root..'/Export/'..c.name..'.png')
 s.layers[4].isVisible=false;local plain=Image(s.spec);plain:drawSprite(s,1);plain:saveAs(root..'/Export/'..c.name..'_no_fx.png')
 s:close()
 table.insert(report,{candidate=c.n,name=c.name,muzzle={mx,my},removed_generated_fx_pixels=removed,isolated_sample_specks_removed=#specks,changes='aged cheeks and jowls, bushy white brows, low oval spectacles, small hooded eyes, smile folds, rounder middle and shorter legs; original costume and gun preserved in edited source'})
end
local f=assert(io.open(root..'/QA/refinement.json','w'));f:write(json.encode(report));f:close()
print('Saved aged 05 and 07 only; 64x64, 4 layers, full and no-FX PNGs.')
