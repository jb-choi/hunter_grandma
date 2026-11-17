#!/usr/bin/env node
import { createHash } from 'node:crypto';
import { existsSync, readFileSync, readdirSync, mkdirSync, writeFileSync } from 'node:fs';
import { dirname, resolve, join, relative, sep } from 'node:path';
import { fileURLToPath, pathToFileURL } from 'node:url';

const root = resolve(dirname(fileURLToPath(import.meta.url)), '../../../..');
const toolRoot = join(root, 'Tools/PixelFX');
const vendor = join(toolRoot, 'vendor/pixelfx');
const sha = data => createHash('sha256').update(data).digest('hex');
const json = file => JSON.parse(readFileSync(file, 'utf8').replace(/^\uFEFF/, ''));
const fail = message => { throw new Error(message); };

function inside(base, path) {
  const result = resolve(base, path);
  const rel = relative(base, result);
  if (rel === '..' || rel.startsWith('..' + sep) || resolve(result) === resolve(base) || rel.includes(':')) fail('Path escapes its expected directory: ' + path);
  return result;
}

export function verifyVendor() {
  const lock = json(join(toolRoot, 'pixelfx.lock.json'));
  if (!/^[a-f0-9]{40}$/.test(lock.commit) || !Array.isArray(lock.files) || !lock.files.length) fail('Invalid pixelfx source lock');
  for (const file of lock.files) {
    const path = inside(vendor, file.path);
    if (!existsSync(path) || sha(readFileSync(path)) !== file.sha256) fail('Pinned source mismatch: ' + file.path);
  }
  return lock;
}

export async function exportRecipe(recipePath, destination) {
  const lock = verifyVendor();
  const recipe = json(resolve(recipePath));
  const source = recipe.source ?? 'pixelfx';
  if (!['pixelfx', 'custom'].includes(source)) fail('source must be pixelfx or custom');
  if (!/^[a-z0-9][a-z0-9-]*\/[a-z0-9][a-z0-9-]*$/.test(recipe.effectId ?? '')) fail('effectId must be category/name');
  const name = recipe.name ?? recipe.effectId.split('/')[1];
  if (!/^[a-z0-9][a-z0-9-]*$/.test(name)) fail('name must contain lowercase letters, digits and hyphens');
  const seed = recipe.seed ?? 42;
  const ppu = recipe.pixelsPerUnit ?? 32;
  if (!Number.isSafeInteger(seed) || !Number.isFinite(ppu) || ppu <= 0) fail('Invalid seed or pixelsPerUnit');
  if (recipe.fps !== undefined && (!Number.isFinite(recipe.fps) || recipe.fps < 1 || recipe.fps > 120)) fail('fps must be 1..120');
  const options = recipe.options ?? {};
  if (!options || Array.isArray(options) || typeof options !== 'object') fail('options must be an object');
  if ('seed' in options || 'actor' in options) fail('Use top-level seed; stock actor overrides are not supported by this exporter');
  const out = resolve(destination);
  const vendorRel = relative(vendor, out);
  if (vendorRel === '' || (!vendorRel.startsWith('..' + sep) && vendorRel !== '..' && !vendorRel.includes(':'))) fail('Do not export into the pinned vendor directory');
  if (existsSync(out) && readdirSync(out).length) fail('Output folder is not empty; choose a new version: ' + out);

  const lib = await import(pathToFileURL(join(vendor, 'tools/lib/fxlib.mjs')).href);
  const { encodePNG } = await import(pathToFileURL(join(vendor, 'tools/lib/png.mjs')).href);
  const fxDir = source === 'custom' ? join(toolRoot, 'custom/fx') : lib.FX_DIR;
  const fx = await lib.loadEffect(recipe.effectId, fxDir);
  if (fx.mod.act?.onBody) fail('This effect paints the stock dummy; adapt it to the real actor before exporting for the game');
  const renderOptions = { fps: recipe.fps, seed, actor: false, options };
  const dark = lib.renderFrames(fx, { ...renderOptions, bg: 'dark' });
  const light = lib.renderFrames(fx, { ...renderOptions, bg: 'light' });
  if (dark.n > 1200 || dark.w * dark.h * dark.n > 40_000_000) fail('Export is too large; reduce recipe fps or effect duration/size');
  const sheet = lib.spriteSheet(fx, dark, name);
  // Upstream's canvas paste premultiplies RGB onto transparent cells. Preserve straight RGBA
  // so atlas frames and separately exported images have exactly the same alpha/color data.
  const atlasWidth = sheet.json.meta.size.w;
  const atlasHeight = sheet.json.meta.size.h;
  const atlasPixels = new Uint8Array(atlasWidth * atlasHeight * 4);
  for (let i = 0; i < dark.frames.length; i++) {
    const rect = sheet.json.frames[i].frame;
    for (let y = 0; y < dark.h; y++) {
      const start = y * dark.w * 4;
      atlasPixels.set(dark.frames[i].clear.subarray(start, start + dark.w * 4), ((rect.y + y) * atlasWidth + rect.x) * 4);
    }
  }
  const [ox, oy] = fx.meta.origin;
  if (![ox, oy].every(Number.isFinite) || ox < 0 || ox > dark.w || oy < 0 || oy > dark.h) fail('Origin must be inside the untrimmed frame');
  const manifest = {
    schemaVersion: 1, name, effectId: recipe.effectId, source, upstreamCommit: lock.commit,
    moduleSha256: sha(readFileSync(fx.file)), seed, fps: dark.fps, loop: !!fx.meta.loop,
    width: dark.w, height: dark.h, pixelsPerUnit: ppu,
    pivot: { x: ox / dark.w, y: 1 - oy / dark.h },
    pivotTopLeftPx: { x: ox, y: oy },
    frames: dark.frames.map((_, i) => ({ file: `frames/frame_${String(i).padStart(4, '0')}.png`, duration: 1 / dark.fps }))
  };
  const resolvedRecipe = { ...recipe, name, source, seed, pixelsPerUnit: ppu, fps: dark.fps, options };
  mkdirSync(join(out, 'frames'), { recursive: true });
  const writeJson = (file, value) => writeFileSync(join(out, file), JSON.stringify(value, null, 2) + '\n');
  writeFileSync(join(out, name + '.png'), encodePNG(atlasWidth, atlasHeight, atlasPixels));
  writeJson(name + '.json', sheet.json);
  writeJson(name + '.pixelfx.json', manifest);
  writeJson('recipe.json', resolvedRecipe);
  writeFileSync(join(out, 'contact-dark.png'), lib.contactSheet(fx, dark, { max: 24, zoom: 2 }));
  writeFileSync(join(out, 'contact-light.png'), lib.contactSheet(fx, light, { max: 24, zoom: 2 }));
  for (let i = 0; i < dark.frames.length; i++) {
    writeFileSync(join(out, manifest.frames[i].file), encodePNG(dark.w, dark.h, dark.frames[i].clear));
  }
  writeFileSync(join(out, 'LICENSE.pixelfx.txt'), readFileSync(join(vendor, 'LICENSE')));
  writeFileSync(join(out, 'NOTICE.pixelfx.md'), readFileSync(join(vendor, 'NOTICE.md')));
  return { output: out, effect: recipe.effectId, frames: dark.n, width: dark.w, height: dark.h, manifest: join(out, name + '.pixelfx.json') };
}

async function main(args) {
  const [command, ...rest] = args;
  if (command === 'verify') {
    const lock = verifyVendor();
    console.log(JSON.stringify({ commit: lock.commit, version: lock.version, verifiedFiles: lock.files.length }));
  } else if (command === 'list') {
    verifyVendor();
    const lib = await import(pathToFileURL(join(vendor, 'tools/lib/fxlib.mjs')).href);
    const words = rest.join(' ').toLowerCase().split(/\s+/).filter(Boolean);
    for (const id of lib.allIds()) {
      const fx = await lib.loadEffect(id);
      const searchable = `${id} ${fx.meta.name} ${fx.meta.desc} ${fx.meta.tags.join(' ')}`.toLowerCase();
      if (words.every(word => searchable.includes(word))) console.log(`${id}\t${fx.meta.loop ? 'loop' : 'once'}\t${fx.meta.size.join('x')}\t${fx.meta.desc}`);
    }
  } else if (command === 'export') {
    if (rest.length !== 4 || rest[0] !== '--recipe' || rest[2] !== '--out') fail('Usage: export --recipe <recipe.json> --out <fresh-directory>');
    console.log(JSON.stringify(await exportRecipe(rest[1], rest[3])));
  } else fail('Usage: pixelfx.mjs verify | list [words] | export --recipe <recipe.json> --out <fresh-directory>');
}

if (process.argv[1] && resolve(process.argv[1]) === fileURLToPath(import.meta.url)) {
  main(process.argv.slice(2)).catch(error => { console.error(error.message); process.exitCode = 1; });
}
