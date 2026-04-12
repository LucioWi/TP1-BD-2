using MongoDB.Bson.Serialization.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class CertificadoPiedra
{
    [BsonElement("numeroCertificado")]
    public string? NumeroCertificado { get; set; }

    [BsonElement("laboratorio")]
    public string? Laboratorio { get; set; }

    [BsonElement("piedraId")]
    public string? PiedraId { get; set; }

    [BsonElement("tipoCertificacion")]
    public string? TipoCertificacion { get; set; }

    [BsonElement("fecha")]
    public string? Fecha { get; set; }

    [BsonElement("detalles")]
    public DetallesCertificado? Detalles { get; set; }
}

public class DetallesCertificado
{
    [BsonElement("tipo")]
    public string? Tipo { get; set; }

    [BsonElement("diametro")]
    public string? Diametro { get; set; }

    [BsonElement("color")]
    public string? Color { get; set; }

    [BsonElement("calidad")]
    public string? Calidad { get; set; }

    [BsonElement("tratamiento")]
    public string? Tratamiento { get; set; }
}
