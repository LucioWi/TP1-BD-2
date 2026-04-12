using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI_TP1_BD2.Models;

[BsonIgnoreExtraElements]
public class Especificaciones
{
    [BsonElement("tipo_reloj")]
    public string? TipoReloj { get; set; }

    [BsonElement("tipo_anillo")]
    public string? TipoAnillo { get; set; }

    [BsonElement("tipo_brazalete")]
    public string? TipoBrazalete { get; set; }

    [BsonElement("diametro_mm")]
    public double? DiametroMm { get; set; }

    [BsonElement("grosor_mm")]
    public double? GrosorMm { get; set; }

    [BsonElement("material_caja")]
    public string? MaterialCaja { get; set; }

    [BsonElement("material_cristal")]
    public string? MaterialCristal { get; set; }

    [BsonElement("resistencia_agua_m")]
    public int? ResistenciaAguaM { get; set; }

    [BsonElement("calibre")]
    public string? Calibre { get; set; }

    [BsonElement("reserva_marcha_horas")]
    public int? ReservaMarchaHoras { get; set; }

    [BsonElement("funciones")]
    public List<string>? Funciones { get; set; }

    [BsonElement("peso_g")]
    public double? PesoG { get; set; }

    [BsonElement("helium_escape_valve")]
    public bool? HeliumEscapeValve { get; set; }

    [BsonElement("rotula_unidireccional")]
    public string? RotulaUnidireccional { get; set; }

    [BsonElement("certificacion")]
    public string? Certificacion { get; set; }

    [BsonElement("piedra_principal")]
    public PiedraPrincipal? PiedraPrincipal { get; set; }

    [BsonElement("metal")]
    public string? Metal { get; set; }

    [BsonElement("perla_principal")]
    public PerlaPrincipal? PerlaPrincipal { get; set; }

    [BsonElement("perlas_secundarias")]
    public List<PerlaSecundaria>? PerlasSecundarias { get; set; }

    [BsonElement("estiramiento_circ")]
    public string? EstiramientoCirc { get; set; }

    [BsonElement("peso_aprox_g")]
    public double? PesoAproxG { get; set; }

    [BsonElement("acabado")]
    public string? Acabado { get; set; }

    [BsonElement("mantenimiento")]
    public string? Mantenimiento { get; set; }
}

public class PiedraPrincipal
{
    [BsonElement("tipo")]
    public string? Tipo { get; set; }

    [BsonElement("quilates")]
    public double? Quilates { get; set; }

    [BsonElement("color")]
    public string? Color { get; set; }
}

public class PerlaPrincipal
{
    [BsonElement("tipo")]
    public string? Tipo { get; set; }

    [BsonElement("diametro_mm")]
    public double? DiametroMm { get; set; }

    [BsonElement("color")]
    public string? Color { get; set; }

    [BsonElement("brillo")]
    public string? Brillo { get; set; }

    [BsonElement("origen")]
    public string? Origen { get; set; }
}

public class PerlaSecundaria
{
    [BsonElement("tipo")]
    public string? Tipo { get; set; }

    [BsonElement("cantidad")]
    public int? Cantidad { get; set; }

    [BsonElement("diametro_mm")]
    public double? DiametroMm { get; set; }

    [BsonElement("brilol")]
    public string? Brillo { get; set; }

    [BsonElement("material")]
    public string? Material { get; set; }

    [BsonElement("diseno")]
    public string? Diseno { get; set; }
}
