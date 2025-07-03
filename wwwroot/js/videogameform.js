// Animación suave para los campos de selección múltiple
document.querySelectorAll('select[multiple]').forEach(select => {
    select.addEventListener('focus', function () {
        this.style.transform = 'scale(1.02)';
    });

    select.addEventListener('blur', function () {
        this.style.transform = 'scale(1)';
    });
});

// Vista previa de imagen
const imageUrlInput = document.querySelector('input[name="Videogame.ImageUrl"]');
if (imageUrlInput) {
    imageUrlInput.addEventListener('blur', function () {
        const url = this.value;
        if (url && (url.includes('http') || url.includes('https'))) {
            // Crear vista previa si no existe
            let preview = document.querySelector('.image-preview');
            if (!preview) {
                preview = document.createElement('div');
                preview.className = 'image-preview';
                preview.style.cssText = 'margin-top: 0.5rem; text-align: center;';
                this.parentNode.appendChild(preview);
            }
            preview.innerHTML = `<img src="${url}" alt="Vista previa" style="max-width: 200px; max-height: 150px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);" onerror="this.style.display='none'">`;
        }
    });
}
document.getElementById("Videogame_Name").addEventListener("blur", async function () {
    const name = this.value;
    if (name.length < 3) return;

    const response = await fetch(`/Videogames/AutocompleteFromRawg?name=${encodeURIComponent(name)}`);
    if (!response.ok) return;

    const data = await response.json();
    if (!data) return;

    // Mapeo del rating de RAWG a los valores esperados en el select
    const ratingMap = {
        "everyone": "E",
        "everyone-10-plus": "E10+",
        "teen": "T",
        "mature": "M",
        "adults-only": "AO",
        "rating-pending": "RP"
    };

    const mappedRating = ratingMap[data.rating?.toLowerCase()] || "";

    document.getElementById("Videogame_Description").value = data.description || "";
    document.getElementById("Videogame_ReleaseDate").value = data.releaseDate || "";
    document.getElementById("Videogame_Rating").value = mappedRating;
    document.getElementById("Videogame_ImageUrl").value = data.imageUrl || "";

    // Seleccionar plataformas y géneros automáticamente
    selectOptionsByNames("SelectedPlatformIds", data.platforms);
    selectOptionsByNames("SelectedGenreIds", data.genres);
});

function selectOptionsByNames(selectId, namesArray) {
    const select = document.getElementById(selectId);
    if (!select || !namesArray) return;

    for (let i = 0; i < select.options.length; i++) {
        const option = select.options[i];
        if (namesArray.includes(option.text)) {
            option.selected = true;
        } else {
            option.selected = false;
        }
    }



document.addEventListener("DOMContentLoaded", function () {
        const nameInput = document.getElementById("Videogame_Name");
        const datalist = document.createElement("datalist");
        datalist.id = "titleSuggestions";
        document.body.appendChild(datalist);
        nameInput.setAttribute("list", "titleSuggestions");

        let timeoutId;
        nameInput.addEventListener("input", function () {
            clearTimeout(timeoutId);
            const term = this.value;
            if (term.length < 3) return;

            timeoutId = setTimeout(async () => {
                const response = await fetch(`/Videogames/SuggestTitlesFromRawg?term=${encodeURIComponent(term)}`);
                if (!response.ok) return;

                const titles = await response.json();
                datalist.innerHTML = "";
                titles.forEach(title => {
                    const option = document.createElement("option");
                    option.value = title;
                    datalist.appendChild(option);
                });
            }, 300);
        });
    });
}

