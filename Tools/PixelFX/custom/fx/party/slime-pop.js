import { Rng, hex } from "../../../vendor/pixelfx/src/core/index.js";

export const meta = {
  name: "Party slime pop", desc: "A compact candy star snap and falling balloon-coloured confetti.",
  tags: ["party", "slime", "pop", "confetti"], size: [112, 112], origin: [56, 64],
  duration: .7, loop: false, fps: 30, bg: "none",
  options: { seed: 21, palette: ["#703686", "#d880f5", "#ff8ed8", "#ffe36c", "#82d5ff", "#ffffff"] }
};

export function draw(p, t, o) {
  if (t >= meta.duration - .025) return;
  const C = o.palette.map(hex), R = new Rng(o.seed), fade = Math.min(1, (meta.duration-t)/.18);
  // Two-frame white snap opens into a purple/gold comic star.
  if (t < .15) {
    const r = 4 + 16*Math.sin(Math.PI*t/.15), pts = [];
    for(let i=0;i<16;i++) { const a=i*Math.PI/8, d=i%2?r*.35:r; pts.push([Math.cos(a)*d,Math.sin(a)*d]); }
    p.poly(pts,C[0]); p.poly(pts.map(([x,y])=>[x*.8,y*.8]),t<.055?C[5]:C[3]);
  }
  for(let i=0;i<24;i++) {
    const a=R.range(0,Math.PI*2), v=R.range(32,70), lift=R.range(12,45), color=C[1+i%4];
    const x=Math.cos(a)*v*t, y=Math.sin(a)*v*t-lift*t+55*t*t;
    const w=1+Math.abs(Math.sin(i+t*16))*2;
    p.rect(x-1,y-1,w+2,4,C[0],fade); p.rect(x,y,w,2,color,fade);
    if(i%4===0 && t<.38) { p.line(x-3,y,x+3,y,C[5],fade); p.line(x,y-3,x,y+3,C[5],fade); }
  }
}
