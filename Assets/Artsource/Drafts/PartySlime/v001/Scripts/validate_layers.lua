local base="Assets/Artsource/Drafts/PartySlime/v001/"
for i=1,10 do
  local id=string.format("Z%02d",i)
  local sprite=Sprite{fromFile=base.."Masters/"..id..".aseprite"}
  assert(#sprite.layers==1 and sprite.layers[1].isVisible)
  local shown=Image(120,120,ColorMode.RGB); shown:drawSprite(sprite,1)
  shown:saveAs(base.."QA/"..id.."_shown.png")
  sprite.layers[1].isVisible=false
  local path=base.."QA/"..id.."_hidden.aseprite"
  sprite:saveAs(path);sprite:close()
  local hidden=Sprite{fromFile=path}; assert(not hidden.layers[1].isVisible)
  local image=Image(120,120,ColorMode.RGB);image:drawSprite(hidden,1)
  for pixel in image:pixels() do assert(app.pixelColor.rgbaA(pixel())==0) end
  hidden:close()
  local original=Sprite{fromFile=base.."Masters/"..id..".aseprite"}
  assert(original.layers[1].isVisible);original:close()
  print(id..": default/solo composition, saved hidden state, empty hidden render, original visibility preserved PASS")
end
