using System.ComponentModel.DataAnnotations;

namespace WebAPI_TP1_BD2.DTOs;

public class UpdateProductoDto
{
    [StringLength(200)]
    public string? Nombre { get; set; }

    [StringLength(100)]
    public string? Marca { get; set; }

    [StringLength(50)]
    public string? Categoria { get; set; }

    [StringLength(2000)]
    public string? Descripcion { get; set; }

    [Range(0, long.MaxValue)]
    public long? PrecioActual { get; set; }

    public EspecificacionesDto? Especificaciones { get; set; }
    public List<VarianteDto>? Variantes { get; set; }
    public List<PrecioHistoriaDto>? HistorialPrecios { get; set; }
    public List<ReviewDto>? Reviews { get; set; }
    public DisponibilidadDto? Disponibilidad { get; set; }
    public List<string>? Colecciones { get; set; }
    public List<ProductoRelacionadoDto>? ProductosRelacionados { get; set; }
    public MetadataDto? Metadata { get; set; }
    public List<CertificacionDto>? Certificaciones { get; set; }
    public List<CertificadoPiedraDto>? CertificadosPiedras { get; set; }
}
