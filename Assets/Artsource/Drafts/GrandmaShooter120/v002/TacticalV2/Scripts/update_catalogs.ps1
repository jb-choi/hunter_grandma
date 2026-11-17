param([string]$Root)
$ErrorActionPreference='Stop'
$r=$Root
$b="$r/QA/BeforeTacticalV2Set_20261102_005752"
function Read-Json([string]$Path){Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -AsHashtable}
function Write-Json([string]$Path,$Value){ConvertTo-Json -InputObject $Value -Depth 100 | Set-Content -LiteralPath $Path -Encoding utf8}
foreach($relative in @('Export/sundeok_parts.json','QA/part_inventory.json','QA/review.md')){
 $dest=Join-Path $b $relative
 if(-not(Test-Path -LiteralPath $dest)){New-Item -ItemType Directory -Path (Split-Path $dest) -Force | Out-Null;Copy-Item -LiteralPath "$r/$relative" -Destination $dest}
}
$prompts=Read-Json "$r/TacticalV2/Source/generation_prompts.json"
$originalPrompts=Read-Json "$b/Source/generation_prompts.json"
$promptAssets=@($prompts.assets | Select-Object -First 3)
$promptAssets[0].output='Source/walk_v2_tiptoe_body_initial.png'
$promptAssets[0].status='Superseded by toe correction and alpha extraction'
$promptAssets[1].status='Generated study only; final reload reuses selected narrow forefoot body with native 0-2px torso compression and original reload hands/shell'
$promptAssets[2].status='Generated study only; final fire reuses selected narrow forefoot body with native compact torso recoil and Aim assembly'
$corrections=Read-Json "$r/TacticalV2/Source/correction_prompts.json"
$prompts.assets=@($promptAssets)+@($corrections.assets)
$prompts.calls=5
$prompts.final_method='Selected ImageGen forefoot walk body atlas; Aseprite nearest sampling/palette registration. Reload/Fire: same body stance plus native upper-body compression/recoil; reuse authored reload hands/shell, Aim arms/shotgun and Fire2 flash. Reload/Fire body generation studies are unused.'
Write-Json "$r/TacticalV2/Source/generation_prompts.json" $prompts
foreach($asset in $prompts.assets){
 $copy=@{};foreach($key in $asset.Keys){$copy[$key]=$asset[$key]}
 $copy.name='TacticalV2/'+$asset.name
 $copy.output='TacticalV2/'+$asset.output
 if($asset.references){$copy.references=@($asset.references | ForEach-Object {'TacticalV2/'+$_})}
 $originalPrompts.assets+=@($copy)
}
$originalPrompts.latest_revision='2026-11-02 TacticalV2 forefoot walk/reload/fire'
$originalPrompts.calls=$originalPrompts.assets.Count
Write-Json "$r/Source/generation_prompts.json" $originalPrompts
$old=Read-Json "$b/Source/pose_metadata.json"
$set=Read-Json "$r/TacticalV2/pose_metadata.json"
$base=@($old.frames | Where-Object {$_.frame -le 27})
$aliases=@{Walk='walk_v1_normal';Reload='reload_v1_normal';Fire='fire_v1_normal'}
foreach($f in $base){if($aliases.ContainsKey($f.action)){$f.aseprite_tag=$aliases[$f.action]}else{$f.aseprite_tag=$f.action}}
foreach($f in $set.frames){
 $source=$base | Where-Object frame -eq $f.upper_source_frame | Select-Object -First 1
 $dx=$f.upper_offset_xy[0];$dy=$f.upper_offset_xy[1]
 foreach($key in @('hand_back','hand_front','shoulder_back','shoulder_front','support_grip')){
  if($source[$key]){$f[$key]=@(($source[$key][0]+$dx),($source[$key][1]+$dy))}
 }
 foreach($key in @('support_skin_distance','trigger_skin_distance')){if($source.ContainsKey($key)){$f[$key]=$source[$key]}}
 $f.registration='Native separated assembly translated without resampling; forefoot body registered by gray hair'
 if($f.action -ne 'walk_v2_tactical'){$f.body_source_master_frame=28;$f.source_scale=1;$f.body_derivation='Frame28 narrow forefoot body; 0-2px upper-body compression/recoil anchored at waist y82; legs y83-119 fixed';$f.generated_body_study_used=$false}
 else{$f.body_derivation='Selected ImageGen forefoot atlas; nearest sampling and shared26-color palette'}
}
$set.final_body_atlas='Source/walk_v2_tiptoe_body.png'
$set.unused_studies=@('Source/reload_v2_body.png','Source/fire_v2_body.png')
Write-Json "$r/TacticalV2/pose_metadata.json" $set
$old.frames=@($base)+@($set.frames)
$old.clips=@($old.clips | Where-Object {$_.name -ne 'walk_v2_tactical'})
foreach($clip in $old.clips){if($aliases.ContainsKey($clip.name)){$alias=$clip.name;$clip.name=$aliases[$alias];$clip.runtime_action_alias=$alias};$clip.durations_ms=@($base | Where-Object aseprite_tag -eq $clip.name | ForEach-Object duration_ms)}
foreach($clip in $set.clips){$old.clips+=@(@{name=$clip.tag;count=$clip.count;start=$clip.start;end=($clip.start+$clip.count-1);loop=($clip.kind -eq 'Walk');durations_ms=$clip.ms})}
$old.runtime_action_aliases=$aliases
$old.latest_revision='2026-11-02:43frames/9tags; Walk forefoot refinement + Reload/Fire v2'
$old.tactical_runtime_connected=$false
Write-Json "$r/Source/pose_metadata.json" $old
$parts=Read-Json "$b/Export/sundeok_parts.json"
$newParts=Read-Json "$r/TacticalV2/Export/parts.json"
foreach($part in $newParts.parts){
 $part.png='TacticalV2/'+$part.png
 $part.size_px=$part.size
 $f=$set.frames | Where-Object frame -eq $part.frame | Select-Object -First 1
 $pivot=switch($part.layer){'Body'{@(60,118)} 'FrontArm'{$f.shoulder_front} 'BackArm'{$f.shoulder_back} default{$f.weapon_pivot}}
 $part.pivot_canvas_xy=@($pivot)
 $part.pivot_crop_xy=@(($pivot[0]-$part.origin_xy[0]),($pivot[1]-$part.origin_xy[1]))
 $part.left_origin_xy=@((120-$part.origin_xy[0]-$part.size[0]),$part.origin_xy[1])
 $part.left_pivot_canvas_xy=if($part.layer -eq 'Body'){@(60,118)}else{@((119-$pivot[0]),$pivot[1])}
 $part.muzzle_canvas_xy=$f.muzzle
 $part.hand_front_canvas_xy=$f.hand_front
 $part.hand_back_canvas_xy=$f.hand_back
}
# Root metadata resolves every current part from the v002 directory.
$parts.parts=@($parts.parts)+@($newParts.parts)
$parts.latest_revision='2026-11-02 current43-frame master; 116 base slots +68 TacticalV2 slots; old TacticalWalk pack is historical'
$parts.total_nonempty_png_slots=$parts.parts.Count
Write-Json "$r/Export/sundeok_parts.json" $parts
# TacticalV2 metadata keeps its own paths relative to the TacticalV2 folder.
foreach($part in $newParts.parts){$part.png=$part.png.Substring('TacticalV2/'.Length)}
Write-Json "$r/TacticalV2/Export/parts.json" $newParts
$parts=Read-Json "$r/Export/sundeok_parts.json"
$inventory=@($parts.parts | Group-Object layer | ForEach-Object {
 $hashes=@($_.Group | ForEach-Object {(Get-FileHash -LiteralPath "$r/$($_.png)" -Algorithm SHA256).Hash})
 @{layer=$_.Name;nonempty_png_slots=$_.Count;unique_png_images=@($hashes | Sort-Object -Unique).Count}
})
Write-Json "$r/QA/part_inventory.json" $inventory
$m=Read-Json "$b/asset_manifest.json"
$m.latest_revision='2026-11-02 forefoot tactical v2 set'
$m.actions=@($m.actions | Where-Object {$_.name -ne 'walk_v2_tactical'})
foreach($action in $m.actions){if($aliases.ContainsKey($action.name)){$alias=$action.name;$action.name=$aliases[$alias];$action.native='Clips/sundeok_'+$action.name+'.aseprite';$action.runtime_action_alias=$alias}}
foreach($clip in $set.clips){$tag=$clip.tag;$m.actions+=@(@{name=$tag;frames=$clip.count;master_range=@($clip.start,($clip.start+$clip.count-1));durations_ms=$clip.ms;native="Clips/sundeok_$tag.aseprite";sheet="TacticalV2/Export/Sheets/$tag.png";gif="TacticalV2/Export/Gifs/$tag.gif";preview_gif="TacticalV2/Previews/$($tag)_3x.gif";loop=($clip.kind -eq 'Walk')})}
$m.total_action_frames=43
$m.cropped_part_png_slots=$parts.parts.Count
$m.part_library_frame_slots=301
$m.generation.calls=$originalPrompts.assets.Count
$m.generation.native_processing=$prompts.final_method
$m.validation.native_visibility_states=42
$m.validation.latest='TacticalV2/QA/validation.json'
$m.tactical_part_metadata='TacticalV2/Export/parts.json'
$m.tactical_validation='TacticalV2/QA/validation.json'
$m.tactical_generation_prompts='TacticalV2/Source/generation_prompts.json'
$m.runtime_action_aliases=$aliases
$m.runtime_connected_frame_count=27
$m.tactical_runtime_connected=$false
$m.latest_preview='TacticalV2/Previews/tactical_v2_set_3x.gif'
Write-Json "$r/asset_manifest.json" $m
Add-Type -AssemblyName System.Drawing
$gifs=@()
foreach($path in @((Get-ChildItem "$r/TacticalV2/Export/Gifs" -Filter '*.gif').FullName)+@((Get-ChildItem "$r/TacticalV2/Previews" -Filter '*.gif').FullName)){
 $im=[System.Drawing.Image]::FromFile($path)
 try{
  $count=$im.GetFrameCount([System.Drawing.Imaging.FrameDimension]::Time)
  $raw=$im.GetPropertyItem(0x5100).Value
  $ms=@(for($i=0;$i -lt $count;$i++){[BitConverter]::ToUInt32($raw,$i*4)*10})
  $expected=if($path -like '*tactical_v2_set*'){@(140)*6+@(150)*6+@(100,70,100,150)}elseif($path -like '*walk*'){@(140)*6}elseif($path -like '*reload*'){@(150)*6}else{@(100,70,100,150)}
  if($count -ne $expected.Count -or ($ms -join ',') -ne ($expected -join ',')){throw "GIF timing mismatch: $path"}
  $gifs+=@{path=$path.Substring($r.Length+1);frames=$count;durations_ms=$ms;canvas=@($im.Width,$im.Height);passed=$true}
 }finally{$im.Dispose()}
}
if($gifs.Count -ne 10){throw 'Expected10 GIF exports'}
Write-Json "$r/TacticalV2/QA/gif_validation.json" @{date='2026-11-02';gifs=$gifs;passed=$true}
$protected=@(Read-Json "$b/protected_hashes.json")
foreach($f in $protected){$f.after_sha256=(Get-FileHash -LiteralPath $f.path -Algorithm SHA256).Hash;$f.matches=($f.sha256 -eq $f.after_sha256);if(-not $f.matches){throw "Protected file changed: $($f.path)"}}
Write-Json "$r/TacticalV2/QA/protected_hashes_after.json" $protected
[pscustomobject]@{master_frames=43;tags=$m.actions.Count;pose_records=$old.frames.Count;part_slots=$parts.parts.Count;prompts=$originalPrompts.assets.Count;gif_exports=$gifs.Count;protected=$protected.Count;runtime_v2=$false} | ConvertTo-Json

