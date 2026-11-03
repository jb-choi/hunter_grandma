import { test } from 'node:test';
import assert from 'node:assert/strict';
import { readFileSync, writeFileSync, readdirSync, mkdirSync, mkdtempSync, existsSync } from 'node:fs';
import { join, resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { inflateSync } from 'node:zlib';
import { exportRecipe, verifyVendor } from '../../../.agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs';

const root = resolve(dirname(fileURLToPath(import.meta.url)), '../../..');
const testRoot = join(root, 'Library/PixelFXTests');
mkdirSync(testRoot, { recursive: true });
const run = mkdtempSync(join(testRoot, 'run-'));
const recipe = JSON.parse(readFileSync(join(root, '.agents/skills/hunter-grandma-pixelfx/assets/impact-example.json')));
function makeRecipe(name, changes = {}) {
  const file = join(run, name + '.json');
  writeFileSync(file, JSON.stringify({ ...recipe, ...changes }));
  return file;
}
const input = makeRecipe('impact');

// Independent PNG readback, so premultiplied atlas colors cannot pass a metadata-only check.
function decodePNG(file) {
  const png = readFileSync(file);
  assert.deepEqual([...png.subarray(0, 8)], [137, 80, 78, 71, 13, 10, 26, 10]);
  let width, height;
  const compressed = [];
  for (let at = 8; at < png.length;) {
    const length = png.readUInt32BE(at);
    const type = png.toString('ascii', at + 4, at + 8);
    const data = png.subarray(at + 8, at + 8 + length);
    if (type === 'IHDR') {
      width = data.readUInt32BE(0); height = data.readUInt32BE(4);
      assert.equal(data[8], 8); assert.equal(data[9], 6); assert.equal(data[12], 0);
    }
    if (type === 'IDAT') compressed.push(data);
    at += length + 12;
  }
  const raw = inflateSync(Buffer.concat(compressed));
  const pixels = Buffer.alloc(width * height * 4), stride = width * 4;
  const paeth = (a, b, c) => {
    const p = a + b - c, da = Math.abs(p - a), db = Math.abs(p - b), dc = Math.abs(p - c);
    return da <= db && da <= dc ? a : db <= dc ? b : c;
  };
  for (let y = 0; y < height; y++) {
    const filter = raw[y * (stride + 1)];
    assert.ok(filter >= 0 && filter <= 4);
    for (let x = 0; x < stride; x++) {
      const at = y * stride + x, a = x >= 4 ? pixels[at - 4] : 0;
      const b = y > 0 ? pixels[at - stride] : 0, c = y > 0 && x >= 4 ? pixels[at - stride - 4] : 0;
      const predictor = [0, a, b, Math.floor((a + b) / 2), paeth(a, b, c)][filter];
      pixels[at] = (raw[y * (stride + 1) + 1 + x] + predictor) & 255;
    }
  }
  return { width, height, pixels };
}

test('fixed upstream source matches its recorded hashes', () => {
  const lock = verifyVendor();
  assert.equal(lock.commit, 'a84159688af3ca37a4be05a0d422b94451b2e24e');
  assert.ok(lock.files.length > 250);
  assert.ok(lock.files.every(f => !f.path.startsWith('gallery/')));
});

test('same recipe renders byte-identical sheets, frames and manifests', async () => {
  const a = await exportRecipe(input, join(run, 'first'));
  const b = await exportRecipe(input, join(run, 'repeated'));
  const names = readdirSync(a.output).filter(n => n !== 'frames');
  for (const name of names) assert.deepEqual(readFileSync(join(a.output, name)), readFileSync(join(b.output, name)), name);
  for (const frame of readdirSync(join(a.output, 'frames'))) {
    assert.deepEqual(readFileSync(join(a.output, 'frames', frame)), readFileSync(join(b.output, 'frames', frame)), frame);
  }
  const manifest = JSON.parse(readFileSync(a.manifest));
  assert.equal(manifest.frames.length, a.frames);
  assert.equal(readdirSync(join(a.output, 'frames')).length, a.frames);
  assert.equal(manifest.loop, false);
  assert.ok(manifest.frames.every(f => f.duration === 1 / manifest.fps));
  assert.ok(Math.abs(manifest.pivot.x * manifest.width - manifest.pivotTopLeftPx.x) < 1e-6);
  assert.ok(Math.abs((1 - manifest.pivot.y) * manifest.height - manifest.pivotTopLeftPx.y) < 1e-6);
});

test('another seed produces a different particle sequence', async () => {
  const a = await exportRecipe(input, join(run, 'seed-a'));
  const b = await exportRecipe(makeRecipe('other-seed', { seed: 84 }), join(run, 'seed-b'));
  assert.notDeepEqual(readFileSync(join(a.output, 'impact-example.png')), readFileSync(join(b.output, 'impact-example.png')));
});

test('each atlas rectangle preserves the exact straight RGBA of its individual frame', async () => {
  const exported = await exportRecipe(input, join(run, 'pixel-readback'));
  const metadata = JSON.parse(readFileSync(join(exported.output, 'impact-example.json')));
  const atlas = decodePNG(join(exported.output, 'impact-example.png'));
  const manifest = JSON.parse(readFileSync(exported.manifest));
  for (let i = 0; i < manifest.frames.length; i++) {
    const frame = decodePNG(join(exported.output, manifest.frames[i].file));
    const rect = metadata.frames[i].frame;
    assert.equal(rect.w, frame.width); assert.equal(rect.h, frame.height);
    for (let y = 0; y < frame.height; y++) {
      const start = ((rect.y + y) * atlas.width + rect.x) * 4;
      assert.deepEqual(atlas.pixels.subarray(start, start + frame.width * 4), frame.pixels.subarray(y * frame.width * 4, (y + 1) * frame.width * 4));
    }
  }
});

test('existing output and user files are preserved on an export retry', async () => {
  const out = join(run, 'occupied');
  mkdirSync(out);
  writeFileSync(join(out, 'user-edit.txt'), 'preserve me');
  await assert.rejects(exportRecipe(input, out), /not empty/);
  assert.deepEqual(readdirSync(out), ['user-edit.txt']);
  assert.equal(readFileSync(join(out, 'user-edit.txt'), 'utf8'), 'preserve me');
});

test('path traversal and a second seed authority are rejected before output', async () => {
  const out = join(run, 'invalid');
  await assert.rejects(exportRecipe(makeRecipe('traversal', { effectId: '../outside' }), out), /category\/name/);
  await assert.rejects(exportRecipe(makeRecipe('seed-override', { options: { seed: 2 } }), out), /top-level seed/);
  assert.equal(existsSync(out), false);
});

test('body-painted stock actor effects cannot silently enter game exports', async () => {
  const out = join(run, 'stock-actor');
  await assert.rejects(exportRecipe(makeRecipe('on-body', { effectId: 'impacts/hit-flash' }), out), /stock dummy/);
  assert.equal(existsSync(out), false);
});

test('loop export keeps its loop flag and complete frame package', async () => {
  const out = await exportRecipe(makeRecipe('loop', { name: 'coin-example', effectId: 'pickups/coin-spin', fps: 20 }), join(run, 'loop'));
  const manifest = JSON.parse(readFileSync(out.manifest));
  assert.equal(manifest.loop, true);
  assert.equal(manifest.fps, 20);
  assert.equal(manifest.frames.length, 32);
  assert.ok(existsSync(join(out.output, 'LICENSE.pixelfx.txt')));
  assert.ok(existsSync(join(out.output, 'NOTICE.pixelfx.md')));
});

console.log('Retained test artifacts: ' + run);
