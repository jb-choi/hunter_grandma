-- Native pixel edit of Sundeok: preserve her head/lower body; author tea pose.
local root=assert(app.params['root'])
local dest=root..'/grandma_04_sundeok_tea.aseprite'
assert(not app.fs.isFile(dest) or app.params['revise']=='true','Tea native already exists')
local pc=app.pixelColor
local src=Sprite{fromFile=root..'/grandma_04_sundeok.aseprite'}
src.layers[4].isVisible=false
local original=Image(src.spec);original:drawSprite(src,1)
local palette=Palette(#src.palettes[1])
for i=0,#palette-1 do palette:setColor(i,src.palettes[1]:getColor(i)) end
src.layers[3].isVisible=false
local base=Image(src.spec);base:drawSprite(src,1)
local C={o='281e32',d='47243e',k='6d2949',r='a1455e',p='d66b6e',a='ae615b',s='f4bf9f',m='d98b77',l='ffe1b4',w='fff5d7',h='d7ccc8',j='aa9ba9',t='744632',b='a7663a',g='d3934d',y='efb34a',v='153d49',c='2e6c77',u='5b9d9c',n='7c8d9e',f='ee403d'}
local ink={}
for k,h in pairs(C) do ink[k]=pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255) end
local function blank() return Image(64,64,ColorMode.RGB) end
local im
local function px(x,y,c) if x>=0 and x<64 and y>=0 and y<64 then im:drawPixel(x,y,ink[c]) end end
local function rect(x1,y1,x2,y2,c) for y=y1,y2 do for x=x1,x2 do px(x,y,c) end end end
local function line(x1,y1,x2,y2,c)
 local dx,dy=math.abs(x2-x1),math.abs(y2-y1);local sx,sy=x1<x2 and 1 or -1,y1<y2 and 1 or -1;local er=dx-dy
 while true do px(x1,y1,c);if x1==x2 and y1==y2 then break end;local e=er*2;if e>-dy then er=er-dy;x1=x1+sx end;if e<dx then er=er+dx;y1=y1+sy end end
end
local function poly(points,c)
 local lo,hi=63,0
 for _,p in ipairs(points) do lo=math.min(lo,p[2]);hi=math.max(hi,p[2]) end
 for y=lo,hi do local xs={}
  for i,p in ipairs(points) do local q=points[i%#points+1]
   if (p[2]<=y and q[2]>y) or(q[2]<=y and p[2]>y) then xs[#xs+1]=p[1]+(y-p[2])*(q[1]-p[1])/(q[2]-p[2]) end
  end
  table.sort(xs);for i=1,#xs-1,2 do for x=math.ceil(xs[i]),math.floor(xs[i+1]) do px(x,y,c) end end
 end
 for i,p in ipairs(points) do local q=points[i%#points+1];line(p[1],p[2],q[1],q[2],c) end
end
local function grid(x,y,rows)
 for yy,row in ipairs(rows) do for xx=1,#row do local c=row:sub(xx,xx);if c~='.' then px(x+xx-1,y+yy-1,c) end end end
end
local legs=blank();im=legs
for y=38,63 do for x=0,63 do legs:drawPixel(x,y,original:getPixel(x,y)) end end
-- Entire torso beneath both arms; independently editable, with covered surfaces.
local body=blank();im=body
poly({{14,22},{24,22},{29,24},{32,31},{34,38},{27,41},{18,40},{10,37},{11,28}},'o')
poly({{14,23},{22,23},{28,25},{30,32},{32,37},{26,39},{18,38},{12,36},{12,28}},'b')
poly({{17,24},{24,24},{27,28},{29,35},{26,38},{19,37},{15,34}},'g')
poly({{21,25},{25,26},{27,31},{26,36},{23,37},{22,32}},'y')
line(20,26,19,35,'t');line(21,27,21,35,'g')
rect(17,29,18,31,'y');rect(14,33,16,35,'g');rect(27,34,28,36,'y')
line(13,36,18,38,'t');line(26,38,31,36,'b')
px(23,28,'w');px(23,31,'w');px(24,34,'w')
-- Far arm bent upward, with a red cardigan sleeve and separate skin hand.
local rear=blank();im=rear
poly({{26,24},{30,25},{34,29},{36,30},{38,26},{41,26},{41,30},{38,35},{35,36},{30,33},{27,29}},'o')
poly({{27,25},{30,26},{34,31},{36,32},{38,28},{40,28},{39,31},{37,34},{35,34},{31,31},{28,28}},'r')
line(29,26,34,31,'p');line(35,33,37,31,'p');line(30,30,33,33,'k')
rect(38,27,40,28,'p');line(38,29,39,29,'k')
local head=blank();im=head
for y=0,22 do for x=0,30 do
 if y<19 or x<29 then head:drawPixel(x,y,base:getPixel(x,y)) end
end end
-- Remove the former shotgun overlap; reconstruct cheek, chin and neck.
for y=19,22 do for x=27,30 do head:drawPixel(x,y,0) end end
rect(27,19,28,20,'s');px(29,19,'m');px(29,20,'o')
line(26,21,28,21,'m');line(24,22,27,22,'o')
rect(19,22,23,23,'s');line(19,24,22,24,'m')
local face=blank()
local fc=src.layers[3]:cel(1)
if fc then face:drawImage(fc.image,fc.position) end
-- Same glasses and wink, with the cup rim touching the mouth at (28,21).
local cup=blank();im=cup
grid(28,20,{
 '.oooooooo....',
 'owttttttwooo.',
 'owllllllwuhho',
 'owwhhhhhuo.wo',
 '.owwhhhuuo.wo',
 '.owwhhhuuohho',
 '.ouuuuuucooo.',
 '..ouuuuco....',
 '...ooooo.....'
})
-- Teal glazed cup, tea surface and a single pale ornamental band.
line(30,24,34,24,'w');px(33,25,'y')
local arm=blank();im=arm
poly({{10,22},{14,23},{16,27},{17,30},{21,30},{24,28},{27,30},{25,34},{21,36},{15,35},{11,32},{8,27},{8,24}},'o')
poly({{10,23},{13,24},{14,28},{16,32},{20,32},{23,30},{25,31},{23,33},{20,34},{15,33},{12,31},{9,27},{9,24}},'r')
line(10,24,11,28,'p');line(12,29,15,32,'p');line(16,33,20,33,'k')
px(10,27,'k');rect(12,25,13,26,'p');line(21,31,23,30,'p')
-- A red cuff below the supporting palm, keeping it distinct from the cup.
line(23,29,25,31,'p');line(24,29,26,30,'k')
local hands=blank();im=hands
grid(25,27,{
 '...mmmmmm.',
 '..mssssssm',
 '.mssllllsm',
 'msssssmmmo',
 'omssmmo...',
 '.ommo.....'
})
-- The far hand wraps the outer handle instead of floating beside the cup.
grid(38,24,{'mmm.','slsm','sssm','mmmo','.oo.'})
local steam=blank();im=steam
grid(34,12,{'..h...h','..wh..w','...h.hw','..h..h.','.wh..wh','..h...h'})
local s=Sprite(64,64,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(palette)
local function layer(name,img) local l=s:newLayer();l.name=name;l.blendMode=BlendMode.NORMAL;s:newCel(l,1,img,Point(0,0)) end
layer('01_Legs_Shoes',legs);layer('02_Torso_Vest',body);layer('03_Far_Arm_Sleeve',rear)
layer('04_Head_Hair',head);layer('05_Face_Glasses',face);layer('06_Teacup',cup)
layer('07_Near_Arm_Sleeve',arm);layer('08_Hands',hands);layer('09_Tea_Steam',steam)
s.frames[1].duration=0.1;local tag=s:newTag(1,1);tag.name='tea_sip'
s:saveAs(dest);s:close();src:close()
-- Reopen the saved native before export and verify all actual visibility states.
local function flat(v) local r=Image(v.spec);r:drawSprite(v,1);return r end
local function scaled(img,n)
 local out=Image(img.width*n,img.height*n,ColorMode.RGB)
 for y=0,out.height-1 do for x=0,out.width-1 do out:drawPixel(x,y,img:getPixel(x//n,y//n)) end end
 return out
end
local function background(img,h)
 local out=Image(img.width,img.height,ColorMode.RGB);out:clear(pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255));out:drawImage(img);return out
end
local qa=root..'/QA/SundeokTea';app.fs.makeAllDirectories(qa..'/States')
s=Sprite{fromFile=dest};assert(#s.frames==1 and #s.layers==9)
local full=flat(s);full:saveAs(root..'/Export/grandma_04_sundeok_tea.png')
scaled(background(full,'eee5d6'),8):saveAs(root..'/Previews/grandma_04_sundeok_tea_8x.png')
scaled(background(full,'252637'),8):saveAs(root..'/Previews/grandma_04_sundeok_tea_dark_8x.png')
local review=Image(128,64,ColorMode.RGB);review:drawImage(background(full,'eee5d6'));review:drawImage(background(full,'252637'),Point(64,0));scaled(review,6):saveAs(qa..'/light_dark_6x.png')
local palset={};for i=0,#palette-1 do local c=palette:getColor(i);palset[pc.rgba(c.red,c.green,c.blue,c.alpha)]=true end
local stats={canvas={64,64},frames=1,tag='tea_sip',layers={},opaque_pixels=0,partial_alpha_pixels=0,palette_outside_pixels=0,used_colors=0,bounds_xywh={},native_png_difference=0,visibility_states={}}
local colors={};local bx,by,ex,ey=64,64,-1,-1
local exported=Image{fromFile=root..'/Export/grandma_04_sundeok_tea.png'}
for y=0,63 do for x=0,63 do local p=full:getPixel(x,y);local a=pc.rgbaA(p)
 if p~=exported:getPixel(x,y) then stats.native_png_difference=stats.native_png_difference+1 end
 if a>0 then
  stats.opaque_pixels=stats.opaque_pixels+1;colors[p]=true;bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y)
  if a~=255 then stats.partial_alpha_pixels=stats.partial_alpha_pixels+1 end
  if not palset[p] then stats.palette_outside_pixels=stats.palette_outside_pixels+1 end
 end
end end
for _ in pairs(colors) do stats.used_colors=stats.used_colors+1 end
stats.bounds_xywh={bx,by,ex-bx+1,ey-by+1};stats.last_opaque_y=ey;stats.palette_slots=#palette
assert(stats.partial_alpha_pixels==0 and stats.palette_outside_pixels==0 and stats.native_png_difference==0 and ey==60)
for i,l in ipairs(s.layers) do stats.layers[i]={index=i,name=l.name,visible=l.isVisible,opacity=l.opacity} end
for _,mode in ipairs({'solo','hidden'}) do
 local sheet=Image(192,192,ColorMode.RGB);sheet:clear(pc.rgba(238,229,214,255))
 for i=1,#s.layers do
  for j,l in ipairs(s.layers) do l.isVisible=(mode=='solo' and i==j) or (mode=='hidden' and i~=j) end
  local path=qa..'/States/'..mode..'_'..i..'.aseprite';s:saveAs(path)
  local state=Sprite{fromFile=path}
  for j,l in ipairs(state.layers) do assert(l.isVisible==((mode=='solo' and i==j) or (mode=='hidden' and i~=j))) end
  local rendered=flat(state);rendered:saveAs(qa..'/States/'..mode..'_'..i..'.png')
  sheet:drawImage(background(rendered,'eee5d6'),Point(((i-1)%3)*64,((i-1)//3)*64))
  stats.visibility_states[#stats.visibility_states+1]={mode=mode,layer=i,path=path};state:close()
 end
 scaled(sheet,4):saveAs(qa..'/'..mode..'_4x.png')
end
for _,l in ipairs(s.layers) do l.isVisible=true end
local restored=flat(s)
for y=0,63 do for x=0,63 do assert(restored:getPixel(x,y)==full:getPixel(x,y)) end end
s:close() -- QA save-as never overwrites the final default-visible native.
local f=assert(io.open(qa..'/validation.json','w'));f:write(json.encode(stats));f:close()
print('Saved Sundeok tea native + PNG + previews; reopened 18 layer states; palette/alpha/native-PNG checks passed.')
