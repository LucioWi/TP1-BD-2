using MongoDB.Bson.Serialization.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class Disponibilidad
{
    [BsonElement("tienda_fisica_buenos_aires")]
    public TiendaFisica? TiendaFisicaBuenosAires { get; set; }

    [BsonElement("tienda_fisica_cordoba")]
    public TiendaFisica? TiendaFisicaCordoba { get; set; }

    [BsonElement("online")]
    public DisponibilidadOnline? Online { get; set; }
}

public class TiendaFisica
{
    [BsonElement("stock")]
    public int Stock { get; set; }

    [BsonElement("reservable")]
    public bool Reservable { get; set; }
}

public class DisponibilidadOnline
{
    [BsonElement("stock")]
    public int Stock { get; set; }

    [BsonElement("envio_dias")]
    public int? EnvioDias { get; set; }

    [BsonElement("envio_costo")]
    public long? EnvioCosto { get; set; }
}
