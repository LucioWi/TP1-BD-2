using MongoDB.Bson.Serialization.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class Metadata
{
    [BsonElement("creadoEn")]
    public string? CreadoEn { get; set; }

    [BsonElement("actualizadoEn")]
    public string? ActualizadoEn { get; set; }

    [BsonElement("vistas")]
    public int? Vistas { get; set; }

    [BsonElement("compras")]
    public int? Compras { get; set; }

    [BsonElement("tags")]
    public List<string>? Tags { get; set; }
}
