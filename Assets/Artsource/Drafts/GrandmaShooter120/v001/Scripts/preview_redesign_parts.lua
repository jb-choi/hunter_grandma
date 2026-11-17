local root=assert(app.params['root']);local pc=app.pixelColor
local function scale(img,n)local out=Image(img.width*n,img.height*n,ColorMode.RGB);for y=0,out.height-1 do for x=0,out.width-1 do out:drawPixel(x,y,img:getPixel(x//n,y//n))end end;return out end
local sheet=Image(480,120,ColorMode.RGB);sheet:clear(pc.rgba(238,229,214,255))
local paths={'Parts/Body/sundeok_body_base.png','Parts/Arms/sundeok_backarm_base.png','Weapons/Shotgun/sundeok_shotgun.png','Parts/Arms/sundeok_frontarm_base.png'}
for i,p in ipairs(paths)do local img=Image{fromFile=root..'/'..p};local x,y=(i-1)*120,0;if i==3 then x=x+21;y=48 end;sheet:drawImage(img,Point(x,y))end
scale(sheet,2):saveAs(root..'/Previews/independent_parts_2x.png')
local old=Image{fromFile=root..'/Source/Generated/sundeok_redesign_full.png'}
print('Independent part preview saved: body, back arm, shotgun, front arm.')
