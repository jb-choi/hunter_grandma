local root=assert(app.params['root']);local pc=app.pixelColor
local m=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'}
local meta=json.decode(assert(io.open(root..'/Source/pose_metadata.json')):read('*a'))
local function flat(s,f)local im=Image(s.spec);im:drawSprite(s,f);return im end
local function layer(s,l,f)local im=Image(s.spec);local c=s.layers[l]:cel(f);if c then im:drawImage(c.image,c.position)end;return im end
local function same(a,b)assert(a.width==b.width and a.height==b.height);for y=0,a.height-1 do for x=0,a.width-1 do assert(a:getPixel(x,y)==b:getPixel(x,y),'Pixel difference')end end end
local report={action_clip_frames=0,part_library_frames=0,ui_native_png_difference=0,clip_master_difference=0,library_master_difference=0}
for _,c in ipairs(meta.clips)do
 local s=Sprite{fromFile=root..'/Clips/sundeok_'..c.name:lower()..'.aseprite'};assert(#s.frames==c.count and #s.layers==7)
 local n=0;for f,r in ipairs(meta.frames)do if r.action==c.name then n=n+1;same(flat(s,n),flat(m,f));assert(math.abs(s.frames[n].duration-m.frames[f].duration)<.001);report.action_clip_frames=report.action_clip_frames+1 end end;s:close()
end
for i,l in ipairs(m.layers)do
 local s=Sprite{fromFile=root..'/Parts/Native/sundeok_'..l.name:lower()..'.aseprite'};assert(#s.frames==27 and #s.layers==1 and #s.tags==6)
 for f=1,27 do same(flat(s,f),layer(m,i,f));report.part_library_frames=report.part_library_frames+1 end;s:close()
end
for _,name in ipairs({'sundeok_portrait','sundeok_portrait_locked'})do
 local s=Sprite{fromFile=root..'/UI/'..name..'.aseprite'};local im=Image{fromFile=root..'/UI/'..name..'.png'};assert(s.width==120 and s.height==120 and #s.frames==1);same(flat(s,1),im)
 for y=0,119 do for x=0,119 do local a=pc.rgbaA(im:getPixel(x,y));assert(a==0 or a==255)end end;s:close()
end
m:close();local f=assert(io.open(root..'/QA/package_validation.json','w'));f:write(json.encode(report));f:close()
print('Verified 27 clip frames, 189 independent part-library frames, and 2 UI native/PNG pairs exactly.')
