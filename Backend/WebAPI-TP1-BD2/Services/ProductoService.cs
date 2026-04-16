using MongoDB.Bson;
using WebAPI_TP1_BD2.DTOs;
using WebAPI_TP1_BD2.Models;
using WebAPI_TP1_BD2.Repositories;

namespace WebAPI_TP1_BD2.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductoResponseDto>> GetAllAsync()
    {
        var productos = await _repository.GetAllAsync();
        return productos.Select(MapToResponse).ToList();
    }

    public async Task<ProductoResponseDto?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return null;

        var producto = await _repository.GetByIdAsync(id);
        return producto is null ? null : MapToResponse(producto);
    }

    public async Task<ProductoResponseDto> CreateAsync(CreateProductoDto dto)
    {
        var producto = MapToModel(dto);
        var created = await _repository.CreateAsync(producto);
        return MapToResponse(created);
    }

    public async Task<ProductoResponseDto?> UpdateAsync(string id, UpdateProductoDto dto)
    {
        if (!ObjectId.TryParse(id, out _))
            return null;

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return null;

        UpdateModelFromDto(existing, dto);
        var updated = await _repository.UpdateAsync(id, existing);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        return await _repository.DeleteAsync(id);
    }

    private static ProductoResponseDto MapToResponse(Producto producto)
    {
        return new ProductoResponseDto
        {
            Id = producto.Id!,
            Nombre = producto.Nombre,
            Marca = producto.Marca,
            Categoria = producto.Categoria,
            Descripcion = producto.Descripcion,
            PrecioActual = producto.PrecioActual,
            Especificaciones = MapEspecificaciones(producto.Especificaciones),
            Variantes = producto.Variantes?.Select(MapVariante).ToList(),
            Ventas = producto.Ventas?.Select(MapVenta).ToList(),
            HistorialPrecios = producto.HistorialPrecios?.Select(MapPrecioHistoria).ToList(),
            Reviews = producto.Reviews?.Select(MapReview).ToList(),
            Disponibilidad = MapDisponibilidad(producto.Disponibilidad),
            Colecciones = producto.Colecciones,
            ProductosRelacionados = producto.ProductosRelacionados?.Select(MapProductoRelacionado).ToList(),
            Metadata = MapMetadata(producto.Metadata),
            Certificaciones = producto.Certificaciones?.Select(MapCertificacion).ToList(),
            CertificadosPiedras = producto.CertificadosPiedras?.Select(MapCertificadoPiedra).ToList()
        };
    }

    private static Producto MapToModel(CreateProductoDto dto)
    {
        return new Producto
        {
            Nombre = dto.Nombre,
            Marca = dto.Marca,
            Categoria = dto.Categoria,
            Descripcion = dto.Descripcion,
            PrecioActual = dto.PrecioActual,
            Especificaciones = MapEspecificacionesDto(dto.Especificaciones),
            Variantes = dto.Variantes?.Select(MapVarianteDto).ToList(),
            Ventas = dto.Ventas?.Select(MapVentaDto).ToList(),
            HistorialPrecios = dto.HistorialPrecios?.Select(MapPrecioHistoriaDto).ToList(),
            Reviews = dto.Reviews?.Select(MapReviewDto).ToList(),
            Disponibilidad = MapDisponibilidadDto(dto.Disponibilidad),
            Colecciones = dto.Colecciones,
            ProductosRelacionados = dto.ProductosRelacionados?.Select(MapProductoRelacionadoDto).ToList(),
            Metadata = MapMetadataDto(dto.Metadata),
            Certificaciones = dto.Certificaciones?.Select(MapCertificacionDto).ToList(),
            CertificadosPiedras = dto.CertificadosPiedras?.Select(MapCertificadoPiedraDto).ToList()
        };
    }

    private static void UpdateModelFromDto(Producto producto, UpdateProductoDto dto)
    {
        if (dto.Nombre is not null) producto.Nombre = dto.Nombre;
        if (dto.Marca is not null) producto.Marca = dto.Marca;
        if (dto.Categoria is not null) producto.Categoria = dto.Categoria;
        if (dto.Descripcion is not null) producto.Descripcion = dto.Descripcion;
        if (dto.PrecioActual.HasValue) producto.PrecioActual = dto.PrecioActual.Value;
        if (dto.Especificaciones is not null) producto.Especificaciones = MapEspecificacionesDto(dto.Especificaciones);
        if (dto.Variantes is not null) producto.Variantes = dto.Variantes.Select(MapVarianteDto).ToList();
        if (dto.Ventas is not null) producto.Ventas = dto.Ventas.Select(MapVentaDto).ToList();
        if (dto.HistorialPrecios is not null) producto.HistorialPrecios = dto.HistorialPrecios.Select(MapPrecioHistoriaDto).ToList();
        if (dto.Reviews is not null) producto.Reviews = dto.Reviews.Select(MapReviewDto).ToList();
        if (dto.Disponibilidad is not null) producto.Disponibilidad = MapDisponibilidadDto(dto.Disponibilidad);
        if (dto.Colecciones is not null) producto.Colecciones = dto.Colecciones;
        if (dto.ProductosRelacionados is not null) producto.ProductosRelacionados = dto.ProductosRelacionados.Select(MapProductoRelacionadoDto).ToList();
        if (dto.Metadata is not null) producto.Metadata = MapMetadataDto(dto.Metadata);
        if (dto.Certificaciones is not null) producto.Certificaciones = dto.Certificaciones.Select(MapCertificacionDto).ToList();
        if (dto.CertificadosPiedras is not null) producto.CertificadosPiedras = dto.CertificadosPiedras.Select(MapCertificadoPiedraDto).ToList();
    }

    private static EspecificacionesDto? MapEspecificaciones(Especificaciones? esp)
    {
        if (esp is null) return null;
        return new EspecificacionesDto
        {
            TipoReloj = esp.TipoReloj,
            TipoAnillo = esp.TipoAnillo,
            TipoBrazalete = esp.TipoBrazalete,
            MaterialPrincipal = esp.MaterialPrincipal,
            Estilo = esp.Estilo,
            Cronometro = esp.Cronometro,
            Cronografo = esp.Cronografo,
            DiametroMm = esp.DiametroMm,
            GrosorMm = esp.GrosorMm,
            MaterialCaja = esp.MaterialCaja,
            MaterialCristal = esp.MaterialCristal,
            ResistenciaAguaM = esp.ResistenciaAguaM,
            Calibre = esp.Calibre,
            ReservaMarchaHoras = esp.ReservaMarchaHoras,
            Funciones = esp.Funciones,
            PesoG = esp.PesoG,
            HeliumEscapeValve = esp.HeliumEscapeValve,
            RotulaUnidireccional = esp.RotulaUnidireccional,
            Certificacion = esp.Certificacion,
            PiedraPrincipal = MapPiedraPrincipal(esp.PiedraPrincipal),
            Metal = esp.Metal,
            PerlaPrincipal = MapPerlaPrincipal(esp.PerlaPrincipal),
            PerlasSecundarias = esp.PerlasSecundarias?.Select(MapPerlaSecundaria).ToList()!,
            EstiramientoCirc = esp.EstiramientoCirc,
            PesoAproxG = esp.PesoAproxG,
            Acabado = esp.Acabado,
            Mantenimiento = esp.Mantenimiento
        };
    }

    private static PiedraPrincipalDto? MapPiedraPrincipal(PiedraPrincipal? piedra)
        => piedra is null ? null : new PiedraPrincipalDto { Tipo = piedra.Tipo, Quilates = piedra.Quilates, Color = piedra.Color };

    private static PerlaPrincipalDto? MapPerlaPrincipal(PerlaPrincipal? perla)
        => perla is null ? null : new PerlaPrincipalDto { Tipo = perla.Tipo, DiametroMm = perla.DiametroMm, Color = perla.Color, Brillo = perla.Brillo, Origen = perla.Origen };

    private static PerlaSecundariaDto? MapPerlaSecundaria(PerlaSecundaria perla)
        => new() { Tipo = perla.Tipo, Cantidad = perla.Cantidad, DiametroMm = perla.DiametroMm, Brillo = perla.Brillo, Material = perla.Material, Diseno = perla.Diseno };

    private static VentaDto MapVenta(Venta venta)
        => new()
        {
            Fecha = venta.Fecha,
            Cantidad = venta.Cantidad,
            MontoTotal = venta.MontoTotal,
            Plataforma = MapPlataformaVenta(venta.Plataforma)
        };

    private static Venta MapVentaDto(VentaDto dto)
        => new()
        {
            Fecha = dto.Fecha,
            Cantidad = dto.Cantidad,
            MontoTotal = dto.MontoTotal,
            Plataforma = MapPlataformaVentaDto(dto.Plataforma)
        };

    private static PlataformaVentaDto? MapPlataformaVenta(PlataformaVenta? plataforma)
        => plataforma is null ? null : new PlataformaVentaDto
        {
            Online = plataforma.Online,
            Fisico = plataforma.Fisico
        };

    private static PlataformaVenta? MapPlataformaVentaDto(PlataformaVentaDto? dto)
        => dto is null ? null : new PlataformaVenta
        {
            Online = dto.Online,
            Fisico = dto.Fisico
        };

    private static VarianteDto MapVariante(Variante variante)
        => new()
        {
            id = variante.id,
            ColorCaja = variante.ColorCaja,
            ColorCintura = variante.ColorCintura,
            Stock = variante.Stock,
            PrecioVariante = variante.PrecioVariante,
            Imagen = variante.Imagen,
            ColorEsfera = variante.ColorEsfera,
            Correa = variante.Correa,
            TalleAnillo = variante.TalleAnillo,
            ColorPerlas = variante.ColorPerlas,
            Distanciadores = variante.Distanciadores
        };

    private static ReviewDto MapReview(Review review)
        => new()
        {
            Id = review.Id,
            Usuario = review.Usuario,
            Calificacion = review.Calificacion,
            Titulo = review.Titulo,
            Contenido = review.Contenido,
            Fecha = review.Fecha,
            Util = review.Util
        };

    private static DisponibilidadDto? MapDisponibilidad(Disponibilidad? disp)
    {
        if (disp is null) return null;
        return new DisponibilidadDto
        {
            TiendaFisicaBuenosAires = MapTiendaFisica(disp.TiendaFisicaBuenosAires),
            TiendaFisicaCordoba = MapTiendaFisica(disp.TiendaFisicaCordoba),
            Online = MapDisponibilidadOnline(disp.Online)
        };
    }

    private static TiendaFisicaDto? MapTiendaFisica(TiendaFisica? tienda)
        => tienda is null ? null : new TiendaFisicaDto { Stock = tienda.Stock, Reservable = tienda.Reservable };

    private static DisponibilidadOnlineDto? MapDisponibilidadOnline(DisponibilidadOnline? online)
        => online is null ? null : new DisponibilidadOnlineDto { Stock = online.Stock, EnvioDias = online.EnvioDias, EnvioCosto = online.EnvioCosto };

    private static MetadataDto? MapMetadata(Metadata? metadata)
        => metadata is null ? null : new MetadataDto { CreadoEn = metadata.CreadoEn, ActualizadoEn = metadata.ActualizadoEn, Vistas = metadata.Vistas, Compras = metadata.Compras, Tags = metadata.Tags };

    private static PrecioHistoriaDto MapPrecioHistoria(PrecioHistoria ph)
        => new() { Fecha = ph.Fecha, Precio = ph.Precio, Razon = ph.Razon };

    private static ProductoRelacionadoDto MapProductoRelacionado(ProductoRelacionado pr)
        => new() { ProductoId = pr.ProductoId, Tipo = pr.Tipo, Nombre = pr.Nombre };

    private static Especificaciones MapEspecificacionesDto(EspecificacionesDto? dto)
    {
        if (dto is null) return new Especificaciones();
        return new Especificaciones
        {
            TipoReloj = dto.TipoReloj,
            TipoAnillo = dto.TipoAnillo,
            TipoBrazalete = dto.TipoBrazalete,
            MaterialPrincipal = dto.MaterialPrincipal,
            Estilo = dto.Estilo,
            Cronometro = dto.Cronometro,
            Cronografo = dto.Cronografo,
            DiametroMm = dto.DiametroMm,
            GrosorMm = dto.GrosorMm,
            MaterialCaja = dto.MaterialCaja,
            MaterialCristal = dto.MaterialCristal,
            ResistenciaAguaM = dto.ResistenciaAguaM,
            Calibre = dto.Calibre,
            ReservaMarchaHoras = dto.ReservaMarchaHoras,
            Funciones = dto.Funciones,
            PesoG = dto.PesoG,
            HeliumEscapeValve = dto.HeliumEscapeValve,
            RotulaUnidireccional = dto.RotulaUnidireccional,
            Certificacion = dto.Certificacion,
            PiedraPrincipal = MapPiedraPrincipalDto(dto.PiedraPrincipal),
            Metal = dto.Metal,
            PerlaPrincipal = MapPerlaPrincipalDto(dto.PerlaPrincipal),
            PerlasSecundarias = dto.PerlasSecundarias?.Select(MapPerlaSecundariaDto).ToList(),
            EstiramientoCirc = dto.EstiramientoCirc,
            PesoAproxG = dto.PesoAproxG,
            Acabado = dto.Acabado,
            Mantenimiento = dto.Mantenimiento
        };
    }

    private static PiedraPrincipal? MapPiedraPrincipalDto(PiedraPrincipalDto? dto)
        => dto is null ? null : new PiedraPrincipal { Tipo = dto.Tipo, Quilates = dto.Quilates, Color = dto.Color };

    private static PerlaPrincipal? MapPerlaPrincipalDto(PerlaPrincipalDto? dto)
        => dto is null ? null : new PerlaPrincipal { Tipo = dto.Tipo, DiametroMm = dto.DiametroMm, Color = dto.Color, Brillo = dto.Brillo, Origen = dto.Origen };

    private static PerlaSecundaria MapPerlaSecundariaDto(PerlaSecundariaDto dto)
        => new() { Tipo = dto.Tipo, Cantidad = dto.Cantidad, DiametroMm = dto.DiametroMm, Brillo = dto.Brillo, Material = dto.Material, Diseno = dto.Diseno };

    private static Variante MapVarianteDto(VarianteDto dto)
        => new()
        {
            id = dto.id,
            ColorCaja = dto.ColorCaja,
            ColorCintura = dto.ColorCintura,
            Stock = dto.Stock,
            PrecioVariante = dto.PrecioVariante,
            Imagen = dto.Imagen,
            ColorEsfera = dto.ColorEsfera,
            Correa = dto.Correa,
            TalleAnillo = dto.TalleAnillo,
            ColorPerlas = dto.ColorPerlas,
            Distanciadores = dto.Distanciadores
        };

    private static Review MapReviewDto(ReviewDto dto)
        => new()
        {
            Id = dto.Id,
            Usuario = dto.Usuario,
            Calificacion = dto.Calificacion,
            Titulo = dto.Titulo,
            Contenido = dto.Contenido,
            Fecha = dto.Fecha,
            Util = dto.Util
        };

    private static Disponibilidad MapDisponibilidadDto(DisponibilidadDto? dto)
    {
        if (dto is null) return new Disponibilidad();
        return new Disponibilidad
        {
            TiendaFisicaBuenosAires = MapTiendaFisicaDto(dto.TiendaFisicaBuenosAires),
            TiendaFisicaCordoba = MapTiendaFisicaDto(dto.TiendaFisicaCordoba),
            Online = MapDisponibilidadOnlineDto(dto.Online)
        };
    }

    private static TiendaFisica? MapTiendaFisicaDto(TiendaFisicaDto? dto)
        => dto is null ? null : new TiendaFisica { Stock = dto.Stock, Reservable = dto.Reservable };

    private static DisponibilidadOnline? MapDisponibilidadOnlineDto(DisponibilidadOnlineDto? dto)
        => dto is null ? null : new DisponibilidadOnline { Stock = dto.Stock, EnvioDias = dto.EnvioDias, EnvioCosto = dto.EnvioCosto };

    private static Metadata MapMetadataDto(MetadataDto? dto)
    {
        if (dto is null) return new Metadata();
        return new Metadata { CreadoEn = dto.CreadoEn, ActualizadoEn = dto.ActualizadoEn, Vistas = dto.Vistas, Compras = dto.Compras, Tags = dto.Tags };
    }

    private static PrecioHistoria MapPrecioHistoriaDto(PrecioHistoriaDto dto)
        => new() { Fecha = dto.Fecha, Precio = dto.Precio, Razon = dto.Razon };

    private static ProductoRelacionado MapProductoRelacionadoDto(ProductoRelacionadoDto dto)
        => new() { ProductoId = dto.ProductoId, Tipo = dto.Tipo, Nombre = dto.Nombre };

    private static CertificacionDto MapCertificacion(Certificacion cert)
        => new()
        {
            Tipo = cert.Tipo,
            Emisora = cert.Emisora,
            Numero = cert.Numero,
            FechaEmision = cert.FechaEmision,
            Vigente = cert.Vigente,
            Comentario = cert.Comentario,
            DuracionAnos = cert.DuracionAnos,
            Cobertura = cert.Cobertura,
            Detalles = cert.Detalles
        };

    private static CertificadoPiedraDto MapCertificadoPiedra(CertificadoPiedra cert)
        => new()
        {
            NumeroCertificado = cert.NumeroCertificado,
            Laboratorio = cert.Laboratorio,
            PiedraId = cert.PiedraId,
            TipoCertificacion = cert.TipoCertificacion,
            Fecha = cert.Fecha,
            Detalles = MapDetallesCertificado(cert.Detalles)
        };

    private static DetallesCertificadoDto? MapDetallesCertificado(DetallesCertificado? detalles)
        => detalles is null ? null : new DetallesCertificadoDto
        {
            Tipo = detalles.Tipo,
            Diametro = detalles.Diametro,
            Color = detalles.Color,
            Calidad = detalles.Calidad,
            Tratamiento = detalles.Tratamiento
        };

    private static Certificacion MapCertificacionDto(CertificacionDto dto)
        => new()
        {
            Tipo = dto.Tipo,
            Emisora = dto.Emisora,
            Numero = dto.Numero,
            FechaEmision = dto.FechaEmision,
            Vigente = dto.Vigente,
            Comentario = dto.Comentario,
            DuracionAnos = dto.DuracionAnos,
            Cobertura = dto.Cobertura,
            Detalles = dto.Detalles
        };

    private static CertificadoPiedra MapCertificadoPiedraDto(CertificadoPiedraDto dto)
        => new()
        {
            NumeroCertificado = dto.NumeroCertificado,
            Laboratorio = dto.Laboratorio,
            PiedraId = dto.PiedraId,
            TipoCertificacion = dto.TipoCertificacion,
            Fecha = dto.Fecha,
            Detalles = MapDetallesCertificadoDto(dto.Detalles)
        };

    private static DetallesCertificado? MapDetallesCertificadoDto(DetallesCertificadoDto? dto)
        => dto is null ? null : new DetallesCertificado
        {
            Tipo = dto.Tipo,
            Diametro = dto.Diametro,
            Color = dto.Color,
            Calidad = dto.Calidad,
            Tratamiento = dto.Tratamiento
        };
}
