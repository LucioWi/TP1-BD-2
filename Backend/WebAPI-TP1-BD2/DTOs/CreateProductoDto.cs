using System.ComponentModel.DataAnnotations;

namespace WebAPI_TP1_BD2.DTOs;

public class CreateProductoDto
{
    [Required]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Marca { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Categoria { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Descripcion { get; set; }

    [Required]
    [Range(0, long.MaxValue)]
    public long PrecioActual { get; set; }

    public EspecificacionesDto? Especificaciones { get; set; }
    public List<VarianteDto>? Variantes { get; set; }
    public List<VentaDto>? Ventas { get; set; }
    public List<PrecioHistoriaDto>? HistorialPrecios { get; set; }
    public List<ReviewDto>? Reviews { get; set; }
    public DisponibilidadDto? Disponibilidad { get; set; }
    public List<string>? Colecciones { get; set; }
    public List<ProductoRelacionadoDto>? ProductosRelacionados { get; set; }
    public MetadataDto? Metadata { get; set; }
    public List<CertificacionDto>? Certificaciones { get; set; }
    public List<CertificadoPiedraDto>? CertificadosPiedras { get; set; }
}

public class EspecificacionesDto
{
    public string? TipoReloj { get; set; }
    public string? TipoAnillo { get; set; }
    public string? TipoBrazalete { get; set; }
    public string? MaterialPrincipal { get; set; }
    public string? Estilo { get; set; }
    public bool? Cronometro { get; set; }
    public bool? Cronografo { get; set; }
    public double? DiametroMm { get; set; }
    public double? GrosorMm { get; set; }
    public string? MaterialCaja { get; set; }
    public string? MaterialCristal { get; set; }
    public int? ResistenciaAguaM { get; set; }
    public string? Calibre { get; set; }
    public int? ReservaMarchaHoras { get; set; }
    public List<string>? Funciones { get; set; }
    public double? PesoG { get; set; }
    public bool? HeliumEscapeValve { get; set; }
    public string? RotulaUnidireccional { get; set; }
    public string? Certificacion { get; set; }
    public PiedraPrincipalDto? PiedraPrincipal { get; set; }
    public string? Metal { get; set; }
    public PerlaPrincipalDto? PerlaPrincipal { get; set; }
    public List<PerlaSecundariaDto>? PerlasSecundarias { get; set; }
    public string? EstiramientoCirc { get; set; }
    public double? PesoAproxG { get; set; }
    public string? Acabado { get; set; }
    public string? Mantenimiento { get; set; }
}

public class PiedraPrincipalDto
{
    public string? Tipo { get; set; }
    public double? Quilates { get; set; }
    public string? Color { get; set; }
}

public class PerlaPrincipalDto
{
    public string? Tipo { get; set; }
    public double? DiametroMm { get; set; }
    public string? Color { get; set; }
    public string? Brillo { get; set; }
    public string? Origen { get; set; }
}

public class PerlaSecundariaDto
{
    public string? Tipo { get; set; }
    public int? Cantidad { get; set; }
    public double? DiametroMm { get; set; }
    public string? Brillo { get; set; }
    public string? Material { get; set; }
    public string? Diseno { get; set; }
}

public class VarianteDto
{
    public string id { get; set; } = string.Empty;
    public string? ColorCaja { get; set; }
    public string? ColorCintura { get; set; }
    public int Stock { get; set; }
    public long PrecioVariante { get; set; }
    public string? Imagen { get; set; }
    public string? ColorEsfera { get; set; }
    public string? Correa { get; set; }
    public int? TalleAnillo { get; set; }
    public string? ColorPerlas { get; set; }
    public string? Distanciadores { get; set; }
}

public class ReviewDto
{
    public string? Id { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public double Calificacion { get; set; }
    public string? Titulo { get; set; }
    public string? Contenido { get; set; }
    public string? Fecha { get; set; }
    public int? Util { get; set; }
}

public class DisponibilidadDto
{
    public TiendaFisicaDto? TiendaFisicaBuenosAires { get; set; }
    public TiendaFisicaDto? TiendaFisicaCordoba { get; set; }
    public DisponibilidadOnlineDto? Online { get; set; }
}

public class TiendaFisicaDto
{
    public int Stock { get; set; }
    public bool Reservable { get; set; }
}

public class DisponibilidadOnlineDto
{
    public int Stock { get; set; }
    public int? EnvioDias { get; set; }
    public long? EnvioCosto { get; set; }
}

public class MetadataDto
{
    public string? CreadoEn { get; set; }
    public string? ActualizadoEn { get; set; }
    public int? Vistas { get; set; }
    public int? Compras { get; set; }
    public List<string>? Tags { get; set; }
}

public class PrecioHistoriaDto
{
    public string Fecha { get; set; } = string.Empty;
    public long Precio { get; set; }
    public string? Razon { get; set; }
}

public class ProductoRelacionadoDto
{
    public string ProductoId { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string? Nombre { get; set; }
}

public class CertificacionDto
{
    public string Tipo { get; set; } = string.Empty;
    public string? Emisora { get; set; }
    public string? Numero { get; set; }
    public string? FechaEmision { get; set; }
    public bool? Vigente { get; set; }
    public string? Comentario { get; set; }
    public int? DuracionAnos { get; set; }
    public List<string>? Cobertura { get; set; }
    public string? Detalles { get; set; }
}

public class CertificadoPiedraDto
{
    public string? NumeroCertificado { get; set; }
    public string? Laboratorio { get; set; }
    public string? PiedraId { get; set; }
    public string? TipoCertificacion { get; set; }
    public string? Fecha { get; set; }
    public DetallesCertificadoDto? Detalles { get; set; }
}

public class DetallesCertificadoDto
{
    public string? Tipo { get; set; }
    public string? Diametro { get; set; }
    public string? Color { get; set; }
    public string? Calidad { get; set; }
    public string? Tratamiento { get; set; }
}
