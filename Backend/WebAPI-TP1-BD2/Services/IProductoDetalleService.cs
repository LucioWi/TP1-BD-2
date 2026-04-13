using WebAPI_TP1_BD2.Models;

namespace WebAPI_TP1_BD2.Services;

public interface IProductoDetalleService
{
    Task<Producto?> GetProductoAsync(string id);
    Task<bool> AddVarianteAsync(string id, Variante variante);
    Task<bool> RemoveVarianteAsync(string id, string varianteId);
    Task<bool> AddReviewAsync(string id, Review review);
    Task<bool> RemoveReviewAsync(string id, string reviewId);
    Task<bool> UpdateEspecificacionesAsync(string id, Especificaciones especificaciones);
    Task<bool> UpdateDisponibilidadAsync(string id, Disponibilidad disponibilidad);
}
