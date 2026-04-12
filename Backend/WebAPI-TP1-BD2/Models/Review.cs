using MongoDB.Bson.Serialization.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class Review
{
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("usuario")]
    public string Usuario { get; set; } = string.Empty;

    [BsonElement("calificacion")]
    public double Calificacion { get; set; }

    [BsonElement("titulo")]
    public string? Titulo { get; set; }

    [BsonElement("contenido")]
    public string? Contenido { get; set; }

    [BsonElement("fecha")]
    public string? Fecha { get; set; }

    [BsonElement("util")]
    public int? Util { get; set; }
}
