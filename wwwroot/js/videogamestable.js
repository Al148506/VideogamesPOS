// Videogame Index JavaScript Functionality

document.addEventListener('DOMContentLoaded', function () {
    // Initialize all functionality
    initializeImageLazyLoading();
    initializeTooltips();
});




// Escape special regex characters
function escapeRegExp(string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}
// Image lazy loading and error handling
function initializeImageLazyLoading() {
    const images = document.querySelectorAll('.game-image');

    images.forEach(img => {
        // Evitar múltiples registros
        img.onload = function () {
            this.style.opacity = '1';
        };

        img.onerror = function () {
            const placeholder = document.createElement('div');
            placeholder.className = 'no-image';
            placeholder.textContent = '🎮';
            this.parentNode.replaceChild(placeholder, this);
        };

        img.style.opacity = '0';
        img.style.transition = 'opacity 0.3s ease';

        // Si la imagen ya está cargada (caché), ejecuta onload manualmente
        if (img.complete && img.naturalWidth !== 0) {
            img.onload();
        } else if (img.complete && img.naturalWidth === 0) {
            img.onerror();
        }
    });
}


// Tooltip functionality
function initializeTooltips() {
    const elementsWithTitles = document.querySelectorAll('[title]');

    elementsWithTitles.forEach(element => {
        let tooltip;

        element.addEventListener('mouseenter', function (e) {
            const title = this.getAttribute('title');
            if (!title) return;

            // Create tooltip
            tooltip = document.createElement('div');
            tooltip.className = 'custom-tooltip';
            tooltip.textContent = title;
            tooltip.style.cssText = `
                position: absolute;
                background: rgba(0, 0, 0, 0.9);
                color: white;
                padding: 0.5rem 0.75rem;
                border-radius: 0.5rem;
                font-size: 0.875rem;
                z-index: 1000;
                pointer-events: none;
                opacity: 0;
                transition: opacity 0.3s ease;
                max-width: 300px;
                word-wrap: break-word;
            `;

            document.body.appendChild(tooltip);

            // Position tooltip
            const rect = this.getBoundingClientRect();
            tooltip.style.left = rect.left + 'px';
            tooltip.style.top = (rect.top - tooltip.offsetHeight - 8) + 'px';

            // Show tooltip
            setTimeout(() => {
                if (tooltip) tooltip.style.opacity = '1';
            }, 100);

            // Remove original title to prevent default tooltip
            this.setAttribute('data-original-title', title);
            this.removeAttribute('title');
        });

        element.addEventListener('mouseleave', function () {
            if (tooltip) {
                tooltip.remove();
                tooltip = null;
            }

            // Restore original title
            const originalTitle = this.getAttribute('data-original-title');
            if (originalTitle) {
                this.setAttribute('title', originalTitle);
                this.removeAttribute('data-original-title');
            }
        });
    });
}
