namespace WebAPI_TP1_BD2.DTOs;

public class ProductoResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public long PrecioActual { get; set; }
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
