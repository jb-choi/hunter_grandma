-- Curated material ramps; map sampled pixels in Oklab. Never dithers or blurs.
local root=assert(app.params['root']);local pc=app.pixelColor
local specs=json.decode(assert(io.open(root..'/Source/generation_prompts.json','r')):read('*a'))
local hex={
 '281e32','47243e','6d2949','a1455e','d66b6e','ee403d',
 '4a5034','7b874f','a9b16e','153d49','2e6c77','5b9d9c',
 'ae624f','df9166','ffc68b','ffe8b6','fff5d7','d7ccc8',
 'aa9ba9','7d6c86','57475f','424a61','7c8d9e','744632',
 'a7663a','d3934d','efb34a','ffdc78','653979','985d9d','30334d','5f627a'
}
local function lin(v) v=v/255;if v<=0.04045 then return v/12.92 else return ((v+0.055)/1.055)^2.4 end end
local function lab(r,g,b)
 r,g,b=lin(r),lin(g),lin(b)
 local l=(0.4122214708*r+0.5363325363*g+0.0514459929*b)^(1/3)
 local m=(0.2119034982*r+0.6806995451*g+0.1073969566*b)^(1/3)
 local s=(0.0883024619*r+0.2817188376*g+0.6299787005*b)^(1/3)
 return {0.2104542553*l+0.7936177850*m-0.0040720468*s,1.9779984951*l-2.4285922050*m+0.4505937099*s,0.0259040371*l+0.7827717662*m-0.8086757660*s}
end
local pals,pix={},{}
for i,h in ipairs(hex) do
 local r,g,b=tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16)
 pix[i]=pc.rgba(r,g,b,255);pals[i]=lab(r,g,b)
end
local cache={}
local function mapped(p)
 if pc.rgbaA(p)==0 then return 0 end
 if cache[p] then return cache[p] end
 local c=lab(pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p));local best,bi=1000,1
 for i,v in ipairs(pals) do
   local d=(c[1]-v[1])^2*1.2+(c[2]-v[2])^2+(c[3]-v[3])^2
   if d<best then best=d;bi=i end
 end
 cache[p]=pix[bi];return pix[bi]
end
for _,spec in ipairs(specs.candidates) do
 local src=Image{fromFile=root..'/Source/'..spec.name..'_sampled.png'}
 local im=Image(64,64,ColorMode.RGB)
 for y=0,63 do for x=0,63 do im:drawPixel(x,y,mapped(src:getPixel(x,y))) end end
 local s=Sprite(64,64,ColorMode.RGB);s:newCel(s.layers[1],1,im)
 s.layers[1].name='Character_Combined_Base'
 local p=Palette(#hex+1);p:setColor(0,Color{r=0,g=0,b=0,a=0})
 for i,h in ipairs(hex) do p:setColor(i,Color{r=tonumber(h:sub(1,2),16),g=tonumber(h:sub(3,4),16),b=tonumber(h:sub(5,6),16),a=255}) end
 s:setPalette(p);s.frames[1].duration=0.1
 s:saveAs(root..'/Source/'..spec.name..'_curated.aseprite')
 s:close()
end
local f=assert(io.open(root..'/Source/curated_palette.json','w'));f:write(json.encode(hex));f:close()
print('Saved 5 curated 32-color RGB sprites.')
