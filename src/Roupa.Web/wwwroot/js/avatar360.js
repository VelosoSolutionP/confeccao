// ═══════════════════════════════════════════════════════
//  Avatar 360° — drag-to-rotate
// ═══════════════════════════════════════════════════════
window.Avatar360 = {
    dragging: false,
    startX: 0,
    currentAngle: 0,
    targetAngle: 0,
    rafId: null,
    el: null,

    init(elementId) {
        const el = document.getElementById(elementId);
        if (!el) return;
        this.el = el;
        this.currentAngle = 0;
        this.targetAngle = 0;

        el.addEventListener('mousedown', (e) => { this.dragging = true; this.startX = e.clientX; });
        el.addEventListener('touchstart', (e) => { this.dragging = true; this.startX = e.touches[0].clientX; }, { passive: true });

        window.addEventListener('mousemove', (e) => {
            if (!this.dragging) return;
            const dx = e.clientX - this.startX;
            this.startX = e.clientX;
            this.targetAngle += dx * 0.5;
            this.animate();
        });

        window.addEventListener('touchmove', (e) => {
            if (!this.dragging) return;
            const dx = e.touches[0].clientX - this.startX;
            this.startX = e.touches[0].clientX;
            this.targetAngle += dx * 0.5;
            this.animate();
        }, { passive: true });

        window.addEventListener('mouseup', () => { this.dragging = false; this.snapToNearest(); });
        window.addEventListener('touchend', () => { this.dragging = false; this.snapToNearest(); });
    },

    animate() {
        if (this.rafId) cancelAnimationFrame(this.rafId);
        const step = () => {
            this.currentAngle += (this.targetAngle - this.currentAngle) * 0.15;
            const normalized = ((this.currentAngle % 360) + 360) % 360;
            const imgs = this.el?.querySelectorAll('.av-frame');
            if (imgs) {
                imgs.forEach((img, i) => {
                    const viewAngle = i * 90;
                    let diff = Math.abs(normalized - viewAngle);
                    if (diff > 180) diff = 360 - diff;
                    const opacity = Math.max(0, 1 - diff / 90);
                    const scaleX = i % 2 === 0 ? 1 : Math.max(0.1, Math.abs(Math.cos((normalized - viewAngle) * Math.PI / 180)));
                    img.style.opacity = opacity;
                    img.style.transform = `scaleX(${scaleX})`;
                });
            }
            if (Math.abs(this.targetAngle - this.currentAngle) > 0.5)
                this.rafId = requestAnimationFrame(step);
        };
        this.rafId = requestAnimationFrame(step);
    },

    snapToNearest() {
        const normalized = ((this.targetAngle % 360) + 360) % 360;
        const nearest = Math.round(normalized / 90) * 90;
        this.targetAngle = this.targetAngle - normalized + nearest;
        this.animate();
    },

    updateFrames(elementId, urls) {
        const el = document.getElementById(elementId);
        if (!el) return;
        const frames = el.querySelectorAll('.av-frame');
        frames.forEach((img, i) => {
            if (urls[i]) img.src = urls[i];
        });
    }
};

// ═══════════════════════════════════════════════════════
//  PreloadImages — aquece o cache do browser para URLs Pollinations
// ═══════════════════════════════════════════════════════
window.PreloadImages = {
    preload(urls) {
        if (!urls || !urls.length) return;
        urls.forEach(url => {
            const img = new Image();
            img.src = url;
        });
    }
};

// ═══════════════════════════════════════════════════════
//  OutfitAnyone — virtual try-on via Gradio API
//  Fluxo: descrição da peça → flat-lay (Pollinations) →
//         manequim vestido (OutfitAnyone) → 4 vistas 360°
// ═══════════════════════════════════════════════════════
window.OutfitAnyone = {

    SPACE: 'https://humanaigc-outfitanyone.hf.space',

    // Manequins base (Pollinations, seeds fixas = sempre a mesma imagem)
    _mannequin(view) {
        const seeds = { front: 9001, right: 9002, back: 9003, left: 9004 };
        const prompts = {
            front: 'white female dress form mannequin front view no clothing white studio background professional',
            right: 'white female dress form mannequin right side view no clothing white studio background professional',
            back:  'white female dress form mannequin back view no clothing white studio background professional',
            left:  'white female dress form mannequin left side view no clothing white studio background professional',
        };
        const p = prompts[view] || prompts.front;
        const s = seeds[view] || seeds.front;
        return `https://image.pollinations.ai/prompt/${encodeURIComponent(p)}?width=420&height=600&model=flux&seed=${s}&nologo=true`;
    },

    // Flat-lay da peça para servir de input ao OutfitAnyone
    _garmentUrl(tipo, cores, tecido, logo) {
        const prompt = `flat lay fashion ${tipo}, ${cores} color, ${tecido} fabric, ${logo}, isolated on pure white background, professional product photography, no model, no mannequin`;
        return `https://image.pollinations.ai/prompt/${encodeURIComponent(prompt)}?width=400&height=500&model=flux&seed=5500&nologo=true`;
    },

    // Tenta OutfitAnyone Gradio API; retorna URL da imagem ou null
    async _tryOn(mannequinUrl, garmentUrl) {
        const session = Math.random().toString(36).slice(2, 10);

        // ── Estratégia 1: endpoint simples /api/predict ──────────
        try {
            const r = await fetch(`${this.SPACE}/api/predict`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    fn_index: 0,
                    data: [mannequinUrl, garmentUrl],
                    session_hash: session
                }),
                signal: AbortSignal.timeout(45000)
            });
            if (r.ok) {
                const j = await r.json();
                const d = j?.data?.[0];
                if (typeof d === 'string' && d.length > 10) return d.startsWith('http') || d.startsWith('data:') ? d : null;
                if (d?.url) return d.url;
                if (d?.path) return `${this.SPACE}/file=${d.path}`;
            }
        } catch (e) { console.warn('[OutfitAnyone] /api/predict:', e.message); }

        // ── Estratégia 2: fila Gradio /queue/join + SSE ──────────
        try {
            const joinR = await fetch(`${this.SPACE}/queue/join`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    fn_index: 0,
                    data: [mannequinUrl, garmentUrl],
                    session_hash: session
                }),
                signal: AbortSignal.timeout(10000)
            });
            if (!joinR.ok) throw new Error('join failed');

            return await new Promise((resolve, reject) => {
                const src = new EventSource(`${this.SPACE}/queue/data?session_hash=${session}`);
                const timer = setTimeout(() => { src.close(); reject(new Error('timeout')); }, 60000);

                src.addEventListener('message', (ev) => {
                    try {
                        const msg = JSON.parse(ev.data);
                        if (msg.msg === 'process_completed') {
                            clearTimeout(timer); src.close();
                            const d = msg.output?.data?.[0];
                            if (typeof d === 'string') resolve(d);
                            else if (d?.url) resolve(d.url);
                            else if (d?.path) resolve(`${this.SPACE}/file=${d.path}`);
                            else reject(new Error('no image'));
                        } else if (msg.msg === 'queue_full') {
                            clearTimeout(timer); src.close();
                            reject(new Error('queue_full'));
                        }
                    } catch (_) { /* ignore parse errors */ }
                });
                src.onerror = () => { clearTimeout(timer); src.close(); reject(new Error('sse_error')); };
            });
        } catch (e) { console.warn('[OutfitAnyone] /queue:', e.message); }

        return null;
    },

    // ── API pública chamada pelo Blazor ─────────────────────────
    // Retorna array de 4 URLs: [frente, direita, costas, esquerda]
    // ou null se falhar (Blazor usa Pollinations como fallback)
    async gerar4Vistas(tipo, cores, tecido, logo) {
        const garmentUrl = this._garmentUrl(tipo, cores, tecido, logo);
        const views = ['front', 'right', 'back', 'left'];

        console.log('[OutfitAnyone] gerando 4 vistas para:', tipo, cores);

        // Gera frente primeiro para ver se a API está respondendo
        const frontUrl = await this._tryOn(this._mannequin('front'), garmentUrl);
        if (!frontUrl) {
            console.warn('[OutfitAnyone] API indisponível, usando Pollinations');
            return null;
        }

        // Gera as outras 3 vistas em paralelo
        const [rightUrl, backUrl, leftUrl] = await Promise.all(
            ['right', 'back', 'left'].map(v => this._tryOn(this._mannequin(v), garmentUrl))
        );

        return [
            frontUrl,
            rightUrl  || frontUrl,
            backUrl   || frontUrl,
            leftUrl   || frontUrl
        ];
    }
};
