local root=assert(app.params['root']);local pc=app.pixelColor
local hex={'281e32','47243e','6d2949','ae615b','d98b77','f4bf9f','ffe1b4','fff5d7','57475f','7d6c86','aa9ba9','d7ccc8','744632','a7663a','d3934d','efb34a','ffdc78','4a5034','a1455e','d66b6e','ee403d','7b874f','30334d','424a61','7c8d9e','5f627a'}
local pal=Palette(27);pal:setColor(0,Color{r=0,g=0,b=0,a=0});local pix,labpal={},{}
local function linear(v)v=v/255;if v<=.04045 then return v/12.92 else return((v+.055)/1.055)^2.4 end end
local function lab(r,g,b)r,g,b=linear(r),linear(g),linear(b);local l=(.4122214708*r+.5363325363*g+.0514459929*b)^(1/3);local m=(.2119034982*r+.6806995451*g+.1073969566*b)^(1/3);local s=(.0883024619*r+.2817188376*g+.6299787005*b)^(1/3);return{.2104542553*l+.793617785*m-.0040720468*s,1.9779984951*l-2.428592205*m+.4505937099*s,.0259040371*l+.7827717662*m-.808675766*s}end
for i,h in ipairs(hex)do local r,g,b=tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16);pix[i]=pc.rgba(r,g,b,255);labpal[i]=lab(r,g,b);pal:setColor(i,Color{r=r,g=g,b=b,a=255})end
local cache={};local function mapped(p)if pc.rgbaA(p)<128 then return 0 end;if cache[p]then return cache[p]end;local c=lab(pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p));local best,idx=1e9,1;for i,v in ipairs(labpal)do local d=1.2*(c[1]-v[1])^2+(c[2]-v[2])^2+(c[3]-v[3])^2;if d<best then best,idx=d,i end end;cache[p]=pix[idx];return pix[idx]end
local specs={{name='Idle',count=4,cols=2,rows=2,ms=220},{name='Walk',count=6,cols=3,rows=2,ms=110},{name='Reload',count=6,cols=3,rows=2,ms=150},{name='Aim',count=3,cols=3,rows=1,ms=140},{name='Fire',count=4,cols=2,rows=2,ms=100},{name='Collect',count=4,cols=2,rows=2,ms=170}}
local function bounds(img,x0,y0,w,h)
 local bx,by,ex,ey=w,h,-1,-1;for y=0,h-1 do for x=0,w-1 do if pc.rgbaA(img:getPixel(x0+x,y0+y))>=128 then bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y)end end end
 assert(ex>=bx and ey>=by,'Empty generated frame');return{bx,by,ex-bx+1,ey-by+1}
end
local function footCenter(img,x0,y0,w,h,b)
 local bx,ex=w,-1;local ey=b[2]+b[4]-1
 for y=math.max(0,math.floor(ey-b[4]*.08)),ey do for x=0,w-1 do local p=img:getPixel(x0+x,y0+y);local r,g,bb=pc.rgbaR(p),pc.rgbaG(p),pc.rgbaB(p)
  if pc.rgbaA(p)>=128 and r>g*1.3 and bb>g*.75 then bx=math.min(bx,x);ex=math.max(ex,x)end
 end end
 if ex<bx then return b[1]+b[3]/2 end;return(bx+ex)/2
end
local reports={};local contact=Image(720,720,ColorMode.RGB);contact:clear(pc.rgba(238,229,214,255))
local function clean(img,bodyOnly)
 local visited,groups={},{}
 for y=0,119 do for x=0,119 do local key=y*120+x;if not visited[key]and pc.rgbaA(img:getPixel(x,y))>0 then
  local group,queue={},{{x,y}};visited[key]=true;local qi=1
  while qi<=#queue do local p=queue[qi];qi=qi+1;group[#group+1]=p;for dy=-1,1 do for dx=-1,1 do local xx,yy=p[1]+dx,p[2]+dy;if xx>=0 and xx<120 and yy>=0 and yy<120 then local k=yy*120+xx;if not visited[k]and pc.rgbaA(img:getPixel(xx,yy))>0 then visited[k]=true;queue[#queue+1]={xx,yy}end end end end end
  groups[#groups+1]=group
 end end end
 table.sort(groups,function(a,b)return#a>#b end)
 for i,g in ipairs(groups)do if(i>1 and bodyOnly)or#g<8 then for _,p in ipairs(g)do img:drawPixel(p[1],p[2],0)end end end;return img
end
local function oppositeStride(img)
 local out=Image(img);for y=82,119 do for x=0,119 do out:drawPixel(x,y,0)end end
 for y=82,94 do for x=0,119 do local xx=108-x;if xx>=0 and xx<120 then out:drawPixel(xx,y,img:getPixel(x,y))end end end
 for _,left in ipairs({true,false})do local sum,n=0,0;for x=0,119 do if((left and x<54)or(not left and x>=54))and pc.rgbaA(img:getPixel(x,94))>0 then sum=sum+x;n=n+1 end end
  local anchor=n>0 and sum/n or(left and 30 or 78);local dx=math.floor(108-2*anchor+.5)
  for y=95,119 do for x=0,119 do if(left and x<54)or(not left and x>=54)then local xx=x+dx;if xx>=0 and xx<120 then local p=img:getPixel(x,y);if pc.rgbaA(p)>0 then out:drawPixel(xx,y,p)end end end end end
 end;return out
end
local function scale(img,n)local out=Image(img.width*n,img.height*n,ColorMode.RGB);for y=0,out.height-1 do for x=0,out.width-1 do out:drawPixel(x,y,img:getPixel(x//n,y//n))end end;return out end
for ci,spec in ipairs(specs)do
 local full=Image{fromFile=root..'/Source/Generated/'..spec.name..'.png'}
 local body=Image{fromFile=root..'/Source/Generated/'..spec.name..(spec.name=='Walk'and''or'_Body')..'.png'}
 local fw,fh=math.floor(full.width/spec.cols),math.floor(full.height/spec.rows);local bw,bh=body.width/spec.cols,body.height/spec.rows
 local data={name=spec.name,count=spec.count,ms=spec.ms,source_size={full.width,full.height},body_source_size={body.width,body.height},grid={spec.cols,spec.rows},frames={}}
 local maxH,maxW=0,0
 for f=1,spec.count do local cx,cy=(f-1)%spec.cols,(f-1)//spec.cols;local bb=bounds(full,cx*fw,cy*fh,fw,fh);local bodyb=bounds(body,math.floor(cx*bw),math.floor(cy*bh),math.floor(bw),math.floor(bh));local anchor=footCenter(body,math.floor(cx*bw),math.floor(cy*bh),math.floor(bw),math.floor(bh),bodyb)*fw/bw
  local floor=(bodyb[2]+bodyb[4]-1)*fh/bh;maxH=math.max(maxH,bodyb[4]*fh/bh);maxW=math.max(maxW,bb[3]);data.frames[f]={cell={cx,cy},source_bounds=bb,body_bounds=bodyb,foot_x=anchor,floor_y=floor}
 end
 local sc=math.min(112/maxH,110/maxW);local minx,maxx=120,0
 for _,d in ipairs(data.frames)do minx=math.min(minx,54+(d.source_bounds[1]-d.foot_x)*sc);maxx=math.max(maxx,54+(d.source_bounds[1]+d.source_bounds[3]-d.foot_x)*sc)end
 local offset=0;if minx<2 then offset=2-minx end;if maxx+offset>118 then offset=offset+118-(maxx+offset)end
 data.scale=sc;data.anchor_target_x=54+offset
 app.fs.makeAllDirectories(root..'/Source/Sampled/'..spec.name)
 for f,d in ipairs(data.frames)do
  local cx,cy=table.unpack(d.cell)
  local out,bo=Image(120,120,ColorMode.RGB),Image(120,120,ColorMode.RGB)
  for y=0,119 do for x=0,119 do local sx=d.foot_x+(x-data.anchor_target_x+.5)/sc;local sy=d.floor_y+(y-117)/sc
   local ix,iy=math.floor(sx),math.floor(sy)
   if ix>=0 and ix<fw and iy>=0 and iy<fh then out:drawPixel(x,y,mapped(full:getPixel(cx*fw+ix,cy*fh+iy)))end
   local bx,by=math.floor(sx*bw/fw),math.floor(sy*bh/fh)
   if bx>=0 and bx<bw and by>=0 and by<bh then bo:drawPixel(x,y,mapped(body:getPixel(math.floor(cx*bw)+bx,math.floor(cy*bh)+by)))end
  end end
  out=clean(out,spec.name~='Collect');bo=clean(bo,true)
  -- Walk uses the corrected generated six-pose drawing, without leg mirroring.
  local pre=root..'/Source/Sampled/'..spec.name..'/'
  out:saveAs(pre..string.format('Full_%02d.png',f));bo:saveAs(pre..string.format('Body_%02d.png',f))
  local s=Sprite(120,120,ColorMode.RGB);s:setPalette(pal);s:newCel(s.layers[1],1,out);s:saveAs(pre..string.format('Full_%02d.aseprite',f));s:close()
  contact:drawImage(out,Point((f-1)*120,(ci-1)*120))
 end
 reports[#reports+1]=data
end
contact:saveAs(root..'/Previews/generated_poses_native.png');scale(contact,2):saveAs(root..'/Previews/generated_poses_2x.png')
local f=assert(io.open(root..'/Source/sampling.json','w'));f:write(json.encode({method='Generated sprite-sheet cells, registered by shoe/floor, nearest sampling and Oklab 26-color mapping',palette=hex,actions=reports}));f:close();print('Sampled 27 generated pose cells and 27 restored body cells to 120px.')
