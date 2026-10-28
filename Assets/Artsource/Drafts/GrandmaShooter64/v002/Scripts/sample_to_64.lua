-- Generation concept -> fixed 64x64 nearest sample. All editing is in Aseprite.
local root=assert(app.params['root'])
local pc=app.pixelColor
local specs=json.decode(assert(io.open(root..'/Source/generation_prompts.json','r')):read('*a'))
local report={}
for _,spec in ipairs(specs.candidates) do
  local src=Image{fromFile=root..'/Source/Generated/'..spec.name..'_generated.png'}
  local bx,by,ex,ey=src.width,src.height,-1,-1
  for y=0,src.height-1 do for x=0,src.width-1 do
    if pc.rgbaA(src:getPixel(x,y))>=128 then bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y) end
  end end
  local w,h=ex-bx+1,ey-by+1
  local scale=math.min(58/w,56/h)
  local ow,oh=math.floor(w*scale+0.5),math.floor(h*scale+0.5)
  local ox,oy=math.floor((64-ow)/2),60-oh
  local im=Image(64,64,ColorMode.RGB)
  for y=0,oh-1 do for x=0,ow-1 do
    local sx=bx+math.min(w-1,math.floor((x+0.5)*w/ow))
    local sy=by+math.min(h-1,math.floor((y+0.5)*h/oh))
    local p=src:getPixel(sx,sy)
    if pc.rgbaA(p)>=128 then im:drawPixel(ox+x,oy+y,pc.rgba(pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p),255)) end
  end end
  local s=Sprite(64,64,ColorMode.RGB);s.layers[1].name='Generated_Base_Sampled'
  s:newCel(s.layers[1],1,im)
  s.frames[1].duration=0.1
  s:saveAs(root..'/Source/'..spec.name..'_sampled.aseprite')
  im:saveAs(root..'/Source/'..spec.name..'_sampled.png')
  table.insert(report,{name=spec.name,source_px={src.width,src.height},source_bounds={bx,by,w,h},target_bounds={ox,oy,ow,oh},sampling='nearest pixel centers',alpha_threshold=128})
  s:close()
end
local f=assert(io.open(root..'/QA/sampling.json','w'));f:write(json.encode(report));f:close()
print('Saved five 64x64 nearest-sampled source sprites.')
