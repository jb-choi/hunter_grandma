-- Appended to the shared native-validation prefix by the build command.
local before={}
for i,c in ipairs(spec.candidates) do before[i]=Image{fromFile=root..'/Source/Before/'..c.name..'.png'} end
local captions={'05 REDHOOD','07 MOSSWITCH'}
local current=Image(160,100,ColorMode.RGB);current:clear(rgba('f7f0e4'))
text(current,8,6,'GRANDMA / AGE REVISION',rgba('281e32'))
for i=1,2 do
 local x=8+(i-1)*80
 fill(current,x-1,18,x+64,83,rgba('d9cfc4'));current:drawImage(bg(flats[i],'eee5d6'),Point(x,19));text(current,x,89,captions[i],rgba('281e32'))
end
scale(current,6,root..'/Previews/aged_candidates_05_07.png')
local comparison=Image(160,202,ColorMode.RGB);comparison:clear(rgba('f7f0e4'))
text(comparison,8,6,'BEFORE',rgba('7d6c86'));text(comparison,88,6,'AFTER / V003',rgba('281e32'))
for i=1,2 do
 local y=22+(i-1)*88
 comparison:drawImage(bg(before[i],'eee5d6'),Point(8,y));comparison:drawImage(bg(flats[i],'eee5d6'),Point(88,y))
 text(comparison,8,y+69,captions[i],rgba('281e32'));text(comparison,88,y+69,captions[i],rgba('281e32'))
end
text(comparison,8,191,'64 X 64 / NEAREST 6X',rgba('7d6c86'))
scale(comparison,6,root..'/Previews/age_before_after_64px_6x.png')
local native=Image(128,64,ColorMode.RGB);native:drawImage(flats[1]);native:drawImage(flats[2],Point(64,0));native:saveAs(root..'/Previews/aged_candidates_native.png')
-- Keep 04/06/08 from v002; only substitute revised 05 and 07 in the current set.
local board=Image(232,198,ColorMode.RGB);board:clear(rgba('f7f0e4'))
text(board,8,6,'GRANDMA / CURRENT BASES',rgba('281e32'))
local images={Image{fromFile=root..'/../v002/Export/grandma_04_sundeok.png'},flats[1],Image{fromFile=root..'/../v002/Export/grandma_06_teatime.png'},flats[2],Image{fromFile=root..'/../v002/Export/grandma_08_wildrose.png'}}
local titles={'04 SUNDEOK','05 REDHOOD','06 TEATIME','07 MOSSWITCH','08 WILDROSE'}
local slots={{8,19},{84,19},{160,19},{46,105},{122,105}}
for i,p in ipairs(slots) do
 fill(board,p[1]-1,p[2]-1,p[1]+64,p[2]+64,rgba('d9cfc4'));board:drawImage(bg(images[i],'eee5d6'),Point(p[1],p[2]));text(board,p[1]+1,p[2]+69,titles[i],rgba('281e32'))
end
text(board,8,187,'05 / 07 AGE REVISION V003',rgba('7d6c86'))
scale(board,5,root..'/Previews/current_candidates_04-08_64px.png')
local concepts=Image(560,650,ColorMode.RGB);concepts:clear(rgba('f7f0e4'))
text(concepts,16,12,'CONCEPT / BEFORE AND AFTER',rgba('281e32'),3)
for i,c in ipairs(spec.candidates) do
 for j,part in ipairs({'Before','Generated'}) do
  local path=root..'/Source/'..part..'/'..c.name..'_generated.png'
  local source=Image{fromFile=path};local thumb=Image(240,240,ColorMode.RGB)
  for y=0,239 do for x=0,239 do thumb:drawPixel(x,y,source:getPixel(math.min(source.width-1,math.floor((x+0.5)*source.width/240)),math.min(source.height-1,math.floor((y+0.5)*source.height/240)))) end end
  local x,y=20+(j-1)*280,48+(i-1)*300
  concepts:drawImage(thumb,Point(x,y));text(concepts,x,y+248,string.format('%02d',c.n)..((j==1) and ' BEFORE' or ' AFTER'),rgba('281e32'),3)
 end
end
concepts:saveAs(root..'/Previews/age_before_after_concepts.png')
write(root..'/QA/native_validation.json',{schema_version=1,date='2026-10-21',tool='Aseprite Lua independent reload of 2 revised natives and 16 saved visible states',candidates=reports})
print('PASS: 2 age revisions, 4 native/PNG comparisons, 16 reopened layer states, palette membership and binary alpha.')
