/**
 * Dashboard ejecutivo — datos solo desde la API (getProductos).
 * Forma de documentos alineada con Backend/WebAPI-TP1-BD2/Joyeria_DB_final.json
 * (export con _id.$oid, ventas[], especificaciones en snake_case o camelCase, stock en variantes[], etc.)
 */
import { getProductos } from "./services/apiService.js";

const formatCurrency = (value) =>
  new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS", maximumFractionDigits: 0 }).format(
    value || 0
  );

const charts = {};
let allProducts = [];

const els = {
  timeMode: document.getElementById("timeMode"),
  yearFilter: document.getElementById("yearFilter"),
  monthFilter: document.getElementById("monthFilter"),
  monthFilterWrap: document.getElementById("monthFilterWrap"),
  categoryFilter: document.getElementById("categoryFilter"),
  productFilter: document.getElementById("productFilter"),
  materialFilter: document.getElementById("materialFilter"),
  statusText: document.getElementById("statusText"),
};

function getProductId(product) {
  if (product?.id) return String(product.id);
  if (product?._id?.$oid) return String(product._id.$oid);
  if (product?._id) return String(product._id);
  return "sin-id";
}

function getProductName(product) {
  return product?.nombre || product?.name || `Producto ${getProductId(product).slice(-4)}`;
}

function getCategory(product) {
  return (product?.categoria || "sin-categoria").toLowerCase();
}

/** Lee material según JSON de ejemplo: material_principal, metal, material_caja (snake o camel). */
function getMaterial(product) {
  const esp = product?.especificaciones || {};
  const raw =
    esp.materialPrincipal ??
    esp.material_principal ??
    esp.metal ??
    esp.materialCaja ??
    esp.material_caja ??
    "";
  const s = String(raw).trim();
  return s || "Sin especificar";
}

/** Stock: suma variantes[].stock; si no hay variantes, usa disponibilidad (DTO API). */
function getTotalStock(product) {
  const variantes = product?.variantes;
  if (Array.isArray(variantes) && variantes.length > 0) {
    return variantes.reduce((acc, v) => acc + Number(v?.stock || 0), 0);
  }
  const disponibilidad = product?.disponibilidad || {};
  const online = Number(disponibilidad?.online?.stock ?? disponibilidad?.Online?.stock ?? 0);
  const fisica = Object.entries(disponibilidad)
    .filter(([key]) => key.toLowerCase() !== "online")
    .reduce((acc, [, value]) => acc + Number(value?.stock || 0), 0);
  return online + fisica;
}

function getFiscalYear(date) {
  const month = date.getMonth() + 1;
  return month >= 3 ? date.getFullYear() : date.getFullYear() - 1;
}

function getPlatformCounts(plataforma, legacyChannel) {
  const counts = { online: 0, fisico: 0 };

  const addByLabel = (label, value = 1) => {
    const normalized = String(label || "")
      .toLowerCase()
      .normalize("NFD")
      .replace(/[\u0300-\u036f]/g, "");
    const amount = Number(value || 0);
    if (!Number.isFinite(amount) || amount <= 0) return;
    if (normalized.includes("online")) {
      counts.online += amount;
      return;
    }
    if (normalized.includes("fisico") || normalized.includes("fisica") || normalized.includes("tienda") || normalized.includes("offline")) {
      counts.fisico += amount;
    }
  };

  if (Array.isArray(plataforma)) {
    plataforma.forEach((entry) => {
      if (typeof entry === "string") {
        addByLabel(entry, 1);
      } else if (entry && typeof entry === "object") {
        Object.entries(entry).forEach(([key, value]) => addByLabel(key, value));
      }
    });
  } else if (plataforma && typeof plataforma === "object") {
    Object.entries(plataforma).forEach(([key, value]) => addByLabel(key, value));
  } else {
    addByLabel(plataforma || legacyChannel, 1);
  }

  return counts;
}

function normalizeSales(product) {
  //console.log(product);
  return (product?.ventas || [])
    .map((sale) => {
      //console.log(sale)
      const date = new Date(sale?.fecha);
      if (Number.isNaN(date.getTime())) return null;
      const plataforma = sale?.plataforma;
      const platformCounts = getPlatformCounts(plataforma, sale?.channel);

      return {
        date,
        year: date.getFullYear(),
        month: date.getMonth() + 1,
        fiscalYear: getFiscalYear(date),
        cantidad: Number(sale?.cantidad || 0),
        montoTotal: Number(sale?.montoTotal ?? sale?.monto_total ?? 0),
        plataforma: platformCounts,
        platformOnline: platformCounts.online,
        platformFisico: platformCounts.fisico,
      };
    })
    .filter(Boolean);
}

/** Anios calendario unicos que aparecen en ventas[].fecha (solo datos reales, sin inventar anios). */
function getCalendarYearsFromVentas(products) {

  const years = new Set();
  for (const product of products) {
    for (const sale of product?.ventas || []) {
      const raw = sale?.fecha;
      if (raw == null || raw === "") continue;
      const date = new Date(raw);
      if (Number.isNaN(date.getTime())) continue;
      years.add(date.getFullYear());
    }
  }
  return [...years].sort((a, b) => b - a);
}

function getSelectedFilterYear() {
  const y = parseInt(els.yearFilter.value, 10);
  return Number.isFinite(y) ? y : null;
}

function matchesTimeFilter(sale) {
  const mode = els.timeMode.value;
  const year = getSelectedFilterYear();
  if (year === null) return true;
  if (mode === "year") return sale.year === year;
  if (mode === "month") return sale.year === year && sale.month === Number(els.monthFilter.value);
  return sale.fiscalYear === year;
}

function previousPeriodMatcher(sale) {
  const mode = els.timeMode.value;
  const year = getSelectedFilterYear();
  if (year === null) return false;
  if (mode === "year") return sale.year === year - 1;
  if (mode === "month") return sale.year === year - 1 && sale.month === Number(els.monthFilter.value);
  return sale.fiscalYear === year - 1;
}

function refreshProductFilter(products) {
  const previous = els.productFilter.value;
  const options = [`<option value="all">Todos</option>`];
  products.forEach((product) => {
    options.push(`<option value="${getProductId(product)}">${getProductName(product)}</option>`);
  });
  els.productFilter.innerHTML = options.join("");
  if ([...els.productFilter.options].some((opt) => opt.value === previous)) {
    els.productFilter.value = previous;
  }
}

function renderList(targetId, items, emptyLabel, formatter) {
  const target = document.getElementById(targetId);
  if (!items.length) {
    target.innerHTML = `<li class="text-zinc-500">${emptyLabel}</li>`;
    return;
  }
  target.innerHTML = items.map(formatter).join("");
}

function upsertChart(key, type, ctxId, data, options = {}) {
  if (charts[key]) charts[key].destroy();
  charts[key] = new Chart(document.getElementById(ctxId), {
    type,
    data,
    options: {
      responsive: true,
      plugins: { legend: { labels: { color: "#d4d4d8" } } },
      scales: {
        x: { ticks: { color: "#a1a1aa" }, grid: { color: "rgba(161,161,170,0.1)" } },
        y: { ticks: { color: "#a1a1aa" }, grid: { color: "rgba(161,161,170,0.1)" } },
      },
      ...options,
    },
  });
}

function computeAndRender() {
  const selectedCategory = els.categoryFilter.value;
  const selectedProductId = els.productFilter.value;
  const selectedMaterial = els.materialFilter.value;

  let products = allProducts.filter((product) => {
    const byCategory = selectedCategory === "all" || getCategory(product) === selectedCategory;
    const byProduct = selectedProductId === "all" || getProductId(product) === selectedProductId;
    const byMaterial = selectedMaterial === "all" || getMaterial(product).toLowerCase() === selectedMaterial;
    return byCategory && byProduct && byMaterial;
  });

  refreshProductFilter(products);
  if (els.productFilter.value !== "all") {
    products = products.filter((p) => getProductId(p) === els.productFilter.value);
  }

  const productStats = products.map((product) => {
    const sales = normalizeSales(product);
    const currentSales = sales.filter(matchesTimeFilter);
    const previousSales = sales.filter(previousPeriodMatcher);
    const qtyCurrent = currentSales.reduce((acc, sale) => acc + sale.cantidad, 0);
    const amountCurrent = currentSales.reduce((acc, sale) => acc + sale.montoTotal, 0);
    const qtyPrevious = previousSales.reduce((acc, sale) => acc + sale.cantidad, 0);
    const amountPrevious = previousSales.reduce((acc, sale) => acc + sale.montoTotal, 0);
    const meta = product?.metadata || {};
    const createdRaw = meta.creadoEn ?? meta.creado_en;
    return {
      product,
      name: getProductName(product),
      material: getMaterial(product),
      stock: getTotalStock(product),
      views: Number(meta.vistas ?? meta.Vistas ?? 0),
      purchases: Number(meta.compras ?? meta.Compras ?? 0),
      hasCert: Array.isArray(product?.certificaciones) && product.certificaciones.length > 0,
      createdAt: createdRaw ? new Date(createdRaw) : null,
      qtyCurrent,
      amountCurrent,
      qtyPrevious,
      amountPrevious,
      salesCurrent: currentSales,
    };
  });

  const totalRevenue = productStats.reduce((acc, p) => acc + p.amountCurrent, 0);
  const totalUnits = productStats.reduce((acc, p) => acc + p.qtyCurrent, 0);
  const stockValue = productStats.reduce(
    (acc, p) => acc + Number(p.product?.precioActual || 0) * p.stock,
    0
  );

  document.getElementById("kpiRevenue").textContent = formatCurrency(totalRevenue);
  document.getElementById("kpiTicket").textContent = formatCurrency(totalUnits ? totalRevenue / totalUnits : 0);
  document.getElementById("kpiStockValue").textContent = formatCurrency(stockValue);

  const materialMap = {};
  productStats.forEach((entry) => {
    materialMap[entry.material] = (materialMap[entry.material] || 0) + entry.amountCurrent;
  });
  upsertChart(
    "materialPie",
    "pie",
    "materialPieChart",
    {
      labels: Object.keys(materialMap),
      datasets: [
        {
          data: Object.values(materialMap),
          backgroundColor: ["#C08081", "#C09D80", "#B8B080", "#8DB080", "#80B0A0", "#8098B0", "#9180B0", "#B080A4", "#B09C80", "#A68D61"],
        },
      ],
    },
    { scales: {} }
  );

  const currentSeries = new Array(12).fill(0);
  const previousSeries = new Array(12).fill(0);
  productStats.forEach((entry) => {
    entry.salesCurrent.forEach((sale) => {
      currentSeries[sale.month - 1] += sale.montoTotal;
    });
    normalizeSales(entry.product)
      .filter(previousPeriodMatcher)
      .forEach((sale) => {
        previousSeries[sale.month - 1] += sale.montoTotal;
      });
  });
  upsertChart("performanceLine", "line", "performanceLineChart", {
    labels: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
    datasets: [
      {
        label: "Periodo actual",
        data: currentSeries,
        borderColor: "#d4af37",
        backgroundColor: "rgba(212,175,55,0.25)",
        tension: 0.3,
      },
      {
        label: "Periodo anterior",
        data: previousSeries,
        borderColor: "#71717a",
        backgroundColor: "rgba(113,113,122,0.25)",
        tension: 0.3,
      },
    ],
  });

  const byConversion = [...productStats].sort((a, b) => b.views - a.views).slice(0, 8);
  upsertChart("conversionBar", "bar", "conversionBarChart", {
    labels: byConversion.map((p) => p.name.slice(0, 12)),
    datasets: [
      { label: "Vistas", data: byConversion.map((p) => p.views), backgroundColor: "#52525b" },
      { label: "Compras", data: byConversion.map((p) => p.purchases), backgroundColor: "#d4af37" },
    ],
  });

  const channelTotals = { Online: 0, "Tiendas fisicas": 0 };
  productStats.forEach((entry) => {
    entry.salesCurrent.forEach((sale) => {
      //console.log(sale)
      const onlineCount = Number(sale.platformOnline || 0);
      const fisicoCount = Number(sale.platformFisico || 0);
      const totalCount = onlineCount + fisicoCount;

      if (totalCount > 0) {
        channelTotals.Online += sale.montoTotal * (onlineCount / totalCount);
        channelTotals["Tiendas fisicas"] += sale.montoTotal * (fisicoCount / totalCount);
        return;
      }

      const onlineStock = Number(entry.product?.disponibilidad?.online?.stock || 0);
      const totalStock = entry.stock || 1;
      const onlineShare = totalStock ? onlineStock / totalStock : 0;
      channelTotals.Online += sale.montoTotal * onlineShare;
      channelTotals["Tiendas fisicas"] += sale.montoTotal * (1 - onlineShare);
    });
  });
  upsertChart("channelBar", "bar", "channelBarChart", {
    labels: Object.keys(channelTotals),
    datasets: [{ label: "Ingresos", data: Object.values(channelTotals), backgroundColor: ["#d4af37", "#3f3f46"] }],
  });

  const ranking = [...productStats].sort((a, b) => b.qtyCurrent - a.qtyCurrent);
  const top = ranking.slice(0, 5);
  const bottom = [...ranking].reverse().slice(0, 5);

  renderList(
    "topProductsList",
    top,
    "Sin ventas en el periodo",
    (item, index) =>
      `<li class="flex justify-between border-b border-zinc-800 pb-1"><span>${index + 1}. ${item.name}</span><span>${item.qtyCurrent} u.</span></li>`
  );
  renderList(
    "bottomProductsList",
    bottom,
    "Sin ventas en el periodo",
    (item, index) =>
      `<li class="flex justify-between border-b border-zinc-800 pb-1"><span>${index + 1}. ${item.name}</span><span>${item.qtyCurrent} u.</span></li>`
  );

  const lowStock = productStats.filter((p) => p.stock < 3);
  renderList(
    "lowStockList",
    lowStock,
    "Sin alertas criticas",
    (item) =>
      `<li class="flex justify-between border-b border-zinc-800 pb-1"><span>${item.name}</span><span class="text-amber-300">${item.stock} u.</span></li>`
  );

  const now = new Date();
  const aging = productStats
    .filter((p) => p.createdAt instanceof Date && !Number.isNaN(p.createdAt.getTime()))
    .map((p) => {
      const ageDays = Math.floor((now.getTime() - p.createdAt.getTime()) / (1000 * 60 * 60 * 24));
      return { ...p, ageDays, agingScore: ageDays * Math.max(p.stock, 1) };
    })
    .filter((p) => p.ageDays >= 180 && p.stock >= 3)
    .sort((a, b) => b.agingScore - a.agingScore)
    .slice(0, 5);
  renderList(
    "agingList",
    aging,
    "Sin productos con envejecimiento critico",
    (item) =>
      `<li class="flex justify-between border-b border-zinc-800 pb-1"><span>${item.name}</span><span>${item.ageDays} dias / ${item.stock} u.</span></li>`
  );

  const certYes = productStats.filter((p) => p.hasCert);
  const certNo = productStats.filter((p) => !p.hasCert);
  const certYesRevenue = certYes.reduce((acc, p) => acc + p.amountCurrent, 0);
  const certNoRevenue = certNo.reduce((acc, p) => acc + p.amountCurrent, 0);
  document.getElementById("certImpactBox").innerHTML = `
        <p>Con certificacion: <strong>${certYes.length}</strong> productos, ingresos <strong>${formatCurrency(certYesRevenue)}</strong>.</p>
        <p class="mt-2">Sin certificacion: <strong>${certNo.length}</strong> productos, ingresos <strong>${formatCurrency(certNoRevenue)}</strong>.</p>
        <p class="mt-2 text-zinc-400">Ratio ingresos certificados/no certificados: <strong>${
          certNoRevenue > 0 ? (certYesRevenue / certNoRevenue).toFixed(2) : "N/A"
        }</strong>.</p>
      `;

  const previousRevenue = productStats.reduce((acc, p) => acc + p.amountPrevious, 0);
  const growth = previousRevenue > 0 ? ((totalRevenue - previousRevenue) / previousRevenue) * 100 : 0;
  const summaryItems = [
    `Cobertura analizada: ${products.length} productos filtrados`,
    `Variacion ingresos vs periodo anterior: ${growth.toFixed(1)}%`,
    `Canal dominante: ${
      channelTotals.Online >= channelTotals["Tiendas fisicas"] ? "Online" : "Tiendas fisicas"
    }`,
    `Productos con baja conversion (vistas > compras x10): ${
      productStats.filter((p) => p.views > p.purchases * 10).length
    }`,
  ];
  document.getElementById("summaryList").innerHTML = summaryItems
    .map((text) => `<li class="border-b border-zinc-800 pb-1">${text}</li>`)
    .join("");

  els.statusText.textContent = `${products.length} productos evaluados en tiempo real`;
}

function buildInitialFilters(products) {
  const categories = new Set();
  const materials = new Set();

  products.forEach((product) => {
    categories.add(getCategory(product));
    materials.add(getMaterial(product).toLowerCase());
  });

  const availableYears = getCalendarYearsFromVentas(products);

  if (availableYears.length === 0) {
    els.yearFilter.innerHTML = `<option value="">Sin fechas en ventas</option>`;
  } else {
    els.yearFilter.innerHTML = availableYears.map((y) => `<option value="${y}">${y}</option>`).join("");
  }
  els.categoryFilter.innerHTML =
    `<option value="all">Todas</option>` + [...categories].sort().map((cat) => `<option value="${cat}">${cat}</option>`).join("");
  els.materialFilter.innerHTML =
    `<option value="all">Todos</option>` +
    [...materials].sort().map((mat) => `<option value="${mat}">${mat}</option>`).join("");
  refreshProductFilter(products);
}

function bindEvents() {
  [els.timeMode, els.yearFilter, els.monthFilter, els.categoryFilter, els.productFilter, els.materialFilter].forEach(
    (el) => {
      el.addEventListener("change", () => {
        const isMonth = els.timeMode.value === "month";
        els.monthFilterWrap.classList.toggle("hidden", !isMonth);
        els.monthFilterWrap.classList.toggle("flex", isMonth);
        computeAndRender();
      });
    }
  );
}

async function init() {
  try {
    allProducts = await getProductos();
    if (!Array.isArray(allProducts)) {
      throw new Error("La API no devolvio un array de productos");
    }
    buildInitialFilters(allProducts);
    bindEvents();
    computeAndRender();
  } catch (error) {
    console.error(error);
    els.statusText.textContent = "No se pudo cargar la informacion del backend";
  }
}

init();
