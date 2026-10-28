-- Reopen native files, independently render all visibility states, and validate.
local root=assert(app.params['root'])
local pc=app.pixelColor
local function rgba(h)
  return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)
end
local function write(path,obj)
  local f=assert(io.open(path,'w'));f:write(json.encode(obj));f:close()
end
local function fill(im,x1,y1,x2,y2,c)
  for y=y1,y2 do for x=x1,x2 do im:drawPixel(x,y,c) end end
end
local function flat(s)
  local im=Image(s.spec);im:drawSprite(s,1);return im
end
local function scale(im,n,path)
  local r=Image(im.width*n,im.height*n,ColorMode.RGB)
  for y=0,r.height-1 do for x=0,r.width-1 do r:drawPixel(x,y,im:getPixel(x//n,y//n)) end end
  r:saveAs(path);return r
end
local function onbg(im,c)
  local bg=Image(im.width,im.height,ColorMode.RGB);bg:clear(rgba(c));bg:drawImage(im);return bg
end
local font={
  ['0']={'111','101','101','101','111'},['1']={'010','110','010','010','111'},
  ['2']={'111','001','111','100','111'},['3']={'111','001','111','001','111'},
  ['4']={'101','101','111','001','001'},['6']={'111','100','111','101','111'},
  ['5']={'111','100','111','001','111'},['7']={'111','001','010','010','010'},
  ['A']={'010','101','111','101','101'},['B']={'110','101','110','101','110'},
  ['D']={'110','101','101','101','110'},['E']={'111','100','110','100','111'},
  ['G']={'111','100','101','101','111'},['H']={'101','101','111','101','101'},
  ['I']={'111','010','010','010','111'},['L']={'100','100','100','100','111'},
  ['M']={'10001','11011','10101','10101','10101'},['N']={'1001','1101','1011','1001','1001'},
  ['O']={'111','101','101','101','111'},['R']={'110','101','110','101','101'},
  ['S']={'111','100','111','001','111'},['T']={'111','010','010','010','010'},
  ['V']={'101','101','101','101','010'},['X']={'101','101','010','101','101'},
  ['/']={'001','001','010','100','100'},['-']={'000','000','111','000','000'}
}
local function text(im,x,y,str,c)
  for i=1,#str do
    local glyph=font[str:sub(i,i)]
    if glyph then
      for yy,row in ipairs(glyph) do for xx=1,#row do
        if row:sub(xx,xx)=='1' then im:drawPixel(x+xx-1,y+yy-1,c) end
      end end
      x=x+#glyph[1]+1
    else x=x+3 end
  end
end
local names={'grandma_01_rose','grandma_02_ranger','grandma_03_lavender'}
local titles={'01 ROSE','02 RANGER','03 LAVENDER'}
local flats={};local clean={};local reports={}
app.fs.makeAllDirectories(root..'/Previews')
app.fs.makeAllDirectories(root..'/QA/LayerReview/States')
app.fs.makeAllDirectories(root..'/QA/LayerReview/Solo')
app.fs.makeAllDirectories(root..'/QA/LayerReview/Hidden')
local function inspect(im)
  local cset={};local opaque,partial=0,0
  local x1,y1,x2,y2=64,64,-1,-1
  for y=0,63 do for x=0,63 do
    local p=im:getPixel(x,y);local a=pc.rgbaA(p)
    if a>0 then
      opaque=opaque+1;cset[p]=true
      x1=math.min(x1,x);y1=math.min(y1,y);x2=math.max(x2,x);y2=math.max(y2,y)
      if a<255 then partial=partial+1 end
    end
  end end
  local n=0;for _ in pairs(cset) do n=n+1 end
  return {opaque_pixels=opaque,partial_alpha_pixels=partial,used_opaque_colors=n,bounds={x1,y1,x2-x1+1,y2-y1+1}}
end
for ci,name in ipairs(names) do
  local native=root..'/'..name..'.aseprite'
  local s=Sprite{fromFile=native}
  assert(s.width==64 and s.height==64 and #s.frames==1 and #s.layers==7)
  local full=flat(s);flats[ci]=full
  local png=Image{fromFile=root..'/Export/'..name..'.png'}
  local diff=0
  for y=0,63 do for x=0,63 do if full:getPixel(x,y)~=png:getPixel(x,y) then diff=diff+1 end end end
  assert(diff==0,name..': PNG mismatch')
  local summary=inspect(full);assert(summary.partial_alpha_pixels==0)
  summary.name=name;summary.canvas={s.width,s.height};summary.frames=#s.frames
  summary.color_mode='RGB';summary.frame_duration_ms=s.frames[1].duration*1000
  summary.png_pixel_difference=diff;summary.tag=s.tags[1].name
  summary.layers={};summary.states={};summary.palette={}
  for i=0,#s.palettes[1]-1 do
    local c=s.palettes[1]:getColor(i)
    table.insert(summary.palette,{index=i,r=c.red,g=c.green,b=c.blue,a=c.alpha})
  end
  local palset={}
  for i=0,#s.palettes[1]-1 do
    local c=s.palettes[1]:getColor(i);palset[pc.rgba(c.red,c.green,c.blue,c.alpha)]=true
  end
  for y=0,63 do for x=0,63 do
    local p=full:getPixel(x,y);assert(pc.rgbaA(p)==0 or palset[p],name..': color absent from palette')
  end end
  for i,l in ipairs(s.layers) do
    assert(l.isVisible and l.opacity==255)
    table.insert(summary.layers,{index=i,name=l.name,isVisible=l.isVisible,opacity=l.opacity,blend_mode=tostring(l.blendMode)})
  end
  scale(onbg(full,'f1e8da'),8,root..'/Previews/'..name..'_8x.png')
  scale(onbg(full,'2c293b'),8,root..'/Previews/'..name..'_dark_8x.png')
  s.layers[7].isVisible=false;clean[ci]=flat(s)
  clean[ci]:saveAs(root..'/Export/'..name..'_no_fx.png')
  s.layers[7].isVisible=true
  for _,mode in ipairs({'Solo','Hidden'}) do
    local contact=Image(256,128,ColorMode.RGB);contact:clear(rgba('f5ede0'))
    for i,l in ipairs(s.layers) do
      for j,ll in ipairs(s.layers) do ll.isVisible=(mode=='Solo' and j==i) or (mode=='Hidden' and j~=i) end
      local statepath=root..'/QA/LayerReview/States/'..name..'_'..mode:lower()..'_'..i..'.aseprite'
      s:saveAs(statepath)
      local state=Sprite{fromFile=statepath}
      local actual={};for j,ll in ipairs(state.layers) do
        actual[j]=ll.isVisible
        assert(ll.isVisible==((mode=='Solo' and j==i) or (mode=='Hidden' and j~=i)))
      end
      local render=flat(state)
      local pngpath=root..'/QA/LayerReview/'..mode..'/'..name..'_'..i..'.png'
      render:saveAs(pngpath)
      local slotx,sloty=((i-1)%4)*64,((i-1)//4)*64
      local tile=onbg(render,(i%2==0) and 'e8dfd5' or 'f5ede0')
      contact:drawImage(tile,Point(slotx,sloty))
      text(contact,slotx+2,sloty+2,string.format('%02d',i),rgba('706089'))
      table.insert(summary.states,{mode=mode,layer_index=i,layer_name=l.name,isVisible=actual,state_file=statepath,png_file=pngpath,pixels=inspect(render)})
      state:close()
    end
    scale(contact,4,root..'/QA/LayerReview/'..name..'_'..mode:lower()..'_4x.png')
  end
  for _,l in ipairs(s.layers) do l.isVisible=true end
  local restored=flat(s)
  local restoreDiff=0
  for y=0,63 do for x=0,63 do if restored:getPixel(x,y)~=full:getPixel(x,y) then restoreDiff=restoreDiff+1 end end end
  assert(restoreDiff==0);summary.restored_composite_difference=restoreDiff
  -- The production native is never saved during QA; only state copies are saved.
  s:close();table.insert(reports,summary)
end
local board=Image(232,101,ColorMode.RGB);board:clear(rgba('f7f0e5'))
text(board,9,6,'GRANDMA / SHOOT BASE',rgba('352c42'))
text(board,179,6,'64 X 64',rgba('8b89a4'))
for i=1,3 do
  local x=8+(i-1)*76
  fill(board,x-1,17,x+65,82,rgba('e8dfd5'))
  fill(board,x,18,x+63,81,rgba('f1e8da'))
  board:drawImage(flats[i],Point(x,18))
  text(board,x+3,88,titles[i],rgba('352c42'))
end
scale(board,6,root..'/Previews/grandma_candidates_1-3.png')
local strip=Image(192,64,ColorMode.RGB)
for i=1,3 do strip:drawImage(flats[i],Point((i-1)*64,0)) end
strip:saveAs(root..'/Previews/grandma_candidates_native.png')
local strip2=Image(192,64,ColorMode.RGB)
for i=1,3 do strip2:drawImage(clean[i],Point((i-1)*64,0)) end
strip2:saveAs(root..'/Previews/grandma_candidates_no_fx_native.png')
write(root..'/QA/native_validation.json',{schema_version=1,tool='Aseprite native Lua; reopened native files and saved visibility-state copies',date='2026-10-21',candidates=reports})
print('Validated PNG/native equality, palette membership, alpha, and 42 reopened layer states.')
