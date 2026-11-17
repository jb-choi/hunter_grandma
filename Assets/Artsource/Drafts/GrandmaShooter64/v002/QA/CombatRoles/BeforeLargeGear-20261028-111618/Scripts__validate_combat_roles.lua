-- Independently reopen saved role natives and every solo/hidden state.
local root=assert(app.params['root']);local pc=app.pixelColor
local qa=root..'/QA/CombatRoles';local specs=json.decode(assert(io.open(qa..'/role_specs.json','r')):read('*a'))
local function flat(s) local r=Image(s.spec);r:drawSprite(s,1);return r end
local function scale(img,n)
 local out=Image(img.width*n,img.height*n,ColorMode.RGB)
 for y=0,out.height-1 do for x=0,out.width-1 do out:drawPixel(x,y,img:getPixel(x//n,y//n)) end end;return out
end
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local function bg(img,h) local r=Image(img.width,img.height,ColorMode.RGB);r:clear(rgba(h));r:drawImage(img);return r end
local function diff(a,b)
 assert(a.width==b.width and a.height==b.height);local n=0
 for y=0,a.height-1 do for x=0,a.width-1 do if a:getPixel(x,y)~=b:getPixel(x,y)then n=n+1 end end end;return n
end
local function inspect(im)
 local r={opaque_pixels=0,partial_alpha_pixels=0,used_colors=0,bounds_xywh={},last_opaque_y=-1};local colors={};local bx,by,ex,ey=im.width,im.height,-1,-1
 for y=0,im.height-1 do for x=0,im.width-1 do local p=im:getPixel(x,y);local a=pc.rgbaA(p)
  if a>0 then r.opaque_pixels=r.opaque_pixels+1;colors[p]=true;bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y);if a~=255 then r.partial_alpha_pixels=r.partial_alpha_pixels+1 end end
 end end
 for _ in pairs(colors)do r.used_colors=r.used_colors+1 end
 if ex>=0 then r.bounds_xywh={bx,by,ex-bx+1,ey-by+1};r.last_opaque_y=ey end
 return r
end
local function write(p,t)local f=assert(io.open(p,'w'));f:write(json.encode(t));f:close()end
local reports={};local lineup=Image(320,64,ColorMode.RGB);lineup:clear(rgba('eee5d6'))
local source=Sprite{fromFile=root..'/grandma_04_sundeok.aseprite'};lineup:drawImage(flat(source));source:close()
local roles=Image(256,64,ColorMode.RGB);roles:clear(rgba('eee5d6'))
local sils=Image(256,64,ColorMode.RGB);sils:clear(rgba('eee5d6'))
local beforeAfter=Image(256,128,ColorMode.RGB);beforeAfter:clear(rgba('eee5d6'))
for index,spec in ipairs(specs)do
 local stateDir=qa..'/'..spec.name..'/States';app.fs.makeAllDirectories(stateDir)
 local s=Sprite{fromFile=root..'/'..spec.name..'.aseprite'}
 assert(s.width==64 and s.height==64 and #s.frames==1 and #s.layers==8)
 local full=flat(s);local r=inspect(full)
 r.name=spec.name;r.role=spec.role;r.canvas={64,64};r.frames=1;r.duration_ms=s.frames[1].duration*1000;r.tag=s.tags[1].name;r.palette_slots=#s.palettes[1];r.palette_outside_pixels=0;r.layers={};r.states={}
 local allowed={}
 for i=0,#s.palettes[1]-1 do local c=s.palettes[1]:getColor(i);allowed[pc.rgba(c.red,c.green,c.blue,c.alpha)]=true end
 for y=0,63 do for x=0,63 do local p=full:getPixel(x,y);if pc.rgbaA(p)>0 and not allowed[p]then r.palette_outside_pixels=r.palette_outside_pixels+1 end end end
 assert(r.partial_alpha_pixels==0 and r.palette_outside_pixels==0 and r.last_opaque_y==60 and r.palette_slots==33)
 for i,l in ipairs(s.layers)do assert(l.isVisible and l.opacity==255);local cel=assert(l:cel(1));local pixels=inspect(cel.image);assert(pixels.opaque_pixels>0);r.layers[i]={index=i,name=l.name,visible=l.isVisible,opacity=l.opacity,pixels=pixels.opaque_pixels}end
 full:saveAs(root..'/Export/'..spec.name..'.png')
 r.native_png_difference=diff(full,Image{fromFile=root..'/Export/'..spec.name..'.png'});assert(r.native_png_difference==0)
 s.layers[8].isVisible=false;local clean=flat(s);clean:saveAs(root..'/Export/'..spec.name..'_no_fx.png')
 r.no_fx_native_png_difference=diff(clean,Image{fromFile=root..'/Export/'..spec.name..'_no_fx.png'});assert(r.no_fx_native_png_difference==0);s.layers[8].isVisible=true
 local sil=Image(64,64,ColorMode.RGB)
 for y=0,63 do for x=0,63 do if pc.rgbaA(clean:getPixel(x,y))>0 then sil:drawPixel(x,y,pc.rgba(0,0,0,255))end end end
 sil:saveAs(root..'/Export/'..spec.name..'_silhouette.png')
 scale(bg(full,'eee5d6'),8):saveAs(root..'/Previews/'..spec.name..'_8x.png')
 scale(bg(full,'252637'),8):saveAs(root..'/Previews/'..spec.name..'_dark_8x.png')
 local pair=Image(128,64,ColorMode.RGB);pair:drawImage(bg(full,'eee5d6'));pair:drawImage(bg(full,'252637'),Point(64,0));scale(pair,6):saveAs(qa..'/'..spec.name..'/light_dark_6x.png')
 lineup:drawImage(full,Point(index*64,0));roles:drawImage(full,Point((index-1)*64,0));sils:drawImage(sil,Point((index-1)*64,0))
 local old=Sprite{fromFile=root..'/'..spec.old..'.aseprite'};beforeAfter:drawImage(flat(old),Point((index-1)*64,0));old:close();beforeAfter:drawImage(full,Point((index-1)*64,64))
 for _,mode in ipairs({'solo','hidden'})do
  local sheet=Image(256,128,ColorMode.RGB);sheet:clear(rgba('eee5d6'))
  for i=1,#s.layers do
   for j,l in ipairs(s.layers)do l.isVisible=(mode=='solo'and i==j)or(mode=='hidden'and i~=j)end
   local statePath=stateDir..'/'..mode..'_'..i..'.aseprite';s:saveAs(statePath)
   local state=Sprite{fromFile=statePath}
   for j,l in ipairs(state.layers)do assert(l.isVisible==((mode=='solo'and i==j)or(mode=='hidden'and i~=j)))end
   local rendered=flat(state);rendered:saveAs(stateDir..'/'..mode..'_'..i..'.png')
   sheet:drawImage(bg(rendered,'eee5d6'),Point(((i-1)%4)*64,((i-1)//4)*64))
   r.states[#r.states+1]={mode=mode,layer=i,visible_state_verified=true,path=statePath,pixels=inspect(rendered)};state:close()
  end
  scale(sheet,3):saveAs(qa..'/'..spec.name..'/'..mode..'_3x.png')
 end
 for _,l in ipairs(s.layers)do l.isVisible=true end;assert(diff(flat(s),full)==0);s:close()
 -- A second open confirms the delivered native itself still defaults to all visible.
 local final=Sprite{fromFile=root..'/'..spec.name..'.aseprite'};for _,l in ipairs(final.layers)do assert(l.isVisible)end;assert(diff(flat(final),full)==0);final:close()
 r.default_visibility_restored=true;reports[#reports+1]=r
 local metadata={name=spec.name,candidate=spec.n,role=spec.role,title=spec.title,source=spec.old..'.aseprite',native=spec.name..'.aseprite',png='Export/'..spec.name..'.png',png_no_fx='Export/'..spec.name..'_no_fx.png',silhouette='Export/'..spec.name..'_silhouette.png',canvas={64,64},bounds_xywh=r.bounds_xywh,last_opaque_y=60,ground_boundary_y=61,frames=1,duration_ms=100,tag=r.tag,palette_slots=r.palette_slots,used_colors=r.used_colors,layers=r.layers,muzzle_xy=spec.muzzle,hand_grip_xy=spec.grips,wheel_centers_xy=spec.wheel_centers,output_policy='All eight layers visible; optional no-FX hides layer 8; silhouette derives from no-FX; no trim',runtime_connected=false,source_document='https://app.notion.com/p/4067313398e08130965aebafbb62a76d',transport_override='User requested motorcycle in place of the document cart',method='Existing native pixel reuse plus directly authored Aseprite gear, arms and motorcycle'}
 write(root..'/Export/'..spec.name..'.json',metadata)
end
lineup:saveAs(root..'/Previews/combat_lineup_04-08_native.png');scale(lineup,4):saveAs(root..'/Previews/combat_lineup_04-08_4x.png')
roles:saveAs(root..'/Previews/combat_roles_05-08_native.png');scale(roles,4):saveAs(root..'/Previews/combat_roles_05-08_4x.png')
scale(sils,4):saveAs(root..'/Previews/combat_roles_05-08_silhouettes_4x.png')
scale(beforeAfter,4):saveAs(root..'/Previews/combat_roles_before_after_4x.png')
write(qa..'/validation.json',{date='2026-10-28',method='Native Aseprite save/reopen, exact pixel comparisons and actual visibility state reload',candidates=reports,checked_visibility_states=64})
print('Verified four 64px natives, eight PNG comparisons and 64 saved/reopened visibility states.')
