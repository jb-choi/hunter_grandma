-- Run with Aseprite -b --script this-file from the Unity project root.
-- Preserve the supplied sheets; create 120px nearest-neighbour animation masters.
local base = "Assets/Artsource/Drafts/PartySlime/v001/"
local runtime = "Assets/HunterGrandma/Art/PartySlime/"
local names = {
  "Z01_balloon_party_zombie_sheet", "Z02_horn_party_zombie_sheet",
  "Z03_confetti_zombie_sheet", "Z04_rhythm_step_zombie_sheet",
  "Z05_disco_slime_zombie_sheet", "Z06_dj_zombie_sheet",
  "Z07_mirrorball_brute_sheet", "Z08_parade_zombie_sheet",
  "Z09_party_king_slime_sheet", "Z10_grand_festival_titan_sheet"
}
app.fs.makeAllDirectories(base .. "Masters")
app.fs.makeAllDirectories(base .. "QA")
for index, name in ipairs(names) do
  local id = string.format("Z%02d", index)
  local dest = base .. "Masters/" .. id .. ".aseprite"
  if not app.fs.isFile(dest) then
  app.fs.makeAllDirectories(runtime .. id)
  local source = Image { fromFile=base .. "Originals/" .. name .. ".png" }
  local sprite = Sprite(120, 120, ColorMode.RGB)
  sprite.layers[1].name = "Imported artwork (flattened source)"
  for frame=0,15 do
    if frame>0 then sprite:newEmptyFrame() end
    local col, row = frame%4, math.floor(frame/4)
    local x0, x1 = math.floor(col*1254/4), math.floor((col+1)*1254/4)
    local y0, y1 = math.floor(row*1254/4), math.floor((row+1)*1254/4)
    local image = Image(120,120,ColorMode.RGB)
    for y=0,119 do for x=0,119 do
      local sx=x0+math.min(x1-x0-1, math.floor((x+.5)*(x1-x0)/120))
      local sy=y0+math.min(y1-y0-1, math.floor((y+.5)*(y1-y0)/120))
      local color=source:getPixel(sx,sy)
      -- Remove low-alpha extraction noise and make pixel silhouettes explicit.
      if app.pixelColor.rgbaA(color)>=128 then
        image:drawPixel(x,y,app.pixelColor.rgba(app.pixelColor.rgbaR(color),app.pixelColor.rgbaG(color),app.pixelColor.rgbaB(color),255))
      end
    end end
    sprite:newCel(sprite.layers[1],frame+1,image,Point(0,0))
    sprite.frames[frame+1].duration = row==0 and .20 or row==1 and .13 or row==2 and .15 or ({.09,.12,.16,.35})[col+1]
    image:saveAs(runtime .. id .. string.format("/frame_%02d.png",frame+1))
  end
  for row,nameTag in ipairs({"Idle","Walk","Attack","Dead"}) do
    local tag=sprite:newTag((row-1)*4+1,row*4); tag.name=nameTag
  end
  sprite:saveAs(dest)
  sprite:close()
  end -- Existing masters are validated without being rewritten.
  local reopened=app.open(dest)
  assert(#reopened.frames==16 and #reopened.layers==1 and #reopened.tags==4)
  for frame=1,16 do
    local png=Image {fromFile=runtime .. id .. string.format("/frame_%02d.png",frame)}
    local cel=reopened.layers[1]:cel(frame)
    for y=0,119 do for x=0,119 do assert(png:getPixel(x,y)==cel.image:getPixel(x-cel.position.x,y-cel.position.y),id .. " pixel mismatch") end end
  end
  reopened:close()
  print(id .. ": 16 frames, saved/reopened PNG pixel identity PASS")
end
