using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

public class Producto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("marca")]
    public string Marca { get; set; } = string.Empty;

    [BsonElement("categoria")]
    public string Categoria { get; set; } = string.Empty;

    [BsonElement("descripcion")]
    public string? Descripcion { get; set; }

    [BsonElement("precioActual")]
    public long PrecioActual { get; set; }

    [BsonElement("especificaciones")]
    public Especificaciones? Especificaciones { get; set; }

    [BsonElement("variantes")]
    public List<Variante>? Variantes { get; set; }

    [BsonElement("historialPrecios")]
    public List<PrecioHistoria>? HistorialPrecios { get; set; }

    [BsonElement("reviews")]
    public List<Review>? Reviews { get; set; }

    [BsonElement("disponibilidad")]
    public Disponibilidad? Disponibilidad { get; set; }

    [BsonElement("colecciones")]
    public List<string>? Colecciones { get; set; }

    [BsonElement("productosRelacionados")]
    public List<ProductoRelacionado>? ProductosRelacionados { get; set; }

    [BsonElement("metadata")]
    public Metadata? Metadata { get; set; }

    [BsonElement("certificaciones")]
    public List<Certificacion>? Certificaciones { get; set; }

    [BsonElement("certificadosPiedras")]
    public List<CertificadoPiedra>? CertificadosPiedras { get; set; }
}

public class ProductoRelacionado
{
    [BsonElement("productoid")]
    public string ProductoId { get; set; } = string.Empty;

    [BsonElement("tipo")]
    public string? Tipo { get; set; }

    [BsonElement("nombre")]
    public string? Nombre { get; set; }
}
