local root=assert(app.params['root']);local pc=app.pixelColor
local s=Sprite{fromFile=root..'/zombie_03_rescue_human_120.aseprite'}
local meta=json.decode(assert(io.open(root..'/z03_human/pose_metadata.json')):read('*a'))
for f,r in ipairs(meta.frames)do
 local part=Image(120,120,ColorMode.RGB);local c=s.layers[1]:cel(f);if c then part:drawImage(c.image,c.position)end
 local reference=Image{fromFile=root..'/Source/Sampled/z03_human/'..r.action..string.format('/Restored_%02d.png',r.local_frame)}
 for y=0,119 do for x=0,119 do assert(part:getPixel(x,y)==reference:getPixel(x,y),'Restored human mismatch at frame '..f)end end
end
s:close()
local f=assert(io.open(root..'/QA/restored_human_validation.json','w'));f:write(json.encode({frames=28,restored_source_body_difference=0,passed=true}));f:close()
print('All28 HumanBody poses exactly match the restored human samples.')
