param([Parameter(Mandatory=$true)][string]$Root)
$ErrorActionPreference='Stop'
Add-Type -AssemblyName System.Drawing
$Root=[IO.Path]::GetFullPath($Root)
function Write-Json([string]$Path,$Value){ $Value | ConvertTo-Json -Depth 30 | Set-Content -LiteralPath $Path -Encoding utf8 }
$cfg=Get-Content -LiteralPath "$Root/Source/config.json" -Raw | ConvertFrom-Json
$qa=Get-Content -LiteralPath "$Root/QA/validation.json" -Raw | ConvertFrom-Json
if($qa.total_frames -ne 84 -or $qa.total_clips -ne 15){throw 'Incomplete native validation'}
$gifChecks=@()
foreach($ch in $cfg.characters){
 $meta=Get-Content -LiteralPath "$Root/$($ch.id)/pose_metadata.json" -Raw | ConvertFrom-Json
 foreach($clip in $meta.clips){
  $expected=@($meta.frames | Where-Object action -eq $clip.name | ForEach-Object duration_ms)
  $low=$clip.name.ToLowerInvariant()
  foreach($kind in @('native','preview')){
   if($kind -eq 'native'){$path="$Root/$($ch.id)/Export/Gifs/$($ch.id)_$low.gif";$size=120}else{$path="$Root/$($ch.id)/Previews/$($clip.name)_3x.gif";$size=360}
   $im=[Drawing.Image]::FromFile($path)
   try{
    $frames=$im.GetFrameCount([Drawing.Imaging.FrameDimension]::Time)
    $value=$im.GetPropertyItem(0x5100).Value
    $actual=@(0..($frames-1)|ForEach-Object{[BitConverter]::ToInt32($value,4*$_)*10})
    if($im.Width -ne $size -or $im.Height -ne $size -or $frames -ne $clip.count){throw "GIF format mismatch: $path"}
    if(($actual -join ',') -ne ($expected -join ',')){throw "GIF timing mismatch: $path"}
    $gifChecks+=@{path=[IO.Path]::GetRelativePath($Root,$path).Replace('\','/');width=$im.Width;height=$im.Height;frames=$frames;durations_ms=$actual;timing_exact=$true}
   }finally{$im.Dispose()}
  }
 }
}
$path="$Root/Previews/three_characters_all_actions_3x.gif"
$im=[Drawing.Image]::FromFile($path)
try{
 $n=$im.GetFrameCount([Drawing.Imaging.FrameDimension]::Time);$v=$im.GetPropertyItem(0x5100).Value
 $ms=@(0..($n-1)|ForEach-Object{[BitConverter]::ToInt32($v,4*$_)*10})
 $normal=Get-Content -LiteralPath "$Root/z01_normal/pose_metadata.json" -Raw | ConvertFrom-Json
 if($n -ne 28 -or $im.Width -ne 1080 -or $im.Height -ne 360 -or ($ms -join ',') -ne (@($normal.frames|ForEach-Object duration_ms) -join ',')){throw 'Comparison GIF mismatch'}
 $gifChecks+=@{path='Previews/three_characters_all_actions_3x.gif';width=1080;height=360;frames=28;durations_ms=$ms;timing_exact=$true;note='Uses z01 timings for simultaneous comparison; each native actor clip retains its own timing.'}
}finally{$im.Dispose()}
Write-Json "$Root/QA/gif_validation.json" @{date='2026-10-30';files=$gifChecks;count=$gifChecks.Count;passed=$true}
$inventory=@()
foreach($ch in $cfg.characters){
 $parts=Get-Content -LiteralPath "$Root/$($ch.id)/Export/parts.json" -Raw | ConvertFrom-Json
 # Preserve superseded derived files under QA and deliver referenced parts.
 $expectedParts=[Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
 foreach($part in $parts.parts){[void]$expectedParts.Add([IO.Path]::GetFullPath("$Root/$($ch.id)/$($part.png)"))}
 foreach($action in $cfg.actions){
  foreach($file in Get-ChildItem -LiteralPath "$Root/$($ch.id)/Parts/$($action.name)" -Filter '*.png' -File){
   if(-not $expectedParts.Contains($file.FullName)){
    if(-not $file.FullName.StartsWith($Root+[IO.Path]::DirectorySeparatorChar,[StringComparison]::OrdinalIgnoreCase)){throw 'Unexpected derived part path'}
    $target="$Root/QA/StaleDerivedParts/$($ch.id)/$($action.name)"
    New-Item -ItemType Directory -Path $target -Force | Out-Null
    Move-Item -LiteralPath $file.FullName -Destination (Join-Path $target $file.Name) -Force
   }
  }
 }
 $groups=@($parts.parts | Group-Object layer)
 $layers=@(foreach($g in $groups){
  $hashes=@($g.Group | ForEach-Object{(Get-FileHash -LiteralPath "$Root/$($ch.id)/$($_.png)" -Algorithm SHA256).Hash})
  @{layer=$g.Name;nonempty_png_slots=$g.Count;unique_png_files=@($hashes | Sort-Object -Unique).Count;library_frame_slots=28}
 })
 $inventory+=@{character=$ch.id;nonempty_png_slots=$parts.parts.Count;native_library_slots=84;layers=$layers}
}
Write-Json "$Root/QA/part_inventory.json" @{date='2026-10-30';total_native_library_slots=252;total_nonempty_cropped_png_slots=$qa.total_cropped_parts;note='Slot counts are not counts of unique drawings. PNG byte hashes measure identical saved cropped images.';characters=$inventory}
$gen=Get-Content -LiteralPath "$Root/Source/generation_prompts.json" -Raw | ConvertFrom-Json
$manifest=@{date='2026-10-30';version='v002';art_direction='Cute chibi human forms; rounded faces and soft limbs; pastel slime material';canvas=@(120,120);material_form='slime material, human zombie anatomy';native_master_count=3;native_action_clips=15;animation_poses=84;actions=@('Idle','Walk','Run','Attack','Dead');root_pivot_right=@(52,118);root_pivot_left=@(68,118);max_opaque_rgb_per_character=26;alpha=@(0,255);native_libraries=9;native_library_frame_slots=252;cropped_part_png_slots=$qa.total_cropped_parts;gifs=31;notion_page='https://app.notion.com/p/4047313398e0818db924d95c5c455419';notion_section='2.3 슬라임 좀비·구조 대상 120px 캐릭터 디자인';unity_runtime_connected=$false;characters=@();source_generation=@{method='Built-in ImageGen cute lineup, action boards and restored human board; Aseprite cell extraction, native nearest sampling, palette limiting and material/body separation';calls=$gen.assets.Count;derived_action_sheets=20;atlas_grid=@(6,5);delivered_poses_per_actor=28;unused_idle_cells_per_actor=2;prompts='Source/generation_prompts.json'};qa='QA/validation.json';gif_qa='QA/gif_validation.json';previous_art_preserved='../Monsters'}
$manifest.human_coating_revision=$cfg.human_coating_revision
$manifest.human_dead_revision=$cfg.human_dead_revision
$manifest.human_face_coating=$cfg.human_face_coating
foreach($ch in $cfg.characters){
 $stats=$qa.characters | Where-Object id -eq $ch.id
 $manifest.characters+=@{id=$ch.id;name=$ch.name;role=$ch.role;master=$ch.native;frames=28;clips=5;rgb_union=$stats.rgb_union;ending=$stats.ending;folder=$ch.id;pose_metadata="$($ch.id)/pose_metadata.json";parts_metadata="$($ch.id)/Export/parts.json";clips_folder="$($ch.id)/Clips";sheets_folder="$($ch.id)/Export/Sheets";previews_folder="$($ch.id)/Previews";body_without_slime=$(if($ch.role -eq 'rescue'){"$($ch.id)/human_without_slime_120.aseprite"}else{$null})}
}
Write-Json "$Root/asset_manifest.json" $manifest
$before=Get-Content -LiteralPath "$Root/QA/preserved_before.json" -Raw | ConvertFrom-Json
$preservation=@()
for($i=0;$i -lt $before.Count;$i++){
 $entry=$before[$i];$hash=(Get-FileHash -LiteralPath $entry.path -Algorithm SHA256).Hash
 if($hash -ne $entry.sha256){throw "Protected previous art changed: $($entry.path)"};$status='unchanged'
 $preservation+=@{path=$entry.path;before_sha256=$entry.sha256;after_sha256=$hash;status=$status}
}
Write-Json "$Root/QA/source_preservation.json" @{date='2026-10-30';files=$preservation;passed=$true}
foreach($pair in @(@('v002_readme_before.md',"$Root/../README.md"),@('root_readme_before.md',"$Root/../../README.md"))){
 $old=[IO.File]::ReadAllText("$Root/QA/$($pair[0])");$new=[IO.File]::ReadAllText($pair[1])
 if(-not $new.StartsWith($old.TrimEnd())){throw "Parent README earlier content changed: $($pair[1])"}
}
$hashes=@(Get-ChildItem -LiteralPath $Root -Recurse -File | Where-Object{$_.FullName -notmatch '[\\/]QA[\\/]' -and $_.Extension -ne '.meta'} | ForEach-Object{@{path=[IO.Path]::GetRelativePath($Root,$_.FullName).Replace('\','/');sha256=(Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash}})
Write-Json "$Root/QA/delivery_hashes.json" @{date='2026-10-30';files=$hashes;count=$hashes.Count}
Write-Output "GIFs: $($gifChecks.Count) dimensions / counts / exact timing PASS. Native: 84 poses / 15 clips. Parts: $($qa.total_cropped_parts) nonempty slots. Preserved previous monster/hero masters: 5."
