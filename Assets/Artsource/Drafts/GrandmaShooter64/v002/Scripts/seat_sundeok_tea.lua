-- Seated revision from the preserved standing tea native. Native Aseprite pixels.
local root=assert(app.params['root'])
local source=assert(app.params['source'],'Preserved tea native required')
local dest=root..'/grandma_04_sundeok_tea.aseprite'
local pc=app.pixelColor
local src=Sprite{fromFile=source}
assert(#src.layers==9 and src.width==64 and src.height==64)
local pal=Palette(#src.palettes[1]);for i=0,#pal-1 do pal:setColor(i,src.palettes[1]:getColor(i)) end
local C={o='281e32',d='47243e',k='6d2949',r='a1455e',p='d66b6e',a='ae615b',s='f4bf9f',m='d98b77',w='fff5d7',h='d7ccc8',j='aa9ba9',t='744632',b='a7663a',g='d3934d',y='efb34a',v='4a5034',n='7c8d9e',f='ee403d'}
local ink={};for k,h in pairs(C) do ink[k]=pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255) end
local function blank() return Image(64,64,ColorMode.RGB) end
local im
local function px(x,y,c) im:drawPixel(x,y,ink[c]) end
local function rect(x1,y1,x2,y2,c) for y=y1,y2 do for x=x1,x2 do px(x,y,c) end end end
local function line(x1,y1,x2,y2,c)
 local dx,dy=math.abs(x2-x1),math.abs(y2-y1);local sx,sy=x1<x2 and 1 or -1,y1<y2 and 1 or -1;local er=dx-dy
 while true do px(x1,y1,c);if x1==x2 and y1==y2 then break end;local e=er*2;if e>-dy then er=er-dy;x1=x1+sx end;if e<dx then er=er+dx;y1=y1+sy end end
end
local function poly(points,c)
 local lo,hi=63,0;for _,p in ipairs(points) do lo=math.min(lo,p[2]);hi=math.max(hi,p[2]) end
 for y=lo,hi do local xs={};for i,p in ipairs(points) do local q=points[i%#points+1]
  if(p[2]<=y and q[2]>y)or(q[2]<=y and p[2]>y)then xs[#xs+1]=p[1]+(y-p[2])*(q[1]-p[1])/(q[2]-p[2]) end
 end;table.sort(xs);for i=1,#xs-1,2 do for x=math.ceil(xs[i]),math.floor(xs[i+1]) do px(x,y,c) end end end
 for i,p in ipairs(points) do local q=points[i%#points+1];line(p[1],p[2],q[1],q[2],c) end
end
local chair=blank();im=chair
-- Complete wooden chair remains editable behind the sitter.
rect(5,26,8,59,'o');rect(6,28,7,58,'t');line(6,29,6,56,'g')
rect(6,25,17,29,'o');rect(7,26,16,27,'b');line(8,26,15,26,'g')
rect(14,28,17,46,'o');rect(15,29,16,45,'b')
rect(8,33,15,36,'o');rect(8,34,14,35,'t')
rect(6,45,33,49,'o');rect(7,45,31,46,'g');rect(7,47,32,48,'t')
poly({{9,49},{13,49},{12,60},{8,60}},'o');line(10,50,10,58,'b');px(9,59,'t')
poly({{27,49},{30,49},{31,60},{27,60}},'o');line(28,50,29,58,'b')
line(12,55,27,55,'o');line(13,56,26,56,'t')
local legs=blank();im=legs
-- Far thigh reaches forward; knee folds down instead of the shooting stance.
poly({{20,41},{29,41},{36,43},{39,46},{38,51},{35,55},{35,57},{42,57},{45,59},{44,60},{32,60},{30,57},{30,51},{23,49},{17,46}},'o')
poly({{21,42},{29,42},{35,44},{37,46},{36,50},{33,54},{33,57},{31,57},{31,49},{24,47},{19,45}},'d')
poly({{25,43},{32,44},{35,46},{34,49},{32,51},{32,48},{26,47}},'k')
line(34,48,35,46,'r');line(32,52,33,52,'k')
rect(32,56,34,57,'j');rect(33,56,34,56,'h')
poly({{32,58},{35,57},{39,58},{42,58},{44,59},{43,59},{33,59}},'d')
line(35,58,39,58,'r');px(40,59,'p')
-- Near thigh sits horizontally on the seat; the shin hangs below the knee.
poly({{14,40},{25,41},{32,43},{35,46},{34,49},{30,51},{29,56},{32,57},{36,57},{39,60},{24,60},{22,57},{23,50},{18,49},{13,47},{11,44}},'o')
poly({{15,41},{24,42},{31,44},{33,46},{32,48},{28,49},{27,56},{24,56},{25,48},{18,47},{13,45}},'k')
poly({{14,43},{21,44},{29,45},{31,46},{29,47},{22,46},{16,46}},'r')
line(15,42,21,43,'p');line(29,48,32,47,'d');line(25,50,25,54,'d')
line(27,50,27,53,'r');line(24,55,26,55,'d')
-- Authored flower clusters match the red floral trousers of the base sprite.
rect(19,44,20,45,'f');px(18,44,'p');px(21,45,'p');px(20,46,'v')
px(29,45,'f');px(30,44,'p');px(30,46,'v')
px(25,52,'f');px(26,51,'p');px(26,53,'v')
px(34,47,'f');px(35,46,'p');px(35,48,'v')
-- Socks and compact slippers at the shared y60 floor contact.
rect(24,56,27,57,'j');line(24,56,26,56,'h')
poly({{24,58},{27,57},{30,58},{34,58},{37,59},{37,59},{25,59},{24,59}},'d')
line(25,58,28,58,'r');line(29,58,33,58,'p');px(34,59,'r')
local s=Sprite(64,64,ColorMode.RGB);s:deleteLayer(s.layers[1]);s:setPalette(pal)
local function layer(name,img) local l=s:newLayer();l.name=name;l.blendMode=BlendMode.NORMAL;s:newCel(l,1,img,Point(0,0));return l end
layer('01_Chair',chair);layer('02_Seated_Legs_Shoes',legs)
local upperNames={'03_Torso_Vest','04_Far_Arm_Sleeve','05_Head_Hair','06_Face_Glasses','07_Teacup','08_Near_Arm_Sleeve','09_Hands','10_Tea_Steam'}
for i=2,9 do
 local cel=assert(src.layers[i]:cel(1));local img=blank();img:drawImage(cel.image,Point(cel.position.x,cel.position.y+2));layer(upperNames[i-1],img)
end
s.frames[1].duration=0.1;local tag=s:newTag(1,1);tag.name='tea_sip_seated'
s:saveAs(dest);s:close();src:close()
-- Saved-native QA, not in-memory-only export.
local function flat(v) local r=Image(v.spec);r:drawSprite(v,1);return r end
local function scaled(img,n)
 local out=Image(img.width*n,img.height*n,ColorMode.RGB)
 for y=0,out.height-1 do for x=0,out.width-1 do out:drawPixel(x,y,img:getPixel(x//n,y//n)) end end;return out
end
local function bg(img,h)
 local out=Image(img.width,img.height,ColorMode.RGB);out:clear(pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255));out:drawImage(img);return out
end
local qa=root..'/QA/SundeokTea/Seated';app.fs.makeAllDirectories(qa..'/States')
s=Sprite{fromFile=dest};assert(#s.frames==1 and #s.layers==10)
local full=flat(s);full:saveAs(root..'/Export/grandma_04_sundeok_tea.png')
scaled(bg(full,'eee5d6'),8):saveAs(root..'/Previews/grandma_04_sundeok_tea_8x.png')
scaled(bg(full,'252637'),8):saveAs(root..'/Previews/grandma_04_sundeok_tea_dark_8x.png')
local review=Image(128,64,ColorMode.RGB);review:drawImage(bg(full,'eee5d6'));review:drawImage(bg(full,'252637'),Point(64,0));scaled(review,6):saveAs(qa..'/light_dark_6x.png')
local palset={};for i=0,#pal-1 do local c=pal:getColor(i);palset[pc.rgba(c.red,c.green,c.blue,c.alpha)]=true end
local stats={canvas={64,64},frames=1,tag='tea_sip_seated',layers={},opaque_pixels=0,partial_alpha_pixels=0,palette_outside_pixels=0,used_colors=0,bounds_xywh={},native_png_difference=0,visibility_states={},upper_body_shift_xy={0,2}}
local colors={};local bx,by,ex,ey=64,64,-1,-1;local export=Image{fromFile=root..'/Export/grandma_04_sundeok_tea.png'}
for y=0,63 do for x=0,63 do local p=full:getPixel(x,y);local a=pc.rgbaA(p)
 if p~=export:getPixel(x,y) then stats.native_png_difference=stats.native_png_difference+1 end
 if a>0 then
  stats.opaque_pixels=stats.opaque_pixels+1;colors[p]=true;bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y)
  if a~=255 then stats.partial_alpha_pixels=stats.partial_alpha_pixels+1 end
  if not palset[p] then stats.palette_outside_pixels=stats.palette_outside_pixels+1 end
 end
end end
for _ in pairs(colors) do stats.used_colors=stats.used_colors+1 end
stats.bounds_xywh={bx,by,ex-bx+1,ey-by+1};stats.last_opaque_y=ey;stats.palette_slots=#pal
assert(stats.partial_alpha_pixels==0 and stats.palette_outside_pixels==0 and stats.native_png_difference==0 and ey==60)
for i,l in ipairs(s.layers) do assert(l.isVisible);stats.layers[i]={index=i,name=l.name,visible=l.isVisible,opacity=l.opacity} end
for _,mode in ipairs({'solo','hidden'}) do
 local sheet=Image(256,192,ColorMode.RGB);sheet:clear(pc.rgba(238,229,214,255))
 for i=1,#s.layers do
  for j,l in ipairs(s.layers) do l.isVisible=(mode=='solo' and i==j) or(mode=='hidden' and i~=j) end
  local path=qa..'/States/'..mode..'_'..i..'.aseprite';s:saveAs(path)
  local state=Sprite{fromFile=path}
  for j,l in ipairs(state.layers) do assert(l.isVisible==((mode=='solo' and i==j)or(mode=='hidden' and i~=j))) end
  local rendered=flat(state);rendered:saveAs(qa..'/States/'..mode..'_'..i..'.png')
  sheet:drawImage(bg(rendered,'eee5d6'),Point(((i-1)%4)*64,((i-1)//4)*64))
  stats.visibility_states[#stats.visibility_states+1]={mode=mode,layer=i,path=path};state:close()
 end
 scaled(sheet,4):saveAs(qa..'/'..mode..'_4x.png')
end
for _,l in ipairs(s.layers) do l.isVisible=true end;local restored=flat(s)
for y=0,63 do for x=0,63 do assert(restored:getPixel(x,y)==full:getPixel(x,y)) end end
s:close()
local f=assert(io.open(qa..'/validation.json','w'));f:write(json.encode(stats));f:close()
print('Seated tea native saved; PNG equality, palette, alpha and 20 saved layer states verified.')
