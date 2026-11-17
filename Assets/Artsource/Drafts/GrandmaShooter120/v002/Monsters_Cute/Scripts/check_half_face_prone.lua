local root=assert(app.params['root']);local before=assert(app.params['before']);local pc=app.pixelColor
local cfg=json.decode(assert(io.open(root..'/Source/config.json')):read('*a'));local pal=cfg.characters[3].palette;local cyan,skin={},{}
local function rgba(h)return pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)end
for _,i in ipairs({3,4,5,6,7,8,9,25})do cyan[rgba(pal[i])]=true end
for _,i in ipairs({10,11,12,13})do skin[rgba(pal[i])]=true end
local function images(s,f)local full,body=Image(120,120,ColorMode.RGB),Image(120,120,ColorMode.RGB);full:drawSprite(s,f);local c=s.layers[1]:cel(f);if c then body:drawImage(c.image,c.position)end;return full,body end
local function measure(s,f)
 local full,body=images(s,f);local bx,by,ex,ey=120,120,-1,-1;local pixels=0
 for y=0,119 do for x=0,119 do if pc.rgbaA(body:getPixel(x,y))>0 then bx=math.min(bx,x);by=math.min(by,y);ex=math.max(ex,x);ey=math.max(ey,y);pixels=pixels+1 end end end
 local faceN,faceC,bodyC=0,0,0
 for y=by,ey do for x=bx,ex do local p=body:getPixel(x,y);if pc.rgbaA(p)>0 then
  if cyan[full:getPixel(x,y)]then bodyC=bodyC+1 end
  if y<by+(ey-by+1)*.38 and skin[p]then faceN=faceN+1;if cyan[full:getPixel(x,y)]then faceC=faceC+1 end end
 end end end
 return{body_bounds={bx,by,ex-bx+1,ey-by+1},body_pixels=pixels,cyan_on_body_ratio=bodyC/math.max(1,pixels),facial_skin_pixels=faceN,cyan_over_facial_skin_ratio=faceC/math.max(1,faceN)}
end
local now=Sprite{fromFile=root..'/zombie_03_rescue_human_120.aseprite'};local old=Sprite{fromFile=before..'/zombie_03_rescue_human_120.aseprite'}
local report={definition='Face proxy: cyan visible over restored warm skin in upper38% of standing HumanBody. Not a literal percentage of three-dimensional facial surface.',before_idle=measure(old,1),after_idle=measure(now,1),dead_frames={}}
assert(report.after_idle.cyan_over_facial_skin_ratio>report.before_idle.cyan_over_facial_skin_ratio+.15,'Facial coating did not increase')
assert(report.after_idle.cyan_over_facial_skin_ratio>.2 and report.after_idle.cyan_over_facial_skin_ratio<.85,'Expected partly exposed face')
for f=23,28 do report.dead_frames[#report.dead_frames+1]=measure(now,f)end
local first,last=report.dead_frames[1],report.dead_frames[6]
assert(last.body_bounds[3]>last.body_bounds[4]*1.5 and last.body_bounds[4]<first.body_bounds[4]*.65,'Final Dead is not prone')
assert(last.body_pixels>first.body_pixels*.5 and last.cyan_on_body_ratio>.3,'Whole human and most coating must remain')
report.passed=true;old:close();now:close();local f=assert(io.open(root..'/QA/half_face_prone_validation.json','w'));f:write(json.encode(report));f:close()
print('Half-face proxy '..report.before_idle.cyan_over_facial_skin_ratio..' -> '..report.after_idle.cyan_over_facial_skin_ratio..'; final prone cyan ratio '..last.cyan_on_body_ratio)
