import { getProductoById, updateProducto, getProductos } from "./services/apiService.js";

let productoActual = null;
let todosProductos = [];
let productosFiltrados = [];

/**
 * Carga todos los productos desde la API y construye el selector
 */
async function cargarListaProductos() {
  try {
    todosProductos = await getProductos();
    productosFiltrados = todosProductos;
    construirSelectProductos();
  } catch (error) {
    console.error('Error cargando productos:', error);
    alert('No se pudo cargar la lista de productos.');
  }
}

/**
 * Construye el selector de productos en el dropdown
 */
function construirSelectProductos() {
  const select = document.getElementById('selectProducto');
  if (!select) return;

  select.innerHTML = '<option value="">-- Seleccionar producto --</option>';
  
  productosFiltrados.forEach((producto) => {
    const option = document.createElement('option');
    option.value = producto._id?.$oid || producto.id;
    const nombre = producto.nombre || 'Producto sin nombre';
    const marca = producto.marca ? ` - ${producto.marca}` : '';
    option.textContent = `${nombre}${marca}`;
    select.appendChild(option);
  });
}

/**
 * Filtra productos según búsqueda y muestra sugerencias
 */
function filtrarProductos() {
  const busqueda = document.getElementById('buscarProducto')?.value.toLowerCase() || '';
  const sugerenciasDiv = document.getElementById('sugerenciasProductos');
  
  if (busqueda.trim() === '') {
    productosFiltrados = todosProductos;
    if (sugerenciasDiv) sugerenciasDiv.classList.add('hidden');
    construirSelectProductos();
    return;
  }
  
  productosFiltrados = todosProductos.filter(p => {
    const nombre = (p.nombre || '').toLowerCase();
    const marca = (p.marca || '').toLowerCase();
    const categoria = (p.categoria || '').toLowerCase();
    return nombre.includes(busqueda) || marca.includes(busqueda) || categoria.includes(busqueda);
  });
  
  // Mostrar sugerencias
  if (productosFiltrados.length > 0) {
    let html = '<ul class="max-h-48 overflow-y-auto">';
    productosFiltrados.slice(0, 8).forEach(p => {
      const nombre = p.nombre || 'Producto sin nombre';
      const marca = p.marca ? ` - ${p.marca}` : '';
      const productId = p._id?.$oid || p.id;
      html += `
        <li class="hover:bg-blue-100 cursor-pointer p-2 border-b" onclick="seleccionarProductoDeSugerencia('${productId}', '${nombre}${marca}')">
          <p class="font-semibold text-sm">${nombre}</p>
          <p class="text-xs text-gray-600">${marca} | ${p.categoria || 'Sin categoría'}</p>
        </li>
      `;
    });
    html += '</ul>';
    sugerenciasDiv.innerHTML = html;
    sugerenciasDiv.classList.remove('hidden');
  } else {
    sugerenciasDiv.innerHTML = '<p class="p-3 text-gray-500 text-sm">No se encontraron productos</p>';
    sugerenciasDiv.classList.remove('hidden');
  }
  
  construirSelectProductos();
}

/**
 * Selecciona un producto desde las sugerencias
 */
function seleccionarProductoDeSugerencia(productId, nombreProducto) {
  document.getElementById('selectProducto').value = productId;
  document.getElementById('buscarProducto').value = nombreProducto;
  document.getElementById('sugerenciasProductos').classList.add('hidden');
  cargarProductoSeleccionado();
}

/**
 * Carga el producto seleccionado del selector
 */
async function cargarProductoSeleccionado() {
  const select = document.getElementById('selectProducto');
  if (!select) return;

  const productId = select.value;
  
  // Validación: producto es obligatorio
  if (!productId || productId.trim() === '') {
    alert('Por favor, selecciona un producto para continuar.');
    document.getElementById('formEditar').classList.add('hidden');
    return;
  }

  try {
    productoActual = await getProductoById(productId);
    
    if (!productoActual) {
      alert('No se pudo cargar el producto.');
      return;
    }

    // Mostrar el formulario
    cargarFormulario(productoActual);
    actualizarListaVariantes();
    document.getElementById('formEditar').classList.remove('hidden');
    document.getElementById('seccionBusqueda').classList.add('hidden');
    
    // Scroll al formulario
    document.querySelector('form').scrollIntoView({ behavior: 'smooth' });
  } catch (error) {
    console.error('Error cargando producto:', error);
    alert('No se pudo cargar el producto.');
    productoActual = null;
    document.getElementById('formEditar').classList.add('hidden');
  }
}

/**
 * Vuelve al selector de productos (Cambiar de producto)
 */
function volverAlSelector() {
  productoActual = null;
  document.getElementById('formEditar').classList.add('hidden');
  document.getElementById('seccionBusqueda').classList.remove('hidden');
  document.getElementById('selectProducto').value = '';
  document.getElementById('buscarProducto').value = '';
}


/**
 * Actualiza la visibilidad de los campos dinámicos (obsoleto - ya no se usa)
 */
function actualizarCampos() {
  // Esta función ya no es necesaria ya que se eliminó la sección de especificaciones
}

/**
 * Carga los datos de una variante en el formulario (obsoleto)
 */
function cargarVariante(index) {
  // Esta función ya no es necesaria
}

/**
 * Muestra la lista de variantes en la sección de edición
 */
function actualizarListaVariantes() {
  const container = document.getElementById('variantesEdicion');
  if (!container) return;

  container.innerHTML = '';

  if (!productoActual?.variantes || productoActual.variantes.length === 0) {
    container.innerHTML = '<p class="text-gray-500 text-sm">No hay variantes. Agrega una nueva.</p>';
    return;
  }

  productoActual.variantes.forEach((v, i) => {
    const descripcion = v.colorCaja || v.talleAnillo || v.metal || `Variante ${i + 1}`;
    const imagenUrl = v.imagen || '';
    const tieneImagen = imagenUrl && imagenUrl.trim() !== '';
    
    const html = `
      <div class="border-2 border-blue-200 p-4 rounded bg-blue-50 variante-row" data-index="${i}">
        <div class="flex justify-between items-start mb-4">
          <div class="flex-1">
            <p class="font-bold text-gray-800 text-lg">${descripcion}</p>
            <p class="text-xs text-gray-600 mt-1">Variante ${i + 1}</p>
          </div>
          <button type="button" onclick="eliminarVariante(${i})" class="bg-red-100 text-red-600 px-3 py-1 rounded text-sm hover:bg-red-200 font-semibold">
            ✕ Eliminar
          </button>
        </div>
        
        <!-- Contenedor principal: input + datos a la izquierda, imagen a la derecha -->
        <div class="flex gap-4">
          <div class="flex-1">
            <!-- Controles de imagen -->
            <div class="mb-4">
              <label class="text-xs text-gray-600 font-semibold block mb-2">🖼️ Imagen URL</label>
              <div class="flex gap-2">
                <input id="imagen-input-${i}" class="flex-1 border p-2 rounded variante-imagen text-xs" placeholder="https://ejemplo.com/imagen.jpg" value="${imagenUrl}">
                <button type="button" class="bg-blue-600 text-white px-3 py-2 rounded text-sm hover:bg-blue-700 font-semibold whitespace-nowrap" onclick="refrescarImagenVariante(${i})" title="Actualizar vista previa">
                  🔄
                </button>
              </div>
            </div>
            
            <!-- Campos de edición -->
            <div class="grid grid-cols-3 gap-2">
              <div>
                <label class="text-xs text-gray-600 font-semibold block mb-1">📦 Stock</label>
                <input class="w-full border-2 border-blue-400 p-2 rounded font-bold text-lg variante-stock" type="number" placeholder="Stock" value="${v.stock || 0}">
              </div>
              <div>
                <label class="text-xs text-gray-600 font-semibold block mb-1">💰 Precio</label>
                <input class="w-full border p-2 rounded variante-precio text-xs" type="number" placeholder="Precio" value="${v.precioVariante || 0}">
              </div>
              <div>
                <label class="text-xs text-gray-600 font-semibold block mb-1">🏷️ Descripción</label>
                <input class="w-full border p-2 rounded variante-descripcion text-xs" placeholder="Color, Talle, Material..." value="${descripcion}">
              </div>
            </div>
          </div>
          
          <!-- Imagen a la derecha -->
          <div class=" flex items-start justify-end" style="width: 8rem;">
            <div class=" h-32 border-2 border-gray-300 rounded flex items-center justify-center bg-white overflow-hidden flex-shrink-0">
              <img id="preview-${i}" src="${imagenUrl}" alt="Preview" class=" h-8 object-cover ${!tieneImagen ? 'hidden' : ''}" onerror="document.getElementById('preview-${i}').classList.add('hidden'); document.getElementById('no-image-${i}').classList.remove('hidden');">
              <div id="no-image-${i}" class="text-center text-gray-400 text-xs p-2 ${tieneImagen ? 'hidden' : ''}">
                <p>📷</p>
                <p>Sin imagen</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    `;
    container.innerHTML += html;
  });
}

/**
 * Refresca la vista previa de imagen de una variante
 */
function refrescarImagenVariante(index) {
  const inputElement = document.getElementById(`imagen-input-${index}`);
  const previewImg = document.getElementById(`preview-${index}`);
  const noImageDiv = document.getElementById(`no-image-${index}`);
  
  if (!inputElement) return;
  
  const inputUrl = inputElement.value.trim();
  
  if (!inputUrl) {
    // URL vacía
    if (previewImg) previewImg.classList.add('hidden');
    if (noImageDiv) noImageDiv.classList.remove('hidden');
    return;
  }
  
  // Crear una imagen temporal para validar
  const tempImg = new Image();
  tempImg.onload = function() {
    if (previewImg) {
      previewImg.src = inputUrl;
      previewImg.classList.remove('hidden');
    }
    if (noImageDiv) noImageDiv.classList.add('hidden');
  };
  tempImg.onerror = function() {
    // Error al cargar la imagen
    alert('No se pudo cargar la imagen. Verifica la URL.');
    if (previewImg) previewImg.classList.add('hidden');
    if (noImageDiv) noImageDiv.classList.remove('hidden');
  };
  tempImg.src = inputUrl;
}

/**
 * Agrega una nueva variante
 */
function agregarVariante() {
  if (!productoActual.variantes) {
    productoActual.variantes = [];
  }

  productoActual.variantes.push({
    colorCaja: '',
    stock: 0,
    precioVariante: 0,
    imagen: ''
  });

  actualizarListaVariantes();
}

/**
 * Elimina una variante
 */
function eliminarVariante(index) {
  if (!confirm('¿Estás seguro de que deseas eliminar esta variante?')) return;

  productoActual.variantes.splice(index, 1);
  actualizarListaVariantes();
}

/**
 * Inicializa la página cargando la lista de productos
 */
async function inicializarPagina() {
  await cargarListaProductos();
}

/**
 * Carga los datos generales del producto en el formulario
 */
function cargarFormulario(p) {
  document.getElementById('nombre').value = p.nombre || '';
  document.getElementById('marca').value = p.marca || '';
  document.getElementById('categoria').value = p.categoria || '';
  document.getElementById('descripcion').value = p.descripcion || '';
}

/**
 * Obtiene las variantes editadas desde el formulario
 */
function obtenerVariantesDesdeFormulario() {
  return Array.from(document.querySelectorAll('.variante-row')).map((row) => {
    const descripcion = row.querySelector('.variante-descripcion')?.value || 'Variante sin nombre';
    const stock = parseInt(row.querySelector('.variante-stock')?.value) || 0;
    const precioVariante = parseInt(row.querySelector('.variante-precio')?.value) || 0;
    const imagen = row.querySelector('.variante-imagen')?.value || '';
    return {
      colorCaja: descripcion,
      stock,
      precioVariante,
      imagen
    };
  });
}

/**
 * Maneja el envío del formulario
 */
document.getElementById('formEditar')?.addEventListener('submit', async function(e) {
  e.preventDefault();
  
  const categoria = document.getElementById('categoria').value;
  const variantes = obtenerVariantesDesdeFormulario();

  if (variantes.length === 0) {
    alert('Debe haber al menos una variante.');
    return;
  }

  const producto = {
    id: productoActual.id,
    nombre: document.getElementById('nombre').value,
    marca: document.getElementById('marca').value,
    categoria,
    descripcion: document.getElementById('descripcion').value,
    precioActual: productoActual.precioActual || 0,
    especificaciones: productoActual.especificaciones || {},
    variantes,
    disponibilidad: productoActual.disponibilidad || {},
    reviews: productoActual.reviews || [],
    metadata: {
      ...productoActual.metadata,
      actualizadoEn: new Date().toISOString().split('T')[0]
    }
  };

  try {
    await updateProducto(productoActual.id, producto);
    alert('Producto actualizado correctamente.');
    window.location.href = 'index.html';
  } catch (error) {
    console.error('Error actualizando producto:', error);
    alert('No se pudo actualizar el producto. Asegúrate de tener el backend disponible.');
  }
});

/**
 * Actualiza la vista previa de imagen
 */
document.getElementById('imagen')?.addEventListener('change', function() {
  document.getElementById('imagenPreview').src = this.value;
});

// Exponer funciones globales para el HTML
window.actualizarCampos = actualizarCampos;
window.agregarVariante = agregarVariante;
window.eliminarVariante = eliminarVariante;
window.cargarVariante = cargarVariante;
window.cargarProductoSeleccionado = cargarProductoSeleccionado;
window.volverAlSelector = volverAlSelector;
window.filtrarProductos = filtrarProductos;
window.seleccionarProductoDeSugerencia = seleccionarProductoDeSugerencia;
window.refrescarImagenVariante = refrescarImagenVariante;

// Inicializar cuando el DOM esté listo
window.addEventListener('DOMContentLoaded', () => {
  inicializarPagina();
  
  // Agregar búsqueda en tiempo real
  const buscarInput = document.getElementById('buscarProducto');
  if (buscarInput) {
    buscarInput.addEventListener('input', filtrarProductos);
  }
});

