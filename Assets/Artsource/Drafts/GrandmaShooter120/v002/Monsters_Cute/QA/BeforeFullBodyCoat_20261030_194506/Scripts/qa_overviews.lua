local root=assert(app.params['root'])
local ids={'z01_normal','z02_large','z03_human'}
local actions={'Idle','Walk','Run','Attack','Dead'}
for _,id in ipairs(ids)do
 local overview=Image(1440,1200,ColorMode.RGB)
 for row,a in ipairs(actions)do
  for col,mode in ipairs({'solo','hidden'})do
   local im=Image{fromFile=root..'/'..id..'/QA/'..a..'_'..mode..'_2x.png'}
   overview:drawImage(im,Point((col-1)*720,(row-1)*240))
  end
 end
 overview:saveAs(root..'/'..id..'/QA/material_states_overview_2x.png')
end
