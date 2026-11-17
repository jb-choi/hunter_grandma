local root=assert(app.params['root']);local before=assert(app.params['before']);local pc=app.pixelColor
local cfg=json.decode(assert(io.open(root..'/Source/config.json')):read('*a'));local cyan={}
for _,i in ipairs({3,4,5,6,7,8,9,25})do local h=cfg.characters[3].palette[i];cyan[pc.rgba(tonumber(h:sub(1,2),16),tonumber(h:sub(3,4),16),tonumber(h:sub(5,6),16),255)]=true end
local function image(s,f,layer)local im=Image(120,120,ColorMode.RGB);if layer then local c=s.layers[layer]:cel(f);if c then im:drawImage(c.image,c.position)end else im:drawSprite(s,f)end;return im end
local function coverage(s,f)
 local shown,body=image(s,f),image(s,f,1);local top,bottom=120,-1
 for y=0,119 do for x=0,119 do if pc.rgbaA(body:getPixel(x,y))>0 then top=math.min(top,y);bottom=math.max(bottom,y)end end end
 local bins={whole={n=0,cyan=0},torso={n=0,cyan=0},legs={n=0,cyan=0}}
 for y=top,bottom do for x=0,119 do if pc.rgbaA(body:getPixel(x,y))>0 then
  local zone=(y-top)/(bottom-top+1);local hit=cyan[shown:getPixel(x,y)]and 1 or 0
  bins.whole.n=bins.whole.n+1;bins.whole.cyan=bins.whole.cyan+hit
  local q=zone>=.65 and bins.legs or zone>=.35 and bins.torso or nil
  if q then q.n=q.n+1;q.cyan=q.cyan+hit end
 end end end
 for _,b in pairs(bins)do b.ratio=b.cyan/math.max(1,b.n)end;return bins
end
local now=Sprite{fromFile=root..'/zombie_03_rescue_human_120.aseprite'};local old=Sprite{fromFile=before..'/zombie_03_rescue_human_120.aseprite'}
local result={definition='Fraction of restored HumanBody pixels covered by cyan palette colors in the visible composite; torso normalized bodyY .35-.65, legs .65-1',frames={},before_idle=coverage(old,1),after_idle=coverage(now,1)}
for f=1,22 do result.frames[#result.frames+1]={frame=f,coverage=coverage(now,f)}end
assert(result.after_idle.legs.ratio>result.before_idle.legs.ratio+.2,'Leg coverage did not increase')
assert(result.after_idle.torso.ratio>result.before_idle.torso.ratio+.1,'Torso coverage did not increase')
result.passed=true;old:close();now:close()
local f=assert(io.open(root..'/QA/full_body_coverage.json','w'));f:write(json.encode(result));f:close()
print('Full-body cyan coverage increased: torso '..result.before_idle.torso.ratio..' -> '..result.after_idle.torso.ratio..'; legs '..result.before_idle.legs.ratio..' -> '..result.after_idle.legs.ratio)
