using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class Venta
{
    [BsonElement("fecha")]
    public string Fecha { get; set; } = string.Empty;

    [BsonElement("cantidad")]
    public int Cantidad { get; set; }

    [BsonElement("montoTotal")]
    public long MontoTotal { get; set; }

    [BsonElement("plataforma")]
    public PlataformaVenta? Plataforma { get; set; }
}

[BsonIgnoreExtraElements]
public class PlataformaVenta
{
    [BsonElement("online")]
    public int Online { get; set; }

    [BsonElement("fisico")]
    public int Fisico { get; set; }
}
