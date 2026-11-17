-- Separate card portrait assets derived from the restored current body drawing.
local root=assert(app.params['root']);local pc=app.pixelColor
app.fs.makeAllDirectories(root..'/UI')
local body=Image{fromFile=root..'/Source/Sampled/Idle/Body_01.png'}
local portrait=Image(120,120,ColorMode.RGB);portrait:drawImage(body,Point(15,12))
for y=98,119 do for x=0,119 do portrait:drawPixel(x,y,0)end end
local silhouette=Image(120,120,ColorMode.RGB)
for y=0,119 do for x=0,119 do if pc.rgbaA(portrait:getPixel(x,y))>0 then silhouette:drawPixel(x,y,pc.rgba(40,30,50,255))end end end
local base=Sprite{fromFile=root..'/grandma_01_sundeok_120.aseprite'};local pal=base.palettes[1]
for _,item in ipairs({{'sundeok_portrait',portrait},{'sundeok_portrait_locked',silhouette}})do
 local s=Sprite(120,120,ColorMode.RGB);s:setPalette(pal);s.layers[1].name='Portrait';s:newCel(s.layers[1],1,item[2]);s:saveAs(root..'/UI/'..item[1]..'.aseprite');item[2]:saveAs(root..'/UI/'..item[1]..'.png');s:close()
end
portrait:saveAs(root..'/Export/sundeok_portrait.png');base:close()
print('Saved separate 120px UI portrait and locked portrait assets, without weapons.')
