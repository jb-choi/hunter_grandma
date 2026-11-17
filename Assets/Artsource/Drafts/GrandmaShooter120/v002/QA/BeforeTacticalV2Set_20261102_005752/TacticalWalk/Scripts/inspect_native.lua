local root=assert(app.params['root']);local s=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'}
local r={frames=#s.frames,tags={},layers={},frame19={}}
for _,tag in ipairs(s.tags)do r.tags[#r.tags+1]={name=tag.name,from=tag.fromFrame.frameNumber,to=tag.toFrame.frameNumber}end
for _,l in ipairs(s.layers)do r.layers[#r.layers+1]=l.name end
local meta=json.decode(assert(io.open(root..'/Source/pose_metadata.json')):read('*a'));r.frame19=meta.frames[19]
local f=assert(io.open(root..'/TacticalWalk/QA/native_before.json','w'));f:write(json.encode(r));f:close();print(json.encode(r));s:close()
