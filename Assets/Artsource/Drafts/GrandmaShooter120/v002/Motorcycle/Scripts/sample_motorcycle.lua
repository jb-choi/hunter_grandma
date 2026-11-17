local root=assert(app.params['root']);local pc=app.pixelColor
local hex={'191e2b','352c3c','a82932','e0493b','ad674e','dc9671','ffd0a0','ffedc3','66667c','a2a3b4','d9d5d3','123b48','215967','397b85','273a56','485675','414333','72704f','aaa06c','603d2c','995c33','c88b4f','683447','a14b5b','6f8192','b5c6cb'}
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
local pal=Palette(27);pal:setColor(0,Color{r=0,g=0,b=0,a=0})
local function lin(v)v=v/255;return v<=.04045 and v/12.92 or((v+.055)/1.055)^2.4 end
local function lab(r,g,b)r,g,b=lin(r),lin(g),lin(b);local l=(.4122214708*r+.5363325363*g+.0514459929*b)^(1/3);local m=(.2119034982*r+.6806995451*g+.1073969566*b)^(1/3);local s=(.0883024619*r+.2817188376*g+.6299787005*b)^(1/3);return{.2104542553*l+.793617785*m-.0040720468*s,1.9779984951*l-2.428592205*m+.4505937099*s,.0259040371*l+.7827717662*m-.808675766*s}end
local pixels,labs={},{};for i,h in ipairs(hex)do local p=rgba(h);pixels[i]=p;labs[i]=lab(pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p));pal:setColor(i,Color{r=pc.rgbaR(p),g=pc.rgbaG(p),b=pc.rgbaB(p),a=255})end
local cache={};local function map(p)if pc.rgbaA(p)<128 then return 0 end;if cache[p]then return cache[p]end;local c=lab(pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p));local best,idx=1e9,1;for i,v in ipairs(labs)do local d=1.2*(v[1]-c[1])^2+(v[2]-c[2])^2+(v[3]-c[3])^2;if d<best then best,idx=d,i end end;cache[p]=pixels[idx];return pixels[idx]end
local function crop(img,x,y,w,h)local out=Image(w,h,ColorMode.RGB);out:drawImage(img,Point(-x,-y));return out end
local function groups(img,pred)
 local visited,out={},{}
 for y=0,img.height-1 do for x=0,img.width-1 do local k=y*img.width+x
 if not visited[k]and pred(x,y,img:getPixel(x,y))then
  local q,idx={{x,y}},1;local g={pts={},bx=x,by=y,ex=x,ey=y};visited[k]=true
  while idx<=#q do local p=q[idx];idx=idx+1;g.pts[#g.pts+1]=p;g.bx=math.min(g.bx,p[1]);g.by=math.min(g.by,p[2]);g.ex=math.max(g.ex,p[1]);g.ey=math.max(g.ey,p[2])
   for dy=-1,1 do for dx=-1,1 do local xx,yy=p[1]+dx,p[2]+dy;if xx>=0 and xx<img.width and yy>=0 and yy<img.height then local kk=yy*img.width+xx;if not visited[kk]and pred(xx,yy,img:getPixel(xx,yy))then visited[kk]=true;q[#q+1]={xx,yy}end end end end
  end
  g.count=#g.pts;g.width=g.ex-g.bx+1;g.cx=(g.bx+g.ex)/2;out[#out+1]=g
 end end end
 table.sort(out,function(a,b)return a.count>b.count end);return out
end
local function features(img)
 local hg=groups(img,function(x,y,p)local r,g,b=pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p);return pc.rgbaA(p)>=128 and y<img.height*.72 and r>45 and r<200 and g>35 and g>=r*.60 and g<=r*1.12 and g>b*1.10 end)
 local bg=groups(img,function(x,y,p)local r,g,b=pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p);return pc.rgbaA(p)>=128 and y>img.height*.48 and x>img.width*.23 and x<img.width*.77 and r>65 and r<205 and g<150 and r>g*1.4 and g>b*1.05 end)
 assert(hg[1]and bg[1],'Missing helmet or boot')
 return{helmet={cx=hg[1].cx,by=hg[1].by,width=hg[1].width,bx=hg[1].bx,ey=hg[1].ey},boot={cx=bg[1].cx,ey=bg[1].ey,width=bg[1].width}}
end
local function clean(im)
 local gs=groups(im,function(x,y,p)return pc.rgbaA(p)>0 end)
 for i,g in ipairs(gs)do if i>1 and g.count<10 then for _,p in ipairs(g.pts)do im:drawPixel(p[1],p[2],0)end end end;return im
end
local function sampled(img,sx,sy,ax,ay,tx,ty)
 local out=Image(120,120,ColorMode.RGB)
 for y=0,119 do for x=0,119 do local xx,yy=math.floor(ax+(x-tx)/sx+.5),math.floor(ay+(y-ty)/sy+.5);if xx>=0 and yy>=0 and xx<img.width and yy<img.height then out:drawPixel(x,y,map(img:getPixel(xx,yy)))end end end
 return clean(out)
end
local function scale(im,n)local out=Image(im.width*n,im.height*n,ColorMode.RGB);for y=0,out.height-1 do for x=0,out.width-1 do out:drawPixel(x,y,im:getPixel(x//n,y//n))end end;return out end
local specs={{name='Idle',count=4,cols=2,rows=2,ms=180},{name='Ride',count=6,cols=3,rows=2,ms=80},{name='Collect',count=8,cols=4,rows=2,ms=85}}
local records={};local contact=Image(960,360,ColorMode.RGB);contact:clear(rgba('ffedc3'))
for ci,spec in ipairs(specs)do
 local all=Image{fromFile=root..'/Source/Generated/'..spec.name..'_Rider.png'};local bodyAll=Image{fromFile=root..'/Source/Generated/'..spec.name..'_Body.png'}
 local cw,ch=math.floor(all.width/spec.cols),math.floor(all.height/spec.rows);local bw,bh=math.floor(bodyAll.width/spec.cols),math.floor(bodyAll.height/spec.rows)
 local cells,fs={},{};local widths={}
 for f=1,spec.count do local cx,cy=(f-1)%spec.cols,(f-1)//spec.cols;cells[f]=crop(all,cx*cw,cy*ch,cw,ch);fs[f]=features(cells[f]);widths[#widths+1]=fs[f].helmet.width end
 table.sort(widths);local s=24/widths[math.ceil(#widths/2)];app.fs.makeAllDirectories(root..'/Source/Sampled/'..spec.name)
 for f=1,spec.count do
  local cx,cy=(f-1)%spec.cols,(f-1)//spec.cols;local ft=fs[f];local full=sampled(cells[f],s,s,ft.boot.cx,ft.boot.ey,57,101)
  local bc=crop(bodyAll,cx*bw,cy*bh,bw,bh);local bf=features(bc);local sx=ft.helmet.width*s/bf.helmet.width
  local sy=(ft.boot.ey-ft.helmet.by)*s/(bf.boot.ey-bf.helmet.by)
  local hx=57+(ft.helmet.cx-ft.boot.cx)*s;local hy=101+(ft.helmet.by-ft.boot.ey)*s
  local body=sampled(bc,sx,sy,bf.helmet.cx,bf.helmet.by,hx,hy)
  local pre=root..'/Source/Sampled/'..spec.name..'/';full:saveAs(pre..string.format('Rider_%02d.png',f));body:saveAs(pre..string.format('Body_%02d.png',f))
  local native=Sprite(120,120,ColorMode.RGB);native:setPalette(pal);native:newCel(native.layers[1],1,full);native:saveAs(pre..string.format('Rider_%02d.aseprite',f));native:close()
  contact:drawImage(full,Point((f-1)*120,(ci-1)*120))
  records[#records+1]={action=spec.name,local_frame=f,scale=s,helmet_native={hx,hy,ft.helmet.width*s,(ft.helmet.ey-ft.helmet.by+1)*s},helmet_source=ft.helmet,boot_source=ft.boot,body_scale={sx,sy},boot_anchor={57,101}}
 end
end
local vs=Image{fromFile=root..'/Source/Generated/Vehicle.png'};local v=crop(vs,0,0,vs.width//2,vs.height//2)
local vx=groups(v,function(x,y,p)return pc.rgbaA(p)>=128 end)[1];local sc=108/(vx.ex-vx.bx+1)
local vehicle=sampled(v,sc,sc,vx.bx,369,6,103);vehicle:saveAs(root..'/Vehicle/motorcycle_raw.png')
contact:saveAs(root..'/Previews/rider_samples_native.png');scale(contact,2):saveAs(root..'/Previews/rider_samples_2x.png')
local f=assert(io.open(root..'/Source/sampling.json','w'));f:write(json.encode({palette=hex,frames=records,vehicle={scale=sc,raw_wheel_centers={{272,369},{732,369}},native_wheel_centers={{6+(272-vx.bx)*sc,103},{6+(732-vx.bx)*sc,103}},bounds={vx.bx,vx.by,vx.width,vx.ey-vx.by+1}},method='Native nearest sampling, Oklab 26-color mapping; rider helmet/boot registration; restored body matched by helmet and boot landmarks'}));f:close()
local g=assert(io.open(root..'/Export/motorcycle_26.gpl','w'));g:write('GIMP Palette\nName: Motorcycle Grandma 26\nColumns: 13\n#\n');for _,h in ipairs(hex)do g:write(string.format('%3d %3d %3d  #%s\n',tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),h))end;g:close()
print('Sampled 18 rider and 18 restored-body frames; independent motorcycle, 26-color palette.')
