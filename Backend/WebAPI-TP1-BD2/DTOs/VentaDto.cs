namespace WebAPI_TP1_BD2.DTOs;

public class VentaDto
{
    public string Fecha { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public long MontoTotal { get; set; }
    public PlataformaVentaDto? Plataforma { get; set; }
}

public class PlataformaVentaDto
{
    public int Online { get; set; }
    public int Fisico { get; set; }
}
