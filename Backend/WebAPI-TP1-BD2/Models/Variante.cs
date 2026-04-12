using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class Variante
{
    [BsonElement("id")]
    public string id { get; set; } = string.Empty;

    [BsonElement("colorCaja")]
    public string? ColorCaja { get; set; }

    [BsonElement("colorCintura")]
    public string? ColorCintura { get; set; }

    [BsonElement("stock")]
    public int Stock { get; set; }

    [BsonElement("precioVariante")]
    public long PrecioVariante { get; set; }

    [BsonElement("imagen")]
    public string? Imagen { get; set; }

    [BsonElement("colorEsfera")]
    public string? ColorEsfera { get; set; }

    [BsonElement("correa")]
    public string? Correa { get; set; }

    [BsonElement("talleAnillo")]
    public int? TalleAnillo { get; set; }

    [BsonElement("colorPerlas")]
    public string? ColorPerlas { get; set; }

    [BsonElement("distanciadores")]
    public string? Distanciadores { get; set; }
}
