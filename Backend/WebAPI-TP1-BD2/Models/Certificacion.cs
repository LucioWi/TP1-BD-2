using MongoDB.Bson.Serialization.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class Certificacion
{
    [BsonElement("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [BsonElement("emisora")]
    public string? Emisora { get; set; }

    [BsonElement("numero")]
    public string? Numero { get; set; }

    [BsonElement("fechaEmision")]
    public string? FechaEmision { get; set; }

    [BsonElement("vigente")]
    public bool? Vigente { get; set; }

    [BsonElement("comentario")]
    public string? Comentario { get; set; }

    [BsonElement("duracion_anos")]
    public int? DuracionAnos { get; set; }

    [BsonElement("cobertura")]
    public List<string>? Cobertura { get; set; }

    [BsonElement("detalles")]
    public string? Detalles { get; set; }
}
