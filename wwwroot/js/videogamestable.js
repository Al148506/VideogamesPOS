// Videogame Index JavaScript Functionality

document.addEventListener('DOMContentLoaded', function () {
    // Initialize all functionality
    initializeSearch();
    initializeSorting();
    initializeImageLazyLoading();
    initializeTooltips();
    updateResultsCount();
});

// Search functionality
function initializeSearch() {
    const searchInput = document.getElementById('searchInput');
    const table = document.getElementById('videogameTable');
    const rows = table.querySelectorAll('tbody tr');

    searchInput.addEventListener('input', function () {
        const searchTerm = this.value.toLowerCase().trim();
        let visibleCount = 0;

        rows.forEach(row => {
            const name = row.querySelector('.game-name').textContent.toLowerCase();
            const description = row.querySelector('.game-description').textContent.toLowerCase();

            const matchesSearch = name.includes(searchTerm) || description.includes(searchTerm);

            if (matchesSearch) {
                row.classList.remove('hidden');
                row.style.display = '';
                visibleCount++;
                // Add highlight effect
                highlightSearchTerm(row, searchTerm);
            } else {
                row.classList.add('hidden');
                row.style.display = 'none';
            }
        });

        updateVisibleCount(visibleCount);

        // Show/hide empty state
        toggleEmptyState(visibleCount === 0 && searchTerm !== '');
    });
}

// Highlight search terms
function highlightSearchTerm(row, searchTerm) {
    if (!searchTerm) return;

    const nameCell = row.querySelector('.game-name');
    const descriptionCell = row.querySelector('.game-description');

    [nameCell, descriptionCell].forEach(cell => {
        if (cell) {
            const originalText = cell.getAttribute('data-original') || cell.textContent;
            cell.setAttribute('data-original', originalText);

            if (searchTerm) {
                const regex = new RegExp(`(${escapeRegExp(searchTerm)})`, 'gi');
                const highlightedText = originalText.replace(regex, '<mark style="background: #fef08a; padding: 0.125rem 0.25rem; border-radius: 0.25rem;">$1</mark>');
                cell.innerHTML = highlightedText;
            } else {
                cell.textContent = originalText;
            }
        }
    });
}

// Escape special regex characters
function escapeRegExp(string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

// Sorting functionality
function initializeSorting() {
    const sortSelect = document.getElementById('sortSelect');
    const table = document.getElementById('videogameTable');
    const tbody = table.querySelector('tbody');
    const rows = Array.from(tbody.querySelectorAll('tr'));

    sortSelect.addEventListener('change', function () {
        const sortBy = this.value;
        sortTable(rows, tbody, sortBy);
    });

    // Add click sorting to headers
    const sortableHeaders = table.querySelectorAll('th.sortable');
    sortableHeaders.forEach(header => {
        header.addEventListener('click', function () {
            const sortType = this.getAttribute('data-sort');
            sortTable(rows, tbody, sortType);

            // Update select to match
            sortSelect.value = sortType;

            // Add visual feedback
            sortableHeaders.forEach(h => h.classList.remove('sorted'));
            this.classList.add('sorted');
        });
    });
}

// Sort table function
function sortTable(rows, tbody, sortBy) {
    const sortedRows = rows.slice().sort((a, b) => {
        let aValue, bValue;

        switch (sortBy) {
            case 'name':
                aValue = a.querySelector('.game-name').textContent.toLowerCase();
                bValue = b.querySelector('.game-name').textContent.toLowerCase();
                return aValue.localeCompare(bValue);

            case 'rating':
                aValue = parseFloat(a.querySelector('.rating-value').textContent) || 0;
                bValue = parseFloat(b.querySelector('.rating-value').textContent) || 0;
                return bValue - aValue; // Descending

            case 'price':
                aValue = parseFloat(a.querySelector('.price').textContent.replace('$', '')) || 0;
                bValue = parseFloat(b.querySelector('.price').textContent.replace('$', '')) || 0;
                return bValue - aValue; // Descending

            case 'release':
                aValue = new Date(a.getAttribute('data-release') || '1900-01-01');
                bValue = new Date(b.getAttribute('data-release') || '1900-01-01');
                return bValue - aValue; // Most recent first

            case 'stock':
                aValue = parseInt(a.querySelector('.stock').textContent) || 0;
                bValue = parseInt(b.querySelector('.stock').textContent) || 0;
                return bValue - aValue; // Descending

            default:
                return 0;
        }
    });

    // Re-append sorted rows
    sortedRows.forEach(row => tbody.appendChild(row));

    // Re-apply animation delays
    sortedRows.forEach((row, index) => {
        row.style.animationDelay = `${index * 0.05}s`;
    });
}

// Image lazy loading and error handling
function initializeImageLazyLoading() {
    const images = document.querySelectorAll('.game-image');

    images.forEach(img => {
        // Add loading placeholder
        img.addEventListener('load', function () {
            this.style.opacity = '1';
        });

        // Handle image errors
        img.addEventListener('error', function () {
            const placeholder = document.createElement('div');
            placeholder.className = 'no-image';
            placeholder.textContent = '🎮';
            this.parentNode.replaceChild(placeholder, this);
        });

        // Set initial opacity for smooth loading
        img.style.opacity = '0';
        img.style.transition = 'opacity 0.3s ease';
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

// Update results count
function updateResultsCount() {
    const totalCount = document.querySelectorAll('#videogameTable tbody tr').length;
    document.getElementById('totalCount').textContent = totalCount;
}

function updateVisibleCount(count) {
    document.getElementById('visibleCount').textContent = count;
}