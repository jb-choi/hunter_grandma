# HU-BASE · imagegen 시안 프롬프트

방식: 내장 image_gen, 기존 hunter.png 참고, transparent_background=true.

아래 프롬프트는 고해상도 디자인 시안용이다. 최종 납품본은 Aseprite에서 실제 32×32 격자로 재작화했다.

```text
Use case: stylized-concept / pixel-sprite redesign. Asset type: Hunting Idle friendly hunter base sprite, exactly one right-facing idle character, transparent background. Input image: local hunter.png is the existing character reference; preserve recognizable moss-green brimmed hat with small cream feather, warm bright face, woodland green clothing and brown boots, but REDESIGN the silhouette and REMOVE THE OLD GUN completely. Primary request: a clean production pixel-art redesign constrained to a 32 by 32 logical pixel canvas, shown as an integer nearest-neighbor enlarged sprite. Every logical pixel is a solid square block; total figure about 27-29 logical pixels tall, centered with 1-2 px padding, feet aligned. Cozy forest/camp theme matching small existing woodland animal pixel sprites. Slightly large readable head, distinct brim, calm tiny dark eye facing right, short readable nose, forest tunic, simple leather belt, short separate legs and rounded boots. Relaxed base pose. A small simple curved wooden bow can rest in the forward hand, close to body; no arrow being fired, no aiming pose. Broad limited color clusters, 1-logical-pixel dark #18141F silhouette outline, left/top lighting, moss colors #536D45 #91AA63, warm cream/skin colors #F7EACA #FAC780 #D78B4E, wood/leather #7E4930 #BE7443. No gradients, no antialiasing, no subpixel texture, no soft shading, no dither, no cast ground shadow, no landscape, no grid, no UI, no typography, no watermarks. Just a single isolated sprite with truly transparent surrounding pixels, a character base suitable for later Aseprite hand cleanup into exact 32x32 and separate equipment layers.
```

