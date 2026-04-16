import { getProductos } from "./services/apiService.js";

let todosProductos = [];

async function cargarProductos() {
  try {
    todosProductos = await getProductos();
  } catch (error) {
    console.error("Error al cargar productos desde la API:", error);
    todosProductos = [];
    document.getElementById("contadorProductos").textContent = "Error al cargar productos";
    document.getElementById("gridProductos").innerHTML =
      '<p class="col-span-full text-center text-red-500 py-20 font-light text-lg">No se pudieron cargar los productos desde la base de datos.</p>';
    return;
  }

  mostrarProductos(todosProductos);
}

function mostrarProductos(productos) {
  const grid = document.getElementById("gridProductos");
  const contador = document.getElementById("contadorProductos");
  grid.innerHTML = "";

  contador.textContent = `${productos.length} Pieza${productos.length !== 1 ? "s" : ""}`;

  if (productos.length === 0) {
    grid.innerHTML =
      '<p class="col-span-full text-center text-gray-400 py-20 font-light text-lg">No se encontraron piezas con estos criterios.</p>';
    return;
  }

  productos.forEach((producto) => {
    const variante = producto.variantes?.[0] || {};
    const imagenUrl = variante.imagen || "https://images.unsplash.com/photo-1617038260897-41a1f14a8ca0?w=600";
    const review = producto.reviews?.[0];

    const stockInfo =
      variante.stock > 0
        ? `<span class="flex items-center gap-1.5 text-[#2c5234]"><span class="w-1.5 h-1.5 rounded-full bg-[#2c5234]"></span> ${variante.stock} disp.</span>`
        : '<span class="flex items-center gap-1.5 text-gray-400"><span class="w-1.5 h-1.5 rounded-full bg-gray-300"></span> Agotado</span>';

    const rating = review
      ? `<span class="flex items-center gap-1 text-xs text-gray-500"><span class="text-yellow-600">★</span> ${review.calificacion}</span>`
      : "";

    const html = `
      <div class="group flex flex-col cursor-pointer">
        <div class="relative overflow-hidden bg-[#f4f4f4] aspect-[4/5] mb-5">
          <img src="${imagenUrl}" alt="${producto.nombre}" class="w-full h-full object-cover object-center mix-blend-multiply transition-transform duration-700 ease-in-out group-hover:scale-105">
          <div class="absolute inset-0 bg-black/5 opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>
        </div>

        <div class="flex flex-col flex-1">
          <div class="flex justify-between items-start mb-1">
            <p class="text-[10px] font-semibold tracking-[0.2em] text-gray-500 uppercase">${producto.marca}</p>
            ${rating}
          </div>
          <h2 class="text-lg font-light text-gray-900 leading-snug mb-1">${producto.nombre}</h2>
          <p class="text-sm font-medium text-gray-900 mt-auto pt-2">$${producto.precioActual.toLocaleString("es-AR")}</p>
        </div>

        <div class="mt-4 pt-4 border-t border-gray-100 flex items-center justify-between text-[11px] font-medium tracking-widest uppercase">
          <div class="text-gray-500">${stockInfo}</div>
          <div class="flex gap-4">
            <a href="detalle.html?id=${producto.id}" class="text-gray-400 hover:text-black transition-colors">Ver</a>
            <a href="editar.html?id=${producto.id}" class="text-gray-400 hover:text-black transition-colors">Editar</a>
          </div>
        </div>
      </div>
    `;

    grid.innerHTML += html;
  });
}

function filtrarProductos() {
  let filtrados = todosProductos;

  const categorias = Array.from(document.querySelectorAll(".filter-check:checked")).map((el) => el.value);
  if (categorias.length > 0) {
    filtrados = filtrados.filter((p) => categorias.includes(p.categoria?.toLowerCase()));
  }

  const marca = document.getElementById("filtroMarca").value.toLowerCase();
  if (marca) {
    filtrados = filtrados.filter((p) => p.marca?.toLowerCase().includes(marca));
  }

  const precioMax = parseInt(document.getElementById("filtroPrecio").value, 10);
  filtrados = filtrados.filter((p) => p.precioActual <= precioMax);

  const busqueda = document.getElementById("busqueda").value.toLowerCase();
  if (busqueda) {
    filtrados = filtrados.filter(
      (p) =>
        p.nombre?.toLowerCase().includes(busqueda) ||
        p.marca?.toLowerCase().includes(busqueda) ||
        p.categoria?.toLowerCase().includes(busqueda)
    );
  }

  mostrarProductos(filtrados);
}

function inicializarFiltros() {
  document.querySelectorAll(".filter-check").forEach((el) => el.addEventListener("change", filtrarProductos));
  document.getElementById("filtroMarca").addEventListener("input", filtrarProductos);
  document.getElementById("busqueda").addEventListener("input", filtrarProductos);

  const sliderPrecio = document.getElementById("filtroPrecio");
  const labelPrecioMax = document.getElementById("precioMax");
  sliderPrecio.value = 50000000;

  sliderPrecio.addEventListener("input", () => {
    labelPrecioMax.textContent = parseInt(sliderPrecio.value, 10).toLocaleString("es-AR");
    filtrarProductos();
  });
}

inicializarFiltros();
cargarProductos();
