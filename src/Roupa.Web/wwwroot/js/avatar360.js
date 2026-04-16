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

            // 4 zones: 0-90 front→right, 90-180 right→back, 180-270 back→left, 270-360 left→front
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

            if (Math.abs(this.targetAngle - this.currentAngle) > 0.5) {
                this.rafId = requestAnimationFrame(step);
            }
        };
        this.rafId = requestAnimationFrame(step);
    },

    snapToNearest() {
        const normalized = ((this.targetAngle % 360) + 360) % 360;
        const nearest = Math.round(normalized / 90) * 90;
        this.targetAngle = this.targetAngle - normalized + nearest;
        this.animate();
    },

    autoRotate(elementId) {
        let angle = 0;
        return setInterval(() => {
            const el = document.getElementById(elementId);
            if (!el) return;
            angle = (angle + 0.4) % 360;
            const imgs = el.querySelectorAll('.av-frame');
            imgs?.forEach((img, i) => {
                const viewAngle = i * 90;
                let diff = Math.abs(angle - viewAngle);
                if (diff > 180) diff = 360 - diff;
                const opacity = Math.max(0, 1 - diff / 90);
                img.style.opacity = opacity;
            });
        }, 16);
    }
};
