const API_BASE_URL = "http://localhost:5033/api";

async function apiRequest(endpoint, options = {}) {
  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {}),
    },
    ...options,
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Error ${response.status}: ${errorText || response.statusText}`);
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
}

export async function getProductos() {
  return apiRequest("/Producto");
}

export async function getProductoById(productId) {
  return apiRequest(`/producto/${productId}`);
}

export async function createProducto(payload) {
  return apiRequest("/producto", {
    method: "POST",
    body: JSON.stringify(payload),
  });
}

export async function updateProducto(productId, payload) {
  return apiRequest(`/producto/${productId}`, {
    method: "PUT",
    body: JSON.stringify(payload),
  });
}
