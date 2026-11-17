-- Find whole connected characters before grouping rows; nominal gutters can
-- run through long feet or arms. Retain exact source coordinates for underlayers.
local root=assert(app.params['root']);local pc=app.pixelColor;local only=app.params['only']
local ids={'z01_normal','z02_large','z03_human'}
if only then ids={only}end
local actions={'Idle','Walk','Run','Attack','Dead'}
local function groups(im)
 local w,h=im.width,im.height;local seen,out={},{}
 for y=0,h-1 do for x=0,w-1 do local k=y*w+x
  if not seen[k]and pc.rgbaA(im:getPixel(x,y))>=128 then
   local q,i={k},1;local g={pts={},bx=x,by=y,ex=x,ey=y};seen[k]=true
   while i<=#q do local v=q[i];i=i+1;local xx,yy=v%w,v//w;g.pts[#g.pts+1]=v;g.bx=math.min(g.bx,xx);g.by=math.min(g.by,yy);g.ex=math.max(g.ex,xx);g.ey=math.max(g.ey,yy)
    for dy=-1,1 do for dx=-1,1 do local nx,ny=xx+dx,yy+dy;if nx>=0 and nx<w and ny>=0 and ny<h then local nk=ny*w+nx;if not seen[nk]and pc.rgbaA(im:getPixel(nx,ny))>=128 then seen[nk]=true;q[#q+1]=nk end end end end
   end;g.n=#g.pts;out[#out+1]=g
  end
 end end
 table.sort(out,function(a,b)return a.n>b.n end);return out
end
local function partition(im,label)
 local gs=groups(im);assert(#gs>=30,'Missing connected characters in '..label)
 local largest={};for i=1,30 do assert(gs[i].n>150,'Tiny main in '..label);largest[i]=gs[i]end
 table.sort(largest,function(a,b)return(a.by+a.ey)<(b.by+b.ey)end)
 local mains={}
 for row=1,5 do local line={};for col=1,6 do line[col]=largest[(row-1)*6+col]end;table.sort(line,function(a,b)return(a.bx+a.ex)<(b.bx+b.ex)end);for col=1,6 do mains[(row-1)*6+col]=line[col]end end
 local own,cells={},{}
 for i,m in ipairs(mains)do own[m]=i;cells[i]={pts={},bx=m.bx,by=m.by,ex=m.ex,ey=m.ey,main={m.bx,m.by,m.ex-m.bx+1,m.ey-m.by+1},main_pixels=m.n}end
 local labels={}
 for _,g in ipairs(gs)do
  local ci=own[g]
  if not ci then
   local best=1e30;local cx,cy=(g.bx+g.ex)/2,(g.by+g.ey)/2
   for i,m in ipairs(mains)do local dx=math.max(m.bx-cx,0,cx-m.ex);local dy=math.max(m.by-cy,0,cy-m.ey);local d=dx*dx+dy*dy+.01*((cx-(m.bx+m.ex)/2)^2+(cy-(m.by+m.ey)/2)^2);if d<best then best,ci=d,i end end
  end
  local c=cells[ci];for _,v in ipairs(g.pts)do c.pts[#c.pts+1]=v;labels[v]=ci end
  c.bx=math.min(c.bx,g.bx);c.by=math.min(c.by,g.by);c.ex=math.max(c.ex,g.ex);c.ey=math.max(c.ey,g.ey)
 end
 return cells,labels
end
local metadata={format='30 largest whole connected sprites sorted into5 rows x6 columns; other components assigned to nearest body; first4 Idle selected',characters={}}
for _,id in ipairs(ids)do
 app.fs.makeAllDirectories(root..'/Source/Generated/'..id)
 local board=Image{fromFile=root..'/Source/Generated/Boards/'..id..'.png'}
 local body=id=='z03_human'and Image{fromFile=root..'/Source/Generated/Boards/'..id..'_Body.png'}or nil
 local cells=partition(board,id);local bodyCells,bodyLabels;local rx,ry=1,1
 if body then bodyCells,bodyLabels=partition(body,id..' restored');rx,ry=body.width/board.width,body.height/board.height end
 local records={}
 for row,a in ipairs(actions)do
  local count=a=='Idle'and 4 or 6;local cols=a=='Idle'and 2 or 3;local cw,ch=0,0
  for col=1,count do
   local i=(row-1)*6+col;local c=cells[i];local bx,by,ex,ey=c.bx,c.by,c.ex,c.ey
   if body then local b=bodyCells[i];bx=math.min(bx,math.floor(b.bx/rx));by=math.min(by,math.floor(b.by/ry));ex=math.max(ex,math.ceil(b.ex/rx));ey=math.max(ey,math.ceil(b.ey/ry))end
   c.x=math.max(0,bx-4);c.y=math.max(0,by-4);c.w=math.min(board.width-1,ex+4)-c.x+1;c.h=math.min(board.height-1,ey+4)-c.y+1;cw=math.max(cw,c.w);ch=math.max(ch,c.h)
  end
  local rects={}
  for _,kind in ipairs(body and{'full','body'}or{'full'})do
   local out=Image(cw*cols,ch*2,ColorMode.RGB)
   for col=1,count do
    local i=(row-1)*6+col;local c=cells[i];local tx=((col-1)%cols)*cw+(cw-c.w)//2;local ty=((col-1)//cols)*ch+(ch-c.h)
    if kind=='full'then
     for _,v in ipairs(c.pts)do local xx,yy=v%board.width,v//board.width;out:drawPixel(tx+xx-c.x,ty+yy-c.y,board:getPixel(xx,yy))end
     rects[col]={x=c.x,y=c.y,w=c.w,h=c.h,main_bounds=c.main,main_pixels=c.main_pixels,assigned_pixels=#c.pts}
    else
     for y=0,c.h-1 do for x=0,c.w-1 do local sx=math.min(body.width-1,math.floor((c.x+x)*rx));local sy=math.min(body.height-1,math.floor((c.y+y)*ry));if bodyLabels[sy*body.width+sx]==i then out:drawPixel(tx+x,ty+y,body:getPixel(sx,sy))end end end
    end
   end
   out:saveAs(root..'/Source/Generated/'..id..'/'..a..(kind=='body'and'_Body'or'')..'.png')
  end
  records[#records+1]={action=a,selected_cells=count,source_row=row-1,repacked_cell={cw,ch},source_cells=rects}
 end
 metadata.characters[#metadata.characters+1]={id=id,actions=records,source_board={board.width,board.height}}
 print('Extracted whole sprites for '..id..':5 action source sheets.')
end
if only then
 local previous=json.decode(assert(io.open(root..'/Source/board_splitting.json')):read('*a'))
 local replacement=metadata.characters[1]
 for i,character in ipairs(previous.characters)do if character.id==only then previous.characters[i]=replacement end end
 metadata=previous
end
local f=assert(io.open(root..'/Source/board_splitting.json','w'));f:write(json.encode(metadata));f:close()
