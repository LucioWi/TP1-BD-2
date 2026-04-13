using WebAPI_TP1_BD2.Models;
using WebAPI_TP1_BD2.Repositories;

namespace WebAPI_TP1_BD2.Services;

public class ProductoDetalleService : IProductoDetalleService
{
    private readonly IProductoDetalleRepository _repository;

    public ProductoDetalleService(IProductoDetalleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Producto?> GetProductoAsync(string id)
    {
        return await _repository.GetProductoAsync(id);
    }

    public async Task<bool> AddVarianteAsync(string id, Variante variante)
    {
        return await _repository.AddVarianteAsync(id, variante);
    }

    public async Task<bool> RemoveVarianteAsync(string id, string varianteId)
    {
        return await _repository.RemoveVarianteAsync(id, varianteId);
    }

    public async Task<bool> AddReviewAsync(string id, Review review)
    {
        return await _repository.AddReviewAsync(id, review);
    }

    public async Task<bool> RemoveReviewAsync(string id, string reviewId)
    {
        return await _repository.RemoveReviewAsync(id, reviewId);
    }

    public async Task<bool> UpdateEspecificacionesAsync(string id, Especificaciones especificaciones)
    {
        return await _repository.UpdateEspecificacionesAsync(id, especificaciones);
    }

    public async Task<bool> UpdateDisponibilidadAsync(string id, Disponibilidad disponibilidad)
    {
        return await _repository.UpdateDisponibilidadAsync(id, disponibilidad);
    }
}
