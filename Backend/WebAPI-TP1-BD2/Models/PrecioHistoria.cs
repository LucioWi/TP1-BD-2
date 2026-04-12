using MongoDB.Bson.Serialization.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class PrecioHistoria
{
    [BsonElement("fecha")]
    public string Fecha { get; set; } = string.Empty;

    [BsonElement("precio")]
    public long Precio { get; set; }

    [BsonElement("razon")]
    public string? Razon { get; set; }
}
